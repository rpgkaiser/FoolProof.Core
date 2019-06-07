using FoolProof.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace FoolProof.Core.UnitTests
{
    [TestClass()]
    public class GreaterThanAttributeTest
    {
        private class DateModel : ModelBase<GreaterThanAttribute>
        {
            public DateTime? Value1 { get; set; }

            [GreaterThan("Value1")]
            public DateTime? Value2 { get; set; }
        }

        private class DateModelWithPassOnNull : ModelBase<GreaterThanAttribute>
        {
            public DateTime? Value1 { get; set; }

            [GreaterThan("Value1", PassOnNull = true)]
            public DateTime? Value2 { get; set; }
        }

        private class Int16Model : ModelBase<GreaterThanAttribute>
        {
            public Int16 Value1 { get; set; }

            [GreaterThan("Value1")]
            public Int16 Value2 { get; set; }
        }

        [TestMethod()]
        public void DateIsValid()
        {
            var model = new DateModel() { Value1 = DateTime.Now, Value2 = DateTime.Now.AddDays(1) };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void DateIsNotValid()
        {
            var model = new DateModel() { Value1 = DateTime.Now, Value2 = DateTime.Now.AddDays(-1) };
            Assert.IsFalse(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void DateWithNullsIsNotValid()
        {
            var model = new DateModel() { };
            Assert.IsFalse(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void DateWithValue1NullIsNotValid()
        {
            var model = new DateModel() { Value2 = DateTime.Now };
            Assert.IsFalse(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void DateWithValue2NullIsNotValid()
        {
            var model = new DateModel() { Value1 = DateTime.Now };
            Assert.IsFalse(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void DateWithValue1NullIsValid()
        {
            var model = new DateModelWithPassOnNull() { Value2 = DateTime.Now };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void DateWithValue2NullIsValid()
        {
            var model = new DateModelWithPassOnNull() { Value1 = DateTime.Now };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void Int16IsValid()
        {
            var model = new Int16Model() { Value1 = 12, Value2 = 120 };
            Assert.IsTrue(model.IsValid("Value2"));
        }

        [TestMethod()]
        public void Int16IsNotValid()
        {
            var model = new Int16Model() { Value1 = 120, Value2 = 12 };
            Assert.IsFalse(model.IsValid("Value2"));
        }    
    }
}
