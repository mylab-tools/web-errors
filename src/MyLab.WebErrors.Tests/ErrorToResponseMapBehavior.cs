using System;
using System.Linq;
using System.Net;
using System.Reflection;
using Xunit;

namespace MyLab.WebErrors.Tests
{
    public class ErrorToResponseMapBehavior
    {
        private static readonly MethodInfo TestActionWithOneBindingMethod =
            typeof(ErrorToResponseMapBehavior).GetMethod(nameof(TestActionWithOneBinding),
                BindingFlags.NonPublic | BindingFlags.Static);
        private static readonly MethodInfo TestActionWithSeveralBindingMethod = 
            typeof(ErrorToResponseMapBehavior).GetMethod(nameof(TestActionWithSeveralBindings),
            BindingFlags.NonPublic | BindingFlags.Static);
        private static readonly MethodInfo TestActionWithNoBindingMethod = 
            typeof(ErrorToResponseMapBehavior).GetMethod(nameof(TestActionWithNoBindings),
            BindingFlags.NonPublic | BindingFlags.Static);

        [Fact]
        public void ShouldDetectOneActionBinding()
        {
            //Arrange

            //Act
            var map = ErrorToResponseMap.LoadFromMethod(TestActionWithOneBindingMethod);

            var singleBinding = map?.FirstOrDefault(b => b.ExceptionType == typeof(InvalidOperationException));

            //Assert
            Assert.NotNull(singleBinding);
            Assert.Equal(HttpStatusCode.BadRequest, singleBinding.ResponseCode);
            Assert.Equal("foo message", singleBinding.Message);
        }

        [Fact]
        public void ShouldDetectSeveralActionBindings()
        {
            //Arrange

            //Act
            var map = ErrorToResponseMap.LoadFromMethod(TestActionWithSeveralBindingMethod);

            var fBinding = map?.FirstOrDefault(b => b.ExceptionType == typeof(InvalidOperationException));
            var sBinding = map?.FirstOrDefault(b => b.ExceptionType == typeof(NotSupportedException));

            //Assert
            Assert.NotNull(fBinding);
            Assert.Equal(HttpStatusCode.BadRequest, fBinding.ResponseCode);
            Assert.Equal("bar message", fBinding.Message);

            Assert.NotNull(sBinding);
            Assert.Equal(HttpStatusCode.HttpVersionNotSupported, sBinding.ResponseCode);
            Assert.Null(sBinding.Message);
        }

        [Fact]
        public void ShouldNotDetectedAbsentBindings()
        {
            //Arrange

            //Act
            var map = ErrorToResponseMap.LoadFromMethod(TestActionWithNoBindingMethod);
            
            //Assert
            Assert.Empty(map);
        }

        [ErrorToResponse(typeof(InvalidOperationException), HttpStatusCode.BadRequest, "foo message")]
        static void TestActionWithOneBinding()
        {

        }

        [ErrorToResponse(typeof(InvalidOperationException), HttpStatusCode.BadRequest, "bar message")]
        [ErrorToResponse(typeof(NotSupportedException), HttpStatusCode.HttpVersionNotSupported)]
        static void TestActionWithSeveralBindings()
        {

        }

        static void TestActionWithNoBindings()
        {

        }
    }
}
