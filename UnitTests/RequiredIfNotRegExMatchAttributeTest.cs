using FoolProof.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace FoolProof.Core.UnitTests
{    
    [TestClass()]
    public class RequiredIfNotRegExMatchAttributeTest
    {
        private class Model : ModelBase<RequiredIfNotRegExMatchAttribute>
        {
            public string Value1 { get; set; }

            [RequiredIfNotRegExMatch("Value1", "^ *(1[0-2]|0?[1-9]):[0-5][0-9] *(a|p|A|P)(m|M) *$")]
            public string Value2 { get; set; }
        }

        [TestMethod()]
        public void IsValidTest()
        {
            var model = new Model() { Value1 = "not a time", Value2 = "hello" };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void IsNotValidTest()
        {
            var model = new Model() { Value1 = "not a time", Value2 = "" };
            Assert.IsFalse(model.IsValid("Value2"));
        }
    }
}
