using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FoolProof.Core.Tests.UnitTests
{
    [TestClass()]
    public class GreaterThanOrEqualToAttributeTest
    {
        [TestMethod()]
        public void DateIsValid()
        {
            var model = new GreaterThanOrEqualTo.DateModel() { Value1 = DateTime.Now, Value2 = DateTime.Now.AddDays(1) };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void DateEqualIsValid()
        {
            var date = DateTime.Now;
            var model = new GreaterThanOrEqualTo.DateModel() { Value1 = date, Value2 = date };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void DateNullValuesIsValid()
        {
            var date = DateTime.Now;
            var model = new GreaterThanOrEqualTo.DateModel() { };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void DateIsNotValid()
        {
            var model = new GreaterThanOrEqualTo.DateModel() { Value1 = DateTime.Now, Value2 = DateTime.Now.AddDays(-1) };
            Assert.IsFalse(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void DateWithNullsIsValid()
        {
            var model = new GreaterThanOrEqualTo.DateModel() { };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void DateWithValue1NullIsNotValid()
        {
            var model = new GreaterThanOrEqualTo.DateModel() { Value2 = DateTime.Now };
            Assert.IsFalse(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void DateWithValue2NullIsNotValid()
        {
            var model = new GreaterThanOrEqualTo.DateModel() { Value1 = DateTime.Now };
            Assert.IsFalse(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void DateWithValue1NullIsValid()
        {
            var model = new GreaterThanOrEqualTo.DateModelWithPassNull() { Value2 = DateTime.Now };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void DateWithValue2NullIsValid()
        {
            var model = new GreaterThanOrEqualTo.DateModelWithPassNull() { Value1 = DateTime.Now };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void Int16IsValid()
        {
            var model = new GreaterThanOrEqualTo.Int16Model() { Value1 = 12, Value2 = 120 };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void Int16EqualIsValid()
        {
            var model = new GreaterThanOrEqualTo.Int16Model() { Value1 = 12, Value2 = 12 };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void Int16IsNotValid()
        {
            var model = new GreaterThanOrEqualTo.Int16Model() { Value1 = 120, Value2 = 12 };
            Assert.IsFalse(model.IsValid("Value2"));
        }    
    }
}