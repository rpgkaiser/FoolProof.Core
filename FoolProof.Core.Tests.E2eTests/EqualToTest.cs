using System.ComponentModel;
using System.Text.RegularExpressions;

namespace FoolProof.Core.Tests.E2eTests
{
    public class EqualToTest
    {
        [TestClass]
        public class Default : CompareBaseTest
        {
            protected override Regex PageTitleRegex() => new (@".+\s+[-]\s+EqualTo");

            protected override Uri PageUri() => new (new Uri(WebAppUrl), "eq2");

            protected override string Value2ValidationError => "Value2 must be equal to Value1";

            protected override string ValuePwnValidationError => "ValuePwn must be equal to Value1";

            protected override string DataType => "string";

            protected override string GetNotValidValue()
            {
                return "Any value is a valid value.";
            }

            protected override (string Value1, string Value2, string ValuePwn) GetValues2PassCompare()
            {
                return ("Value one", "Value one", "Value one");
            }

            protected override (string Value1, string Value2, string ValuePwn) GetValues2FailsCompare()
            {
                return ("Value one", "Value two", "Value three");
            }

            [CustomTestMethod("Empty Values : Valid")]
            public override async Task EmptyValues()
            {
                await LoadPage();

                await ExpectValue1Empty();
                await ExpectValue2Empty();
                await ExpectValuePwnEmpty();

                await CallClientValidation();
                await ExpectValidationSucceed();

                await ResetForm();

                await CallServerValidation();
                await ExpectValidationSucceed();
            }

            [Ignore]
            public override Task InvalidValues()
            {
                return base.InvalidValues();
            }

            [CustomTestMethod("Value1 == Value2 == ValuePwn : Valid")]
            public override Task CompareValuesPass()
            {
                return base.CompareValuesPass();
            }

            [CustomTestMethod("Value1 != Value2 != ValuePwn : Invalid")]
            public override Task CompareValuesFails()
            {
                return base.CompareValuesFails();
            }
        }
    }
}