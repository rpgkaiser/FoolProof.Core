using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FoolProof.Core.Tests.UnitTests
{
    [TestClass()]
    public partial class InAttributeTest
    {
        [TestMethod()]
        public void IsValid()
        {
            var singModel = new In.SingleValueModel() {
                Value1 = "Text one",
                Value2 = "Text one",
                ValuePwn = "Text one"
            };
            Assert.IsTrue(singModel.IsValid(nameof(singModel.Value2)));
            Assert.IsTrue(singModel.IsValid(nameof(singModel.ValuePwn)));
            singModel = new In.SingleValueModel() {
                Value1 = 100,
                Value2 = 100,
                ValuePwn = 100
            };
            Assert.IsTrue(singModel.IsValid(nameof(singModel.Value2)));
            Assert.IsTrue(singModel.IsValid(nameof(singModel.ValuePwn)));

            var dateModel = new In.DateTimeListModel() { 
                Value1 = new[] { 
                    DateTime.ParseExact("05/10/2020", "dd/MM/yyyy", null),
                    DateTime.ParseExact("15/12/2005", "dd/MM/yyyy", null),
                    DateTime.ParseExact("20/06/2010", "dd/MM/yyyy", null),
                    DateTime.ParseExact("10/09/2024", "dd/MM/yyyy", null)
                }, 
                Value2 = DateTime.ParseExact("20/06/2010", "dd/MM/yyyy", null),
                ValuePwn = DateTime.ParseExact("10/09/2024", "dd/MM/yyyy", null)
            };
            Assert.IsTrue(dateModel.IsValid(nameof(dateModel.Value2)));
            Assert.IsTrue(dateModel.IsValid(nameof(dateModel.ValuePwn)));

            var int16Model = new In.In16ListModel() { 
                Value1 = new Int16[] { 1, 200, 400, 500, 600 }, 
                Value2 = 200,
                ValuePwn = 400
            };
            Assert.IsTrue(int16Model.IsValid(nameof(int16Model.Value2)));
            Assert.IsTrue(int16Model.IsValid(nameof(int16Model.ValuePwn)));
        }

        [TestMethod()]
        public void IsNotValid()
        {
            var singModel = new In.SingleValueModel() {
                Value1 = "Text one",
                Value2 = "Text two",
                ValuePwn = "Text three"
            };
            Assert.IsFalse(singModel.IsValid(nameof(singModel.Value2)));
            Assert.IsFalse(singModel.IsValid(nameof(singModel.ValuePwn)));
            singModel = new In.SingleValueModel() {
                Value1 = 100,
                Value2 = 200,
                ValuePwn = 300
            };
            Assert.IsFalse(singModel.IsValid(nameof(singModel.Value2)));
            Assert.IsFalse(singModel.IsValid(nameof(singModel.ValuePwn)));

            var dateModel = new In.DateTimeListModel() { 
                Value1 = new[] { 
                    DateTime.ParseExact("05/10/2020", "dd/MM/yyyy", null),
                    DateTime.ParseExact("15/12/2005", "dd/MM/yyyy", null),
                    DateTime.ParseExact("20/06/2010", "dd/MM/yyyy", null),
                    DateTime.ParseExact("10/09/2024", "dd/MM/yyyy", null)
                }, 
                Value2 = DateTime.ParseExact("11/11/2011", "dd/MM/yyyy", null),
                ValuePwn = DateTime.ParseExact("01/01/1980", "dd/MM/yyyy", null)
            };
            Assert.IsFalse(dateModel.IsValid(nameof(dateModel.Value2)));
            Assert.IsFalse(dateModel.IsValid(nameof(dateModel.ValuePwn)));

            var int16Model = new In.In16ListModel() { 
                Value1 = new Int16[] { 1, 200, 400, 500, 600 }, 
                Value2 = 800,
                ValuePwn = 900
            };
            Assert.IsFalse(int16Model.IsValid(nameof(int16Model.Value2)));
            Assert.IsFalse(int16Model.IsValid(nameof(int16Model.ValuePwn)));
        }

        [TestMethod()]
        public void IsValidWithNulls()
        {
            var singModel = new In.SingleValueModel() {
                Value1 = null,
                Value2 = null,
                ValuePwn = null
            };
            Assert.IsTrue(singModel.IsValid(nameof(singModel.Value2)));
            Assert.IsTrue(singModel.IsValid(nameof(singModel.ValuePwn)));
            
            var dateModel = new In.DateTimeListModel() { 
                Value1 = null, 
                Value2 = null,
                ValuePwn = null
            };
            Assert.IsTrue(dateModel.IsValid(nameof(dateModel.Value2)));
            Assert.IsTrue(dateModel.IsValid(nameof(dateModel.ValuePwn)));

            var int16Model = new In.In16ListModel() { 
                Value1 = null, 
                Value2 = null,
                ValuePwn = null
            };
            Assert.IsTrue(int16Model.IsValid(nameof(int16Model.Value2)));
            Assert.IsTrue(int16Model.IsValid(nameof(int16Model.ValuePwn)));
        }

        [TestMethod()]
        public void WithValue1Null()
        {
            var singModel = new In.SingleValueModel() {
                Value1 = null,
                Value2 = "Text two",
                ValuePwn = "Text three"
            };
            Assert.IsFalse(singModel.IsValid(nameof(singModel.Value2)));
            Assert.IsTrue(singModel.IsValid(nameof(singModel.ValuePwn)));
            singModel = new In.SingleValueModel() {
                Value1 = null,
                Value2 = 200,
                ValuePwn = 300
            };
            Assert.IsFalse(singModel.IsValid(nameof(singModel.Value2)));
            Assert.IsTrue(singModel.IsValid(nameof(singModel.ValuePwn)));

            var dateModel = new In.DateTimeListModel() { 
                Value1 = null, 
                Value2 = DateTime.ParseExact("11/11/2011", "dd/MM/yyyy", null),
                ValuePwn = DateTime.ParseExact("01/01/1980", "dd/MM/yyyy", null)
            };
            Assert.IsFalse(dateModel.IsValid(nameof(dateModel.Value2)));
            Assert.IsTrue(dateModel.IsValid(nameof(dateModel.ValuePwn)));

            var int16Model = new In.In16ListModel() { 
                Value1 = null, 
                Value2 = 800,
                ValuePwn = 900
            };
            Assert.IsFalse(int16Model.IsValid(nameof(int16Model.Value2)));
            Assert.IsTrue(int16Model.IsValid(nameof(int16Model.ValuePwn)));
        }

        [TestMethod()]
        public void IsNotValidWithValue2Null()
        {
            var singModel = new In.SingleValueModel()
            {
                Value1 = "Text one",
                Value2 = null,
                ValuePwn = "Text one"
            };
            Assert.IsFalse(singModel.IsValid(nameof(singModel.Value2)));
            Assert.IsTrue(singModel.IsValid(nameof(singModel.ValuePwn)));
            singModel = new In.SingleValueModel()
            {
                Value1 = 100,
                Value2 = null,
                ValuePwn = 100
            };
            Assert.IsFalse(singModel.IsValid(nameof(singModel.Value2)));
            Assert.IsTrue(singModel.IsValid(nameof(singModel.ValuePwn)));

            var dateModel = new In.DateTimeListModel()
            {
                Value1 = new[] {
                    DateTime.ParseExact("05/10/2020", "dd/MM/yyyy", null),
                    DateTime.ParseExact("15/12/2005", "dd/MM/yyyy", null),
                    DateTime.ParseExact("20/06/2010", "dd/MM/yyyy", null),
                    DateTime.ParseExact("10/09/2024", "dd/MM/yyyy", null)
                },
                Value2 = null,
                ValuePwn = DateTime.ParseExact("15/12/2005", "dd/MM/yyyy", null)
            };
            Assert.IsFalse(dateModel.IsValid(nameof(dateModel.Value2)));
            Assert.IsTrue(dateModel.IsValid(nameof(dateModel.ValuePwn)));

            var int16Model = new In.In16ListModel()
            {
                Value1 = new Int16[] { 1, 200, 400, 500, 550 },
                Value2 = null,
                ValuePwn = 500
            };
            Assert.IsFalse(int16Model.IsValid(nameof(int16Model.Value2)));
            Assert.IsTrue(int16Model.IsValid(nameof(int16Model.ValuePwn)));
        }    

        [TestMethod()]
        public void IsValidWithValuePwnNull()
        {
            var singModel = new In.SingleValueModel()
            {
                Value1 = "Text one",
                Value2 = "Text one",
                ValuePwn = null
            };
            Assert.IsTrue(singModel.IsValid(nameof(singModel.Value2)));
            Assert.IsTrue(singModel.IsValid(nameof(singModel.ValuePwn)));
            singModel = new In.SingleValueModel()
            {
                Value1 = 100,
                Value2 = 100,
                ValuePwn = null
            };
            Assert.IsTrue(singModel.IsValid(nameof(singModel.Value2)));
            Assert.IsTrue(singModel.IsValid(nameof(singModel.ValuePwn)));

            var dateModel = new In.DateTimeListModel()
            {
                Value1 = new[] {
                    DateTime.ParseExact("05/10/2020", "dd/MM/yyyy", null),
                    DateTime.ParseExact("15/12/2005", "dd/MM/yyyy", null),
                    DateTime.ParseExact("20/06/2010", "dd/MM/yyyy", null),
                    DateTime.ParseExact("10/09/2024", "dd/MM/yyyy", null)
                },
                Value2 = DateTime.ParseExact("10/09/2024", "dd/MM/yyyy", null),
                ValuePwn = null
            };
            Assert.IsTrue(dateModel.IsValid(nameof(dateModel.Value2)));
            Assert.IsTrue(dateModel.IsValid(nameof(dateModel.ValuePwn)));

            var int16Model = new In.In16ListModel()
            {
                Value1 = new Int16[] { 1, 200, 400, 500, 600 },
                Value2 = 400,
                ValuePwn = null
            };
            Assert.IsTrue(int16Model.IsValid(nameof(int16Model.Value2)));
            Assert.IsTrue(int16Model.IsValid(nameof(int16Model.ValuePwn)));
        }    
    }
}
