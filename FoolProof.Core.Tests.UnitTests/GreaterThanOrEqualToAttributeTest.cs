using Microsoft.VisualStudio.TestTools.UnitTesting;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FoolProof.Core.Tests.UnitTests
{
    [TestClass()]
    public class GreaterThanOrEqualToAttributeTest
    {
        [TestMethod()]
        public void DateIsValid()
        {
            var model = new GreaterThanOrEqualTo.DateModel() { 
                Value1 = DateOnly.FromDateTime(DateTime.Now), 
                Value2 = DateOnly.FromDateTime(DateTime.Now.AddDays(1)) 
            };
            Assert.IsTrue(model.IsValid(nameof(model.Value2)));
        }

        [TestMethod()]
        public void DateEqualIsValid()
        {
            var date = DateTime.Now;
            var model = new GreaterThanOrEqualTo.DateModel() { 
                Value1 = DateOnly.FromDateTime(date), 
                Value2 = DateOnly.FromDateTime(date)
            };
            Assert.IsTrue(model.IsValid(nameof(model.Value2)));
        }

        [TestMethod()]
        public void DateNullValuesIsValid()
        {
            var date = DateTime.Now;
            var model = new GreaterThanOrEqualTo.DateModel() { };
            Assert.IsTrue(model.IsValid(nameof(model.Value2)));
        }

        [TestMethod()]
        public void DateIsNotValid()
        {
            var model = new GreaterThanOrEqualTo.DateModel() { 
                Value1 = DateOnly.FromDateTime(DateTime.Now),
                Value2 = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)) 
            };
            Assert.IsFalse(model.IsValid(nameof(model.Value2)));
        }

        [TestMethod()]
        public void DateWithNullsIsValid()
        {
            var model = new GreaterThanOrEqualTo.DateModel() { };
            Assert.IsTrue(model.IsValid(nameof(model.Value2)));
        }

        [TestMethod()]
        public void DateWithValue1NullIsNotValid()
        {
            var model = new GreaterThanOrEqualTo.DateModel() { 
                Value2 = DateOnly.FromDateTime(DateTime.Now) 
            };
            Assert.IsFalse(model.IsValid(nameof(model.Value2)));
        }

        [TestMethod()]
        public void DateWithValue2NullIsNotValid()
        {
            var model = new GreaterThanOrEqualTo.DateModel() { 
                Value1 = DateOnly.FromDateTime(DateTime.Now)
            };
            Assert.IsFalse(model.IsValid(nameof(model.Value2)));
        }

        [TestMethod()]
        public void DateWithValue1NullIsValid()
        {
            var model = new GreaterThanOrEqualTo.DateModel() { 
                ValuePwn = DateOnly.FromDateTime(DateTime.Now) 
            };
            Assert.IsTrue(model.IsValid(nameof(model.ValuePwn)));
        }

        [TestMethod()]
        public void DateWithValue2NullIsValid()
        {
            var model = new GreaterThanOrEqualTo.DateModel() { 
                Value1 = DateOnly.FromDateTime(DateTime.Now) 
            };
            Assert.IsTrue(model.IsValid(nameof(model.ValuePwn)));
        }

        [TestMethod()]
        public void Int16IsValid()
        {
            var model = new GreaterThanOrEqualTo.Int16Model() { Value1 = 12, Value2 = 120 };
            Assert.IsTrue(model.IsValid(nameof(model.Value2)));
        }

        [TestMethod()]
        public void Int16EqualIsValid()
        {
            var model = new GreaterThanOrEqualTo.Int16Model() { Value1 = 12, Value2 = 12 };
            Assert.IsTrue(model.IsValid(nameof(model.Value2)));
        }

        [TestMethod()]
        public void Int16IsNotValid()
        {
            var model = new GreaterThanOrEqualTo.Int16Model() { Value1 = 120, Value2 = 12 };
            Assert.IsFalse(model.IsValid(nameof(model.Value2)));
        }    
    }
}