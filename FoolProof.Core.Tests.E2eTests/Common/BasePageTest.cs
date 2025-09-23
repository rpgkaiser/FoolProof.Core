using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Playwright;
using Microsoft.Playwright.MSTest;
using Newtonsoft.Json.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FoolProof.Core.Tests.E2eTests
{
    public abstract class BasePageTest: PlaywrightTest
    {
        public static string WebAppUrl => TestEnv.WebAppUrl ?? string.Empty;

        protected abstract Regex PageTitleRegex();

        protected abstract Uri PageUri();

        protected IBrowser Browser { get; set; }

        protected IBrowserContext Context { get; set; }

        protected IPage Page { get; set; }

        protected bool? UseInputTypes { get; set; } = true;

        protected bool? UseJQuery { get; set; } = true;

        protected virtual string Value1ValidMsgId => "value1-valid-msg";

        protected virtual string Value2ValidMsgId => "value2-valid-msg";

        protected virtual string ValuePwnValidMsgId => "valuepwn-valid-msg";

        [TestInitialize]
        public virtual async Task InitTest()
        {
            Browser = await Playwright.Chromium.LaunchAsync();
            Context = await Browser.NewContextAsync();
            Page = await Context.NewPageAsync();

            if(bool.TryParse(TestContext.Properties["UseJQuery"] + "", out var useJQ))
                UseJQuery = useJQ;
        }

        [TestCleanup]
        public virtual async Task CleanupTest()
        {
            await Page.CloseAsync();
            await Context.CloseAsync();
            await Browser.CloseAsync();
        }

        protected virtual async Task LoadPage()
        {
            await Page.Context.ClearCookiesAsync();
            if (UseInputTypes.HasValue)
                await Page.Context.AddCookiesAsync([ new Cookie {
                    Url = PageUri().AbsoluteUri,
                    Name = "UseInputTypes",
                    Value = UseInputTypes.Value.ToString()
                } ]);

            if (UseJQuery.HasValue)
                await Page.Context.AddCookiesAsync([ new Cookie {
                    Url = PageUri().AbsoluteUri,
                    Name = "UseJQuery",
                    Value = UseJQuery.Value.ToString()
                } ]);

            await Page.GotoAsync(PageUri().AbsoluteUri);
            await Expect(Page).ToHaveTitleAsync(PageTitleRegex());
        }

        protected virtual async Task VerifyValidationResult(InputTestValue input)
        {
            var inputValidMsg = Page.GetByTestId(input.ValidMsgElemTestId);
            if (input.ValidResultText == string.Empty)
                await Expect(inputValidMsg).ToBeEmptyAsync();
            else if (input.ValidResultText is null)
                await Expect(inputValidMsg).Not.ToBeEmptyAsync();
            else
                await Expect(inputValidMsg).ToContainTextAsync(input.ValidResultText);
        }

        protected virtual async Task ExpectValidationSucceed(
            string alertValidationMsg = "Model validation succeed"
        )
        {
            var value1ValidationMessage = Page.GetByTestId(Value1ValidMsgId);
            await Expect(value1ValidationMessage).ToBeEmptyAsync();
            
            var value2ValidationMessage = Page.GetByTestId(Value2ValidMsgId);
            await Expect(value2ValidationMessage).ToBeEmptyAsync();

            var valuePwnValidationMessage = Page.GetByTestId(ValuePwnValidMsgId);
            await Expect(value2ValidationMessage).ToBeEmptyAsync();

            var validAlertDiv = Page.GetByTestId($"valid-alert");
            await Expect(validAlertDiv).ToBeVisibleAsync();
            var cssClass = await validAlertDiv.GetAttributeAsync("class");
            var content = await Page.ContentAsync();
            Assert.IsTrue(cssClass is not null && cssClass.Contains("alert-success"));
            await Expect(validAlertDiv).ToContainTextAsync(alertValidationMsg);
        }

        protected virtual async Task ExpectValidationFailed(
            string? value2ErrorMsg = null,
            string? valuePwnErrorMsg = null,
            string? value1ErrorMsg = null,
            params string[] alertValidationMsgs            
        )
        {
            var value1ValidationMessage = Page.GetByTestId(Value1ValidMsgId);
            if(value1ErrorMsg == string.Empty)
                await Expect(value1ValidationMessage).ToBeEmptyAsync();
            else if(value1ErrorMsg is null)
                await Expect(value1ValidationMessage).Not.ToBeEmptyAsync();
            else
                await Expect(value1ValidationMessage).ToContainTextAsync(value1ErrorMsg);

            var value2ValidationMessage = Page.GetByTestId(Value2ValidMsgId);
            if (value2ErrorMsg == string.Empty)
                await Expect(value2ValidationMessage).ToBeEmptyAsync();
            else if(value2ErrorMsg is null)
                await Expect(value2ValidationMessage).Not.ToBeEmptyAsync();
            else
                await Expect(value2ValidationMessage).ToContainTextAsync(value2ErrorMsg);

            var valuePwnValidationMessage = Page.GetByTestId(ValuePwnValidMsgId);
            if (valuePwnErrorMsg == string.Empty)
                await Expect(valuePwnValidationMessage).ToBeEmptyAsync();
            else if(valuePwnErrorMsg is null)
                await Expect(valuePwnValidationMessage).Not.ToBeEmptyAsync();
            else
                await Expect(valuePwnValidationMessage).ToContainTextAsync(valuePwnErrorMsg);

            var validAlertDiv = Page.GetByTestId($"valid-alert");
            await Expect(validAlertDiv).ToBeVisibleAsync();
            var cssClass = await validAlertDiv.GetAttributeAsync("class");
            Assert.IsTrue(cssClass is not null && cssClass.Contains("alert-danger"));
            if(alertValidationMsgs is null)
                await Expect(validAlertDiv).Not.ToBeEmptyAsync();
            else if(alertValidationMsgs.Length == 0)
                await Expect(validAlertDiv).ToBeEmptyAsync();
            else
                foreach (var msg in alertValidationMsgs)
                    await Expect(validAlertDiv).ToContainTextAsync(msg);
        }

        protected virtual async Task ExpectValidationFailed(
            TestValues testValues,
            params string[] alertValidationMsgs
        )
        {
            foreach (var testVal in testValues.AllValues())
                await VerifyValidationResult(testVal);

            var validAlertDiv = Page.GetByTestId($"valid-alert");
            await Expect(validAlertDiv).ToBeVisibleAsync();
            var cssClass = await validAlertDiv.GetAttributeAsync("class");
            Assert.IsTrue(cssClass is not null && cssClass.Contains("alert-danger"));
            if (alertValidationMsgs is null)
                await Expect(validAlertDiv).Not.ToBeEmptyAsync();
            else if (alertValidationMsgs.Length == 0)
                await Expect(validAlertDiv).ToBeEmptyAsync();
            else
                foreach (var msg in alertValidationMsgs)
                    await Expect(validAlertDiv).ToContainTextAsync(msg);
        }

        protected virtual async Task AssignValue(string inputSeltor, object? value, bool verifyValue = true)
        {
            static async Task<bool> Assign(Func<Task> operation)
            {
                try
                {
                    await operation.Invoke();
                    return true;
                }
                catch { return false; }
            }

            var assigned = false;
            var input = Page.Locator(inputSeltor);
            if (value is IEnumerable vals && value is not string)
            {
                //The input must be a multi <select>
                var strVals = vals.Cast<object>().Select(o => ConvertToString(o)).ToArray();
                assigned = await Assign(
                    () => input.SelectOptionAsync(strVals.Select(v => new SelectOptionValue { Value = v }))
                );

                if(assigned && verifyValue)
                    try
                    {
                        await Expect(input).ToHaveValuesAsync(strVals);
                    }
                    catch
                    {
                        if (strVals.Length > 1)
                            throw;

                        await Expect(input).ToHaveValueAsync(strVals[0]);
                    }

                if (!assigned)
                    throw new Exception("Value could not be assigned and/or verified.");
            }

            if(!assigned && value is bool boolValue)
            {
                //The input may be a check or radio
                assigned = await Assign(
                    async () => {
                        if (boolValue)
                            await input.CheckAsync();
                        else
                            await input.UncheckAsync();
                    }
                );
            }

            var strVal = ConvertToString(value);
            if (!assigned)
            {
                //The input may be a text or textarea
                assigned = await Assign(() => input.FillAsync(strVal));
                if (assigned && verifyValue)
                    await Expect(input).ToHaveValueAsync(strVal);
            }

            if(!assigned)
            {
                //The input may be a <select>
                if (string.IsNullOrEmpty(strVal))
                {
                    var currVal = await input.InputValueAsync();
                    if (!string.IsNullOrEmpty(currVal))
                    {
                        assigned = await Assign(() => input.SelectOptionAsync(new SelectOptionValue { Value = string.Empty }));
                        if (assigned && verifyValue)
                            await Expect(input).ToBeEmptyAsync();
                    }
                    else
                        assigned = true;
                }
                else
                {
                    assigned = await Assign(() => input.SelectOptionAsync(new SelectOptionValue { Value = strVal }));
                    if (assigned && verifyValue)
                        await Expect(input).ToHaveValueAsync(strVal);
                }
            }

            if (!assigned)
                throw new Exception("Value could not be assigned and/or verified.");
        }

        protected virtual Task AssignValue1(object? value, bool verifyValue = true)
            => AssignValue("#Value1", value, verifyValue);

        protected virtual Task ExpectValue1Empty(bool? isSelect = null)
            => ExpectEmpties(new InputTestValue("Value1", isSelect: isSelect));

        protected virtual Task AssignValue2(object? value, bool verifyValue = true)
            => AssignValue("#Value2", value, verifyValue);

        protected virtual Task ExpectValue2Empty(bool? isSelect = null)
            => ExpectEmpties(new InputTestValue("Value2", isSelect: isSelect));

        protected virtual Task AssignValuePwn(object? value, bool verifyValue = true)
            => AssignValue("#ValuePwn", value, verifyValue);

        protected virtual Task ExpectValuePwnEmpty(bool? isSelect = null)
            => ExpectEmpties(new InputTestValue("ValuePwn", isSelect: isSelect));

        protected virtual Task AssignFieldValues(params InputTestValue[] fieldValues)
            => AssignFieldValues(fieldValues ?? [], true);

        protected virtual async Task AssignFieldValues(IEnumerable<InputTestValue> fieldValues, bool verifyValue = true)
        {
            foreach (var val in fieldValues)
                await AssignValue($"#{val.InputId}", val.Value, verifyValue);
        }

        protected virtual async Task ExpectValueEmpty(InputTestValue input)
        {
            var inputLocator = Page.Locator($"#{input.InputId}");
            if(!input.IsSelect.HasValue)
            {
                try { await Expect(inputLocator).ToBeEmptyAsync(); }
                catch { await Expect(inputLocator).ToHaveValueAsync(string.Empty); }
            }
            else if(input.IsSelect.Value)
                await Expect(inputLocator).ToHaveValueAsync(string.Empty);
            else
                await Expect(inputLocator).ToBeEmptyAsync();
        }

        protected virtual async Task ExpectEmpties(params InputTestValue[] inputs)
        {
            foreach (var input in inputs)
                await ExpectValueEmpty(input);
        }

        protected virtual async Task ResetForm(params InputTestValue[] inputs)
        {
            var resetFormBtn = Page.GetByTestId($"btn-reset");
            await resetFormBtn.ClickAsync();

            await ExpectValue1Empty();
            await ExpectValue2Empty();
            await ExpectValuePwnEmpty();

            if(inputs is not null && inputs.Length > 0)
                await ExpectEmpties(inputs);
        }

        protected virtual async Task ResetForm(TestValues testValues)
        {
            var resetFormBtn = Page.GetByTestId($"btn-reset");
            await resetFormBtn.ClickAsync();

            await ExpectEmpties([.. testValues.AllValues().Where(tv => tv.ResetAsEmpty == true)]);
        }

        protected async Task CallClientValidation(TestValues? testValues = null, bool resetFirst = false, bool verifyValidResults = false)
        {
            if(testValues is not null)
                await AssignTestValues(testValues, resetFirst);

            var clientValidationBtn = Page.GetByTestId($"btn-client");
            await clientValidationBtn.ClickAsync();

            if (verifyValidResults && testValues is not null)
                foreach (var testVal in testValues.AllValues())
                    await VerifyValidationResult(testVal);
        }

        protected async Task CallServerValidation(TestValues? testValues = null, bool resetFirst = false, bool verifyValidResults = false)
        {
            if (testValues is not null)
                await AssignTestValues(testValues);

            var serverValidationBtn = Page.GetByTestId($"btn-server");
            await Page.RunAndWaitForResponseAsync(
                async () => {
                    await serverValidationBtn.ClickAsync();
                }, 
                resp => resp.Ok 
                        && string.Equals(resp.Request.Method, "POST", StringComparison.OrdinalIgnoreCase)
                        && new Uri(resp.Url).GetLeftPart(UriPartial.Path).EndsWith("/validate")
            );

            if (verifyValidResults && testValues is not null)
                foreach (var testVal in testValues.AllValues())
                    await VerifyValidationResult(testVal);
        }

        protected virtual string ConvertToString(object? value)
        {
            if (value is null)
                return "";

            return value switch
            {
                DateTime date => UseInputTypes ?? true 
                                 ? date.ToString("yyyy-MM-ddThh:mm") 
                                 : date.ToString("MM/dd/yyyy hh:mm"),
                DateOnly date => UseInputTypes ?? true 
                                 ? date.ToString("yyyy-MM-dd")
                                 : date.ToString("MM/dd/yyyy"),
                TimeSpan time => time.ToString(@"hh\:mm"),
                TimeOnly time => time.ToString(@"hh\:mm"),
                bool boolValue => boolValue.ToString().ToLowerInvariant(),
                _ => value + ""
            };
        }

        protected virtual async Task AssignTestValues(
            TestValues tesValues,
            bool resetFirt = false,
            bool verifyValues = true,
            params string[] ignoreInputIds
        )
        {
            if (resetFirt)
                await ResetForm(tesValues);

            var fieldsToAssign = tesValues.AllValues();
            if (ignoreInputIds is not null && ignoreInputIds.Length > 0)
                fieldsToAssign = fieldsToAssign.Where(iv => !ignoreInputIds.Contains(iv.InputId));

            await AssignFieldValues([.. fieldsToAssign], verifyValues);
        }

        protected class InputTestValue(
            string id,
            object? value = null,
            bool? isSelect = null,
            string? validResText = "",
            string? validMsgElemTestId = null,
            bool resetAsEmpty = true
        )
        {
            public string InputId { get; set; } = id;

            public object? Value { get; set; } = value;

            public bool? IsSelect { get; set; } = isSelect;

            public string? ValidResultText { get; set; } = validResText;

            public string ValidMsgElemTestId { get; set; } = validMsgElemTestId ?? $"{id.ToLowerInvariant()}-valid-msg";

            public bool ResetAsEmpty { get; set; } = resetAsEmpty;

            public InputTestValue Clone(Action<InputTestValue>? modify = null)
            {
                var result = new InputTestValue(InputId, Value, IsSelect, ValidResultText, ValidMsgElemTestId);
                
                modify?.Invoke(result);

                return result;
            }
        }

        protected class TestValues
        {
            protected readonly InputTestValue _value1;
            protected readonly InputTestValue _value2;
            protected readonly InputTestValue _valuePwn;
            protected readonly List<InputTestValue> _otherValues;

            public TestValues(
                object? value1 = default,
                object? value2 = default,
                object? valuePwn = default,
                params InputTestValue[] otherValues
            )
            {
                _value1 = value1 is InputTestValue inpVal1 ? inpVal1 : new InputTestValue("Value1", value1);
                _value2 = value2 is InputTestValue inpVal2 ? inpVal2 : new InputTestValue("Value2", value2);
                _valuePwn = valuePwn is InputTestValue inpValPwn ? inpValPwn : new InputTestValue("ValuePwn", valuePwn);

                if (otherValues is not null && otherValues.Length > 0)
                    _otherValues = [.. otherValues];
                else
                    _otherValues = [];
            }

            public object? Value1
            {
                get => _value1.Value;
                set => _value1.Value = value;
            }

            public object? Value2
            {
                get => _value2.Value;
                set => _value2.Value = value;
            }

            public object? ValuePwn
            {
                get => _valuePwn.Value;
                set => _valuePwn.Value = value;
            }

            public List<InputTestValue> OtherValues => _otherValues;

            public IEnumerable<InputTestValue> AllValues()
            {
                yield return _value1;
                yield return _value2;
                yield return _valuePwn;

                foreach (var val in _otherValues)
                    yield return val;
            }
        }
    }
}