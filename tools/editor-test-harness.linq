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
    editor.Height = 400;
    // editor.SyntaxHighlighting = highlightingDefinition;
    
    editor.FontFamily = new FontFamily("Consolas");
    editor.FontSize = 16;
    
    editor.TextArea.TextView.LineTransformers.RemoveAt(0);
    
    editor.TextArea.TextView.LineTransformers.Insert(0, new WriterColorizingTransformer());
    
    // editor.TextArea.TextView.LineTransformers.Dump();

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

public class WriterColorizingTransformer : DocumentColorizingTransformer 
{    
    protected override void ColorizeLine(DocumentLine line)
    {
        // Transform(line, "passive", el => el.TextRunProperties.SetBackgroundBrush(Brushes.LightGreen));
        
        Readability(line);
        
        PassiveVoice(line);
    }

    private void Readability(DocumentLine line)
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
    
                base.ChangeLinePart(
                    startOffset: line.Offset + start,
                    endOffset: line.Offset + index + 1,
                    action: el => el.TextRunProperties.SetBackgroundBrush(brush)
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

    private void PassiveVoice(DocumentLine line)
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
            base.ChangeLinePart(
                startOffset: line.Offset + match.Index,
                endOffset: line.Offset + match.Index + match.Length,
                action: el => el.TextRunProperties.SetBackgroundBrush(Brushes.LightGreen)
            );
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