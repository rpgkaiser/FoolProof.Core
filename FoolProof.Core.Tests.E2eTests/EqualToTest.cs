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

            [CustomTestMethod("Value1 == Value2 == ValuePwn : Valid")]
            public override Task FormValidationSuccess()
            {
                return base.FormValidationSuccess();
            }

            [CustomTestMethod("Value1 != Value2 != ValuePwn : Invalid")]
            public override Task FormValidationFailure()
            {
                return base.FormValidationFailure();
            }

            protected override CompareTestValues GetValues2PassValidation()
            {
                return new("Value one", "Value one", "Value one", [
                    new(nameof(EqualTo.Model.EqualToValue), 1000),
                    new(nameof(EqualTo.Model.EqualToTime), TimeSpan.Parse("10:30")),
                    new(nameof(EqualTo.Model.EqualToDateTime), DateTime.Parse("01/01/2025 06:30")),
                    new(nameof(EqualTo.Model.EqualToDate), DateOnly.Parse("01/01/2025")),
                    new(nameof(EqualTo.Model.TrueValue), true),
                    new(nameof(EqualTo.Model.FalseValue), false)
                ]);
            }

            protected override CompareTestValues GetValues2FailsValidation()
            {
                return new("Value one", "Value two", "Value three", [
                    new(nameof(EqualTo.Model.EmptyValue), "Any value"),
                    new(nameof(EqualTo.Model.EqualToValue), 100),
                    new(nameof(EqualTo.Model.EqualToTime), TimeSpan.Parse("11:50")),
                    new(nameof(EqualTo.Model.EqualToDateTime), DateTime.Parse("02/02/2025 08:30")),
                    new(nameof(EqualTo.Model.EqualToDate), DateOnly.Parse("02/02/2025")),
                    new(nameof(EqualTo.Model.TrueValue), false),
                    new(nameof(EqualTo.Model.FalseValue), true)
                ]);
            }
        }
    }
}