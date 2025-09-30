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

            protected override CompareTestValues GetValues2PassValidation()
            {
                return new("Value one", "Value two", "Value three", [
                    new(nameof(NotEqualTo.Model.NotEmptyValue), "Any value"),
                    new(nameof(NotEqualTo.Model.NotEqualToValue), 100),
                    new(nameof(NotEqualTo.Model.NotEqualToTime), TimeSpan.Parse("12:45")),
                    new(nameof(NotEqualTo.Model.NotEqualToDateTime), DateTime.Parse("02/02/2025 09:15")),
                    new(nameof(NotEqualTo.Model.NotEqualToDate), DateOnly.Parse("02/02/2025")),
                ]);
            }

            protected override CompareTestValues GetValues2FailsValidation()
            {
                return new("Value one", "Value one", "Value one", [
                    new(nameof(NotEqualTo.Model.NotEqualToValue), 1000),
                    new(nameof(NotEqualTo.Model.NotEqualToTime), TimeSpan.Parse("10:30")),
                    new(nameof(NotEqualTo.Model.NotEqualToDateTime), DateTime.Parse("01/01/2025 06:30")),
                    new(nameof(NotEqualTo.Model.NotEqualToDate), DateOnly.Parse("01/01/2025")),
                ]);
            }

            [CustomTestMethod("Empty Values : Invalid")]
            public override async Task EmptyValues()
            {
                await EmptyValues(false);
            }

            [CustomTestMethod("Value1 is Empty : Valid")]
            public override async Task Value1Empty()
            {
                await LoadPage();

                var testValues = GetValues2PassValidation();
                await AssignTestValues(testValues, false, true, ["Value1"]);
                await ExpectValue1Empty();

                await CallClientValidation();
                await ExpectValidationSucceed();

                await AssignTestValues(testValues, true, true, ["Value1"]);
                await ExpectValue1Empty();

                await CallServerValidation();
                await ExpectValidationSucceed();
            }

            [CustomTestMethod("Value1, Value2 and ValuePwn are Empty : Invalid")]
            public override async Task Value2Empty()
            {
                await LoadPage();

                var testValues = GetValues2PassValidation();
                await AssignTestValues(testValues, false, true, ["Value1", "Value2", "ValuePwn"]);
                await ExpectValue1Empty();
                await ExpectValue2Empty();
                await ExpectValuePwnEmpty();

                await CallClientValidation();
                await ExpectClientValidationFailed();

                await AssignTestValues(testValues, true, true, ["Value1", "Value2", "ValuePwn"]);
                await ExpectValue1Empty();
                await ExpectValue2Empty();
                await ExpectValuePwnEmpty();

                await CallServerValidation();
                await ExpectServerValidationFailed();
            }

            [CustomTestMethod("Value2 != Value1 != ValuePwn : Valid")]
            public override Task FormValidationSuccess()
            {
                return base.FormValidationSuccess();
            }

            [CustomTestMethod("Value1 == Value2 == ValuePwn : Invalid")]
            public override Task FormValidationFailure()
            {
                return base.FormValidationFailure();
            }
        }
    }
}