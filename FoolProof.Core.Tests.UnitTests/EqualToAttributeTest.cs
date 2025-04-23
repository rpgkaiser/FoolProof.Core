using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FoolProof.Core.Tests.UnitTests
{
    [TestClass()]
    public partial class EqualToAttributeTest
    {
        [TestMethod()]
        public void IsValid()
        {
            var model = new EqualTo.Model();
            Assert.IsTrue(model.IsValid(nameof(model.Value2)));
            Assert.IsTrue(model.IsValid(nameof(model.ValuePwn)));
            Assert.IsTrue(model.IsValid(nameof(model.EmptyValue)));
            Assert.IsTrue(model.IsValid(nameof(model.FalseValue)));
            Assert.IsTrue(model.IsValid(nameof(model.TrueValue)));
            Assert.IsTrue(model.IsValid(nameof(model.EqualToDate)));
            Assert.IsTrue(model.IsValid(nameof(model.EqualToValue)));
            Assert.IsTrue(model.IsValid(nameof(model.EqualToTime)));
            Assert.IsTrue(model.IsValid(nameof(model.EqualToDateTime)));

            model = new EqualTo.Model() {
                Value1 = "hello",
                Value2 = "hello",
                ValuePwn = "hello",
                FalseValue = false,
                TrueValue = true,
                EqualToDate = DateOnly.Parse("01/01/2025"),
                EqualToValue = 1000,
                EqualToTime = TimeSpan.Parse("10:30"),
                EqualToDateTime = DateTime.Parse("01/01/2025 06:30")
            };
            Assert.IsTrue(model.IsValid(nameof(model.Value2)));

            model = new EqualTo.Model() { Value1 = "hello" };
            Assert.IsTrue(model.IsValid(nameof(model.ValuePwn)), "ValuePwn is valid if empty");

            model = new EqualTo.Model() { ValuePwn = "hello" };
            Assert.IsTrue(model.IsValid(nameof(model.ValuePwn)), "ValuePwn is valid if Value1 is empty");
        }

        [TestMethod()]
        public void IsNotValid()
        {
            var model = new EqualTo.Model() { 
                Value1 = "hello", 
                Value2 = "goodbye",
                ValuePwn = "other",
                EmptyValue = "not_empty",
                FalseValue = true,
                TrueValue = false,
                EqualToDate = DateOnly.Parse("02/02/2025"),
                EqualToValue = 100,
                EqualToTime = TimeSpan.Parse("12:50"),
                EqualToDateTime = DateTime.Parse("02/02/2025 08:00")
            };
            Assert.IsFalse(model.IsValid(nameof(model.Value2)));
            Assert.IsFalse(model.IsValid(nameof(model.ValuePwn)));
            Assert.IsFalse(model.IsValid(nameof(model.EmptyValue)));
            Assert.IsFalse(model.IsValid(nameof(model.FalseValue)));
            Assert.IsFalse(model.IsValid(nameof(model.TrueValue)));
            Assert.IsFalse(model.IsValid(nameof(model.EqualToDate)));
            Assert.IsFalse(model.IsValid(nameof(model.EqualToValue)));
            Assert.IsFalse(model.IsValid(nameof(model.EqualToTime)));
            Assert.IsFalse(model.IsValid(nameof(model.EqualToDateTime)));
        }
    }
}
