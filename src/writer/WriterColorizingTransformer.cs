using System.Collections.Generic;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;
using static writer.Language;

namespace writer
{
    public class WriterColorizingTransformer : DocumentColorizingTransformer
    {
        private readonly TextEditor _editor;

        public WriterColorizingTransformer(TextEditor editor) =>
            _editor = editor;

        protected override void ColorizeLine(DocumentLine line)
        {
            var highlights = new List<Highlight>();

            highlights.AddRange(Readability(_editor, line, GetForegroundBrush));
            highlights.AddRange(PassiveVoice(_editor, line, PassiveVoiceForegroundBrush));
            highlights.AddRange(Adverbs(_editor, line, AdverbForegroundBrush));
            highlights.AddRange(WeakeningPhrases(_editor, line, WeakeningPhraseForegroundBrush));

            foreach (var highlight in highlights)
            {
                ChangeLinePart(
                    startOffset: highlight.StartOffset,
                    endOffset: highlight.EndOffset,
                    action: el => el.TextRunProperties.SetForegroundBrush(highlight.Brush)
                );
            }
        }
    }
}
