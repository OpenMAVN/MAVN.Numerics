using System;

namespace MAVN.Numerics.Utils
{
    internal static class ThrowHelper
    {
        public static ArgumentNullException SourceArgumentNullException()
        {
            // ReSharper disable once NotResolvedInText
            return new ArgumentNullException("source");
        }

        public static InvalidOperationException SequenceContainsNoElementsException()
        {
            return new InvalidOperationException("Sequence contains no elements");
        }
    }
}