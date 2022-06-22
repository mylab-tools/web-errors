using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MyLab.WebErrors.Tests
{
    public class TypeComparerBehavior
    {
        [Theory]
        [InlineData(typeof(TestBase), typeof(string), -1)]
        [InlineData(typeof(TestBase), typeof(TestBase), 0)]
        [InlineData(typeof(TestBase), typeof(TestChild2), 2)]
        [InlineData(typeof(TestChild1), typeof(TestChild2), 1)]
        [InlineData(typeof(TestChild2), typeof(TestChild1), -1)]
        public void ShouldDetectTypeComparisonRate(Type baseType, Type targetType, int expectedRate)
        {
            //Arrange

            //Act
            var rate = TypeComparer.GetComparisonRate(baseType, targetType);

            //Assert
            Assert.Equal(expectedRate, rate);
        }

        class TestBase
        {

        }

        class TestChild1 : TestBase
        {

        }

        class TestChild2 : TestChild1
        {

        }
    }
}
