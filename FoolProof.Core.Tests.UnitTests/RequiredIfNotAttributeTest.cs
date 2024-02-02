using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FoolProof.Core.Tests.UnitTests
{
    [TestClass()]
    public class RequiredIfNotAttributeTest
    {
        [TestMethod()]
        public void IsValidTest()
        {
            var model = new RequiredIfNot.Model() { Value1 = "goodbye", Value2 = "hello" };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void IsNotValidTest()
        {
            var model = new RequiredIfNot.Model() { Value1 = "goodbye", Value2 = "" };
            Assert.IsFalse(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void IsNotValidWithValue2NullTest()
        {
            var model = new RequiredIfNot.Model() { Value1 = "goodbye", Value2 = null };
            Assert.IsFalse(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void IsNotRequiredTest()
        {
            var model = new RequiredIfNot.Model() { Value1 = "hello" };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void IsRequiredWithValue1NullTest()
        {
            var model = new RequiredIfNot.Model() { Value1 = null };
            Assert.IsFalse(model.IsValid("Value2"));
        }
    }
}
