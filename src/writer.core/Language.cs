using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace writer.core
{
    public static class Language
    {
        public const StringSplitOptions NoEmptyEntries = StringSplitOptions.RemoveEmptyEntries;

        public const RegexOptions RxOptions = RegexOptions.IgnoreCase | RegexOptions.Compiled;

        public const string PassiveSuffixPattern = "ed";

        public static readonly Regex PassiveVoicePattern
            = new(@"\b(is|are|was|were|be|been|being)\s([a-z]{2,30})\b(\sby\b)?", RxOptions);

        public static readonly Regex AdverbPattern
            = new(@"\b([a-z]+ly)\b", RxOptions);

        public static readonly char[] Vowels = new[] { 'A', 'E', 'I', 'O', 'U', 'Y' };

        public static readonly char[] WordSeparators = new[] {
            ' ',
            '\r',
            '\n',
            '\u2014' // EM dash
        };

        public static readonly char[] SentenceSeparators = new[] { '.' };

        public static readonly string[] ParagraphSeparators = new[] { Environment.NewLine + Environment.NewLine };

        public static readonly string[] WeakeningPhrases = new[] {
            "i believe",
            "i consider",
            "i don't believe",
            "i don't consider",
            "i don't feel",
            "i don't suggest",
            "i don't think",
            "i feel",
            "i hope to",
            "i might",
            "i suggest",
            "i think",
            "i was wondering",
            "i will try",
            "i wonder",
            "in my opinion",
            "is kind of",
            "is sort of",
            "just",
            "maybe",
            "perhaps",
            "possibly",
            "we believe",
            "we consider",
            "we don't believe",
            "we don't consider",
            "we don't feel",
            "we don't suggest",
            "we don't think",
            "we feel",
            "we hope to",
            "we might",
            "we suggest",
            "we think",
            "we were wondering",
            "we will try",
            "we wonder"
        };

        public static readonly Regex WeakeningPhrasePattern
            = new(@"\b(" + string.Join("|", WeakeningPhrases) + @")\b", RxOptions);

        public static readonly Dictionary<string, string[]> Simplifications = new() {
            { "a number of", new[] { "many", "some" } },
            { "abundance", new[] { "enough", "plenty" } },
            { "accede to", new[] { "allow", "agree to" } },
            { "accelerate", new[] { "speed up" } },
            { "accentuate", new[] { "stress" } },
            { "accompany", new[] { "go with", "with" } },
            { "accomplish", new[] { "do" } },
            { "accorded", new[] { "given" } },
            { "accrue", new[] { "add", "gain" } },
            { "acquiesce", new[] { "agree" } },
            { "acquire", new[] { "get" } },
            { "additional", new[] { "more", "extra" } },
            { "adjacent to", new[] { "next to" } },
            { "adjustment", new[] { "change" } },
            { "admissible", new[] { "allowed", "accepted" } },
            { "advantageous", new[] { "helpful" } },
            { "adversely impact", new[] { "hurt" } },
            { "advise", new[] { "tell" } },
            { "aforementioned", new[] { "remove" } },
            { "aggregate", new[] { "total", "add" } },
            { "aircraft", new[] { "plane" } },
            { "all of", new[] { "all" } },
            { "alleviate", new[] { "ease", "reduce" } },
            { "allocate", new[] { "divide" } },
            { "along the lines of", new[] { "like", "as in" } },
            { "already existing", new[] { "existing" } },
            { "alternatively", new[] { "or" } },
            { "ameliorate", new[] { "improve", "help" } },
            { "anticipate", new[] { "expect" } },
            { "apparent", new[] { "clear", "plain" } },
            { "appreciable", new[] { "many" } },
            { "as a means of", new[] { "to" } },
            { "as of yet", new[] { "yet" } },
            { "as to", new[] { "on", "about" } },
            { "as yet", new[] { "yet" } },
            { "ascertain", new[] { "find out", "learn" } },
            { "assistance", new[] { "help" } },
            { "at this time", new[] { "now" } },
            { "attain", new[] { "meet" } },
            { "attributable to", new[] { "because" } },
            { "authorize", new[] { "allow", "let" } },
            { "because of the fact that", new[] { "because" } },
            { "belated", new[] { "late" } },
            { "benefit from", new[] { "enjoy" } },
            { "bestow", new[] { "give", "award" } },
            { "by virtue of", new[] { "by", "under" } },
            { "cease", new[] { "stop" } },
            { "close proximity", new[] { "near" } },
            { "commence", new[] { "begin or start" } },
            { "comply with", new[] { "follow" } },
            { "concerning", new[] { "about", "on" } },
            { "consequently", new[] { "so" } },
            { "consolidate", new[] { "join", "merge" } },
            { "constitutes", new[] { "is", "forms", "makes up" } },
            { "demonstrate", new[] { "prove", "show" } },
            { "depart", new[] { "leave", "go" } },
            { "designate", new[] { "choose", "name" } },
            { "discontinue", new[] { "drop", "stop" } },
            { "due to the fact that", new[] { "because", "since" } },
            { "each and every", new[] { "each" } },
            { "economical", new[] { "cheap" } },
            { "eliminate", new[] { "cut", "drop", "end" } },
            { "elucidate", new[] { "explain" } },
            { "employ", new[] { "use" } },
            { "endeavor", new[] { "try" } },
            { "enumerate", new[] { "count" } },
            { "equitable", new[] { "fair" } },
            { "equivalent", new[] { "equal" } },
            { "evaluate", new[] { "test", "check" } },
            { "evidenced", new[] { "showed" } },
            { "exclusively", new[] { "only" } },
            { "expedite", new[] { "hurry" } },
            { "expend", new[] { "spend" } },
            { "expiration", new[] { "end" } },
            { "facilitate", new[] { "ease", "help" } },
            { "factual evidence", new[] { "facts", "evidence" } },
            { "feasible", new[] { "workable" } },
            { "finalize", new[] { "complete", "finish" } },
            { "first and foremost", new[] { "first" } },
            { "for the purpose of", new[] { "to" } },
            { "forfeit", new[] { "lose", "give up" } },
            { "formulate", new[] { "plan" } },
            { "honest truth", new[] { "truth" } },
            { "however", new[] { "but", "yet" } },
            { "if and when", new[] { "if", "when" } },
            { "impacted", new[] { "affected", "harmed", "changed" } },
            { "implement", new[] { "install", "put in place", "tool" } },
            { "in a timely manner", new[] { "on time" } },
            { "in accordance with", new[] { "by", "under" } },
            { "in addition", new[] { "also", "besides", "too" } },
            { "in all likelihood", new[] { "probably" } },
            { "in an effort to", new[] { "to" } },
            { "in between", new[] { "between" } },
            { "in excess of", new[] { "more than" } },
            { "in lieu of", new[] { "instead" } },
            { "in light of the fact that", new[] { "because" } },
            { "in many cases", new[] { "often" } },
            { "in order to", new[] { "to" } },
            { "in regard to", new[] { "about", "concerning", "on" } },
            { "in some instances ", new[] { "sometimes" } },
            { "in terms of", new[] { "omit" } },
            { "in the near future", new[] { "soon" } },
            { "in the process of", new[] { "omit" } },
            { "inception", new[] { "start" } },
            { "incumbent upon", new[] { "must" } },
            { "indicate", new[] { "say", "state", "or show" } },
            { "indication", new[] { "sign" } },
            { "initiate", new[] { "start" } },
            { "is applicable to", new[] { "applies to" } },
            { "is authorized to", new[] { "may" } },
            { "is responsible for", new[] { "handles" } },
            { "it is essential", new[] { "must", "need to" } },
            { "literally", new[] { "omit" } },
            { "magnitude", new[] { "size" } },
            { "maximum", new[] { "greatest", "largest", "most" } },
            { "methodology", new[] { "method" } },
            { "minimize", new[] { "cut" } },
            { "minimum", new[] { "least", "smallest", "small" } },
            { "modify", new[] { "change" } },
            { "monitor", new[] { "check", "watch", "track" } },
            { "multiple", new[] { "many" } },
            { "necessitate", new[] { "cause", "need" } },
            { "nevertheless", new[] { "still", "besides", "even so" } },
            { "not certain", new[] { "uncertain" } },
            { "not many", new[] { "few" } },
            { "not often", new[] { "rarely" } },
            { "not unless", new[] { "only if" } },
            { "not unlike", new[] { "similar", "alike" } },
            { "notwithstanding", new[] { "in spite of", "still" } },
            { "null and void", new[] { "use either null or void" } },
            { "numerous", new[] { "many" } },
            { "objective", new[] { "aim", "goal" } },
            { "obligate", new[] { "bind", "compel" } },
            { "obtain", new[] { "get" } },
            { "on the contrary", new[] { "but", "so" } },
            { "on the other hand", new[] { "omit", "but", "so" } },
            { "one particular", new[] { "one" } },
            { "optimum", new[] { "best", "greatest", "most" } },
            { "overall", new[] { "omit" } },
            { "owing to the fact that", new[] { "because", "since" } },
            { "participate", new[] { "take part" } },
            { "particulars", new[] { "details" } },
            { "pass away", new[] { "die" } },
            { "pertaining to", new[] { "about", "of", "on" } },
            { "point in time", new[] { "time", "point", "moment", "now" } },
            { "portion", new[] { "part" } },
            { "possess", new[] { "have", "own" } },
            { "preclude", new[] { "prevent" } },
            { "previously", new[] { "before" } },
            { "prior to", new[] { "before" } },
            { "prioritize", new[] { "rank", "focus on" } },
            { "procure", new[] { "buy", "get" } },
            { "proficiency", new[] { "skill" } },
            { "provided that", new[] { "if" } },
            { "purchase", new[] { "buy", "sale" } },
            { "put simply", new[] { "omit" } },
            { "readily apparent", new[] { "clear" } },
            { "refer back", new[] { "refer" } },
            { "regarding", new[] { "about", "of", "on" } },
            { "relocate", new[] { "move" } },
            { "remainder", new[] { "rest" } },
            { "remuneration", new[] { "payment" } },
            { "require", new[] { "must", "need" } },
            { "requirement", new[] { "need", "rule" } },
            { "reside", new[] { "live" } },
            { "residence", new[] { "house" } },
            { "retain", new[] { "keep" } },
            { "satisfy", new[] { "meet", "please" } },
            { "shall", new[] { "must", "will" } },
            { "should you wish", new[] { "if you want" } },
            { "similar to", new[] { "like" } },
            { "solicit", new[] { "ask for", "request" } },
            { "span across", new[] { "span", "cross" } },
            { "strategize", new[] { "plan" } },
            { "subsequent", new[] { "later", "next", "after", "then" } },
            { "substantial", new[] { "large", "much" } },
            { "successfully complete", new[] { "complete", "pass" } },
            { "sufficient", new[] { "enough" } },
            { "terminate", new[] { "end", "stop" } },
            { "the month of", new[] { "omit" } },
            { "therefore", new[] { "thus", "so" } },
            { "this day and age", new[] { "today" } },
            { "time period", new[] { "time", "period" } },
            { "took advantage of", new[] { "preyed on" } },
            { "transmit", new[] { "send" } },
            { "transpire", new[] { "happen" } },
            { "until such time as", new[] { "until" } },
            { "utilization", new[] { "use" } },
            { "utilize", new[] { "use" } },
            { "validate", new[] { "confirm" } },
            { "various different", new[] { "various", "different" } },
            { "whether or not", new[] { "whether" } },
            { "with respect to", new[] { "on", "about" } },
            { "with the exception of", new[] { "except for" } },
            { "witnessed", new[] { "saw", "seen" } }
        };

        public static readonly string[] AdverbExceptions = new[] {
            "actually",
            "additionally",
            "allegedly",
            "ally",
            "alternatively",
            "anomaly",
            "apply",
            "approximately",
            "ashely",
            "ashly",
            "assembly",
            "awfully",
            "baily",
            "belly",
            "bely",
            "billy",
            "bradly",
            "bristly",
            "bubbly",
            "bully",
            "burly",
            "butterfly",
            "carly",
            "charly",
            "chilly",
            "comely",
            "completely",
            "comply",
            "consequently",
            "costly",
            "courtly",
            "crinkly",
            "crumbly",
            "cuddly",
            "curly",
            "currently",
            "daily",
            "dastardly",
            "deadly",
            "deathly",
            "definitely",
            "dilly",
            "disorderly",
            "doily",
            "dolly",
            "dragonfly",
            "early",
            "elderly",
            "elly",
            "emily",
            "especially",
            "exactly",
            "exclusively",
            "family",
            "finally",
            "firefly",
            "folly",
            "friendly",
            "frilly",
            "gadfly",
            "gangly",
            "generally",
            "ghastly",
            "giggly",
            "globally",
            "goodly",
            "gravelly",
            "grisly",
            "gully",
            "haily",
            "hally",
            "harly",
            "hardly",
            "heavenly",
            "hillbilly",
            "hilly",
            "holly",
            "holy",
            "homely",
            "homily",
            "horsefly",
            "hourly",
            "immediately",
            "instinctively",
            "imply",
            "italy",
            "jelly",
            "jiggly",
            "jilly",
            "jolly",
            "july",
            "karly",
            "kelly",
            "kindly",
            "lately",
            "likely",
            "lilly",
            "lily",
            "lively",
            "lolly",
            "lonely",
            "lovely",
            "lowly",
            "luckily",
            "mealy",
            "measly",
            "melancholy",
            "mentally",
            "molly",
            "monopoly",
            "monthly",
            "multiply",
            "nightly",
            "oily",
            "only",
            "orderly",
            "panoply",
            "particularly",
            "partly",
            "paully",
            "pearly",
            "pebbly",
            "polly",
            "potbelly",
            "presumably",
            "previously",
            "pualy",
            "quarterly",
            "rally",
            "rarely",
            "recently",
            "rely",
            "reply",
            "reportedly",
            "roughly",
            "sally",
            "scaly",
            "shapely",
            "shelly",
            "shirly",
            "shortly",
            "sickly",
            "silly",
            "sly",
            "smelly",
            "sparkly",
            "spindly",
            "spritely",
            "squiggly",
            "stately",
            "steely",
            "supply",
            "surly",
            "tally",
            "timely",
            "trolly",
            "ugly",
            "underbelly",
            "unfortunately",
            "unholy",
            "unlikely",
            "usually",
            "waverly",
            "weekly",
            "wholly",
            "willy",
            "wily",
            "wobbly",
            "wooly",
            "worldly",
            "wrinkly",
            "yearly"
        };

        // E.g. "Jim was awoken by" -> "Jim awoke"
        public static readonly Dictionary<string, string> PassiveToActiveSubstitutions = new() {
            { "awoken", "awoke" },
            { "beaten", "beat" },
            { "begun", "began" },
            { "bent", "bent" },
            { "bitten", "bit" },
            { "bled", "bled" },
            { "blown", "blew" },
            { "broken", "broke" },
            { "brought", "brought" },
            { "built", "built" },
            { "bought", "bought" },
            { "caught", "caught" },
            { "chosen", "chose" },
            { "cut", "cut" },
            { "dealt", "dealt" },
            { "done", "did" },
            { "drawn", "drew" },
            { "driven", "drove" },
            { "eaten", "ate" },
            { "fed", "fed" },
            { "felt", "felt" },
            { "fought", "fought" },
            { "found", "found" },
            { "forbidden", "forbade" },
            { "forgotten", "forgot" },
            { "forgiven", "forgave" },
            { "frozen", "froze" },
            { "gotten", "got" },
            { "given", "gave" },
            { "ground", "ground" },
            { "hung", "hung" },
            { "heard", "heard" },
            { "hidden", "hid" },
            { "hit", "hit" },
            { "held", "held" },
            { "hurt", "hurt" },
            { "kept", "kept" },
            { "known", "knew" },
            { "laid", "laid" },
            { "led", "led" },
            { "left", "left" },
            { "let", "let" },
            { "lost", "lost" },
            { "made", "made" },
            { "meant", "meant" },
            { "met", "met" },
            { "paid", "paid" },
            { "proven", "proved" },
            { "put", "put" },
            { "read", "read" },
            { "ridden", "rode" },
            { "rung", "rang" },
            { "run", "ran" },
            { "said", "said" },
            { "seen", "saw" },
            { "sold", "sold" },
            { "sent", "sent" },
            { "shaken", "shook" },
            { "shaved", "shaved" },
            { "shot", "shot" },
            { "shown", "showed" },
            { "shut", "shut" },
            { "sung", "sung" },
            { "sunk", "sunk" },
            { "slain", "slew" },
            { "slid", "slid" },
            { "spoken", "spoke" },
            { "spent", "spent" },
            { "spun", "spun" },
            { "split", "split" },
            { "spread", "spread" },
            { "stolen", "stole" },
            { "struck", "struck" },
            { "swept", "swept" },
            { "swung", "swung" },
            { "taken", "took" },
            { "taught", "taught" },
            { "torn", "tore" },
            { "told", "told" },
            { "thought", "thought" },
            { "thrown", "threw" },
            { "undergone", "underwent" },
            { "understood", "understood" },
            { "upset", "upset" },
            { "woken", "woke" },
            { "worn", "wore" },
            { "won", "won" },
            { "withdrawn", "withdrew" },
            { "written", "wrote" }
        };

        public static bool EndsIn(this string word, string test) =>
            word.EndsWith(test, StringComparison.OrdinalIgnoreCase);

        public static int CountSyllables(this string word)
        {
            var w = word.ToUpperInvariant();

            var lastWasVowel = false;

            var syllables = 0;

            foreach (var c in w)
            {
                if (Vowels.Contains(c))
                {
                    if (!lastWasVowel)
                    {
                        syllables++;
                    }

                    lastWasVowel = true;
                }
                else
                {
                    lastWasVowel = false;
                }
            }

            if ((w.EndsIn("E") || w.EndsIn("ES") || w.EndsIn("ED")) && !w.EndsIn("LE"))
            {
                syllables--;
            }

            return syllables;
        }

        public static string[] GetParagraphs(this string text) =>
             text.Split(ParagraphSeparators, NoEmptyEntries);

        public static string[] GetSentences(this string text) =>
             text.Split(SentenceSeparators, NoEmptyEntries);

        public static string[] GetWords(this string text) =>
             text.Split(WordSeparators, NoEmptyEntries);

        public static int GetLetterCount(this string text) =>
             text.Count(c => char.IsLetter(c));

        public static int GetCharacterCount(this string text) =>
             text.Count(c => !char.IsControl(c));

        public static ReadabilityStatistics GetReadability(this string text)
        {
            var characters = text.GetCharacterCount();
            var letters = text.GetLetterCount();
            var sentences = text.GetSentences().Length;
            var words = text.GetWords().Length;
            var paragraphs = text.GetParagraphs().Length;

            // var syllables = text.CountSyllables();

            // var (readability, grade) = CalculateFleschKincaidReadingEase(sentences, words, syllables);

            var (readability, grade) = CalculateAutomatedReadabilityIndex(sentences, words, letters);

            return new ReadabilityStatistics(characters, letters, sentences, words, paragraphs, readability, grade);
        }

        public static (double value, AutomatedReadabilityIndexGrade grade) CalculateAutomatedReadabilityIndex(
            int sentenceCount,
            int wordCount,
            int letterCount)
        {
            /*
            https://en.wikipedia.org/wiki/Automated_readability_index

            DO NOT REMOVE EXPLICIT DOUBLE CASTS!

            Visual Studio reports them as "unnecessary", but removing them will break the
            result completely as the calculation is then done with integer values...
            */

            var averageWordsPerSentence = (double)wordCount / sentenceCount;

            var averageCharactersPerWord = (double)letterCount / wordCount;

            var value = (4.71 * averageCharactersPerWord) + (0.5 * averageWordsPerSentence) - 21.43;

            var roundedValue = (int)Math.Round(value);

            return (value, (AutomatedReadabilityIndexGrade)roundedValue);
        }

        public static (double value, FleschKincaidReadingEaseGrade grade) CalculateFleschKincaidReadingEase(
            int sentenceCount,
            int wordCount,
            int syllableCount)
        {
            /*
            https://en.wikipedia.org/wiki/Flesch%E2%80%93Kincaid_readability_tests

            DO NOT REMOVE EXPLICIT DOUBLE CASTS!

            Visual Studio reports them as "unnecessary", but removing them will break the
            result completely as the calculation is then done with integer values...
            */

            var averageWordsPerSentence = (double)wordCount / sentenceCount;

            var averageSyllablesPerWord = (double)syllableCount / wordCount;

            var value = 206.835 - (1.015 * averageWordsPerSentence) - (84.6 * averageSyllablesPerWord);

            // Start at the lowest (worst) grade, then work up
            var grade = value switch {
                < 10 => FleschKincaidReadingEaseGrade.Professional,
                < 30 => FleschKincaidReadingEaseGrade.CollegeGraduate,
                < 50 => FleschKincaidReadingEaseGrade.College,
                < 60 => FleschKincaidReadingEaseGrade.Grade10To12,
                < 70 => FleschKincaidReadingEaseGrade.Grade8To9,
                < 80 => FleschKincaidReadingEaseGrade.Grade7,
                < 90 => FleschKincaidReadingEaseGrade.Grade6,
                _ => FleschKincaidReadingEaseGrade.Grade5
            };

            return (value, grade);
        }
    }
}
