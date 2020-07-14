<Query Kind="Program">
  <Reference Relative="..\src\writer.core\bin\Debug\netstandard2.0\writer.core.dll">C:\Src\writer\src\writer.core\bin\Debug\netstandard2.0\writer.core.dll</Reference>
  <Reference Relative="..\src\writer\bin\Debug\netcoreapp3.1\writer.dll">C:\Src\writer\src\writer\bin\Debug\netcoreapp3.1\writer.dll</Reference>
  <NuGetReference>AvalonEdit</NuGetReference>
  <Namespace>ICSharpCode.AvalonEdit</Namespace>
  <Namespace>ICSharpCode.AvalonEdit.Document</Namespace>
  <Namespace>ICSharpCode.AvalonEdit.Highlighting</Namespace>
  <Namespace>ICSharpCode.AvalonEdit.Highlighting.Xshd</Namespace>
  <Namespace>ICSharpCode.AvalonEdit.Rendering</Namespace>
  <Namespace>static writer.core.Language</Namespace>
  <Namespace>static writer.Language</Namespace>
  <Namespace>System.Windows</Namespace>
  <Namespace>System.Windows.Media</Namespace>
  <Namespace>writer</Namespace>
  <Namespace>writer.core</Namespace>
</Query>

void Main()
{
    var workingDirectory = Path.GetDirectoryName(Util.CurrentQueryPath);

    var editor = new TextEditor();
    
    editor.WordWrap = true;
    editor.Height = 600;
    editor.FontFamily = new FontFamily("Consolas");
    editor.FontSize = 16;

    // editor.TextArea.TextView.BackgroundRenderers.Insert(1, new WriterBackgroundRenderer(editor));
    
    // editor.TextArea.TextView.BackgroundRenderers.Dump();

    editor.TextArea.TextView.LineTransformers.Insert(0, new WriterColorizingTransformer(editor));

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
