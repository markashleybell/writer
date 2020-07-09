using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Media;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using static writer.core.Language;

namespace writer
{
    public static class Language
    {
        public static SolidColorBrush PassiveVoiceBackgroundBrush { get; set; }
            = Brushes.LightGreen;

        public static SolidColorBrush PassiveVoiceForegroundBrush { get; set; }
            = Brushes.LightGreen;

        public static SolidColorBrush VeryHardSentenceBackgroundBrush { get; set; }
            = Brushes.LightCoral;

        public static SolidColorBrush VeryHardSentenceForegroundBrush { get; set; }
            = Brushes.LightCoral;

        public static SolidColorBrush HardSentenceBackgroundBrush { get; set; }
            = Brushes.PaleGoldenrod;

        public static SolidColorBrush HardSentenceForegroundBrush { get; set; }
            = Brushes.PaleGoldenrod;

        public static SolidColorBrush AdverbBackgroundBrush { get; set; }
            = Brushes.PaleTurquoise;

        public static SolidColorBrush AdverbForegroundBrush { get; set; }
            = Brushes.PaleTurquoise;

        public static SolidColorBrush WeakeningPhraseBackgroundBrush { get; set; }
            = Brushes.LightCyan;

        public static SolidColorBrush WeakeningPhraseForegroundBrush { get; set; }
            = Brushes.LightCyan;

        public static Brush GetBackgroundBrush(double readability) =>
            readability > 14 ? VeryHardSentenceBackgroundBrush : HardSentenceBackgroundBrush;

        public static Brush GetForegroundBrush(double readability) =>
            readability > 14 ? VeryHardSentenceBackgroundBrush : HardSentenceBackgroundBrush;

        public static IEnumerable<Highlight> Adverbs(
            TextEditor editor,
            DocumentLine line,
            Brush brush)
        {
            var lineText = editor.Document.GetText(line);

            var matches = AdverbPattern.Matches(lineText).Where(m => {
                var term = m.Groups[2].Value;

                return !AdverbExceptions.Contains(term);
            });

            foreach (var match in matches)
            {
                yield return new Highlight(
                    startOffset: line.Offset + match.Index,
                    endOffset: line.Offset + match.Index + match.Length,
                    brush: brush
                );
            }
        }

        public static IEnumerable<Highlight> WeakeningPhrases(
            TextEditor editor,
            DocumentLine line,
            Brush brush)
        {
            var lineText = editor.Document.GetText(line);

            foreach (var match in WeakeningPhrasePattern.Matches(lineText).AsEnumerable())
            {
                yield return new Highlight(
                    startOffset: line.Offset + match.Index,
                    endOffset: line.Offset + match.Index + match.Length,
                    brush: brush
                );
            }
        }

        public static IEnumerable<Highlight> PassiveVoice(
            TextEditor editor,
            DocumentLine line,
            Brush brush)
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
                    brush: brush
                );
            }
        }

        public static IEnumerable<Highlight> Readability(
            TextEditor editor,
            DocumentLine line,
            Func<double, Brush> getBrush)
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
                    var brush = getBrush(score.Readability);

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
