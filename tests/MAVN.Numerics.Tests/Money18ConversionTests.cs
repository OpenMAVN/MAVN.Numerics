using FluentAssertions;
using MAVN.Numerics.Tests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace MAVN.Numerics.Tests
{
    [TestClass]
    public class Money18ConversionTests
    {
        [TestMethod]
        public void Money_Should_Be_Serializable_To_Json()
        {
            JsonConvert
                .SerializeObject(Money18.Create(10, 5))
                .Should()
                .Be("\"10.000000000000000000\"");
        }
        
        [TestMethod]
        public void Nullable_Money_Should_Be_Serializable_To_Json()
        {
            // ReSharper disable once JoinDeclarationAndInitializer
            Money18? value;

            value = Money18.Create(10, 5);
            
            JsonConvert
                .SerializeObject(value)
                .Should()
                .Be("\"10.000000000000000000\"");

            value = null;
            
            JsonConvert
                // ReSharper disable once ExpressionIsAlwaysNull
                .SerializeObject(value)
                .Should()
                .Be("null");
        }
        
        [TestMethod]
        public void Money_Should_Be_Deserializable_From_Json()
        {
            JsonConvert
                .DeserializeObject<Money18>("\"10.00000\"")
                .Should()
                .Be(Money18.Create(10, 5));
        }
        
        [TestMethod]
        public void Nullable_Money_Should_Be_Deserializable_From_Json()
        {
            JsonConvert
                .DeserializeObject<Money18?>("\"10.00000\"")
                .Should()
                .Be(Money18.Create(10, 5));
            
            JsonConvert
                .DeserializeObject<Money18?>("null")
                .Should()
                .BeNull();
        }

        [DataTestMethod]
        [DataRow("42.05", "42.05")]
        [DataRow("42.00", "42")]
        public void Cast_To_Decimal__Produces_Correct_Result(
            string value,
            string expectedResult)
        {
            ((decimal) Money18.Parse(value))
                .Should()
                .Be(decimal.Parse(expectedResult));
        }

        [DataTestMethod]
        public void Parsing_From_Decimal__Produces_Correct_Result()
        {
            ((decimal)Money18.Parse(782142909635475295147619.59932m.ToString()))
                .Should()
                .Be(782142909635475295147619.59932m);
        }

        [DataTestMethod]
        public void Conversion_To_Decimal_From_String__Produces_Correct_Result()
        {
            (decimal.Parse(Money18.Parse("782142909635475295147619.59932").ToString()))
                .Should()
                .Be(782142909635475295147619.59932m);
        }
    }
}