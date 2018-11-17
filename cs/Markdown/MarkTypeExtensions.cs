using System;

namespace Markdown
{
    public static class MarkTypeExtensions
    {
        public static bool IsStart(this MarkType markType)
        {
            switch (markType)
            {
                case MarkType.Start:
                case MarkType.StartOrEnd:
                    return true;
                case MarkType.End:
                case MarkType.None:
                    return false;
                default:
                    throw new ArgumentOutOfRangeException(nameof(markType), markType, null);
            }
        }

        public static bool IsEnd(this MarkType markType)
        {
            switch (markType)
            {
                case MarkType.End:
                case MarkType.StartOrEnd:
                    return true;
                case MarkType.Start:
                case MarkType.None:
                    return false;
                default:
                    throw new ArgumentOutOfRangeException(nameof(markType), markType, null);
            }
        }
    }
}
