using FoolProof.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace FoolProof.Core.Tests.UnitTests
{
    [TestClass()]
    public class RequiredIfNotEmptyAttributeTest
    {
        private class Model : ValidationModelBase<RequiredIfNotEmptyAttribute>
        {
            public string Value1 { get; set; }

            [RequiredIfNotEmpty("Value1")]
            public string Value2 { get; set; }
        }

        [TestMethod()]
        public void IsValidTest()
        {
            var model = new Model() { Value1 = "hello", Value2 = "hello" };
            Assert.IsTrue(model.IsValid("Value2"));
        }
        
        [TestMethod()]
        public void IsNotRequiredTest()
        {
            var model = new Model() { Value1 = "", Value2 = "" };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void IsNotRequiredWithValue1NullTest()
        {
            var model = new Model() { Value1 = null, Value2 = null };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void IsNotValidTest()
        {
            var model = new Model() { Value1 = "hello", Value2 = "" };
            Assert.IsFalse(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void IsNotValidWithvalue2NullTest()
        {
            var model = new Model() { Value1 = "hello", Value2 = null };
            Assert.IsFalse(model.IsValid("Value2"));
        }    
    }
}
