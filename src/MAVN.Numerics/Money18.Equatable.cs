using System;
using JetBrains.Annotations;

namespace MAVN.Numerics
{
    public partial struct Money18 : IEquatable<Money18>
    {
        [Pure]
        public override bool Equals(
            object obj)
        {
            switch (obj)
            {
                case null:
                    return false;
                case Money18 other:
                    return Equals(other);
                default:
                    return false;
            }
        }

        /// <inheritdoc cref="IEquatable{Money}" />
        [Pure]
        public bool Equals(
            Money18 other)
        {
            return CompareTo(other) == 0;
        }

        #region Operators
        
        // Money
        
        public static bool operator ==(Money18 left, Money18 right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Money18 left, Money18 right)
        {
            return !left.Equals(right);
        }
        
        #endregion
    }
}