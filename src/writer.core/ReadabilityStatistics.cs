using System;
using System.Collections.Generic;
using System.Text;

namespace writer.core
{
    public class ReadabilityStatistics
    {
        internal ReadabilityStatistics(
            int characters,
            int letters,
            int sentences,
            int words,
            int paragraphs,
            double readability,
            AutomatedReadabilityIndexGrade readbilityGrade)
        {
            Characters = characters;
            Letters = letters;
            Sentences = sentences;
            Words = words;
            Paragraphs = paragraphs;
            Readability = readability;
            ReadabilityGrade = readbilityGrade;
        }

        public int Characters { get; }

        public int Letters { get; }

        public int Sentences { get; }

        public int Words { get; }

        public int Paragraphs { get; }

        public double Readability { get; }

        public AutomatedReadabilityIndexGrade ReadabilityGrade { get; }
    }
}
