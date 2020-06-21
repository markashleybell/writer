<Query Kind="Program">
  <Reference Relative="..\src\writer.core\bin\Debug\netstandard2.0\writer.core.dll">C:\Src\writer\src\writer.core\bin\Debug\netstandard2.0\writer.core.dll</Reference>
  <Namespace>writer.core</Namespace>
</Query>

void Main()
{
    var testWords = new[] {
        "one",
        "apples",
        "oranges",
        "super",
        "chicken",
        "antediluvian",
        "multidimensional"
    };

    // testWords.Select(w => (w, w.CountSyllables())).Dump();

    var text = @"
Hemingway App makes your writing bold and clear.

The app highlights lengthy, complex sentences and common errors; if you see a yellow sentence, shorten or split it. If you see a red highlight, your sentence is so dense and complicated that your readers will get lost trying to follow its meandering, splitting logic — try editing this sentence to remove the red.

You can utilize a shorter word in place of a purple one. Mouse over them for hints.

Adverbs and weakening phrases are helpfully shown in blue. Get rid of them and pick words with force, perhaps.

Phrases in green have been marked to show passive voice.

You can format your text with the toolbar.

Paste in something you're working on and edit away. Or, click the Write button and compose something new.";


    // text.GetReadability().Dump();
    
    // Language.CalculateFleschKincaidReadingEase(11, 133, text.CountSyllables()).Dump();
    
    var s1 = "The app highlights lengthy, complex sentences and common errors; if you see a yellow sentence, shorten or split it.";
    var s2 = "The app highlights lengthy, complex sentences and common errors.";
    
    
    s1.GetReadability().Dump();
    s2.GetReadability().Dump();
}

public static class Extensions 
{

}