using System.Collections.Generic;
using System.Linq;
using Unclutter.SDK.Common;

namespace Unclutter.SDK.Services
{
    public static class OrderHelper
    {
        public static IEnumerable<T> GetOrdered<T>(IEnumerable<T> objects) where T : IOrderedObject
        {
            var items = objects.ToArray();
            var notOrdered = items.Where(i => !i.Order.HasValue);
            var ordered = items.Where(i => i.Order.HasValue).OrderBy(o => o.Order, new OrderedObjectComparer());
            return ordered.Concat(notOrdered);
        }
        public static IEnumerable<T> GetOrderedDescending<T>(IEnumerable<T> objects) where T : IOrderedObject
        {
            var items = objects.ToArray();
            var notOrdered = items.Where(i => !i.Order.HasValue);
            var ordered = items.Where(i => i.Order.HasValue).OrderByDescending(o => o.Order, new OrderedObjectComparer());
            return notOrdered.Concat(ordered);
        }
    }
}
