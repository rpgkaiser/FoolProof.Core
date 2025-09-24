using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace FoolProof.Core.Tests.E2eTests
{
    public abstract class NotInTest<T>: InTest<T>
    {
        protected override Regex PageTitleRegex() => new($@".+\s+[-]\s+NotIn\s+\({DataType}\)");

        protected override Uri PageUri() => new(new Uri(WebAppUrl), $"notin/{DataType}");

        protected override string Value2ValidationError => "Value2 must be not in Value1";

        protected override string ValuePwnValidationError => "ValuePwn must be not in Value1";

        [CustomTestMethod("ValuePwn && Value2 ∉ Value1 : Valid")]
        public override Task FormValidationSuccess()
        {
            return base.FormValidationSuccess();
        }

        [CustomTestMethod("ValuePwn && Value2 ∈ Value1 : Invalid")]
        public override Task FormValidationFailure()
        {
            return base.FormValidationFailure();
        }

        [Ignore]
        public override Task SameValuesPass() => Task.CompletedTask;

        [CustomTestMethod("Value1 == Value2 == ValuePwn : Invalid")]
        public virtual async Task SameValuesFails()
        {
            await LoadPage();

            var testValues = GetValues2PassValidation();
            testValues.Value1 = _value1.Take(1).ToArray();
            testValues.Value2 = testValues.ValuePwn = _value1.First();
            await AssignTestValues(testValues);

            await CallClientValidation();
            await ExpectClientValidationFailed();

            await AssignTestValues(testValues, true);

            await CallServerValidation();
            await ExpectServerValidationFailed();
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

        [CustomTestMethod("Value2 is Empty : Valid")]
        public override async Task Value2Empty()
        {
            await LoadPage();

            var testValues = GetValues2PassValidation();
            await AssignTestValues(testValues, false, true, ["Value2"]);
            await ExpectValue2Empty();

            await CallClientValidation();
            await ExpectValidationSucceed();

            await AssignTestValues(testValues, true, true, ["Value2"]);
            await ExpectValue2Empty();

            await CallServerValidation();
            await ExpectValidationSucceed();
        }
    }

    [TestClass]
    public class SingleValueNotInTest : NotInTest<string>
    {
        public SingleValueNotInTest()
        {
            _value1 = ["Value one"];
        }

        protected override string DataType => "Single";

        protected override TestValues GetValues2PassValidation()
        {
            return new(
                _value1, 
                "Value two", 
                "Value three", 
                [
                    new(nameof(NotIn.SingleValueModel<string>.DateNotIn), DateOnly.Parse("08/08/2028")),
                    new(nameof(NotIn.SingleValueModel<string>.TimeNotIn), TimeSpan.Parse("10:00"))
                ]
            );
        }

        protected override TestValues GetValues2FailsValidation()
        {
            return new(
                _value1, 
                "Value one", 
                "Value one", 
                [
                    new(nameof(NotIn.SingleValueModel<string>.DateNotIn), DateOnly.Parse("03/03/2025")),
                    new(nameof(NotIn.SingleValueModel<string>.TimeNotIn), TimeSpan.Parse("02:00"))
                ]
            );
        }
    }

    [TestClass]
    public class Int16ValuesNotInTest : NotInTest<Int16>
    {
        public Int16ValuesNotInTest()
        {
            _value1 = [ 2, 4, 7, 9 ];
        }

        protected override string DataType => "Int16";

        protected override TestValues GetValues2PassValidation()
        {
            return new(_value1, 3, 8, [
                new (nameof(NotIn.In16ListModel.ValueNotIn), 4)
            ]);
        }

        protected override TestValues GetValues2FailsValidation()
        {
            return new(_value1, 4, 9, [
                new (nameof(NotIn.In16ListModel.ValueNotIn), 5)
            ]);
        }
    }

    [TestClass]
    public class DateTimeValuesNotInTest : NotInTest<DateTime>
    {
        public DateTimeValuesNotInTest()
        {
            _value1 = [
                DateTime.Parse("03/03/2003 03:05"),
                DateTime.Parse("07/07/2007 07:07"),
                DateTime.Parse("12/12/2012 12:12")
            ];
        }

        protected override string DataType => "DateTime";

        protected override TestValues GetValues2PassValidation()
        {
            return new(
                _value1,
                DateTime.Parse("08/08/2008 07:07"),
                DateTime.Parse("05/12/2012 15:15"),
                [
                    new (nameof(NotIn.DateTimeListModel.DateTimeNotIn), DateTime.Parse("08/10/2028 12:00"))
                ]
            );
        }

        protected override TestValues GetValues2FailsValidation()
        {
            return new(
                _value1,
                DateTime.Parse("07/07/2007 07:07"),
                DateTime.Parse("12/12/2012 12:12"),
                [
                    new (nameof(NotIn.DateTimeListModel.DateTimeNotIn), DateTime.Parse("02/02/2025 02:00"))
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