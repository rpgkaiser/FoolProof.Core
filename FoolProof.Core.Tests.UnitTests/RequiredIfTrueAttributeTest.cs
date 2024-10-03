using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FoolProof.Core.Tests.UnitTests
{
    [TestClass()]
    public class RequiredIfTrueAttributeTest
    {
        [TestMethod()]
        public void IsValidTest()
        {
            var model = new RequiredIfTrue.Model() { Value1 = true, Value2 = "hello" };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void IsNotValidTest()
        {
            var model = new RequiredIfTrue.Model() { Value1 = true, Value2 = "" };
            Assert.IsFalse(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void IsNotValidWithValue2NullTest()
        {
            var model = new RequiredIfTrue.Model() { Value1 = true, Value2 = null };
            Assert.IsFalse(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void IsNotRequiredTest()
        {
            var model = new RequiredIfTrue.Model() { Value1 = false, Value2 = "" };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void IsNotRequiredWithValue1NullTest()
        {
            var model = new RequiredIfTrue.Model() { Value1 = null, Value2 = "" };
            Assert.IsTrue(model.IsValid("Value2"));
        }
    }
}