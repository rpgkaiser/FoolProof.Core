using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FoolProof.Core.Tests.UnitTests
{
    [TestClass()]
    public class LessThanOrEqualToAttributeTest
    {
        [TestMethod()]
        public void DateIsValid()
        {
            var model = new LessThanOrEqualTo.DateModel() { Value1 = DateTime.Now, Value2 = DateTime.Now.AddDays(-1) };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void DateEqualIsValid()
        {
            var date = DateTime.Now;
            var model = new LessThanOrEqualTo.DateModel() { Value1 = date, Value2 = date };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void DateNullValuesIsValid()
        {
            var date = DateTime.Now;
            var model = new LessThanOrEqualTo.DateModel() { };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void DateIsNotValid()
        {
            var model = new LessThanOrEqualTo.DateModel() { Value1 = DateTime.Now, Value2 = DateTime.Now.AddDays(1) };
            Assert.IsFalse(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void DateWithNullsIsValid()
        {
            var model = new LessThanOrEqualTo.DateModel() { };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void DateWithValue1NullIsNotValid()
        {
            var model = new LessThanOrEqualTo.DateModel() { Value2 = DateTime.Now };
            Assert.IsFalse(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void DateWithValue2NullIsNotValid()
        {
            var model = new LessThanOrEqualTo.DateModel() { Value1 = DateTime.Now };
            Assert.IsFalse(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void Int16IsValid()
        {
            var model = new LessThanOrEqualTo.Int16Model() { Value1 = 120, Value2 = 12 };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void Int16EqualIsValid()
        {
            var model = new LessThanOrEqualTo.Int16Model() { Value1 = 12, Value2 = 12 };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void Int16IsNotValid()
        {
            var model = new LessThanOrEqualTo.Int16Model() { Value1 = 12, Value2 = 120 };
            Assert.IsFalse(model.IsValid("Value2"));
        }     
    }
}
