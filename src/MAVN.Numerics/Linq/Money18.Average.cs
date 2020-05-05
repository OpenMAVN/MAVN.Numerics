using System;
using System.Collections.Generic;
using System.Linq;
using static MAVN.Numerics.Utils.ThrowHelper;

namespace MAVN.Numerics.Linq
{
    public static partial class LykkeEnumerable
    {
        public static Money18 Average(
            this IEnumerable<Money18> source)
        {
            if (source == null)
            {
                throw SourceArgumentNullException();
            }

            using (var enumerator = source.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                {
                    throw SequenceContainsNoElementsException();
                }

                var sum = enumerator.Current;
                var count = 1UL;
                
                checked
                {
                    while (enumerator.MoveNext())
                    {
                        sum += enumerator.Current;
                        count++;
                    }
                }

                return sum / count;
            }
        }

        public static Money18? Average(
            this IEnumerable<Money18?> source)
        {
            if (source == null)
            {
                throw SourceArgumentNullException();
            }

            using (var enumerator = source.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    var current = enumerator.Current;
                    
                    if (current.HasValue)
                    {
                        var sum = current.Value;
                        var count = 1UL;
                        
                        checked
                        {
                            while (enumerator.MoveNext())
                            {
                                current = enumerator.Current;
                                
                                if (current.HasValue)
                                {
                                    sum += current.Value;
                                    count++;
                                }
                            }
                        }

                        return sum / count;
                    }
                }
            }

            return null;
        }
        
        public static Money18 Average<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, Money18> selector)
        {
            return source
                .Select(selector)
                .Average();
        }

        public static Money18? Average<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, Money18?> selector)
        {
            return source
                .Select(selector)
                .Average();
        }
    }
}