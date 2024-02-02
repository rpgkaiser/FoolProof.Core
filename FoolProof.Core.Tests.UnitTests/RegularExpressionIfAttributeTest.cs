using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FoolProof.Core.Tests.UnitTests
{
    [TestClass()]
    public class RegularExpressionIfAttributeTest
    {
        [TestMethod()]
        public void IsValidTest()
        {
            var model = new RegularExpressionIf.Model() { Value1 = true, Value2 = "8:30 AM" };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void IsNotValidTest()
        {
            var model = new RegularExpressionIf.Model() { Value1 = true, Value2 = "not a time" };
            Assert.IsFalse(model.IsValid("Value2"));
        }    
    }
}