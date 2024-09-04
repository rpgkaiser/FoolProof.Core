using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FoolProof.Core.Tests.UnitTests
{
    [TestClass()]
    public class RequiredIfRegExMatchAttributeTest
    {
        [TestMethod()]
        public void IsValidTest()
        {
            var model = new RequiredIfRegExMatch.Model() { Value1 = "8:30 AM", Value2 = "hello" };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void IsNotValidTest()
        {
            var model = new RequiredIfRegExMatch.Model() { Value1 = "8:30 AM", Value2 = "" };
            Assert.IsFalse(model.IsValid("Value2"));
        }
    }
}