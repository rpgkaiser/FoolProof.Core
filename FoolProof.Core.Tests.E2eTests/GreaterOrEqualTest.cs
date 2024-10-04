using System.Text.RegularExpressions;

namespace FoolProof.Core.Tests.E2eTests
{
    public abstract class GreaterOrEqualTest : CompareBaseTest
    {
        protected override Regex PageTitleRegex() => new($@".+\s+[-]\s+GreaterThanOrEqualTo\s+\({DataType}\)");

        protected override Uri PageUri() => new(new Uri(WebAppUrl), $"ge2/{DataType}");

        protected override string Value2ValidationError => "Value2 must be greater than or equal to Value1.";

        protected override string ValuePwnValidationError => "ValuePwn must be greater than or equal to Value1.";

        [CustomTestMethod("Value2 > Value1 > ValuePwn : Valid")]
        public override Task CompareValuesPass()
        {
            return base.CompareValuesPass();
        }

        [CustomTestMethod("Value2 < Value1 < ValuePwn : Invalid")]
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
        public class DateValues : GreaterOrEqualTest
        {
            protected override string DataType => "Date";

            protected override (string Value1, string Value2, string ValuePwn) GetValues2PassCompare()
            {
                return ("5/5/2005", "10/10/2010", "11/11/2020");
            }

            protected override (string Value1, string Value2, string ValuePwn) GetValues2FailsCompare()
            {
                return ("11/11/2020", "10/10/2010", "5/5/2005");
            }
        }

        [TestClass]
        public class Int16Values : GreaterOrEqualTest
        {
            protected override string DataType => "Int16";

            protected override (string Value1, string Value2, string ValuePwn) GetValues2PassCompare()
            {
                return ("11", "55", "888");
            }

            protected override (string Value1, string Value2, string ValuePwn) GetValues2FailsCompare()
            {
                return ("999", "88", "22");
            }
        }

        [TestClass]
        public class TimeValues : GreaterOrEqualTest
        {
            protected override string DataType => "Time";

            protected override (string Value1, string Value2, string ValuePwn) GetValues2PassCompare()
            {
                return ("08:00", "12:00", "16:00");
            }

            protected override (string Value1, string Value2, string ValuePwn) GetValues2FailsCompare()
            {
                return ("20:30", "14:00", "10:00");
            }
        }
    }
}