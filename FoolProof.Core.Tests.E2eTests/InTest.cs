using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace FoolProof.Core.Tests.E2eTests
{
    public abstract class InTest<TV1, TV2, TVPwn>: CompareBaseTest<TV1, TV2, TVPwn>
    {
        protected override Regex PageTitleRegex() => new($@".+\s+[-]\s+In\s+\({DataType}\)");

        protected override Uri PageUri() => new(new Uri(WebAppUrl), $"in/{DataType}");

        protected override string Value2ValidationError => "Value2 must be in Value1";

        protected override string ValuePwnValidationError => "ValuePwn must be in Value1";

        [TestInitialize]
        public void IniTest()
        {
            UseInputTypes = true;
        }

        [CustomTestMethod("ValuePwn && Value2 ∈ Value1 : Valid")]
        public override Task CompareValuesPass()
        {
            return base.CompareValuesPass();
        }

        [CustomTestMethod("ValuePwn && Value2 ∉ Value1 : Invalid")]
        public override Task CompareValuesFails()
        {
            return base.CompareValuesFails();
        }

        [CustomTestMethod("Value1 == Value2 == ValuePwn : Valid")]
        public virtual async Task SameValuesFails()
        {
            UseInputTypes = false;
            await LoadPage();

            var value = GetValues2PassCompare().Value2;
            await AssignValue1(value);
            await AssignValue2(value);
            await AssignValuePwn(value);

            await CallClientValidation();
            await ExpectValidationSucceed();

            await ResetForm();

            await AssignValue1(value);
            await AssignValue2(value);
            await AssignValuePwn(value);

            await CallServerValidation();
            await ExpectValidationSucceed();
        }

        [CustomTestMethod("Not Valid Values : Invalid")]
        public override async Task InvalidValues()
        {
            UseInputTypes = false;
            await base.InvalidValues();
        }

        protected override string InvalidValue1ValidationError(string invalidValue)
            => $"The value '{invalidValue}' is not valid";

        protected override async Task ExpectValue1Empty()
        {
            var textInput = Page.Locator("#Value1");
            if(UseInputTypes ?? true)
                await Expect(textInput).ToHaveValuesAsync(Enumerable.Empty<string>());
            else
                await Expect(textInput).ToBeEmptyAsync();
        }
    }

    [TestClass]
    public class SingleValueInTest : InTest<string[], string, string>
    {
        protected override string DataType => "Single";

        [Ignore]
        public override Task InvalidValues()
        {
            //This test should be ignored in this class
            return Task.CompletedTask;
        }

        protected override (string[] Value1, string Value2, string ValuePwn) GetValues2PassCompare()
        {
            return (new[] { "Value one" }, "Value one", "Value one");
        }

        protected override (string[] Value1, string Value2, string ValuePwn) GetValues2FailsCompare()
        {
            return (new[] { "Value one" }, "Value two", "Value three");
        }

        protected override async Task ExpectValue1Empty()
        {
            var textInput = Page.Locator("#Value1");
            await Expect(textInput).ToHaveValueAsync("");
        }

        protected override async Task AssignValue1(object? value)
        {
            if(value is string[] strVals)
            {
                var input = Page.Locator("#Value1");
                await input.SelectOptionAsync(strVals);
                await Expect(input).ToHaveValueAsync(strVals.First());
            }
            else
                await base.AssignValue1(value);
        }
    }

    [TestClass]
    public class Int16ValuesInTest : InTest<Int16[], Int16, Int16>
    {
        protected override string DataType => "Int16";

        protected override (Int16[] Value1, Int16 Value2, Int16 ValuePwn) GetValues2PassCompare()
        {
            return (new Int16[] { 2, 4, 7, 9 }, 2, 7);
        }

        protected override (Int16[] Value1, Int16 Value2, Int16 ValuePwn) GetValues2FailsCompare()
        {
            return (new Int16[] { 2, 4, 7, 9 }, 3, 5);
        }
    }

    [TestClass]
    public class DateTimeValuesInTest : InTest<DateTime[], DateTime, DateTime>
    {
        protected override string DataType => "DateTime";

        protected override (DateTime[] Value1, DateTime Value2, DateTime ValuePwn) GetValues2PassCompare()
        {
            return (
                new[] {
                        DateTime.Parse("03/03/2003 03:05"),
                        DateTime.Parse("07/07/2007 07:07"),
                        DateTime.Parse("12/12/2012 12:12")
                },
                DateTime.Parse("07/07/2007 07:07"),
                DateTime.Parse("12/12/2012 12:12")
            );
        }

        protected override (DateTime[] Value1, DateTime Value2, DateTime ValuePwn) GetValues2FailsCompare()
        {
            return (
                new[] {
                        DateTime.Parse("03/03/2003 03:05"),
                        DateTime.Parse("07/07/2007 07:07"),
                        DateTime.Parse("12/12/2012 12:12")
                },
                DateTime.Parse("08/08/2008 07:07"),
                DateTime.Parse("05/12/2012 15:15")
            );
        }

        protected override async Task AssignValue1(object? value)
        {
            if(value is DateTime[] dateVals)
            {
                var strVals = dateVals.Select(d => d.ToString("MM/dd/yyyy hh:mm")).ToArray();
                var input = Page.Locator("#Value1");
                await input.SelectOptionAsync(strVals);
                await Expect(input).ToHaveValuesAsync(strVals);
            }
            else
                await base.AssignValue1(value);
        }
    }
}