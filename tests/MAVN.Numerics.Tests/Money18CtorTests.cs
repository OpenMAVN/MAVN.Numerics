using System;
using FluentAssertions;
using MAVN.Numerics.Tests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MAVN.Numerics.Tests
{
    [TestClass]
    public class Money18CtorTests
    {
        [DataTestMethod]
        [DataRow("0")]
        [DataRow("0.0")]
        [DataRow("0.01")]
        [DataRow("0.010")]
        [DataRow("42")]
        [DataRow("42.0")]
        [DataRow("42.01")]
        [DataRow("42.010")]
        [DataRow("-1")]
        public void Decimal_Value_Passed__Correct_BigDecimal_Created(string a)
        {
            var d = decimal.Parse(a);
            
            Money18
                .Create(d)
                .Should()
                .Be(a);
        }
        
        [TestMethod]
        public void Negative_Scale_Passed__ArgumentException_Thrown()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Action createBigDecimal = () => { new Money18(42, -1); };

            createBigDecimal
                .Should()
                .Throw<ArgumentException>();
        }

        [DataTestMethod]
        [DataRow("-42.42")]
        [DataRow("-42.0")]
        [DataRow("-42")]
        [DataRow("-0.42")]
        [DataRow("0")]
        [DataRow("0.0")]
        [DataRow("0.42")]
        [DataRow("42")]
        [DataRow("42.0")]
        [DataRow("42.42")]
        [DataRow("+1")]
        public void Parse__Value_Correctly_Parsed(string a)
        {
            Money18
                .Parse(a)
                .Should()
                .Be(a);
        }

        [DataTestMethod]
        [DataRow("(1)")]
        [DataRow(" 1")]
        [DataRow("1 ")]
        [DataRow("1,2")]
        [DataRow("1_2")]
        [DataRow("1 2")]
        [DataRow(".12")]
        [DataRow("0.12m")]
        public void Parse__Invalid_String_Passed__FormatException_Thrown(string a)
        {
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            Action parse = () => Money18.Parse(a);

            parse
                .Should()
                .ThrowExactly<FormatException>();
        }
    }
}