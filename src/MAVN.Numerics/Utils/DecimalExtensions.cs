using System.Numerics;

namespace MAVN.Numerics.Utils
{
    internal static class DecimalExtensions
    {
        public static (BigInteger, int) GetSignificandAndScale(
            this decimal value)
        {
            var valueBits = (uint[]) (object) decimal.GetBits(value);
            var scale = (int)((valueBits[3] >> 16) & 31);

            const decimal bitsMultiplier = 4294967296m;
			
            var significand = (valueBits[2] * bitsMultiplier * bitsMultiplier) + (valueBits[1] * bitsMultiplier) + valueBits[0];

            if (value < 0)
            {
                significand = significand * -1;
            }

            return (new BigInteger(significand), scale);
        }
    }
}