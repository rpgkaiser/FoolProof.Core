using FoolProof.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace FoolProof.Core.UnitTests
{
    [TestClass()]
    public class NotEqualToAttributeTest
    {
        private class Model : ModelBase<NotEqualToAttribute>
        {
            public string Value1 { get; set; }

            [NotEqualTo("Value1")]
            public string Value2 { get; set; }
        }

        [TestMethod()]
        public void IsValid()
        {
            var model = new Model() { Value1 = "hello", Value2 = "goodbye" };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void IsNotValid()
        {
            var model = new Model() { Value1 = "hello", Value2 = "hello" };
            Assert.IsFalse(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void IsNotValidWithNulls()
        {
            var model = new Model() { };
            Assert.IsFalse(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void IsValidWithValue1Null()
        {
            var model = new Model() { Value2 = "hello" };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void IsValidWithValue2Null()
        {
            var model = new Model() { Value1 = "hello" };
            Assert.IsTrue(model.IsValid("Value2"));
        }    
    }
}
