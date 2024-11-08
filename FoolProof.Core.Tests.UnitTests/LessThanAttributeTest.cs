using Microsoft.VisualStudio.TestTools.UnitTesting;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FoolProof.Core.Tests.UnitTests
{
    [TestClass()]
    public class LessThanAttributeTest
    {
        [TestMethod()]
        public void DateIsValid()
        {
            var model = new LessThan.DateModel() { 
                Value1 = DateOnly.FromDateTime(DateTime.Now), 
                Value2 = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)) 
            };
            Assert.IsTrue(model.IsValid(nameof(model.Value2)));
        }

        [TestMethod()]
        public void DateIsNotValid()
        {
            var model = new LessThan.DateModel() {
                Value1 = DateOnly.FromDateTime(DateTime.Now), 
                Value2 = DateOnly.FromDateTime(DateTime.Now.AddDays(1)) 
            };
            Assert.IsFalse(model.IsValid(nameof(model.Value2)));
        }

        [TestMethod()]
        public void DateWithNullsIsNotValid()
        {
            var model = new LessThan.DateModel() { };
            Assert.IsFalse(model.IsValid(nameof(model.Value2)));
        }

        [TestMethod()]
        public void DateWithValue1NullIsNotValid()
        {
            var model = new LessThan.DateModel() { 
                Value2 = DateOnly.FromDateTime(DateTime.Now)
            };
            Assert.IsFalse(model.IsValid(nameof(model.Value2)));
        }

        [TestMethod()]
        public void DateWithValue2NullIsNotValid()
        {
            var model = new LessThan.DateModel() { 
                Value1 = DateOnly.FromDateTime(DateTime.Now) 
            };
            Assert.IsFalse(model.IsValid(nameof(model.Value2)));
        }

        [TestMethod()]
        public void DateWithValue1NullIsValid()
        {
            var model = new LessThan.DateModel() { 
                Value1 = DateOnly.FromDateTime(DateTime.Now) 
            };
            Assert.IsTrue(model.IsValid(nameof(model.ValuePwn)));
        }

        [TestMethod()]
        public void DateWithValue2NullIsValid()
        {
            var model = new LessThan.DateModel() { 
                Value1 = DateOnly.FromDateTime(DateTime.Now) 
            };
            Assert.IsTrue(model.IsValid(nameof(model.ValuePwn)));
        }  

        [TestMethod()]
        public void Int16IsValid()
        {
            var model = new LessThan.Int16Model() { Value1 = 120, Value2 = 12 };
            Assert.IsTrue(model.IsValid(nameof(model.Value2)));
        }

        [TestMethod()]
        public void Int16IsNotValid()
        {
            var model = new LessThan.Int16Model() { Value1 = 12, Value2 = 120 };
            Assert.IsFalse(model.IsValid(nameof(model.Value2)));
        }    
    }
}
