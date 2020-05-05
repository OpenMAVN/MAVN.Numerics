using System.Numerics;

namespace MAVN.Numerics
{
    public partial struct Money18
    {
        #region Operators
        
        public static implicit operator Money18(BigInteger value)
        {
            return Create(value, 0);
        }
        
        public static implicit operator Money18(decimal value)
        {
            return Create(value);
        }

        public static explicit operator BigInteger(Money18 value)
        {
            return Truncate(value)._significand;
        }

        public static explicit operator byte(Money18 value)
        {
            return (byte) (BigInteger) value;
        }

        public static explicit operator sbyte(Money18 value)
        {
            return (sbyte) (BigInteger) value;
        }

        public static explicit operator short(Money18 value)
        {
            return (short) (BigInteger) value;
        }

        public static explicit operator ushort(Money18 value)
        {
            return (ushort) (BigInteger) value;
        }
        
        public static explicit operator int(Money18 value)
        {
            return (int) (BigInteger) value;
        }
        
        public static explicit operator uint(Money18 value)
        {
            return (uint) (BigInteger) value;
        }
        
        public static explicit operator long(Money18 value)
        {
            return (long) (BigInteger) value;
        }
        
        public static explicit operator ulong(Money18 value)
        {
            return (ulong) (BigInteger) value;
        }

        public static explicit operator decimal(Money18 value)
        {
            var decimalMaxValue = new BigInteger(decimal.MaxValue);
            var scaleValue = Pow10(value._scale);
            if (value._significand <= decimalMaxValue && scaleValue <= decimalMaxValue)
                return (decimal) value._significand / (decimal) scaleValue;

            var result = value._significand;
            var scale = value._scale;
            while (scale > 0 && result % 10 == 0)
            {
                result /= 10;
                scale -= 1;
            }

            scaleValue = Pow10(scale);
            if (result <= decimalMaxValue && scaleValue <= decimalMaxValue)
                return (decimal)result / (decimal) scaleValue;

            return decimal.Parse(value.ToString());
        }

        #endregion
    }
}