using System;
using System.Collections.Generic;
using System.Text;

namespace writer.core
{
    public class ReadabilityStatistics
    {
        public ReadabilityStatistics(
            int characters,
            int letters,
            int sentences,
            int words,
            int paragraphs)
        {
            Characters = characters;
            Letters = letters;
            Sentences = sentences;
            Words = words;
            Paragraphs = paragraphs;
        }

        public int Characters { get; }

        public int Letters { get; }

        public int Sentences { get; }

        public int Words { get; }

        public int Paragraphs { get; }
    }
}
