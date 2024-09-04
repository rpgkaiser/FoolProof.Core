using System.Text.RegularExpressions;

namespace FoolProof.Core.Tests.E2eTests
{
    public class NotEqualToTest
    {
        [TestClass]
        public class Default : CompareBaseTest
        {
            protected override Regex PageTitleRegex() => new (@".+\s+[-]\s+NotEqualTo");

            protected override Uri PageUri() => new (new Uri(WebAppUrl), "neq2");

            protected override string Value2ValidationError => "Value2 must be not equal to Value1";

            protected override string DataType => "string";

            protected override string GetNotValidValue()
            {
                return "Any value is a valid value.";
            }

            protected override (string Value1, string Value2) GetValues2PassCompare()
            {
                return ("Value one", "Value two");
            }

            protected override (string Value1, string Value2) GetValues2FailsCompare()
            {
                return ("Value one", "Value one");
            }

            [Ignore]
            public override Task InvalidValues()
            {
                return base.InvalidValues();
            }

            [TestMethod("Value1 is Empty : Valid")]
            public override async Task Value1Empty()
            {
                await LoadPage();

                var value = GetValues2PassCompare().Value1;
                await ExpectValue1Empty();
                await AssignValue2(value);

                await CallClientValidation();
                await ExpectValidationSucceed();

                await ResetForm();
                await AssignValue2(value);

                await CallServerValidation();
                await ExpectValidationSucceed();
            }

            [TestMethod("Value2 is Empty : Valid")]
            public override async Task Value2Empty()
            {
                await LoadPage();

                var value = GetValues2PassCompare().Value1;
                await AssignValue1(value);
                await ExpectValue2Empty();

                await CallClientValidation();
                await ExpectValidationSucceed();

                await ResetForm();
                await AssignValue1(value);

                await CallServerValidation();
                await ExpectValidationSucceed();
            }

            [TestMethod("Value2 != Value1 : Valid")]
            public override Task CompareValuesPass()
            {
                return base.CompareValuesPass();
            }

            [TestMethod("Value1 == Value2 : Invalid")]
            public override Task CompareValuesFails()
            {
                return base.CompareValuesFails();
            }
        }
    }
}