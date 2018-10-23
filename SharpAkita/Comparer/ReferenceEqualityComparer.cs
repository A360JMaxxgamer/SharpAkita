// Extension method for deep cloning. Found on https://github.com/Burtsev-Alexey/net-object-deep-copy

using System.Collections.Generic;

namespace System
{
    public class ReferenceEqualityComparer : EqualityComparer<object>
    {
        public override bool Equals(object x, object y)
        {
            return ReferenceEquals(x, y);
        }
        public override int GetHashCode(object obj)
        {
            if (obj == null) return 0;
            return obj.GetHashCode();
        }
    }
}
