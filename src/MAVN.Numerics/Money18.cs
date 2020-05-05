using System;
using System.ComponentModel;
using System.Globalization;
using System.Numerics;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using MAVN.Numerics.TypeConverters;
using MAVN.Numerics.Utils;

namespace MAVN.Numerics
{
	[ImmutableObject(true)]
    [Serializable, PublicAPI]
	[TypeConverter(typeof(Money18TypeConverter))]
    public readonly partial struct Money18 : IFormattable
    {
	    private static readonly Regex MoneyFormat = new Regex(@"^[+-]?\d+\.?\d*$", RegexOptions.Compiled);
	    
	    private readonly int _precision;
	    private readonly int _scale;
	    private readonly BigInteger _significand;
	    private readonly int _trailingZeroesCount;

	    
        public Money18(
	        BigInteger significand,
	        int scale)
		{
			if (scale < 0)
			{
				throw new ArgumentException("Should be greater or equal to zero.", nameof(scale));
			}

			if (scale < 18)
			{
				significand = significand * Pow10(18 - scale);
			}
			
			if (scale > 18)
			{
				significand = BigInteger.DivRem
				(
					dividend: significand,
					divisor: Pow10(scale - 18),
					remainder: out _
				);
			}

			_scale = 18;
			_significand = significand;
			(_precision, _trailingZeroesCount) = CalculatePrecisionAndTrailingZeroesCount(_significand, _scale);
		}


        // ReSharper disable ConvertToAutoPropertyWithPrivateSetter
        
        public BigInteger Significand
	        => _significand;

        public int Scale
	        => _scale;
        
        // ReSharper restore ConvertToAutoPropertyWithPrivateSetter
        
        
        [Pure]
        public override int GetHashCode()
        {
	        var effectiveScale = _scale - _trailingZeroesCount;
	        var trimmedSignificand = _trailingZeroesCount != 0 ? _significand / (10 * _trailingZeroesCount) : _significand;
	        
	        unchecked
	        {
		        return (effectiveScale * 397) ^ trimmedSignificand.GetHashCode();
	        }
        }

        [Pure]
        public BigInteger ToAtto()
        {
	        return Significand;
        }
        
        [Pure]
        public override string ToString()
        {
            return ToString(
                "R",
                _scale,
                CultureInfo.InvariantCulture.NumberFormat);
        }

        //NOTE this is used by Refit for query parameter value generation in URL
        public string ToString(string format, IFormatProvider formatProvider)
        {
            //TODO implement proper support of different formats, like 0.000
            return ToString();
        }

        public string ToString(string intergerPartFormat, NumberFormatInfo nfi = null)
        {
            return ToString(
                intergerPartFormat,
                _scale,
                nfi);
        }

        public string ToString(
            string intergerPartFormat,
            int decimalPlacesCount,
            NumberFormatInfo nfi = null)
        {
            if (string.IsNullOrWhiteSpace(intergerPartFormat))
                intergerPartFormat = "R";

            if (!intergerPartFormat.StartsWith("D") && !intergerPartFormat.StartsWith("d")
            && !intergerPartFormat.StartsWith("G") && !intergerPartFormat.StartsWith("g")
            && !intergerPartFormat.StartsWith("R") && !intergerPartFormat.StartsWith("r")
            && intergerPartFormat != "N0" && intergerPartFormat != "n0")
                throw new NotSupportedException($"Integer part format '{intergerPartFormat}' is not supported");

            if (nfi == null)
                nfi = CultureInfo.InvariantCulture.NumberFormat;

            var s = BigInteger.Abs(_significand).ToString("R");
            s = s.PadLeft(_scale + 1, '0');

            var integerPart = BigInteger.Parse(s.Substring(0, s.Length - _scale));
            var decimalStr = s.Substring(s.Length - _scale, decimalPlacesCount);
            if (decimalPlacesCount == 18 && decimalStr.EndsWith("0"))
                decimalStr = decimalStr.TrimEnd('0');
            if (decimalStr.Length > 0)
                decimalStr = nfi.NumberDecimalSeparator + decimalStr;

            s = $"{integerPart.ToString(intergerPartFormat)}{decimalStr}";

            if (_significand.Sign < 0)
            {
                s = nfi.NegativeSign + s;
            }

            return s;
        }

        /// <summary>
        ///    Converts the decimal to its Money equivalent.
        /// </summary>
        /// <param name="value">
        ///    A decimal value to convert.
        /// </param>
        /// <returns>
        ///    A value that is equivalent to the number specified in the value parameter.
        /// </returns>
        [Pure]
        public static Money18 Create(
	        decimal value)
        {
	        var (significand, scale) = value.GetSignificandAndScale();
	        
	        return new Money18(significand, scale);
        }
        
        /// <summary>
        ///    Converts the decimal to its Money equivalent, rounded according to the specified accuracy.
        /// </summary>
        /// <param name="value">
        ///    A decimal value to convert.
        /// </param>
        /// <param name="accuracy">
        ///    An accuracy of target value.
        /// </param> 
        /// <returns>
        ///    A value that is equivalent to the number specified in the value parameter, rounded according to
        ///    the specified accuracy.
        /// </returns>
        [Pure]
        public static Money18 Create(
	        decimal value,
	        int accuracy)
        {
	        var (significand, scale) = value.GetSignificandAndScale();
	        var money = new Money18(significand, scale);

	        return scale == accuracy ? money : Round(money, accuracy);
        }

        /// <summary>
        ///    Converts the BigInteger to its Money equivalent.
        /// </summary>
        /// <param name="value">
        ///    A BigInteger value to convert.
        /// </param>
        /// <param name="accuracy">
        ///    An accuracy of target value.
        /// </param> 
        /// <returns>
        ///    A value that is equivalent to the number specified in the value parameter with the specified accuracy.
        /// </returns>
        [Pure]
        public static Money18 Create(
	        BigInteger value,
	        int accuracy)
        {
	        return new Money18(value * Pow10(accuracy), accuracy);
        }
        
        /// <summary>
        ///    Converts the byte to its Money equivalent.
        /// </summary>
        /// <param name="value">
        ///    A byte value to convert.
        /// </param>
        /// <param name="accuracy">
        ///    An accuracy of target value.
        /// </param> 
        /// <returns>
        ///    A value that is equivalent to the number specified in the value parameter with the specified accuracy.
        /// </returns>
        [Pure]
        public static Money18 Create(
	        byte value,
	        int accuracy)
        {
	        return Create((BigInteger) value, accuracy);
        }
        
        /// <summary>
        ///    Converts the sbyte to its Money equivalent.
        /// </summary>
        /// <param name="value">
        ///    A sbyte value to convert.
        /// </param>
        /// <param name="accuracy">
        ///    An accuracy of target value.
        /// </param> 
        /// <returns>
        ///    A value that is equivalent to the number specified in the value parameter with the specified accuracy.
        /// </returns>
        [Pure]
        public static Money18 Create(
	        sbyte value,
	        int accuracy)
        {
	        return Create((BigInteger) value, accuracy);
        }
        
        /// <summary>
        ///    Converts the short to its Money equivalent.
        /// </summary>
        /// <param name="value">
        ///    A short value to convert.
        /// </param>
        /// <param name="accuracy">
        ///    An accuracy of target value.
        /// </param> 
        /// <returns>
        ///    A value that is equivalent to the number specified in the value parameter with the specified accuracy.
        /// </returns>
        [Pure]
        public static Money18 Create(
	        short value,
	        int accuracy)
        {
	        return Create((BigInteger) value, accuracy);
        }
        
        /// <summary>
        ///    Converts the ushort to its Money equivalent.
        /// </summary>
        /// <param name="value">
        ///    A ushort value to convert.
        /// </param>
        /// <param name="accuracy">
        ///    An accuracy of target value.
        /// </param> 
        /// <returns>
        ///    A value that is equivalent to the number specified in the value parameter with the specified accuracy.
        /// </returns>
        [Pure]
        public static Money18 Create(
	        ushort value,
	        int accuracy)
        {
	        return Create((BigInteger) value, accuracy);
        }
        
        /// <summary>
        ///    Converts the int to its Money equivalent.
        /// </summary>
        /// <param name="value">
        ///    A int value to convert.
        /// </param>
        /// <param name="accuracy">
        ///    An accuracy of target value.
        /// </param> 
        /// <returns>
        ///    A value that is equivalent to the number specified in the value parameter with the specified accuracy.
        /// </returns>
        [Pure]
        public static Money18 Create(
	        int value,
	        int accuracy)
        {
	        return Create((BigInteger) value, accuracy);
        }
        
        /// <summary>
        ///    Converts the uint to its Money equivalent.
        /// </summary>
        /// <param name="value">
        ///    A uint value to convert.
        /// </param>
        /// <param name="accuracy">
        ///    An accuracy of target value.
        /// </param> 
        /// <returns>
        ///    A value that is equivalent to the number specified in the value parameter with the specified accuracy.
        /// </returns>
        [Pure]
        public static Money18 Create(
	        uint value,
	        int accuracy)
        {
	        return Create((BigInteger) value, accuracy);
        }
        
        /// <summary>
        ///    Converts the long to its Money equivalent.
        /// </summary>
        /// <param name="value">
        ///    A long value to convert.
        /// </param>
        /// <param name="accuracy">
        ///    An accuracy of target value.
        /// </param> 
        /// <returns>
        ///    A value that is equivalent to the number specified in the value parameter with the specified accuracy.
        /// </returns>
        [Pure]
        public static Money18 Create(
	        long value,
	        int accuracy)
        {
	        return Create((BigInteger) value, accuracy);
        }
        
        /// <summary>
        ///    Converts the ulong to its Money equivalent.
        /// </summary>
        /// <param name="value">
        ///    A ulong value to convert.
        /// </param>
        /// <param name="accuracy">
        ///    An accuracy of target value.
        /// </param> 
        /// <returns>
        ///    A value that is equivalent to the number specified in the value parameter with the specified accuracy.
        /// </returns>
        [Pure]
        public static Money18 Create(
	        ulong value,
	        int accuracy)
        {
	        return Create((BigInteger) value, accuracy);
        }

        /// <summary>
        ///    Converts specified amount of attos (1*10^-18) to its Money18 equivalent.
        /// </summary>
        public static Money18 CreateFromAtto(
	        BigInteger value)
        {
	        return new Money18(value, 18);
        }
        
        /// <summary>
        ///    Converts the string representation of a number to its Money equivalent.
        /// </summary>
        /// <param name="value">
        ///    A string that contains the number to convert.
        /// </param>
        /// <returns>
        ///    A value that is equivalent to the number specified in the value parameter.
        /// </returns>
        /// <exception cref="FormatException">
        ///    The value parameter is not in the correct format.
        /// </exception>
        [Pure]
        public static Money18 Parse(
	        string value)
        {
            if (!TryParse(value, out var result))
            {
                throw new FormatException($"Specified value [{value}] does not match Money format [{MoneyFormat}]");
            }

            return result;
        }

        /// <summary>
        ///    Tries to convert the string representation of a number to its Money equivalent,
        ///    and returns a value that indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="value">
        ///    The string representation of a number.
        /// </param>
        /// <param name="result">
        ///    When this method returns, contains the Money equivalent to the number that is contained in value,
        ///    or zero if the conversion fails. The conversion fails if the value parameter is null or is not
        ///    of the correct format. This parameter is passed uninitialized.
        /// </param>
        /// <returns>
        ///    true if value was converted successfully. Otherwise, false.
        /// </returns>
        [Pure]
        public static bool TryParse(
	        string value,
	        out Money18 result)
        {
	        if (value != null && MoneyFormat.IsMatch(value))
	        {
		        var decimalAndFractionalParts = value.Split('.');

		        // ReSharper disable once SwitchStatementMissingSomeCases
		        switch (decimalAndFractionalParts.Length)
		        {
			        case 1:
			        {
				        var significand = BigInteger.Parse(value);
		        
				        result = new Money18(significand, 0);

				        return true;
			        }
			        case 2:
			        {
				        value = string.Concat(decimalAndFractionalParts);
			        
				        var significand = BigInteger.Parse(value);
				        var scale = decimalAndFractionalParts[1].Length;
		        
				        result = new Money18(significand, scale);

				        return true;
			        }
		        }
	        }

	        result = default;

	        return false;
        }
        
        private static (int, int) CalculatePrecisionAndTrailingZeroesCount(
	        BigInteger significand,
	        int scale)
        {
	        var precision = 0;
	        var trailingZeroesCount = 0;

	        var precisionCalculated = false;
	        var trailingZeroesCountCalculated = false;

	        var tmp = significand;

	        while (!precisionCalculated || !trailingZeroesCountCalculated)
	        {
		        if (!precisionCalculated)
		        {
			        if (tmp >= 1)
			        {
				        precision++;
			        }
			        else
			        {
				        precisionCalculated = true;
			        }
		        }

		        if (!trailingZeroesCountCalculated)
		        {
			        if (tmp % 10 == 0 && trailingZeroesCount < scale)
			        {
				        trailingZeroesCount++;
			        }
			        else
			        {
				        trailingZeroesCountCalculated = true;
			        }
		        }

		        tmp /= 10;
	        }

	        return (precision, trailingZeroesCount);
        }

        private static (BigInteger, BigInteger) EqualizeSignificands(
	        Money18 left,
	        Money18 right)
        {
	        if (left._scale == right._scale)
	        {
		        return (left._significand, right._significand);
	        }

            var leftExponent = 0;
            var rightExponent = 0;
		        
            if (left._scale > right._scale)
            {
                rightExponent = left._scale - right._scale;
            }
            else
            {
                leftExponent = right._scale - left._scale;
            }

            var leftSignificand  = left._significand  * Pow10(leftExponent);
            var rightSignificand = right._significand * Pow10(rightExponent);

            return (leftSignificand, rightSignificand);
        }

        private static BigInteger Pow10(
	        int scale)
        {
	        return BigInteger.Pow(10, scale);
        }
    }
}