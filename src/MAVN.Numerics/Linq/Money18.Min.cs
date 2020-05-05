using System;
using System.Collections.Generic;
using System.Linq;
using static MAVN.Numerics.Utils.ThrowHelper;

namespace MAVN.Numerics.Linq
{
    public static partial class LykkeEnumerable
    {
        public static Money18 Min(
            this IEnumerable<Money18> source)
        {
            if (source == null)
            {
                throw SourceArgumentNullException();
            }

            Money18 result;
            
            using (var enumerator = source.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                {
                    throw SequenceContainsNoElementsException();
                }

                result = enumerator.Current;
                
                while (enumerator.MoveNext())
                {
                    var current = enumerator.Current;
                    
                    if (current < result)
                    {
                        result = current;
                    }
                }
            }

            return result;
        }

        public static Money18? Min(
            this IEnumerable<Money18?> source)
        {
            if (source == null)
            {
                throw SourceArgumentNullException();
            }

            Money18? result = null;
            
            using (var enumerator = source.GetEnumerator())
            {
                do
                {
                    if (!enumerator.MoveNext())
                    {
                        return result;
                    }

                    result = enumerator.Current;
                }
                while (!result.HasValue);

                var resultValue = result.GetValueOrDefault();
                
                while (enumerator.MoveNext())
                {
                    var current = enumerator.Current;
                    var currentValue = current.GetValueOrDefault();
                    
                    if (current.HasValue && currentValue < resultValue)
                    {
                        resultValue = currentValue;
                        result = current;
                    }
                }
            }

            return result;
        }
        
        public static Money18 Min<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, Money18> selector)
        {
            return source
                .Select(selector)
                .Min();
        }

        public static Money18? Min<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, Money18?> selector)
        {
            return source
                .Select(selector)
                .Min();
        }
    }
}