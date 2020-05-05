using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Numeric;

namespace MAVN.Numerics.Tests.Utils
{
    public class Money18Assertions : NumericAssertions<Money18>
    {
        public Money18Assertions(Money18 value) 
            : base(value)
        {
            
        }

        public AndConstraint<Money18Assertions> Be(
            string expected,
            string because = "",
            params object[] becauseArgs)
        {
            var money = Money18.Parse(expected);
            
            Execute.Assertion
                .ForCondition(!(Subject is null) && Subject.CompareTo(money) == 0)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected {context:value} to be {0}{reason}, but found {1}.", money, Subject);

            return new AndConstraint<Money18Assertions>(this);
        }
    }
}