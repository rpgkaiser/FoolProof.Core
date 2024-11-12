using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FoolProof.Core.Tests.UnitTests
{
    [TestClass()]
    public partial class PredicateAttributeTest
    {
        [TestMethod()]
        public void IsValid()
        {
            var model = new Predicate.Model() { 
                Value1 = 10, 
                Value2 = 20,
                Value3 = 30,
                Value4 = 40,
                Value5 = 10,
                Value6 = 50,
            };
            Assert.IsTrue(model.IsValid(nameof(model.Value5)));

            model.Value5 = 25;
            Assert.IsTrue(model.IsValid(nameof(model.Value5)));
        }

        [TestMethod()]
        public void IsNotValid()
        {
            var model = new Predicate.Model() { 
                Value1 = 10, 
                Value2 = 20,
                Value3 = 30,
                Value4 = 40,
                Value5 = 40,
                Value6 = 10
            };
            Assert.IsFalse(model.IsValid(nameof(model.Value5)));
        }

        [TestMethod()]
        public void WithNulls()
        {
            var model = new Predicate.Model() { };
            Assert.IsTrue(model.IsValid(nameof(model.Value5)));
            Assert.IsFalse(model.IsValid(nameof(model.Value6)));
        }    
    }
}
