using FoolProof.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace FoolProof.Core.UnitTests
{
    [TestClass()]
    public class RequiredIfAttributeTest
    {
        private class Model : ModelBase<RequiredIfAttribute>
        {
            public string Value1 { get; set; }

            [RequiredIf("Value1", "hello")]
            public string Value2 { get; set; }
        }

        private class ComplexModel : ModelBase<RequiredIfAttribute>
        {
            public class SubModel
            {
                public string InnerValue { get; set; }
            }

            public SubModel Value1 { get; set; }

            [RequiredIf("Value1.InnerValue", "hello")]
            public string Value2 { get; set; }
        }

        [TestMethod()]
        public void IsValidTest()
        {
            var model = new Model() { Value1 = "hello", Value2 = "hello" };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void IsValidTestComplex()
        {
            var model = new ComplexModel() {Value1 = new ComplexModel.SubModel() {InnerValue = "hello"}, Value2 = "bla"};
            Assert.IsTrue(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void IsNotValidTest()
        {
            var model = new Model() { Value1 = "hello", Value2 = "" };
            Assert.IsFalse(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void IsNotValidTestComplex()
        {
            var model = new ComplexModel() { Value1 = new ComplexModel.SubModel() { InnerValue = "hello" }, Value2 = "" };
            Assert.IsFalse(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void IsNotValidWithValue2NullTest()
        {
            var model = new Model() { Value1 = "hello", Value2 = null };
            Assert.IsFalse(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void IsNotRequiredTest()
        {
            var model = new Model() { Value1 = "goodbye" };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void IsNotRequiredWithValue1NullTest()
        {
            var model = new Model() { Value1 = null };
            Assert.IsTrue(model.IsValid("Value2"));
        }
    }
}
