using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using static writer.core.Language;

namespace writer
{
    public static class Functions
    {
        public static IEnumerable<Highlight> PassiveVoice(TextEditor editor, DocumentLine line)
        {
            var lineText = editor.Document.GetText(line);

            var matches = PassiveVoicePattern.Matches(lineText).Where(m => {
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

        public static IEnumerable<Highlight> Readability(TextEditor editor, DocumentLine line)
        {
            var lineText = editor.Document.GetText(line);

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
