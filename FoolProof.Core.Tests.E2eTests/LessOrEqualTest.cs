using System.Text.RegularExpressions;

namespace FoolProof.Core.Tests.E2eTests
{
    public abstract class LessOrEqualTest : CompareBaseTest
    {
        protected override Regex PageTitleRegex() => new($@".+\s+[-]\s+LessThanOrEqualTo\s+\({DataType}\)");

        protected override Uri PageUri() => new(new Uri(WebAppUrl), $"le2/{DataType}");

        protected override string Value2ValidationError => "Value2 must be less than or equal to Value1.";

        protected override string ValuePwnValidationError => "ValuePwn must be less than or equal to Value1.";

        [CustomTestMethod("ValuePwn <= Value1 >= Value2  : Valid")]
        public override Task FormValidationSuccess()
        {
            return base.FormValidationSuccess();
        }

        [CustomTestMethod("ValuePwn > Value1 < Value2  : Invalid")]
        public override Task FormValidationFailure()
        {
            return base.FormValidationFailure();
        }

        [CustomTestMethod("Value1 == Value2 == ValuePwn : Valid")]
        public virtual async Task SameValuesPass()
        {
            await LoadPage();

            var testValues = GetValues2PassValidation();
            testValues.Value2 = testValues.ValuePwn = testValues.Value1;
            await AssignTestValues(testValues);

            await CallClientValidation();
            await ExpectValidationSucceed();

            await AssignTestValues(testValues, true);

            await CallServerValidation();
            await ExpectValidationSucceed();
        }


        [TestClass]
        public class DateValues : LessOrEqualTest
        {
            protected override string DataType => "Date";

            protected override TestValues GetValues2PassValidation()
            {
                return new(
                    DateOnly.Parse("12/12/2020"), 
                    DateOnly.Parse("10/10/2010"), 
                    DateOnly.Parse("05/05/2005"), 
                    [
                        new(nameof(LessThanOrEqualTo.DateModel.MaxDate), DateOnly.Parse("01/01/2024"))
                    ]
                );
            }

            protected override TestValues GetValues2FailsValidation()
            {
                return new(
                    DateOnly.Parse("05/05/2005"), 
                    DateOnly.Parse("10/10/2010"), 
                    DateOnly.Parse("12/12/2020"),
                    [
                        new(nameof(LessThanOrEqualTo.DateModel.MaxDate), DateOnly.Parse("06/06/2026"))
                    ]
                );
            }
        }

        [TestClass]
        public class Int16Values : LessOrEqualTest
        {
            protected override string DataType => "Int16";

            protected override TestValues GetValues2PassValidation()
            {
                return new(222, 55, 11, [
                    new(nameof(LessThanOrEqualTo.Int16Model.MaxValue), 300)
                ]);
            }

            protected override TestValues GetValues2FailsValidation()
            {
                return new(22, 88, 111, [
                    new(nameof(LessThanOrEqualTo.Int16Model.MaxValue), 1300)
                ]);
            }
        }

        [TestClass]
        public class TimeValues : LessOrEqualTest
        {
            protected override string DataType => "Time";

            protected override TestValues GetValues2PassValidation()
            {
                return new(
                    TimeSpan.Parse("22:00"), 
                    TimeSpan.Parse("08:00"), 
                    TimeSpan.Parse("02:00"), [
                        new(nameof(LessThanOrEqualTo.TimeModel.MaxTime), TimeSpan.Parse("02:15"))
                    ]
                );
            }

            protected override TestValues GetValues2FailsValidation()
            {
                return new(
                    TimeSpan.Parse("10:00"), 
                    TimeSpan.Parse("18:30"), 
                    TimeSpan.Parse("23:30"), [
                        new(nameof(LessThanOrEqualTo.TimeModel.MaxTime), TimeSpan.Parse("14:40"))
                    ]
                );
            }
        }

        [TestClass]
        public class DateTimeValues : LessOrEqualTest
        {
            protected override string DataType => "DateTime";

            protected override TestValues GetValues2PassValidation()
            {
                return new(
                    DateTime.Parse("12/12/2020 10:30"),
                    DateTime.Parse("10/10/2010 12:40"),
                    DateTime.Parse("05/05/2005 18:10"),
                    [
                        new(nameof(LessThanOrEqualTo.DateTimeModel.MaxDateTime), DateTime.Parse("01/01/2024 08:25"))
                    ]
                );
            }

            protected override TestValues GetValues2FailsValidation()
            {
                return new(
                    DateTime.Parse("5/5/2005 12:00"),
                    DateTime.Parse("10/10/2010 08:00"),
                    DateTime.Parse("12/12/2020 18:00"),
                    [
                        new(nameof(LessThanOrEqualTo.DateTimeModel.MaxDateTime), DateTime.Parse("06/06/2026 11:50"))
                    ]
                );
            }
        }
    }
}