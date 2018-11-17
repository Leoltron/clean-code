using System;
using Markdown.Rules;

namespace Markdown
{
    public class Mark
    {
        public IMdRule RelatedRule { get; }
        public int Start { get; private set; }
        public int End { get; private set; }
        public MarkType Type { get; }

        public int Length => End - Start;

        public Mark(IMdRule relatedRule, int start, int end, MarkType type)
        {
            RelatedRule = relatedRule ?? throw new ArgumentNullException(nameof(relatedRule));
            Start = start;
            End = end;
            Type = type;
        }

        public bool RelatedToTheSameRule(Mark other)
        {
            return ReferenceEquals(RelatedRule, other.RelatedRule);
        }

        public void Move(int value)
        {
            Start += value;
            End += value;
        }

        public override string ToString()
        {
            return $"[{Type}] ({Start}->{End}) {RelatedRule.Name}";
        }
    }
}
