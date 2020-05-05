using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using MAVN.Numerics.Linq;
using MAVN.Numerics.Tests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MAVN.Numerics.Tests.Linq
{
    [TestClass]
    public class Money18Tests
    {
        private static IEnumerable<Money18?> NullableValues()
        {
            yield return null;
            yield return Money18.Create(10.0000m);
            yield return null;
            yield return Money18.Create(13.00m);
            yield return null;
            yield return Money18.Create(10.300000m);
            yield return null;
        }
        
        private static IEnumerable<Money18> Values()
        {
            yield return Money18.Create(10.0000m);
            yield return Money18.Create(13.00m);
            yield return Money18.Create(10.300000m);
        }
        
        private static IEnumerable<ObjectWithValue<Money18?>> ObjectsWithNullableValues()
            => NullableValues().Select(x => new ObjectWithValue<Money18?>(x));
        
        private static IEnumerable<ObjectWithValue<Money18>> ObjectsWithValues()
            => Values().Select(x => new ObjectWithValue<Money18>(x));

        private static Money18 ExpectedAverage
            => Money18.Create(11.100000m);

        private static Money18? ExpectedNullableAverage
            => ExpectedAverage;
        
        private static Money18 ExpectedMax
            => Money18.Create(13.00m);

        private static Money18? ExpectedNullableMax
            => ExpectedMax;
        
        private static Money18 ExpectedMin
            => Money18.Create(10.0000m);

        private static Money18? ExpectedNullableMin
            => ExpectedMin;
        
        private static Money18 ExpectedSum
            => Money18.Create(33.300000m);

        private static Money18? ExpectedNullableSum
            => ExpectedSum;
        

        #region Average
        
        [TestMethod]
        public void Average__Values_Passed__Correct_Result_Returned()
        {
            Values()
                .Average()
                .Should()
                .Be(ExpectedAverage);
        }

        [TestMethod]
        public void Average__Objects_With_Values_Passed__Correct_Result_Returned()
        {
            ObjectsWithValues()
                .Average(x => x.Value)
                .Should()
                .Be(ExpectedAverage);
        }

        [TestMethod]
        public void Average__Nullable_Values_Passed__Correct_Result_Returned()
        {
            NullableValues()
                .Average()
                .Should()
                .Be(ExpectedNullableAverage);
        }

        [TestMethod]
        public void Average__Objects_With_Nullable_Values_Passed__Correct_Result_Returned()
        {
            ObjectsWithNullableValues()
                .Average(x => x.Value)
                .Should()
                .Be(ExpectedNullableAverage);
        }
        
        [TestMethod]
        public void Average__Null_Values_Passed__Correct_Result_Returned()
        {
            NullableValues()
                .Where(x => x == null)
                .Average()
                .Should()
                .BeNull();
        }
        
        [TestMethod]
        public void Average__Objects_With_Null_Values_Passed__Correct_Result_Returned()
        {
            ObjectsWithNullableValues()
                .Where(x => x.Value == null)
                .Average(x => x.Value)
                .Should()
                .BeNull();
        }
        
        [TestMethod]
        public void Average__Empty_List_Of_Values_Passed__InvalidOperationException_Thrown()
        {
            Action action = () => Values().Take(0).Average();
            
            action
                .Should()
                .ThrowExactly<InvalidOperationException>();
        }
        
        [TestMethod]
        public void Average__Empty_List_Of_Objects_With_Values_Passed__InvalidOperationException_Thrown()
        {
            Action action = () => ObjectsWithValues().Take(0).Average(x => x.Value);
            
            action
                .Should()
                .ThrowExactly<InvalidOperationException>();
        }
        
        [TestMethod]
        public void Average__Empty_List_Of_Nullable_Values_Passed__Correct_Result_Returned()
        {
            NullableValues()
                .Take(0)
                .Average()
                .Should()
                .BeNull();
        }
        
        [TestMethod]
        public void Average__Empty_List_Of_Objects_With_Nullable_Values_Passed__Correct_Result_Returned()
        {
            ObjectsWithNullableValues()
                .Take(0)
                .Average(x => x.Value)
                .Should()
                .BeNull();
        }

        [TestMethod]
        public void Average__Null_Passed_As_List_Of_Values__ArgumentNullException_Thrown()
        {
            Action action = () => ((IEnumerable<Money18>) null).Average();
            
            action
                .Should()
                .ThrowExactly<ArgumentNullException>();
        }
        
        [TestMethod]
        public void Average__Null_Passed_As_List_Of_Objects_With_Values__ArgumentNullException_Thrown()
        {
            Action action = () => ((IEnumerable<ObjectWithValue<Money18>>) null).Average(x => x.Value);
            
            action
                .Should()
                .ThrowExactly<ArgumentNullException>();
        }
        
        [TestMethod]
        public void Average__Null_Passed_As_List_Of_Nullable_Values__ArgumentNullException_Thrown()
        {
            Action action = () => ((IEnumerable<Money18?>) null).Average();
            
            action
                .Should()
                .ThrowExactly<ArgumentNullException>();
        }
        
        [TestMethod]
        public void Average__Null_Passed_As_List_Of_Objects_With_Nullable_Values__ArgumentNullException_Thrown()
        {
            Action action = () => ((IEnumerable<ObjectWithValue<Money18?>>) null).Average(x => x.Value);
            
            action
                .Should()
                .ThrowExactly<ArgumentNullException>();
        }
        
        [TestMethod]
        public void Average__Null_Passed_As_Selector_For_List_Of_Objects_With_Values__ArgumentNullException_Thrown()
        {
            Action action = () => ObjectsWithValues().Average((Func<ObjectWithValue<Money18>, Money18>) null);
            
            action
                .Should()
                .ThrowExactly<ArgumentNullException>();
        }
        
        [TestMethod]
        public void Average__Null_Passed_As_Selector_For_List_Of_Objects_With_Nullable_Values__ArgumentNullException_Thrown()
        {
            Action action = () => ObjectsWithNullableValues().Average((Func<ObjectWithValue<Money18?>, Money18?>) null);
            
            action
                .Should()
                .ThrowExactly<ArgumentNullException>();
        }
        
        #endregion

        #region Max
        
        [TestMethod]
        public void Max__Values_Passed__Correct_Result_Returned()
        {
            Values()
                .Max()
                .Should()
                .Be(ExpectedMax);
        }

        [TestMethod]
        public void Max__Objects_With_Values_Passed__Correct_Result_Returned()
        {
            ObjectsWithValues()
                .Max(x => x.Value)
                .Should()
                .Be(ExpectedMax);
        }

        [TestMethod]
        public void Max__Nullable_Values_Passed__Correct_Result_Returned()
        {
            NullableValues()
                .Max()
                .Should()
                .Be(ExpectedNullableMax);
        }

        [TestMethod]
        public void Max__Objects_With_Nullable_Values_Passed__Correct_Result_Returned()
        {
            ObjectsWithNullableValues()
                .Max(x => x.Value)
                .Should()
                .Be(ExpectedNullableMax);
        }
        
        [TestMethod]
        public void Max__Null_Values_Passed__Correct_Result_Returned()
        {
            NullableValues()
                .Where(x => x == null)
                .Max()
                .Should()
                .BeNull();
        }
        
        [TestMethod]
        public void Max__Objects_With_Null_Values_Passed__Correct_Result_Returned()
        {
            ObjectsWithNullableValues()
                .Where(x => x.Value == null)
                .Max(x => x.Value)
                .Should()
                .BeNull();
        }
        
        [TestMethod]
        public void Max__Empty_List_Of_Values_Passed__InvalidOperationException_Thrown()
        {
            Action action = () => Values().Take(0).Max();
            
            action
                .Should()
                .ThrowExactly<InvalidOperationException>();
        }
        
        [TestMethod]
        public void Max__Empty_List_Of_Objects_With_Values_Passed__InvalidOperationException_Thrown()
        {
            Action action = () => ObjectsWithValues().Take(0).Max(x => x.Value);
            
            action
                .Should()
                .ThrowExactly<InvalidOperationException>();
        }
        
        [TestMethod]
        public void Max__Empty_List_Of_Nullable_Values_Passed__Correct_Result_Returned()
        {
            NullableValues()
                .Take(0)
                .Max()
                .Should()
                .BeNull();
        }
        
        [TestMethod]
        public void Max__Empty_List_Of_Objects_With_Nullable_Values_Passed__Correct_Result_Returned()
        {
            ObjectsWithNullableValues()
                .Take(0)
                .Max(x => x.Value)
                .Should()
                .BeNull();
        }

        [TestMethod]
        public void Max__Null_Passed_As_List_Of_Values__ArgumentNullException_Thrown()
        {
            Action action = () => ((IEnumerable<Money18>) null).Max();
            
            action
                .Should()
                .ThrowExactly<ArgumentNullException>();
        }
        
        [TestMethod]
        public void Max__Null_Passed_As_List_Of_Objects_With_Values__ArgumentNullException_Thrown()
        {
            Action action = () => ((IEnumerable<ObjectWithValue<Money18>>) null).Max(x => x.Value);
            
            action
                .Should()
                .ThrowExactly<ArgumentNullException>();
        }
        
        [TestMethod]
        public void Max__Null_Passed_As_List_Of_Nullable_Values__ArgumentNullException_Thrown()
        {
            Action action = () => ((IEnumerable<Money18?>) null).Max();
            
            action
                .Should()
                .ThrowExactly<ArgumentNullException>();
        }
        
        [TestMethod]
        public void Max__Null_Passed_As_List_Of_Objects_With_Nullable_Values__ArgumentNullException_Thrown()
        {
            Action action = () => ((IEnumerable<ObjectWithValue<Money18?>>) null).Max(x => x.Value);
            
            action
                .Should()
                .ThrowExactly<ArgumentNullException>();
        }
        
        [TestMethod]
        public void Max__Null_Passed_As_Selector_For_List_Of_Objects_With_Values__ArgumentNullException_Thrown()
        {
            Action action = () => ObjectsWithValues().Max((Func<ObjectWithValue<Money18>, Money18>) null);
            
            action
                .Should()
                .ThrowExactly<ArgumentNullException>();
        }
        
        [TestMethod]
        public void Max__Null_Passed_As_Selector_For_List_Of_Objects_With_Nullable_Values__ArgumentNullException_Thrown()
        {
            Action action = () => ObjectsWithNullableValues().Max((Func<ObjectWithValue<Money18?>, Money18?>) null);
            
            action
                .Should()
                .ThrowExactly<ArgumentNullException>();
        }
        
        #endregion
        
        #region Min
        
        [TestMethod]
        public void Min__Values_Passed__Correct_Result_Returned()
        {
            Values()
                .Min()
                .Should()
                .Be(ExpectedMin);
        }

        [TestMethod]
        public void Min__Objects_With_Values_Passed__Correct_Result_Returned()
        {
            ObjectsWithValues()
                .Min(x => x.Value)
                .Should()
                .Be(ExpectedMin);
        }

        [TestMethod]
        public void Min__Nullable_Values_Passed__Correct_Result_Returned()
        {
            NullableValues()
                .Min()
                .Should()
                .Be(ExpectedNullableMin);
        }

        [TestMethod]
        public void Min__Objects_With_Nullable_Values_Passed__Correct_Result_Returned()
        {
            ObjectsWithNullableValues()
                .Min(x => x.Value)
                .Should()
                .Be(ExpectedNullableMin);
        }
        
        [TestMethod]
        public void Min__Null_Values_Passed__Correct_Result_Returned()
        {
            NullableValues()
                .Where(x => x == null)
                .Min()
                .Should()
                .BeNull();
        }
        
        [TestMethod]
        public void Min__Objects_With_Null_Values_Passed__Correct_Result_Returned()
        {
            ObjectsWithNullableValues()
                .Where(x => x.Value == null)
                .Min(x => x.Value)
                .Should()
                .BeNull();
        }
        
        [TestMethod]
        public void Min__Empty_List_Of_Values_Passed__InvalidOperationException_Thrown()
        {
            Action action = () => Values().Take(0).Min();
            
            action
                .Should()
                .ThrowExactly<InvalidOperationException>();
        }
        
        [TestMethod]
        public void Min__Empty_List_Of_Objects_With_Values_Passed__InvalidOperationException_Thrown()
        {
            Action action = () => ObjectsWithValues().Take(0).Min(x => x.Value);
            
            action
                .Should()
                .ThrowExactly<InvalidOperationException>();
        }
        
        [TestMethod]
        public void Min__Empty_List_Of_Nullable_Values_Passed__Correct_Result_Returned()
        {
            NullableValues()
                .Take(0)
                .Min()
                .Should()
                .BeNull();
        }
        
        [TestMethod]
        public void Min__Empty_List_Of_Objects_With_Nullable_Values_Passed__Correct_Result_Returned()
        {
            ObjectsWithNullableValues()
                .Take(0)
                .Min(x => x.Value)
                .Should()
                .BeNull();
        }

        [TestMethod]
        public void Min__Null_Passed_As_List_Of_Values__ArgumentNullException_Thrown()
        {
            Action action = () => ((IEnumerable<Money18>) null).Min();
            
            action
                .Should()
                .ThrowExactly<ArgumentNullException>();
        }
        
        [TestMethod]
        public void Min__Null_Passed_As_List_Of_Objects_With_Values__ArgumentNullException_Thrown()
        {
            Action action = () => ((IEnumerable<ObjectWithValue<Money18>>) null).Min(x => x.Value);
            
            action
                .Should()
                .ThrowExactly<ArgumentNullException>();
        }
        
        [TestMethod]
        public void Min__Null_Passed_As_List_Of_Nullable_Values__ArgumentNullException_Thrown()
        {
            Action action = () => ((IEnumerable<Money18?>) null).Min();
            
            action
                .Should()
                .ThrowExactly<ArgumentNullException>();
        }
        
        [TestMethod]
        public void Min__Null_Passed_As_List_Of_Objects_With_Nullable_Values__ArgumentNullException_Thrown()
        {
            Action action = () => ((IEnumerable<ObjectWithValue<Money18?>>) null).Min(x => x.Value);
            
            action
                .Should()
                .ThrowExactly<ArgumentNullException>();
        }
        
        [TestMethod]
        public void Min__Null_Passed_As_Selector_For_List_Of_Objects_With_Values__ArgumentNullException_Thrown()
        {
            Action action = () => ObjectsWithValues().Min((Func<ObjectWithValue<Money18>, Money18>) null);
            
            action
                .Should()
                .ThrowExactly<ArgumentNullException>();
        }
        
        [TestMethod]
        public void Min__Null_Passed_As_Selector_For_List_Of_Objects_With_Nullable_Values__ArgumentNullException_Thrown()
        {
            Action action = () => ObjectsWithNullableValues().Min((Func<ObjectWithValue<Money18?>, Money18?>) null);
            
            action
                .Should()
                .ThrowExactly<ArgumentNullException>();
        }
        
        #endregion
        
        #region Sum
        
        [TestMethod]
        public void Sum__Values_Passed__Correct_Result_Returned()
        {
            Values()
                .Sum()
                .Should()
                .Be(ExpectedSum);
        }

        [TestMethod]
        public void Sum__Objects_With_Values_Passed__Correct_Result_Returned()
        {
            ObjectsWithValues()
                .Sum(x => x.Value)
                .Should()
                .Be(ExpectedSum);
        }

        [TestMethod]
        public void Sum__Nullable_Values_Passed__Correct_Result_Returned()
        {
            NullableValues()
                .Sum()
                .Should()
                .Be(ExpectedNullableSum);
        }

        [TestMethod]
        public void Sum__Objects_With_Nullable_Values_Passed__Correct_Result_Returned()
        {
            ObjectsWithNullableValues()
                .Sum(x => x.Value)
                .Should()
                .Be(ExpectedNullableSum);
        }
        
        [TestMethod]
        public void Sum__Null_Values_Passed__Correct_Result_Returned()
        {
            NullableValues()
                .Where(x => x == null)
                .Sum()
                .Should()
                .Be((Money18?) 0);
        }
        
        [TestMethod]
        public void Sum__Objects_With_Null_Values_Passed__Correct_Result_Returned()
        {
            ObjectsWithNullableValues()
                .Where(x => x.Value == null)
                .Sum(x => x.Value)
                .Should()
                .Be((Money18?) 0);
        }
        
        [TestMethod]
        public void Sum__Empty_List_Of_Values_Passed__Correct_Result_Returned()
        {
            ObjectsWithNullableValues()
                .Take(0)
                .Sum(x => x.Value)
                .Should()
                .Be((Money18) 0);
        }
        
        [TestMethod]
        public void Sum__Empty_List_Of_Objects_With_Values_Passed__Correct_Result_Returned()
        {
            ObjectsWithNullableValues()
                .Take(0)
                .Sum(x => x.Value)
                .Should()
                .Be((Money18) 0);
        }
        
        [TestMethod]
        public void Sum__Empty_List_Of_Nullable_Values_Passed__Correct_Result_Returned()
        {
            NullableValues()
                .Take(0)
                .Sum()
                .Should()
                .Be((Money18?) 0);
        }
        
        [TestMethod]
        public void Sum__Empty_List_Of_Objects_With_Nullable_Values_Passed__Correct_Result_Returned()
        {
            ObjectsWithNullableValues()
                .Take(0)
                .Sum(x => x.Value)
                .Should()
                .Be((Money18?) 0);
        }

        [TestMethod]
        public void Sum__Null_Passed_As_List_Of_Values__ArgumentNullException_Thrown()
        {
            Action action = () => ((IEnumerable<Money18>) null).Sum();
            
            action
                .Should()
                .ThrowExactly<ArgumentNullException>();
        }
        
        [TestMethod]
        public void Sum__Null_Passed_As_List_Of_Objects_With_Values__ArgumentNullException_Thrown()
        {
            Action action = () => ((IEnumerable<ObjectWithValue<Money18>>) null).Sum(x => x.Value);
            
            action
                .Should()
                .ThrowExactly<ArgumentNullException>();
        }
        
        [TestMethod]
        public void Sum__Null_Passed_As_List_Of_Nullable_Values__ArgumentNullException_Thrown()
        {
            Action action = () => ((IEnumerable<Money18?>) null).Sum();
            
            action
                .Should()
                .ThrowExactly<ArgumentNullException>();
        }
        
        [TestMethod]
        public void Sum__Null_Passed_As_List_Of_Objects_With_Nullable_Values__ArgumentNullException_Thrown()
        {
            Action action = () => ((IEnumerable<ObjectWithValue<Money18?>>) null).Sum(x => x.Value);
            
            action
                .Should()
                .ThrowExactly<ArgumentNullException>();
        }
        
        [TestMethod]
        public void Sum__Null_Passed_As_Selector_For_List_Of_Objects_With_Values__ArgumentNullException_Thrown()
        {
            Action action = () => ObjectsWithValues().Sum((Func<ObjectWithValue<Money18>, Money18>) null);
            
            action
                .Should()
                .ThrowExactly<ArgumentNullException>();
        }
        
        [TestMethod]
        public void Sum__Null_Passed_As_Selector_For_List_Of_Objects_With_Nullable_Values__ArgumentNullException_Thrown()
        {
            Action action = () => ObjectsWithNullableValues().Sum((Func<ObjectWithValue<Money18?>, Money18?>) null);
            
            action
                .Should()
                .ThrowExactly<ArgumentNullException>();
        }
        
        #endregion
        
        private class ObjectWithValue<T>
        {
            public ObjectWithValue(
                T value)
            {
                Value = value;
            }
            
            public T Value { get; }
        }
    }
}