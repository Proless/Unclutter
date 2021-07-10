using System;
using System.Collections.Generic;

namespace Unclutter.SDK.Common
{
    public class OrderedObjectComparer : IComparer<double?>
    {
        public int Compare(double? x, double? y)
        {
            return Nullable.Compare(x, y);
        }
    }
}
