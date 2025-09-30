using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace FoolProof.Core.Tests.E2eTests
{
    public abstract class InTest<T>: CompareBaseTest
    {
        protected T[] _value1 = new T[0];

        protected override Regex PageTitleRegex() => new($@".+\s+[-]\s+In\s+\({DataType}\)");

        protected override Uri PageUri() => new(new Uri(WebAppUrl), $"in/{DataType}");

        protected override string Value2ValidationError => "Value2 must be in Value1";

        protected override string ValuePwnValidationError => "ValuePwn must be in Value1";

        [CustomTestMethod("ValuePwn && Value2 ∈ Value1 : Valid")]
        public override Task FormValidationSuccess()
        {
            return base.FormValidationSuccess();
        }

        [CustomTestMethod("ValuePwn && Value2 ∉ Value1 : Invalid")]
        public override Task FormValidationFailure()
        {
            return base.FormValidationFailure();
        }

        [CustomTestMethod("Value1 == Value2 == ValuePwn : Valid")]
        public virtual async Task SameValuesPass()
        {
            await LoadPage();

            var testValues = GetValues2PassValidation();
            testValues.Value1 = _value1.Take(1).ToArray();
            testValues.Value2 = testValues.ValuePwn = _value1.First();
            await AssignTestValues(testValues);

            await CallClientValidation();
            await ExpectValidationSucceed();

            await AssignTestValues(testValues, true);

            await CallServerValidation();
            await ExpectValidationSucceed();
        }
    }

    [TestClass]
    public class SingleValueInTest : InTest<string>
    {
        public SingleValueInTest()
        {
            _value1 = ["Value one"];
        }

        protected override string DataType => "Single";

        protected override CompareTestValues GetValues2PassValidation()
        {
            return new(_value1, "Value one", "Value one", [
                new (nameof(In.SingleValueModel<string>.DateIn), DateOnly.Parse("03/03/2025")),
                new (nameof(In.SingleValueModel<string>.TimeIn), TimeSpan.Parse("02:30"))
            ]);
        }

        protected override CompareTestValues GetValues2FailsValidation()
        {
            return new(_value1, "Value two", "Value three", [
                new (nameof(In.SingleValueModel<string>.DateIn), DateOnly.Parse("10/10/2025")),
                new (nameof(In.SingleValueModel<string>.TimeIn), TimeSpan.Parse("05:30"))
            ]);
        }
    }

    [TestClass]
    public class Int16ValuesInTest : InTest<Int16>
    {
        public Int16ValuesInTest()
        {
            _value1 = [2, 4, 7, 9];
        }

        protected override string DataType => "Int16";

        protected override CompareTestValues GetValues2PassValidation()
        {
            return new(_value1, 2, 7, [
                new (nameof(In.In16ListModel.ValueIn), -1)
            ]);
        }

        protected override CompareTestValues GetValues2FailsValidation()
        {
            return new(_value1, 3, 5, [
                new (nameof(In.In16ListModel.ValueIn), 4)
            ]);
        }
    }

    [TestClass]
    public class DateTimeValuesInTest : InTest<DateTime>
    {
        public DateTimeValuesInTest()
        {
            _value1 = [
                DateTime.Parse("03/03/2003 03:05"),
                DateTime.Parse("07/07/2007 07:07"),
                DateTime.Parse("12/12/2012 12:12")
            ];
        }

        protected override string DataType => "DateTime";

        protected override CompareTestValues GetValues2PassValidation()
        {
            return new(
                _value1,
                DateTime.Parse("07/07/2007 07:07"),
                DateTime.Parse("12/12/2012 12:12"), 
                [
                    new (nameof(In.DateTimeListModel.DateTimeIn), DateTime.Parse("02/02/2025 02:00"))
                ]
            );
        }

        protected override CompareTestValues GetValues2FailsValidation()
        {
            return new(
                _value1,
                DateTime.Parse("08/08/2008 07:07"),
                DateTime.Parse("05/12/2012 15:15"),
                [
                    new (nameof(In.DateTimeListModel.DateTimeIn), DateTime.Parse("09/09/2025 08:00"))
                ]
            );
        }

        protected override async Task AssignValue(string inputSeltor, object? value, bool verifyValue = true)
        {
            if (!string.Equals(inputSeltor, "#Value1") || value is string || value is not IEnumerable enumVals)
            {
                await base.AssignValue(inputSeltor, value, verifyValue);
                return;
            }

            var strVals = enumVals.Cast<DateTime>().Select(d => d.ToString("MM/dd/yyyy hh:mm")).ToArray();
            var input = Page.Locator("#Value1");
            await input.SelectOptionAsync(strVals);
            if (verifyValue)
                await Expect(input).ToHaveValuesAsync(strVals);
        }
    }
}