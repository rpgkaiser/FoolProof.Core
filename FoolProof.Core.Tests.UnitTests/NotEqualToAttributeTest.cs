using FoolProof.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace FoolProof.Core.Tests.UnitTests
{
    [TestClass()]
    public class NotEqualToAttributeTest
    {
        [TestMethod()]
        public void IsValid()
        {
            var model = new NotEqualTo.Model() { Value1 = "hello", Value2 = "goodbye" };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void IsNotValid()
        {
            var model = new NotEqualTo.Model() { Value1 = "hello", Value2 = "hello" };
            Assert.IsFalse(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void IsNotValidWithNulls()
        {
            var model = new NotEqualTo.Model() { };
            Assert.IsFalse(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void IsValidWithValue1Null()
        {
            var model = new NotEqualTo.Model() { Value2 = "hello" };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void IsValidWithValue2Null()
        {
            var model = new NotEqualTo.Model() { Value1 = "hello" };
            Assert.IsTrue(model.IsValid("Value2"));
        }    
    }
}
