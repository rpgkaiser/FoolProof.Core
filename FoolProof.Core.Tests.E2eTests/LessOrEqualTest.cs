using System.Text.RegularExpressions;

namespace FoolProof.Core.Tests.E2eTests
{
    public abstract class LessOrEqualTest : CompareBaseTest
    {
        protected override Regex PageTitleRegex() => new($@".+\s+[-]\s+LessThanOrEqualTo\s+\({DataType}\)");

        protected override Uri PageUri() => new(new Uri(WebAppUrl), $"ge2/{DataType}");

        protected override string Value2ValidationError => "Value2 must be less than or equal to Value1.";

        protected override string ValuePwnValidationError => "ValuePwn must be less than or equal to Value1.";

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

        [CustomTestMethod("ValuePwn < Value2 < Value1 : Valid")]
        public override Task CompareValuesPass()
        {
            return base.CompareValuesPass();
        }

        [CustomTestMethod("ValuePwn > Value2 > Value1 : Invalid")]
        public override Task CompareValuesFails()
        {
            return base.CompareValuesFails();
        }

        [CustomTestMethod("Value1 == Value2 == ValuePwn : Valid")]
        public virtual async Task SameValuesPass()
        {
            await LoadPage();

            var value1 = GetValues2PassCompare().Value1;
            await AssignValue1(value1);
            await AssignValue2(value1);
            await AssignValuePwn(value1);

            await CallClientValidation();
            await ExpectValidationSucceed();

            await ResetForm();

            await AssignValue1(value1);
            await AssignValue2(value1);
            await AssignValuePwn(value1);

            await CallServerValidation();
            await ExpectValidationSucceed();
        }


        [TestClass]
        public class DateValues : LessThanTest
        {
            protected override string DataType => "Date";

            protected override (string Value1, string Value2, string ValuePwn) GetValues2PassCompare()
            {
                return ("12/12/2020", "10/10/2010", "5/5/2005");
            }

            protected override (string Value1, string Value2, string ValuePwn) GetValues2FailsCompare()
            {
                return ("5/5/2005", "10/10/2010", "12/12/2020");
            }
        }

        [TestClass]
        public class Int16Values : LessThanTest
        {
            protected override string DataType => "Int16";

            protected override (string Value1, string Value2, string ValuePwn) GetValues2PassCompare()
            {
                return ("222", "55", "11");
            }

            protected override (string Value1, string Value2, string ValuePwn) GetValues2FailsCompare()
            {
                return ("22", "88", "111");
            }
        }

        [TestClass]
        public class TimeValues : LessThanTest
        {
            protected override string DataType => "Time";

            protected override (string Value1, string Value2, string ValuePwn) GetValues2PassCompare()
            {
                return ("22:00", "08:00", "02:00");
            }

            protected override (string Value1, string Value2, string ValuePwn) GetValues2FailsCompare()
            {
                return ("10:00", "18:30", "23:30");
            }
        }
    }
}