using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FoolProof.Core.UnitTests
{
	[TestClass()]
    public class EqualToAttributeTest
    {
        private class Model : ModelBase<EqualToAttribute>
        {
            public string Value1 { get; set; }

            [EqualTo("Value1")]
            public string Value2 { get; set; }
        }

        private class ModelWithPassOnNull : ModelBase<EqualToAttribute>
        {
            public string Value1 { get; set; }

            [EqualTo("Value1", PassOnNull = true)]
            public string Value2 { get; set; }
        }

        [TestMethod()]
        public void IsValid()
        {
            var model = new Model() { Value1 = "hello", Value2 = "hello" };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void IsNotValid()
        {
            var model = new Model() { Value1 = "hello", Value2 = "goodbye" };
            Assert.IsFalse(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void IsValidWithNulls()
        {
            var model = new Model() { };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void IsNotValidWithValue1Null()
        {
            var model = new Model() { Value2 = "hello" };
            Assert.IsFalse(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void IsNotValidWithValue2Null()
        {
            var model = new Model() { Value1 = "hello" };
            Assert.IsFalse(model.IsValid("Value2"));
        }    

        [TestMethod()]
        public void IsValidWithValue1Null()
        {
            var model = new ModelWithPassOnNull() { Value2 = "hello" };
            Assert.IsTrue(model.IsValid("Value2"));            
        }

        [TestMethod()]
        public void IsValidWithValue2Null()
        {
            var model = new ModelWithPassOnNull() { Value1 = "hello" };
            Assert.IsTrue(model.IsValid("Value2"));
        }    
    }
}
