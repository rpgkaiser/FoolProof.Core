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

            protected override string ValuePwnValidationError => "ValuePwn must be not equal to Value1";

            protected override string DataType => "string";

            protected override string GetNotValidValue()
            {
                return "Any value is a valid value.";
            }

            protected override (string Value1, string Value2, string ValuePwn) GetValues2PassCompare()
            {
                return ("Value one", "Value two", "Value three");
            }

            protected override (string Value1, string Value2, string ValuePwn) GetValues2FailsCompare()
            {
                return ("Value one", "Value one", "Value one");
            }

            [CustomTestMethod("Empty Values : Invalid")]
            public override async Task EmptyValues()
            {
                await LoadPage();

                await ExpectValue1Empty();
                await ExpectValue2Empty();
                await ExpectValuePwnEmpty();

                await CallClientValidation();
                await ExpectClientValidationFailed();

                await ResetForm();

                await CallServerValidation();
                await ExpectServerValidationFailed();
            }

            [Ignore]
            public override Task InvalidValues()
            {
                return base.InvalidValues();
            }

            [CustomTestMethod("Value1 is Empty : Valid")]
            public override async Task Value1Empty()
            {
                await LoadPage();

                var value = GetValues2PassCompare().Value1;
                await ExpectValue1Empty();
                await AssignValue2(value);
                await AssignValuePwn(value);

                await CallClientValidation();
                await ExpectValidationSucceed();

                await ResetForm();
                await AssignValue2(value);
                await AssignValuePwn(value);

                await CallServerValidation();
                await ExpectValidationSucceed();
            }

            [CustomTestMethod("Value2 and ValuePwn are Empty : Valid")]
            public override async Task Value2Empty()
            {
                await LoadPage();

                var value = GetValues2PassCompare().Value1;
                await AssignValue1(value);
                await ExpectValue2Empty();
                await ExpectValuePwnEmpty();

                await CallClientValidation();
                await ExpectValidationSucceed();

                await ResetForm();
                await AssignValue1(value);
                await ExpectValue2Empty();
                await ExpectValuePwnEmpty();

                await CallServerValidation();
                await ExpectValidationSucceed();
            }

            [CustomTestMethod("Value2 != Value1 != ValuePwn : Valid")]
            public override Task CompareValuesPass()
            {
                return base.CompareValuesPass();
            }

            [CustomTestMethod("Value1 == Value2 == ValuePwn : Invalid")]
            public override Task CompareValuesFails()
            {
                return base.CompareValuesFails();
            }
        }
    }
}