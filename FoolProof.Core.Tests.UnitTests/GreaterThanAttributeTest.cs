using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FoolProof.Core.Tests.UnitTests
{
    [TestClass()]
    public class GreaterThanAttributeTest
    {
        [TestMethod()]
        public void DateIsValid()
        {
            var model = new GreaterThan.DateModel() { Value1 = DateTime.Now, Value2 = DateTime.Now.AddDays(1) };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void DateIsNotValid()
        {
            var model = new GreaterThan.DateModel() { Value1 = DateTime.Now, Value2 = DateTime.Now.AddDays(-1) };
            Assert.IsFalse(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void DateWithNullsIsNotValid()
        {
            var model = new GreaterThan.DateModel() { };
            Assert.IsFalse(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void DateWithValue1NullIsNotValid()
        {
            var model = new GreaterThan.DateModel() { Value2 = DateTime.Now };
            Assert.IsFalse(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void DateWithValue2NullIsNotValid()
        {
            var model = new GreaterThan.DateModel() { Value1 = DateTime.Now };
            Assert.IsFalse(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void DateWithValue1NullIsValid()
        {
            var model = new GreaterThan.DateModelWithPassOnNull() { Value2 = DateTime.Now };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void DateWithValue2NullIsValid()
        {
            var model = new GreaterThan.DateModelWithPassOnNull() { Value1 = DateTime.Now };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void Int16IsValid()
        {
            var model = new GreaterThan.Int16Model() { Value1 = 12, Value2 = 120 };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void Int16IsNotValid()
        {
            var model = new GreaterThan.Int16Model() { Value1 = 120, Value2 = 12 };
            Assert.IsFalse(model.IsValid("Value2"));
        }    
    }
}
