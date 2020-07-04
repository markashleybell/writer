using System.Windows.Media;
using ICSharpCode.AvalonEdit.Document;

namespace writer
{
    public class Highlight : TextSegment
    {
        public Brush Brush { get; }

        public Highlight(int startOffset, int endOffset, Brush brush)
        {
            StartOffset = startOffset;
            EndOffset = endOffset;

            Brush = brush;
        }
    }
}
