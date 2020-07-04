using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;
using static writer.core.Language;


namespace writer
{
    public class ColorBackgroundRenderer : IBackgroundRenderer
    {
        private readonly TextEditor _editor;

        public ColorBackgroundRenderer(TextEditor editor) =>
            _editor = editor;

        // Draw behind selection
        public KnownLayer Layer => KnownLayer.Selection;

        public void Draw(TextView textView, DrawingContext drawingContext)
        {
            if (textView == null)
            {
                throw new ArgumentNullException(nameof(textView));
            }

            if (drawingContext == null)
            {
                throw new ArgumentNullException(nameof(drawingContext));
            }

            if (!textView.VisualLinesValid)
            {
                return;
            }

            var visualLines = textView.VisualLines;

            if (visualLines.Count == 0)
            {
                return;
            }

            // The reason this is so ridiculously confusing is that in a code editor,
            // code folding can mean that one line can represent many, visually.
            // We won't ever encounter that, as this is an editor for writing...
            var viewStart = visualLines[0].FirstDocumentLine.Offset;
            var viewEnd = visualLines.Last().LastDocumentLine.EndOffset;

            var highlights = new TextSegmentCollection<Highlight>();

            foreach (var line in visualLines)
            {
                var documentLine = line.FirstDocumentLine;

                foreach (var c in Readability(documentLine))
                {
                    highlights.Add(c);
                }

                foreach (var h in PassiveVoice(documentLine))
                {
                    highlights.Add(h);
                }
            }

            foreach (var highlight in highlights.FindOverlappingSegments(viewStart, viewEnd - viewStart))
            {
                var geoBuilder = new BackgroundGeometryBuilder {
                    AlignToWholePixels = true,
                    BorderThickness = 0,
                    CornerRadius = 0
                };

                geoBuilder.AddSegment(textView, highlight);

                var geometry = geoBuilder.CreateGeometry();

                if (geometry is object)
                {
                    drawingContext.DrawGeometry(highlight.Brush, null, geometry);
                }
            }
        }

        private IEnumerable<Highlight> PassiveVoice(DocumentLine line)
        {
            var lineText = _editor.Document.GetText(line);

            var matches = PassiveVoicePattern.Matches(lineText).Where(m =>
            {
                var term = m.Groups[2].Value;

                return term.EndsWith(PassiveSuffixPattern, StringComparison.OrdinalIgnoreCase)
                    || PassiveToActiveSubstitutions.ContainsKey(term);
            });

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

            int index;

            while ((index = lineText.IndexOfAny(SentenceSeparators, start)) >= 0)
            {
                var sentence = lineText.Substring(start, index + 1 - start);

                var score = sentence.GetReadability();

                if (score.Words > 13)
                {
                    var brush = score.Readability > 14
                        ? Brushes.LightCoral
                        : Brushes.PaleGoldenrod;

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
}
