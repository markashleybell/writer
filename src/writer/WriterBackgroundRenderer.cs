using System;
using System.Linq;
using System.Windows.Media;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;
using static writer.Language;

namespace writer
{
    public class WriterBackgroundRenderer : IBackgroundRenderer
    {
        private readonly TextEditor _editor;

        public WriterBackgroundRenderer(TextEditor editor) =>
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

                foreach (var c in Readability(_editor, documentLine, GetBackgroundBrush))
                {
                    highlights.Add(c);
                }

                foreach (var h in PassiveVoice(_editor, documentLine, PassiveVoiceBackgroundBrush))
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
    }
}
