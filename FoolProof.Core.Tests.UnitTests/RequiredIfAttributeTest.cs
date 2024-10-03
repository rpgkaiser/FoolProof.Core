using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FoolProof.Core.Tests.UnitTests
{
    [TestClass()]
    public class RequiredIfAttributeTest
    {
        [TestMethod()]
        public void IsValidTest()
        {
            var model = new RequiredIf.Model() { Value1 = "hello", Value2 = "hello" };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void IsValidTestComplex()
        {
            var model = new RequiredIf.ComplexModel() { Value1 = new RequiredIf.ComplexModel.SubModel() { InnerValue = "hello" }, Value2 = "bla" };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void IsNotValidTest()
        {
            var model = new RequiredIf.Model() { Value1 = "hello", Value2 = "" };
            Assert.IsFalse(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void IsNotValidTestComplex()
        {
            var model = new RequiredIf.ComplexModel() { Value1 = new RequiredIf.ComplexModel.SubModel() { InnerValue = "hello" }, Value2 = "" };
            Assert.IsFalse(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void IsNotValidWithValue2NullTest()
        {
            var model = new RequiredIf.Model() { Value1 = "hello", Value2 = null };
            Assert.IsFalse(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void IsNotRequiredTest()
        {
            var model = new RequiredIf.Model() { Value1 = "goodbye" };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void IsNotRequiredWithValue1NullTest()
        {
            var model = new RequiredIf.Model() { Value1 = null };
            Assert.IsTrue(model.IsValid("Value2"));
        }
    }
}
