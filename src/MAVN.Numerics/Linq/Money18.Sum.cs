using System;
using System.Collections.Generic;
using System.Linq;
using static MAVN.Numerics.Utils.ThrowHelper;

namespace MAVN.Numerics.Linq
{
    public static partial class LykkeEnumerable
    {
        public static Money18 Sum(
            this IEnumerable<Money18> source)
        {
            if (source == null)
            {
                throw SourceArgumentNullException();
            }

            var sum = (Money18) 0;

            return source
                .Aggregate(sum, (a, b) => a + b);
        }

        public static Money18? Sum(
            this IEnumerable<Money18?> source)
        {
            if (source == null)
            {
                throw SourceArgumentNullException();
            }

            var sum = (Money18) 0;
            
            return source
                .Where(x => x.HasValue)
                .Aggregate(sum, (a, b) => a + b.Value);
        }
        
        public static Money18 Sum<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, Money18> selector)
        {
            return source
                .Select(selector)
                .Sum();
        }

        public static Money18? Sum<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, Money18?> selector)
        {
            return source
                .Select(selector)
                .Sum();
        }
    }
}