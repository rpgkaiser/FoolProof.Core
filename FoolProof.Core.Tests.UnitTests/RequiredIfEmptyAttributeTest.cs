using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FoolProof.Core.Tests.UnitTests
{
    [TestClass()]
    public class RequiredIfEmptyAttributeTest
    {
        [TestMethod()]
        public void IsValidTest()
        {
            var model = new RequiredIfEmpty.Model() { Value1 = "", Value2 = "hello" };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void IsValidWithValue1NullTest()
        {
            var model = new RequiredIfEmpty.Model() { Value1 = null, Value2 = "hello" };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void IsNotRequiredTest()
        {
            var model = new RequiredIfEmpty.Model() { Value1 = "hello", Value2 = "" };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void IsNotRequiredWithValue2NullTest()
        {
            var model = new RequiredIfEmpty.Model() { Value1 = "hello", Value2 = null };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void IsNotValidTest()
        {
            var model = new RequiredIfEmpty.Model() { Value1 = "", Value2 = "" };
            Assert.IsFalse(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void IsNotValidWithvalue1NullTest()
        {
            var model = new RequiredIfEmpty.Model() { Value1 = null, Value2 = "" };
            Assert.IsFalse(model.IsValid("Value2"));
        }    
    }
}
