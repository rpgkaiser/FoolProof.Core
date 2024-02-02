using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FoolProof.Core.Tests.UnitTests
{
    [TestClass()]
    public class RequiredIfNotRegExMatchAttributeTest
    {
        [TestMethod()]
        public void IsValidTest()
        {
            var model = new RequiredIfNotRegExMatch.Model() { Value1 = "not a time", Value2 = "hello" };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void IsNotValidTest()
        {
            var model = new RequiredIfNotRegExMatch.Model() { Value1 = "not a time", Value2 = "" };
            Assert.IsFalse(model.IsValid("Value2"));
        }
    }
}