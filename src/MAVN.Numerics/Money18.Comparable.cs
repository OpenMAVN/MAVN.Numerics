using System;
using JetBrains.Annotations;

namespace MAVN.Numerics
{
    public partial struct Money18 : IComparable, IComparable<Money18>
    {
        /// <summary>
        ///    Compares this instance to a specified object.
        /// </summary>
        /// <param name="obj">
        ///    The object to compare.
        /// </param>
        /// <returns>
        ///    Returns an integer that indicates whether the value of this instance is less than, equal to,
        ///    or greater than the value of the specified object. Less than zero if the current instance is less than obj.
        ///    Zero if the current instance equals obj. Greater than zero if the current instance is greater than obj,
        ///    or the obj parameter is null.
        /// </returns>
        /// <exception cref="ArgumentException">
        ///    obj is not Money.
        /// </exception>
        [Pure]
        public int CompareTo(
            object obj)
        {
            switch (obj)
            {
                case null:
                    return 1;
                case Money18 other:
                    return CompareTo(other);
                default:
                    throw new ArgumentException($"Object is not a {nameof(Money18)}");
            }
        }

        /// <summary>
        ///    Compares this instance to a second Money.
        /// </summary>
        /// <param name="other">
        ///    The object to compare.
        /// </param>
        /// <returns>
        ///    Returns an integer that indicates whether the value of this instance is less than, equal to, or greater
        ///    than the value of the specified object. Less than zero if the current instance is less than other.
        ///    Zero if the current instance equals other. Greater than zero if the current instance is greater than other.
        /// </returns>
        [Pure]
        public int CompareTo(
            Money18 other)
        {
            if (_scale == other._scale)
            {
                return _significand.CompareTo(other._significand);
            }
            else
            {
                var (left, right) = EqualizeSignificands(this, other);

                return left.CompareTo(right);
            }
        }
        
        /// <summary>
        ///    Returns the larger of two Money values.
        /// </summary>
        /// <param name="left">
        ///    The first value to compare.
        /// </param>
        /// <param name="right">
        ///    The second value to compare.
        /// </param>
        /// <returns>
        ///    The left or right parameter, whichever is larger.
        /// </returns>
        [Pure]
        public static Money18 Max(
            Money18 left,
            Money18 right)
        {
            return left.CompareTo(right) < 0 ? right : left;
        }

        /// <summary>
        ///    Returns the smaller of two Money values.
        /// </summary>
        /// <param name="left">
        ///    The first value to compare.
        /// </param>
        /// <param name="right">
        ///    The second value to compare.
        /// </param>
        /// <returns>
        ///    The left or right parameter, whichever is smaller.
        /// </returns>
        [Pure]
        public static Money18 Min(
            Money18 left,
            Money18 right)
        {
            return left.CompareTo(right) <= 0 ? left : right;
        }

        #region Operators
        
        public static bool operator <(Money18 left, Money18 right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator <=(Money18 left, Money18 right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >(Money18 left, Money18 right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator >=(Money18 left, Money18 right)
        {
            return left.CompareTo(right) >= 0;
        }

        #endregion
    }
}