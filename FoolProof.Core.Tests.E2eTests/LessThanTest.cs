using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace FoolProof.Core.Tests.E2eTests
{
    public abstract class LessThanTest: CompareBaseTest
    {
        protected override Regex PageTitleRegex() => new($@".+\s+[-]\s+LessThan\s+\({DataType}\)");

        protected override Uri PageUri() => new(new Uri(WebAppUrl), $"lt/{DataType}");

        protected override string Value2ValidationError => "Value2 must be less than Value1";

        protected override string ValuePwnValidationError => "ValuePwn must be less than Value1";

        [CustomTestMethod("Empty Values : Invalid")]
        public override async Task EmptyValues()
        {
            await EmptyValues(false);
        }

        [CustomTestMethod("ValuePwn < Value1 > Value2 : Valid")]
        public override Task FormValidationSuccess()
        {
            return base.FormValidationSuccess();
        }

        [CustomTestMethod("ValuePwn > Value1 < Value2 : Invalid")]
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
        public class DateValues : LessThanTest
        {
            protected override string DataType => "Date";

            protected override TestValues GetValues2PassValidation()
            {
                return new(
                    DateOnly.Parse("12/12/2020"), 
                    DateOnly.Parse("10/10/2010"), 
                    DateOnly.Parse("5/5/2005"),
                    [
                        new(nameof(LessThan.DateModel.MaxDate), DateOnly.Parse("01/01/2024"))
                    ]
                );
            }

            protected override TestValues GetValues2FailsValidation()
            {
                return new(
                    DateOnly.Parse("5/5/2005"), 
                    DateOnly.Parse("10/10/2010"), 
                    DateOnly.Parse("12/12/2020"),
                    [
                        new(nameof(LessThan.DateModel.MaxDate), DateOnly.Parse("08/08/2026"))
                    ]
                );
            }
        }

        [TestClass]
        public class Int16Values : LessThanTest
        {
            protected override string DataType => "Int16";

            protected override TestValues GetValues2PassValidation()
            {
                return new(222, 55, 11, [
                    new(nameof(LessThan.Int16Model.MaxValue), 600)
                ]);
            }

            protected override TestValues GetValues2FailsValidation()
            {
                return new(22, 88, 999, [
                    new(nameof(LessThan.Int16Model.MaxValue), 1600)
                ]);
            }
        }

        [TestClass]
        public class TimeValues : LessThanTest
        {
            protected override string DataType => "Time";

            protected override TestValues GetValues2PassValidation()
            {
                return new(
                    TimeSpan.Parse("20:15"), 
                    TimeSpan.Parse("15:30"), 
                    TimeSpan.Parse("10:00"), [
                        new(nameof(LessThan.TimeModel.MaxTime), TimeSpan.Parse("01:10"))
                    ]
                );
            }

            protected override TestValues GetValues2FailsValidation()
            {
                return new(
                    TimeSpan.Parse("08:15"), 
                    TimeSpan.Parse("12:00"), 
                    TimeSpan.Parse("18:30"), [
                        new(nameof(LessThan.TimeModel.MaxTime), TimeSpan.Parse("09:30"))
                    ]
                );
            }
        }

        [TestClass]
        public class DateTimeValues : LessThanTest
        {
            protected override string DataType => "DateTime";

            protected override TestValues GetValues2PassValidation()
            {
                return new(
                    DateTime.Parse("12/12/2020 10:10"),
                    DateTime.Parse("10/10/2010 11:15"),
                    DateTime.Parse("5/5/2005 08:20"),
                    [
                        new(nameof(LessThan.DateTimeModel.MaxDateTime), DateTime.Parse("01/01/2024 10:30"))
                    ]
                );
            }

            protected override TestValues GetValues2FailsValidation()
            {
                return new(
                    DateTime.Parse("5/5/2005"),
                    DateTime.Parse("10/10/2010"),
                    DateTime.Parse("12/12/2020"),
                    [
                        new(nameof(LessThan.DateTimeModel.MaxDateTime), DateTime.Parse("08/08/2026 12:40"))
                    ]
                );
            }
        }
    }
}