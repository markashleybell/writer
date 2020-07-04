<Query Kind="Program">
  <Reference Relative="..\src\writer.core\bin\Debug\netstandard2.0\writer.core.dll">C:\Src\writer\src\writer.core\bin\Debug\netstandard2.0\writer.core.dll</Reference>
  <NuGetReference>AvalonEdit</NuGetReference>
  <Namespace>ICSharpCode.AvalonEdit</Namespace>
  <Namespace>ICSharpCode.AvalonEdit.Document</Namespace>
  <Namespace>ICSharpCode.AvalonEdit.Highlighting</Namespace>
  <Namespace>ICSharpCode.AvalonEdit.Highlighting.Xshd</Namespace>
  <Namespace>ICSharpCode.AvalonEdit.Rendering</Namespace>
  <Namespace>static writer.core.Language</Namespace>
  <Namespace>System.Windows</Namespace>
  <Namespace>System.Windows.Media</Namespace>
  <Namespace>writer.core</Namespace>
</Query>

void Main()
{
    var workingDirectory = Path.GetDirectoryName(Util.CurrentQueryPath);

//    var highlightingDefinitionFilePath = workingDirectory + @"\..\src\writer\Highlighting.xshd";
//
//    IHighlightingDefinition highlightingDefinition;
//
//    using (var reader = new XmlTextReader(File.OpenRead(highlightingDefinitionFilePath)))
//    {
//        highlightingDefinition = HighlightingLoader.Load(reader, HighlightingManager.Instance);
//    }
//
//    // highlightingDefinition.Dump();
//
//    HighlightingManager.Instance.RegisterHighlighting(
//        name: "Writer",
//        extensions: new[] { ".writer", ".txt" },
//        highlighting: highlightingDefinition
//    );

    var editor = new TextEditor();
    
    editor.WordWrap = true;
    editor.Height = 600;
    // editor.SyntaxHighlighting = highlightingDefinition;
    
    editor.FontFamily = new FontFamily("Consolas");
    editor.FontSize = 16;
    
    var bg = new ColorBackgroundRenderer(editor);
    
    editor.TextArea.TextView.BackgroundRenderers.Insert(1, bg);
    
   // editor.TextArea.TextView.BackgroundRenderers.Dump();
    
    // editor.TextArea.TextView.LineTransformers.RemoveAt(0);
    
    // editor.TextArea.TextView.LineTransformers.Insert(0, new WriterColorizingTransformer());
    
    // editor.TextArea.TextView.LineTransformers.Add(new WriterColorizingTransformer(bg));
    
    //editor.TextArea.TextView.LineTransformers.Dump();

    editor.Text = @"The app highlights lengthy, complex sentences and common errors; if you see a yellow sentence, shorten or split it. If you see a red highlight, your sentence is so dense and complicated that your readers will get lost trying to follow its meandering, splitting logic — try editing this sentence to remove the red.

You can utilize a shorter word in place of a purple one. Mouse over them for hints.

Adverbs and weakening phrases are helpfully shown in blue. Get rid of them and pick words with force, perhaps.

Phrases in green have been marked to show passive voice.

You can format your text with the toolbar.

Paste in something you're working on and edit away. Or, click the Write button and compose something new.

There is a considerable range of expertise being demonstrated by the spam senders.

It was determined by the committee that the report was inconclusive.";

    editor.Dump();
}

/// <summary>A search result storing a match and text segment.</summary>
public class Highlight : TextSegment {
    public Brush Brush { get; }

    /// <summary>Constructs the search result from the match.</summary>
    public Highlight(int startOffset, int endOffset, Brush brush)
    {
        this.StartOffset = startOffset;
        this.EndOffset = endOffset;
        this.Brush = brush;
    }
}

/// <summary>Colorizes search results behind the selection.</summary>
public class ColorBackgroundRenderer : IBackgroundRenderer 
{
    private TextEditor _editor;
    
    // private TextSegmentCollection<Highlight> _highlights = new TextSegmentCollection<Highlight>();

    public ColorBackgroundRenderer(TextEditor editor) =>
        _editor = editor;

    /// <summary>Gets the layer on which this background renderer should draw.</summary>
    public KnownLayer Layer
    {
        get
        {
            // draw behind selection
            return KnownLayer.Selection;
        }
    }

    /// <summary>Causes the background renderer to draw.</summary>
    public void Draw(TextView textView, DrawingContext drawingContext)
    {
        // Console.WriteLine("DRAW");
        
        if (textView == null)
            throw new ArgumentNullException("textView");
        if (drawingContext == null)
            throw new ArgumentNullException("drawingContext");

        if (!textView.VisualLinesValid)
            return;

        var visualLines = textView.VisualLines;
        if (visualLines.Count == 0)
            return;

        // visualLines.

        // The reason this is so ridiculously confusing is that code folding can mean 
        // that one line can represent many, visually. We won't ever encounter that...
        int viewStart = visualLines.First().FirstDocumentLine.Offset;
        int viewEnd = visualLines.Last().LastDocumentLine.EndOffset;

        var highlights = new TextSegmentCollection<Highlight>();

        foreach (var line in visualLines)
        {
            var documentLine = line.FirstDocumentLine;

            foreach (var c in Readability(documentLine))
                highlights.Add(c);
                
            foreach (var h in PassiveVoice(documentLine))
                highlights.Add(h);
        }

        foreach (var highlight in highlights.FindOverlappingSegments(viewStart, viewEnd - viewStart))
        {
            BackgroundGeometryBuilder geoBuilder = new BackgroundGeometryBuilder();
            geoBuilder.AlignToWholePixels = true;
            geoBuilder.BorderThickness = 0;
            geoBuilder.CornerRadius = 0;
            geoBuilder.AddSegment(textView, highlight);
            Geometry geometry = geoBuilder.CreateGeometry();
            
            if (geometry != null)
            {
                drawingContext.DrawGeometry(highlight.Brush, null, geometry);
            }
        }
    }

    ///// <summary>Gets the search results for modification.</summary>
    //public TextSegmentCollection<Highlight> Highlights
    //{
    //    get { return _highlights; }
    //}

    private IEnumerable<Highlight> PassiveVoice(DocumentLine line)
    {   
        var lineText = _editor.Document.GetText(line);

        var matches = PassiveVoicePattern.Matches(lineText).Where(m =>
        {
            var term = m.Groups[2].Value;

            if (term.EndsWith(PassiveSuffixPattern, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (PassiveToActiveSubstitutions.ContainsKey(term))
            {
                return true;
            }

            return false;
        });

        // matches3.Select(m => m.Value).Dump();

        foreach (var match in matches)
        {
            yield return new Highlight(
                startOffset: line.Offset + match.Index,
                endOffset: line.Offset + match.Index + match.Length,
                brush: Brushes.LightGreen
            );
        }
    }

    private IEnumerable<Highlight> Readability(DocumentLine line)
    {
        var lineText = _editor.Document.GetText(line);

        var start = 0;
        var index = 0;

        while ((index = lineText.IndexOfAny(SentenceSeparators, start)) >= 0)
        {
            var sentence = lineText.Substring(start, (index + 1) - start);

            var score = sentence.GetReadability();

            if (score.Words > 13)
            {
                (sentence, score).Dump();

                var brush = score.Readability > 14 ? Brushes.LightCoral : Brushes.PaleGoldenrod;

                yield return new Highlight(
                    startOffset: line.Offset + start,
                    endOffset: line.Offset + index + 1,
                    brush: brush
                );
            }

            // Start after the sentence separator character
            start = index + 1;

            // Move forward until we're at the next letter (start of next sentence)
            while (start < lineText.Length && !char.IsLetterOrDigit(lineText[start]))
            {
                start++;
            }
        }
    }
}

public class WriterColorizingTransformer : DocumentColorizingTransformer 
{    
    private ColorBackgroundRenderer _backgroundRenderer;
    
    public WriterColorizingTransformer(ColorBackgroundRenderer backgroundRenderer) =>
        _backgroundRenderer = backgroundRenderer;
    
    protected override void ColorizeLine(DocumentLine line)
    {
        // Transform(line, "passive", el => el.TextRunProperties.SetBackgroundBrush(Brushes.LightGreen));
        
        // Readability(line);

        var pv = PassiveVoice(line);


        //        if (pv.Any())
        //        {
        //            var invalid = _backgroundRenderer.Highlights.Where(h => h.StartOffset > pv.Max(x => x.EndOffset));
        //
        //
        //            foreach (var i in invalid)
        //                _backgroundRenderer.Highlights.Remove(i);
        //        }

        //foreach (var p in pv)
        //{
        //    foreach (var i in _backgroundRenderer.Highlights.FindOverlappingSegments(line.Offset, line.EndOffset))
        //    {
        //        _backgroundRenderer.Highlights.Remove(i);
        //    }
        //    
        //    _backgroundRenderer.Highlights.Add(p);
        //}
        //    
        // _backgroundRenderer.Highlights.Dump();
    }

    private IEnumerable<Highlight> Readability(DocumentLine line)
    {
        var lineText = CurrentContext.Document.GetText(line);
        
        var start = 0;
        var index = 0;

        while ((index = lineText.IndexOfAny(SentenceSeparators, start)) >= 0)
        {
            var sentence = lineText.Substring(start, (index + 1) - start);
            
            var score = sentence.GetReadability();

            if (score.Words > 13)
            { 
                (sentence, score).Dump();
                
                var brush = score.Readability > 14 ? Brushes.LightCoral : Brushes.PaleGoldenrod;
    
                yield return new Highlight(
                    startOffset: line.Offset + start,
                    endOffset: line.Offset + index + 1,
                    brush: brush
                );
    //
    //            base.ChangeLinePart(
    //                startOffset: line.Offset + start,
    //                endOffset: line.Offset + index + 1,
    //                action: el => el.TextRunProperties.SetBackgroundBrush(brush)
    //            );
            }

            // Start after the sentence separator character
            start = index + 1;

            // Move forward until we're at the next letter (start of next sentence)
            while (start < lineText.Length && !char.IsLetterOrDigit(lineText[start]))
            {
                start++;
            }
        }
    }

    private IEnumerable<Highlight> PassiveVoice(DocumentLine line)
    {
        var lineText = CurrentContext.Document.GetText(line);
        
        var matches = PassiveVoicePattern.Matches(lineText).Where(m => {
            var term = m.Groups[2].Value;
        
            if (term.EndsWith(PassiveSuffixPattern, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            
            if (PassiveToActiveSubstitutions.ContainsKey(term))
            {
                return true;
            }
            
            return false;
        });
        
        // matches3.Select(m => m.Value).Dump();

        foreach (var match in matches)
        {
            yield return new Highlight(
                startOffset: line.Offset + match.Index,
                endOffset: line.Offset + match.Index + match.Length,
                brush: Brushes.LightGreen
            );

            //base.ChangeLinePart(
            //    startOffset: line.Offset + match.Index,
            //    endOffset: line.Offset + match.Index,
            //    action: el => el.TextRunProperties.SetBackgroundBrush(Brushes.LightGreen)
            //);
        }
    }

    private void Transform(DocumentLine line, string text, Action<VisualLineElement> transform)
    {
        var lineText = CurrentContext.Document.GetText(line);
        var start = 0;
        
        int index;

        while ((index = lineText.IndexOf(text, start)) >= 0)
        {
            base.ChangeLinePart(
                startOffset: line.Offset + index,
                endOffset: line.Offset + index + text.Length,
                action: transform
            );

            start = index + 1;
        }
    }
}