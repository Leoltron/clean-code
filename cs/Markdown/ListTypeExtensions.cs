using System;

namespace Markdown
{
    public static class ListTypeExtensions
    {
        public static bool DefaultAcceptance(this ListType listType)
        {
            switch (listType)
            {
                case ListType.BlackList:
                    return true;
                case ListType.WhiteList:
                    return false;
                default:
                    throw new ArgumentOutOfRangeException(nameof(listType), listType, null);
            }
        }

        public static bool DoesAcceptIfInList(this ListType listType)
        {
            switch (listType)
            {
                case ListType.BlackList:
                    return false;
                case ListType.WhiteList:
                    return true;
                default:
                    throw new ArgumentOutOfRangeException(nameof(listType), listType, null);
            }
        }
    }
}
