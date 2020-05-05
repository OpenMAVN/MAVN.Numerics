using System;
using System.Linq;
using FluentAssertions;
using MAVN.Numerics.Tests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MAVN.Numerics.Tests
{
    [TestClass]
    public class Money18ArithmeticTests
    {
        [DataTestMethod]
        [DataRow("1.0", "1.0")]
        [DataRow("0",   "0"  )]
        [DataRow("-1",  "1"  )]
        public void Abs__Produces_Correct_Result(string a, string b)
        {
            Money18
                .Abs(Money18.Parse(a))
                .Should()
                .Be(b);
        }
        
        [DataTestMethod]
        // Basic operations
        [DataRow("0",  "0",  "0")]
        [DataRow("1",  "0",  "1")]
        [DataRow("1",  "1",  "2")]
        [DataRow("-1", "1",  "0")]
        [DataRow("1",  "-1", "0")]
        // Operations with fractional part
        [DataRow("0.0000",     "0",       "0.0000"     )]
        [DataRow("1234.5678",  "0.0009",  "1234.5687"  )]
        [DataRow("1234.5678",  "0.00009", "1234.56789" )]
        [DataRow("-1234.5678", "0.00009", "-1234.56771")]
        [DataRow("0.1234",     "0.5678",  "0.6912"     )]
        [DataRow("0.07",       "0.93",    "1.00"       )]
        public void Add__Produces_Correct_Result(string a, string b, string c)
        {
            var (left, right) = Parse(a, b);

            Money18
                .Add(left, right)
                .Should()
                .Be(c);
        }

        [DataTestMethod]
        [DataRow("10",   "10"  )]
        [DataRow("12.6", "13.0")]
        [DataRow("12.1", "13.0")]
        [DataRow("9.5",  "10.0")]
        [DataRow("8.16", "9.00")]
        [DataRow("0.1",  "1.0" )]
        [DataRow("-0.1", "0.0" )]
        [DataRow("-1.1", "-1.0")]
        [DataRow("-1.9", "-1.0")]
        [DataRow("-3.9", "-3.0")]
        public void Ceiling__Produces_Correct_result(string a, string b)
        {
            Money18
                .Ceiling(Money18.Parse(a))
                .Should()
                .Be(b);
        }

        [DataTestMethod]
        // Basic operations
        [DataRow("0",  "1",  "0" )]
        [DataRow("2",  "1",  "2" )]
        [DataRow("3",  "3",  "1" )]
        [DataRow("-6", "3",  "-2")]
        [DataRow("6",  "-3", "-2")]
        // Operations with fractional part
        [DataRow("0.0", "42",   "0.0" )]
        [DataRow("0",   "4.0",  "0.0" )]
        [DataRow("1",   "2",    "0.5"   )]
        [DataRow("1",   "3",    "0.333333333333333333")]
        [DataRow("1",   "3.00", "0.333333333333333333")]
        [DataRow("1.0", "3",    "0.333333333333333333" )]
        [DataRow("1.0", "4",    "0.25" )]
        [DataRow("1.0", "4.00", "0.25")]
        [DataRow("0.5", "0.5",  "1.0" )]
        [DataRow("1",   "0.5",  "2.0" )]
        [DataRow("1",   "0.1",  "10.0")]
        [DataRow("-1",   "3",  "-0.333333333333333333")]
        public void Divide__Produces_Correct_Result(string a, string b, string c)
        {
            var (left, right) = Parse(a, b);

            Money18
                .Divide(left, right)
                .Should()
                .Be(c);
        }

        [TestMethod]
        public void Divide__Divider_Is_Zero__DivideByZeroException_Thrown()
        {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Action divisionByZero = () => Money18.Divide(1, 0);

            divisionByZero
                .Should()
                .ThrowExactly<DivideByZeroException>();
        }

        [DataTestMethod]
        [DataRow("10",   "10"  )]
        [DataRow("12.6", "12.0")]
        [DataRow("12.1", "12.0")]
        [DataRow("9.5",  "9.0" )]
        [DataRow("8.16", "8.00")]
        [DataRow("0.1",  "0.0" )]
        [DataRow("-0.1", "-1.0")]
        [DataRow("-1.1", "-2.0")]
        [DataRow("-1.9", "-2.0")]
        [DataRow("-3.9", "-4.0")]
        public void Floor__Produces_Correct_Result(string a, string b)
        {
            Money18
                .Floor(Money18.Parse(a))
                .Should()
                .Be(b);
        }

        [DataTestMethod]
        // Basic operations
        [DataRow("0",  "0",  "0" )]
        [DataRow("1",  "0",  "0" )]
        [DataRow("2",  "1",  "2" )]
        [DataRow("-2", "3",  "-6")]
        [DataRow("2",  "-3", "-6")]
        // Operations with fractional part
        [DataRow("0.0",  "7",   "0.0" )]
        [DataRow("0",    "7.0", "0.0" )]
        [DataRow("0.5",  "0.5", "0.25" )]
        [DataRow("1.1",  "1.1", "1.21" )]
        [DataRow("1.1",  "1.9", "2.09" )]
        [DataRow("1.10", "1.1", "1.21")]
        [DataRow("10",   "0.1", "1.0" )]
        [DataRow("10.0", "0.1", "1.0" )]
        [DataRow("0.1",  "10",  "1.0" )]
        public void Multiply__Produces_Correct_Result(string a, string b, string c)
        {
            var (left, right) = Parse(a, b);

            Money18
                .Multiply(left, right)
                .Should()
                .Be(c);
        }

        [DataTestMethod]
        [DataRow("1.0", "-1.0")]
        [DataRow("0",   "0"   )]
        [DataRow("0.0", "0.0" )]
        [DataRow("-1",  "1"  )]
        public void Negate__Produces_Correct_Result(string a, string b)
        {
            Money18
                .Negate(Money18.Parse(a))
                .Should()
                .Be(b);
        }

        [DataTestMethod]
        [DataRow("100",    "100" )]
        [DataRow("100.0",  "100" )]
        [DataRow("100.1",  "100" )]
        [DataRow("100.2",  "100" )]
        [DataRow("100.3",  "100" )]
        [DataRow("100.4",  "100" )]
        [DataRow("100.5",  "100" )]
        [DataRow("100.6",  "101" )]
        [DataRow("100.7",  "101" )]
        [DataRow("100.8",  "101" )]
        [DataRow("100.9",  "101" )]
        [DataRow("101.0",  "101" )]
        [DataRow("101.1",  "101" )]
        [DataRow("101.2",  "101" )]
        [DataRow("101.3",  "101" )]
        [DataRow("101.4",  "101" )]
        [DataRow("101.5",  "102" )]
        [DataRow("101.6",  "102" )]
        [DataRow("101.7",  "102" )]
        [DataRow("101.8",  "102" )]
        [DataRow("101.9",  "102" )]
        [DataRow("102.0",  "102" )]
        [DataRow("-101.0", "-101")]
        [DataRow("-101.1", "-101")]
        [DataRow("-101.2", "-101")]
        [DataRow("-101.3", "-101")]
        [DataRow("-101.4", "-101")]
        [DataRow("-101.5", "-102")]
        [DataRow("-101.6", "-102")]
        [DataRow("-101.7", "-102")]
        [DataRow("-101.8", "-102")]
        [DataRow("-101.9", "-102")]
        [DataRow("-102.0", "-102")]
        public void Round__Produces_Correct_Result(string a, string b)
        {
            Money18
                .Round(Money18.Parse(a))
                .Should()
                .Be(b);
        }

        [DataTestMethod]
        [DataRow("0.1",      "0.10"   )]
        [DataRow("100",      "100.00" )]
        [DataRow("100.000",  "100.00" )]
        [DataRow("100.001",  "100.00" )]
        [DataRow("100.002",  "100.00" )]
        [DataRow("100.003",  "100.00" )]
        [DataRow("100.004",  "100.00" )]
        [DataRow("100.005",  "100.00" )]
        [DataRow("100.006",  "100.01" )]
        [DataRow("100.007",  "100.01" )]
        [DataRow("100.008",  "100.01" )]
        [DataRow("100.009",  "100.01" )]
        [DataRow("100.010",  "100.01" )]
        [DataRow("100.011",  "100.01" )]
        [DataRow("100.012",  "100.01" )]
        [DataRow("100.013",  "100.01" )]
        [DataRow("100.014",  "100.01" )]
        [DataRow("100.015",  "100.02" )]
        [DataRow("100.016",  "100.02" )]
        [DataRow("100.017",  "100.02" )]
        [DataRow("100.018",  "100.02" )]
        [DataRow("100.019",  "100.02" )]
        [DataRow("100.020",  "100.02" )]
        [DataRow("-100.011", "-100.01")]
        [DataRow("-100.012", "-100.01")]
        [DataRow("-100.013", "-100.01")]
        [DataRow("-100.014", "-100.01")]
        [DataRow("-100.015", "-100.02")]
        [DataRow("-100.016", "-100.02")]
        [DataRow("-100.017", "-100.02")]
        [DataRow("-100.018", "-100.02")]
        [DataRow("-100.019", "-100.02")]
        [DataRow("-100.020", "-100.02")]
        public void Round__Scale_Specified__Produces_Correct_Result(string a, string b)
        {
            Money18
                .Round(Money18.Parse(a), 2)
                .Should()
                .Be(b);
        }

        [TestMethod]
        public void Round__All_MidpointRounding_Modes_Are_Supported()
        {
            foreach (var mode in Enum.GetValues(typeof(MidpointRounding)).Cast<MidpointRounding>())
            {
                // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                Action round = () => Money18.Round(0.5m, mode);

                round
                    .Should()
                    .NotThrow();
            }
        }

        [DataTestMethod]
        // Basic operations
        [DataRow("0",  "0",  "0" )]
        [DataRow("1",  "0",  "1" )]
        [DataRow("1",  "1",  "0" )]
        [DataRow("-1", "1",  "-2")]
        [DataRow("1",  "-1", "2" )]
        // Operations with fractional part
        [DataRow("1234.5678",  "0.0009",  "1234.5669"  )]
        [DataRow("1234.5678",  "0.00009", "1234.56771" )]
        [DataRow("-1234.5678", "0.00009", "-1234.56789")]
        [DataRow("0.1234",     "0.5678",  "-0.4444"    )]
        [DataRow("-0.07",      "0.93",    "-1.00"      )]
        public void Subtract__Produces_Correct_Result(string a, string b, string c)
        {            
            var (left, right) = Parse(a, b);

            Money18
                .Subtract(left, right)
                .Should()
                .Be(c);
        }

        [DataTestMethod]
        [DataRow("100",    "100" )]
        [DataRow("100.0",  "100" )]
        [DataRow("100.1",  "100" )]
        [DataRow("100.2",  "100" )]
        [DataRow("100.3",  "100" )]
        [DataRow("100.4",  "100" )]
        [DataRow("100.5",  "100" )]
        [DataRow("100.6",  "100" )]
        [DataRow("100.7",  "100" )]
        [DataRow("100.8",  "100" )]
        [DataRow("100.9",  "100" )]
        [DataRow("101.0",  "101" )]
        [DataRow("101.10", "101" )]
        [DataRow("101.21", "101" )]
        [DataRow("101.32", "101" )]
        [DataRow("101.43", "101" )]
        [DataRow("101.54", "101" )]
        [DataRow("101.65", "101" )]
        [DataRow("101.76", "101" )]
        [DataRow("101.87", "101" )]
        [DataRow("101.98", "101" )]
        [DataRow("102.09", "102" )]
        [DataRow("-101.4", "-101")]
        [DataRow("-101.5", "-101")]
        [DataRow("-101.6", "-101")]
        public void Truncate__Produces_Correct_Result(string a, string b)
        {
            Money18
                .Truncate(Money18.Parse(a))
                .Should()
                .Be(b);
        }
        
        private static (Money18 Left, Money18 Right) Parse(string left, string right)
        {
            return (Money18.Parse(left), Money18.Parse(right));
        }
    }
}