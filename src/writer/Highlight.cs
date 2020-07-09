using System.Windows.Media;
using ICSharpCode.AvalonEdit.Document;

namespace writer
{
    public class Highlight : TextSegment
    {
        public Highlight(int startOffset, int endOffset, Brush brush)
        {
            StartOffset = startOffset;
            EndOffset = endOffset;

            Brush = brush;
        }

        public Brush Brush { get; }
    }
}
