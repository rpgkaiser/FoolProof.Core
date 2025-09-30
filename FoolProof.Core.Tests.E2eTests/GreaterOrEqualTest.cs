using System.Text.RegularExpressions;

namespace FoolProof.Core.Tests.E2eTests
{
    public abstract class GreaterOrEqualTest : CompareBaseTest
    {
        protected override Regex PageTitleRegex() => new($@".+\s+[-]\s+GreaterThanOrEqualTo\s+\({DataType}\)");

        protected override Uri PageUri() => new(new Uri(WebAppUrl), $"ge2/{DataType}");

        protected override string Value2ValidationError => "Value2 must be greater than or equal to Value1.";

        protected override string ValuePwnValidationError => "ValuePwn must be greater than or equal to Value1.";

        [CustomTestMethod("ValuePwn <= Value1 >= Value2 : Valid")]
        public override Task FormValidationSuccess()
        {
            return base.FormValidationSuccess();
        }

        [CustomTestMethod("ValuePwn > Value1 < Value2 : Invalid")]
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
        public class DateValues : GreaterOrEqualTest
        {
            protected override string DataType => "Date";

            protected override CompareTestValues GetValues2PassValidation()
            {
                return new(DateOnly.Parse("05/05/2005"), DateOnly.Parse("10/10/2010"), DateOnly.Parse("11/11/2020"), [
                   new(nameof(GreaterThanOrEqualTo.DateModel.MinDate), DateOnly.Parse("05/05/2025"))
                ]);
            }

            protected override CompareTestValues GetValues2FailsValidation()
            {
                return new(DateOnly.Parse("11/11/2020"), DateOnly.Parse("10/10/2010"), DateOnly.Parse("5/5/2005"), [
                   new(nameof(GreaterThanOrEqualTo.DateModel.MinDate), DateOnly.Parse("01/01/2024"))
                ]);
            }
        }

        [TestClass]
        public class Int16Values : GreaterOrEqualTest
        {
            protected override string DataType => "Int16";

            protected override CompareTestValues GetValues2PassValidation()
            {
                return new(11, 55, 888, [
                    new(nameof(GreaterThanOrEqualTo.Int16Model.MinValue), 2000)    
                ]);
            }

            protected override CompareTestValues GetValues2FailsValidation()
            {
                return new(999, 88, 22, [
                    new(nameof(GreaterThanOrEqualTo.Int16Model.MinValue), 100)
                ]);
            }
        }

        [TestClass]
        public class TimeValues : GreaterOrEqualTest
        {
            protected override string DataType => "Time";

            protected override CompareTestValues GetValues2PassValidation()
            {
                return new(TimeSpan.Parse("08:00"), TimeSpan.Parse("12:00"), TimeSpan.Parse("16:00"), [
                    new(nameof(GreaterThanOrEqualTo.TimeModel.MinTime), TimeSpan.Parse("06:30"))
                ]);
            }

            protected override CompareTestValues GetValues2FailsValidation()
            {
                return new(TimeSpan.Parse("20:30"), TimeSpan.Parse("14:00"), TimeSpan.Parse("10:00"), [
                    new(nameof(GreaterThanOrEqualTo.TimeModel.MinTime), TimeSpan.Parse("02:00"))
                ]);
            }
        }

        [TestClass]
        public class DateTimeValues : GreaterOrEqualTest
        {
            protected override string DataType => "DateTime";

            protected override CompareTestValues GetValues2PassValidation()
            {
                return new(DateTime.Parse("05/05/2005 08:00"), DateTime.Parse("10/10/2010 12:00"), DateTime.Parse("11/11/2020 16:00"), [
                    new(nameof(GreaterThanOrEqualTo.DateTimeModel.MinDateTime), DateTime.Parse("03/03/2025 14:00"))
                ]);
            }

            protected override CompareTestValues GetValues2FailsValidation()
            {
                return new(DateTime.Parse("05/05/2005 20:30"), DateTime.Parse("02/02/2002 14:00"), DateTime.Parse("03/03/2003 10:00"), [
                    new(nameof(GreaterThanOrEqualTo.DateTimeModel.MinDateTime), DateTime.Parse("02/02/2024 08:00"))
                ]);
            }
        }
    }
}