using System.Text.RegularExpressions;

namespace FoolProof.Core.Tests.E2eTests
{
    public abstract class GreaterThanTest: CompareBaseTest
    {
        protected override Regex PageTitleRegex() => new($@".+\s+[-]\s+GreaterThan\s+\({DataType}\)");

        protected override Uri PageUri() => new(new Uri(WebAppUrl), $"gt/{DataType}");

        protected override string Value2ValidationError => "Value2 must be greater than Value1";

        protected override string ValuePwnValidationError => "ValuePwn must be greater than Value1";

        [CustomTestMethod("Empty Values : Invalid")]
        public override async Task EmptyValues()
        {
            await EmptyValues(false);
        }

        [CustomTestMethod("Value2 < Value1 > ValuePwn  : Valid")]
        public override Task FormValidationSuccess()
        {
            return base.FormValidationSuccess();
        }

        [CustomTestMethod("Value2 > Value1 < ValuePwn : Invalid")]
        public override Task FormValidationFailure()
        {
            return base.FormValidationFailure();
        }

        [CustomTestMethod("Value1 == Value2 == ValuePwn : Invalid")]
        public virtual async Task SameValuesFails()
        {
            await LoadPage();

            var testValues = GetValues2PassValidation();
            testValues.Value2 = testValues.ValuePwn = testValues.Value1;
            await AssignTestValues(testValues);

            await CallClientValidation();
            await ExpectClientValidationFailed();

            await AssignTestValues(testValues, true);

            await CallServerValidation();
            await ExpectServerValidationFailed();
        }


        [TestClass]
        public class DateValues : GreaterThanTest
        {
            protected override string DataType => "Date";

            protected override TestValues GetValues2PassValidation()
            {
                return new(DateOnly.Parse("5/5/2005"), DateOnly.Parse("10/10/2010"), DateOnly.Parse("11/11/2020"), [
                   new(nameof(GreaterThan.DateModel.MinDate), DateOnly.Parse("05/05/2025"))
                ]);
            }

            protected override TestValues GetValues2FailsValidation()
            {
                return new (DateOnly.Parse("11/11/2020"), DateOnly.Parse("10/10/2010"), DateOnly.Parse("5/5/2005"), [
                   new(nameof(GreaterThan.DateModel.MinDate), DateOnly.Parse("01/01/2024"))
                ]);
            }
        }

        [TestClass]
        public class Int16Values : GreaterThanTest
        {
            protected override string DataType => "Int16";

            protected override TestValues GetValues2PassValidation()
            {
                return new(11, 55, 77, [
                   new(nameof(GreaterThan.Int16Model.MinValue), 2000)
                ]);
            }

            protected override TestValues GetValues2FailsValidation()
            {
                return new(999, 88, 22, [
                   new(nameof(GreaterThan.Int16Model.MinValue), 200)
                ]);
            }
        }

        [TestClass]
        public class TimeValues : GreaterThanTest
        {
            protected override string DataType => "Time";

            protected override TestValues GetValues2PassValidation()
            {
                return new(TimeSpan.Parse("08:00"), TimeSpan.Parse("14:30"), TimeSpan.Parse("22:00"), [
                   new(nameof(GreaterThan.TimeModel.MinTime), TimeSpan.Parse("06:30"))
                ]);
            }

            protected override TestValues GetValues2FailsValidation()
            {
                return new(TimeSpan.Parse("20:30"), TimeSpan.Parse("16:45"), TimeSpan.Parse("10:00"), [
                   new(nameof(GreaterThan.TimeModel.MinTime), TimeSpan.Parse("01:30"))
                ]);
            }
        }

        [TestClass]
        public class DateTimeValues : GreaterThanTest
        {
            protected override string DataType => "DateTime";

            protected override TestValues GetValues2PassValidation()
            {
                return new(
                    DateTime.Parse("05/05/2005 10:10"), 
                    DateTime.Parse("10/10/2010 12:12"), 
                    DateTime.Parse("11/11/2020 05:05"), 
                    [
                        new(nameof(GreaterThan.DateTimeModel.MinDateTime), DateTime.Parse("02/02/2026 16:30"))
                    ]
                );
            }

            protected override TestValues GetValues2FailsValidation()
            {
                return new(
                    DateTime.Parse("11/11/2020 10:10"), 
                    DateTime.Parse("11/11/2020 10:10"), 
                    DateTime.Parse("05/05/2005 08:08"), 
                    [
                        new(nameof(GreaterThan.DateTimeModel.MinDateTime), DateTime.Parse("01/01/2023 08:30"))
                    ]
                );
            }
        }
    }
}