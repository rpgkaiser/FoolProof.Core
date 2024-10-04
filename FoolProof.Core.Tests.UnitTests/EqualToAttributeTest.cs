using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FoolProof.Core.Tests.UnitTests
{
    [TestClass()]
    public partial class EqualToAttributeTest
    {
        [TestMethod()]
        public void IsValid()
        {
            var model = new EqualTo.Model() { Value1 = "hello", Value2 = "hello" };
            Assert.IsTrue(model.IsValid(nameof(model.Value2)));
        }

        [TestMethod()]
        public void IsNotValid()
        {
            var model = new EqualTo.Model() { Value1 = "hello", Value2 = "goodbye" };
            Assert.IsFalse(model.IsValid(nameof(model.Value2)));
        }

        [TestMethod()]
        public void IsValidWithNulls()
        {
            var model = new EqualTo.Model() { };
            Assert.IsTrue(model.IsValid(nameof(model.Value2)));
        }

        [TestMethod()]
        public void IsNotValidWithValue1Null()
        {
            var model = new EqualTo.Model() { Value2 = "hello" };
            Assert.IsFalse(model.IsValid(nameof(model.Value2)));
        }

        [TestMethod()]
        public void IsNotValidWithValue2Null()
        {
            var model = new EqualTo.Model() { Value1 = "hello" };
            Assert.IsFalse(model.IsValid(nameof(model.Value2)));
        }    

        [TestMethod()]
        public void IsValidWithValue1Null()
        {
            var model = new EqualTo.Model() { ValuePwn = "hello" };
            Assert.IsTrue(model.IsValid(nameof(model.ValuePwn)));            
        }

        [TestMethod()]
        public void IsValidWithValuePwnNull()
        {
            var model = new EqualTo.Model() { ValuePwn = "hello" };
            Assert.IsTrue(model.IsValid(nameof(model.Value2)));
        }    
    }
}
