using FoolProof.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.ComponentModel.DataAnnotations;

namespace FoolProof.Core.UnitTests
{
    [TestClass()]
    public class RequiredIfFalseAttributeTest
    {
        private class Model : ModelBase<RequiredIfFalseAttribute>
        {
            public bool? Value1 { get; set; }

            [RequiredIfFalse("Value1")]
            public string Value2 { get; set; }
        }

        [TestMethod()]
        public void IsValidTest()
        {
            var model = new Model() { Value1 = false, Value2 = "hello" };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void IsNotValidTest()
        {
            var model = new Model() { Value1 = false, Value2 = "" };
            Assert.IsFalse(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void IsNotValidWithValue2NullTest()
        {
            var model = new Model() { Value1 = false, Value2 = null };
            Assert.IsFalse(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void IsNotRequiredTest()
        {
            var model = new Model() { Value1 = true, Value2 = "" };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void IsNotRequiredWithValue1NullTest()
        {
            var model = new Model() { Value1 = null, Value2 = "" };
            Assert.IsTrue(model.IsValid("Value2"));
        }
    }
}
