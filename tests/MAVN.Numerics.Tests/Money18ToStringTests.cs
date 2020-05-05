using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MAVN.Numerics.Tests
{
    [TestClass]
    public class Money18ToStringTests
    {
        [TestMethod]
        public void No_Trailing_Zeros()
        {
            Assert.IsFalse(Money18.Parse("0.01").ToString().EndsWith('0'));
        }

        [TestMethod]
        public void Trailing_Zeros_Are_Trimmed()
        {
            Assert.AreEqual(Money18.Parse("0.0100").ToString(), "0.01");
        }

        [TestMethod]
        public void No_Decimal_Places_Added()
        {
            Assert.AreEqual(Money18.Parse("30").ToString(), "30");
        }

        [TestMethod]
        public void Integer_Part_Formats()
        {
            Assert.AreEqual(Money18.Parse("123456789").ToString("D6"), "123456789");
            Assert.AreEqual(Money18.Parse("123456789").ToString("d10"), "0123456789");

            Assert.AreEqual(Money18.Parse("123456789").ToString("G6"), "123456789");
            Assert.AreEqual(Money18.Parse("123456789").ToString("g10"), "0123456789");

            Assert.AreEqual(Money18.Parse("123456789").ToString("R6"), "123456789");
            Assert.AreEqual(Money18.Parse("123456789").ToString("r10"), "0123456789");
        }

        [TestMethod]
        public void Number_Formats()
        {
            var nfi = new NumberFormatInfo();

            nfi.NumberDecimalSeparator = ".";
            Assert.AreEqual(Money18.Parse("0.1").ToString("R", nfi), "0.1");

            nfi.NumberDecimalSeparator = ",";
            Assert.AreEqual(Money18.Parse("0.1").ToString("R", nfi), "0,1");
        }

        [TestMethod]
        public void Decimal_Places()
        {
            var nfi = CultureInfo.InvariantCulture.NumberFormat;
            var str = "123.1239";

            Assert.AreEqual(Money18.Parse(str).ToString("R", 1, nfi), "123.1");
            Assert.AreEqual(Money18.Parse(str).ToString("R", 2, nfi), "123.12");
            Assert.AreEqual(Money18.Parse(str).ToString("R", 3, nfi), "123.123");
            Assert.AreEqual(Money18.Parse("123.1").ToString("R", 2, nfi), "123.10");
        }

        [TestMethod]
        public void Custom_Formats()
        {
            var nfi = CultureInfo.InvariantCulture.NumberFormat;

            Assert.AreEqual(Money18.Parse("123456789.123").ToString("N0", 2, nfi), (123456789.12).ToString("N2"));
        }
    }
}
