using System;
using System.Numerics;
using JetBrains.Annotations;

namespace MAVN.Numerics
{
    public partial struct Money18
    {
        private const MidpointRounding DefaultRoundingMode = MidpointRounding.ToEven;
        
        /// <summary>
        ///    Gets the absolute value of a Money object.
        /// </summary>
        /// <param name="value">
        ///    A value to get absolute value of.
        /// </param>
        /// <returns>
        ///    The absolute value of value parameter.
        /// </returns>
        [Pure]
        public static Money18 Abs(
            Money18 value)
        {
            if (value < 0)
            {
                return -value;
            }
            else
            {
                return value;
            }
        }

        /// <summary>
        ///    Adds two Money values and returns the result.
        /// </summary>
        /// <param name="left">
        ///    The first value to add.
        /// </param>
        /// <param name="right">
        ///    The second value to add.
        /// </param>
        /// <returns>
        ///    The sum of left and right. The sum's scale equals to the left or right scale, whichever is larger. 
        /// </returns>
        [Pure]
        public static Money18 Add(
            Money18 left,
            Money18 right)
        {
            var (leftSignificand, rightSignificand) = EqualizeSignificands(left, right);
            var sumSignificand = leftSignificand + rightSignificand;
            var sumScale = Math.Max(left._scale, right._scale);

            return new Money18(sumSignificand, sumScale);
        }

        /// <summary>
        ///    Returns the smallest integral value that is greater than or equal to the specified decimal number.
        /// </summary>
        /// <param name="value">
        ///    A value to ceil.
        /// </param>
        /// <returns>
        ///    The smallest integral value that is greater than or equal to the value parameter. The scale is preserved.
        /// </returns>
        [Pure]
        public static Money18 Ceiling(
            Money18 value)
        {
            if (value == 0)
            {
                return value;
            }
            
            var significand = BigInteger.DivRem
            (
                dividend: value._significand,
                divisor: Pow10(value._scale),
                remainder: out var remainder
            );

            if (remainder != 0 && value > 0)
            {
                significand++;
            }

            significand *= Pow10(value._scale);
            
            return new Money18(significand, value._scale);
        }

        /// <summary>
        ///    Divides one Money value by another and returns the result.
        /// </summary>
        /// <param name="left">
        ///    A value to be divided.
        /// </param>
        /// <param name="right">
        ///    A value to divide by.
        /// </param>
        /// <returns>
        ///    The quotient of the division. The quotient's scale equals to the left or right scale, whichever is larger.
        /// </returns>
        /// <exception cref="DivideByZeroException">
        ///    Divisor is zero.
        /// </exception>
        /// <remarks>
        ///    Result will be rounded to the scale of either left scale, or right scale, whichever is larger.
        ///    For example, 1 / 2 = 0, 1 / 3 = 0, 1.0 / 3 = 0.3, 1.0 / 4.00 = 0.25
        /// </remarks>
        [Pure]
        public static Money18 Divide(
            Money18 left,
            Money18 right)
        {   
            if (right == 0)
            {
                throw new DivideByZeroException();
            }

            var (leftSignificand, rightSignificand) = EqualizeSignificands(left, right);
            var quotientScale = Math.Max(left._scale, right._scale);

            var quotientSignificand = RoundSignificand
            (
                significand: leftSignificand * Pow10(quotientScale * 2) / rightSignificand, 
                significandScale: quotientScale * 2,
                scale: quotientScale,
                mode: DefaultRoundingMode
            ); 
            
            return new Money18(quotientSignificand, quotientScale);
        }

        /// <summary>
        ///    Rounds a specified Money number to the closest integer toward negative infinity.
        /// </summary>
        /// <param name="value">
        ///    A value to floor.
        /// </param>
        /// <returns>
        ///    The smallest integral value that is lower than or equal to the value parameter. The scale is preserved.
        /// </returns>
        [Pure]
        public static Money18 Floor(
            Money18 value)
        {
            if (value == 0)
            {
                return value;
            }
            
            var significand = BigInteger.DivRem
            (
                dividend: value._significand,
                divisor: Pow10(value._scale),
                remainder: out var remainder
            );

            if (remainder != 0 && value < 0)
            {
                significand--;
            }

            significand *= Pow10(value._scale);
            
            return new Money18(significand, value._scale);
        }

        /// <summary>
        ///    Multiplies two specified Money values.
        /// </summary>
        /// <param name="left">
        ///    The multiplicand.
        /// </param>
        /// <param name="right">
        ///    The multiplier.
        /// </param>
        /// <returns>
        ///    The result of multiplying left and right.
        /// </returns>
        /// <remarks>
        ///    Result will be rounded to the scale of either left scale, or right scale, whichever is larger.
        ///    For example, 0.5 * 0.5 = 0.2, 1.1 * 1.1 = 1.2, 1.10 * 1.1 = 1.21
        /// </remarks>
        [Pure]
        public static Money18 Multiply(
            Money18 left,
            Money18 right)
        {
            var (leftSignificand, rightSignificand) = EqualizeSignificands(left, right);
            var productScale = Math.Max(left._scale, right._scale);
            var productSignificand = RoundSignificand
            (
                significand: leftSignificand * rightSignificand, 
                significandScale: productScale * 2,
                scale: productScale,
                mode: DefaultRoundingMode
            );
            
            return new Money18(productSignificand, productScale);
        }

        /// <summary>
        ///    Negates a specified Money value.
        /// </summary>
        /// <param name="value">
        ///    A value to negate.
        /// </param>
        /// <returns>
        ///    The result of the value parameter multiplied by negative one. The scale is preserved.
        /// </returns>
        [Pure]
        public static Money18 Negate(
            Money18 value)
        {
            return new Money18(-value._significand, value._scale);
        }

        /// <summary>
        ///    Rounds a value to the nearest integer.
        /// </summary>
        /// <param name="value">
        ///    A value to round.
        /// </param>
        /// <returns>
        ///    The integer that is nearest to the value parameter. If value is halfway between two integers,
        ///    one of which is even and the other odd, the even number is returned.
        /// </returns>
        [Pure]
        public static Money18 Round(
            Money18 value)
        {
            return Round(value, 0, DefaultRoundingMode);
        }

        /// <summary>
        ///    Rounds a Money value to the nearest integer. A parameter specifies how to round the value if it
        ///    is midway between two other numbers.
        /// </summary>
        /// <param name="value">
        ///    A value to round.
        /// </param>
        /// <param name="mode">
        ///    A value that specifies how to round value if it is midway between two other numbers.
        /// </param>
        /// <returns>
        ///    The integer that is nearest to the value parameter. If value is halfway between two numbers, one
        ///    of which is even and the other odd, the mode parameter determines which of the two numbers is returned.
        /// </returns>
        [Pure]
        public static Money18 Round(
            Money18 value,
            MidpointRounding mode)
        {
            return Round(value, 0, mode);
        }

        /// <summary>
        ///    Rounds a value to a specified number of decimal places.
        /// </summary>
        /// <param name="value">
        ///    A value to round.
        /// </param>
        /// <param name="scale">
        ///    A value that specifies the number of decimal places to round to.
        /// </param>
        /// <returns>
        ///    The number equivalent to value rounded to decimals number of decimal places.
        /// </returns>
        [Pure]
        public static Money18 Round(
            Money18 value,
            int scale)
        {
            return Round(value, scale, DefaultRoundingMode);
        }

        /// <summary>
        ///    Rounds a value to a specified number of decimal places. A parameter specifies how to round the value
        ///    if it is midway between two other numbers.
        /// </summary>
        /// <param name="value">
        ///    A value to round.
        /// </param>
        /// <param name="scale">
        ///    A value that specifies the number of decimal places to round to.
        /// </param>
        /// <param name="mode">
        ///    A value that specifies how to round value if it is midway between two other numbers.
        /// </param>
        /// <returns>
        ///    The number equivalent to value rounded to decimals number of decimal places. If value is halfway between
        ///    two numbers, one of which is even and the other odd, the mode parameter determines which of the two
        ///    numbers is returned.
        /// </returns>
        [Pure]
        public static Money18 Round(
            Money18 value,
            int scale,
            MidpointRounding mode)
        {
            var significand = RoundSignificand(value._significand, value._scale, scale, mode);
            
            return new Money18(significand, scale);
        }

        /// <summary>
        ///    Subtracts one specified Money value from another.
        /// </summary>
        /// <param name="left">
        ///    The minuend.
        /// </param>
        /// <param name="right">
        ///    The subtrahend.
        /// </param>
        /// <returns>
        ///    The result of subtracting right from left.
        /// </returns>
        [Pure]
        public static Money18 Subtract(
            Money18 left,
            Money18 right)
        {
            var (leftSignificand, rightSignificand) = EqualizeSignificands(left, right);
            var diffSignificand = leftSignificand - rightSignificand;
            var diffScale = Math.Max(left._scale, right._scale);

            return new Money18(diffSignificand, diffScale);
        }

        /// <summary>
        ///    Returns the integral digits of the specified Money. Any fractional digits are discarded.
        /// </summary>
        /// <param name="value">
        ///    A value to truncate.
        /// </param>
        /// <returns>
        ///    The result of value rounded toward zero, to the nearest whole number. The scale is not preserved.
        /// </returns>
        [Pure]
        public static Money18 Truncate(
            Money18 value)
        {
            var significand = value._significand / Pow10(value._scale);
            
            return new Money18(significand, 0);
        }

        private static BigInteger RoundSignificand(
            BigInteger significand,
            int significandScale,
            int scale,
            MidpointRounding mode)
        {
            if (scale < 0)
            {
                throw new ArgumentException("Should be greater or equal to zero.", nameof(scale));
            }
            
            if (significandScale <= scale)
            {
                return significand * Pow10(scale - significandScale);
            }
            else
            {
                var scaleDiff = significandScale - scale;
                
                significand = BigInteger.DivRem
                (
                    dividend: significand,
                    divisor: Pow10(scaleDiff),
                    remainder: out var remainder
                );

                remainder = BigInteger.Abs(remainder);
                
                var midpoint = 5 * Pow10(scaleDiff - 1);

                if (remainder > midpoint)
                {
                    if (significand > 0)
                    {
                        return significand + 1;
                    }
                    else
                    {
                        return significand - 1;
                    }
                }

                if (remainder < midpoint)
                {
                    return significand;
                }
                
                switch (mode)
                {
                    case MidpointRounding.AwayFromZero:
                        if (significand > 0)
                        {
                            return significand + 1;
                        }
                        else
                        {
                            return significand - 1;
                        }
                    
                    case MidpointRounding.ToEven:
                        if (significand % 2 == 0)
                        {
                            return significand;
                        }
                        else
                        {
                            if (significand > 0)
                            {
                                return significand + 1;
                            }
                            else
                            {
                                return significand - 1;
                            }
                        }
                        
                    default:
                        throw new ArgumentOutOfRangeException(nameof(mode), mode, "Rounding mode is not supported.");
                }
            }
        }

        #region Operators
        
        public static Money18 operator +(Money18 value)
        {
            return value;
        }

        public static Money18 operator -(Money18 value)
        {
            return Negate(value);
        }
        
        public static Money18 operator ++(Money18 value)
        {
            return value + 1;
        }

        public static Money18 operator --(Money18 value)
        {
            return value - 1;
        }

        public static Money18 operator +(Money18 left, Money18 right)
        {
            return Add(left, right);
        }
        
        public static Money18 operator -(Money18 left, Money18 right)
        {
            return Subtract(left, right);
        }
        
        public static Money18 operator *(Money18 left, Money18 right)
        {
            return Multiply(left, right);
        }
        
        public static Money18 operator /(Money18 left, Money18 right)
        {
            return Divide(left, right);
        }
        
        #endregion
    }
}