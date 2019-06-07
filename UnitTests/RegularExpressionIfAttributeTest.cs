using FoolProof.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace FoolProof.Core.UnitTests
{    
    [TestClass()]
    public class RegularExpressionIfAttributeTest
    {
        private class Model : ModelBase<RegularExpressionIfAttribute>
        {
            public bool Value1 { get; set; }

            [RegularExpressionIf("^ *(1[0-2]|0?[1-9]):[0-5][0-9] *(a|p|A|P)(m|M) *$", "Value1", true)]
            public string Value2 { get; set; }
        }

        [TestMethod()]
        public void IsValidTest()
        {
            var model = new Model() { Value1 = true, Value2 = "8:30 AM" };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void IsNotValidTest()
        {
            var model = new Model() { Value1 = true, Value2 = "not a time" };
            Assert.IsFalse(model.IsValid("Value2"));
        }    
    }
}
