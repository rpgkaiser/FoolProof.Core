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

        [TestInitialize]
        public virtual async Task InitTest()
        {
            Browser = await Playwright.Chromium.LaunchAsync();
            Context = await Browser.NewContextAsync();
            Page = await Context.NewPageAsync();
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

            await Page.GotoAsync(PageUri().AbsoluteUri);
            await Expect(Page).ToHaveTitleAsync(PageTitleRegex());
        }

        protected async Task ExpectValidationSucceed(
            string alertValidationMsg = "Model validation succeed"
        )
        {
            var value1ValidationMessage = Page.GetByTestId($"value1-valid-msg");
            await Expect(value1ValidationMessage).ToBeEmptyAsync();
            
            var value2ValidationMessage = Page.GetByTestId($"value2-valid-msg");
            await Expect(value2ValidationMessage).ToBeEmptyAsync();

            var valuePwnValidationMessage = Page.GetByTestId($"valuepwn-valid-msg");
            await Expect(value2ValidationMessage).ToBeEmptyAsync();

            var validAlertDiv = Page.GetByTestId($"valid-alert");
            await Expect(validAlertDiv).ToBeVisibleAsync();
            var cssClass = await validAlertDiv.GetAttributeAsync("class");
            var content = await Page.ContentAsync();
            Assert.IsTrue(cssClass is not null && cssClass.Contains("alert-success"));
            await Expect(validAlertDiv).ToContainTextAsync(alertValidationMsg);
        }

        protected async Task ExpectValidationFailed(
            string value2ErrorMsg,
            string? valuePwnErrorMsg = null,
            string? value1ErrorMsg = null,
            params string[] alertValidationMsgs            
        )
        {
            var value1ValidationMessage = Page.GetByTestId($"value1-valid-msg");
            if(string.IsNullOrEmpty(value1ErrorMsg))
                await Expect(value1ValidationMessage).ToBeEmptyAsync();
            else
                await Expect(value1ValidationMessage).ToContainTextAsync(value1ErrorMsg);

            var value2ValidationMessage = Page.GetByTestId($"value2-valid-msg");
            if (string.IsNullOrEmpty(value2ErrorMsg))
                await Expect(value2ValidationMessage).ToBeEmptyAsync();
            else
                await Expect(value2ValidationMessage).ToContainTextAsync(value2ErrorMsg);

            var valuePwnValidationMessage = Page.GetByTestId($"valuepwn-valid-msg");
            if (string.IsNullOrEmpty(valuePwnErrorMsg))
                await Expect(valuePwnValidationMessage).ToBeEmptyAsync();
            else
                await Expect(valuePwnValidationMessage).ToContainTextAsync(valuePwnErrorMsg);

            var validAlertDiv = Page.GetByTestId($"valid-alert");
            await Expect(validAlertDiv).ToBeVisibleAsync();
            var cssClass = await validAlertDiv.GetAttributeAsync("class");
            Assert.IsTrue(cssClass is not null && cssClass.Contains("alert-danger"));
            if(alertValidationMsgs is not null)
                foreach(var msg in alertValidationMsgs)
                    await Expect(validAlertDiv).ToContainTextAsync(msg);
        }

        protected virtual async Task AssignValue(string inputSeltor, object? value, bool verifyValue = true)
        {
            var input = Page.Locator(inputSeltor);
            if (value is IEnumerable vals && value is not string)
            {
                var strVals = vals.Cast<object>().Select(o => ConvertToString(o)).ToArray();
                await input.SelectOptionAsync(strVals.Select(v => new SelectOptionValue { Value = v }));
                if(verifyValue)
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
            }
            else
            {
                var strVal = ConvertToString(value);
                try
                {
                    await input.FillAsync(strVal);
                    if (verifyValue)
                        await Expect(input).ToHaveValueAsync(strVal);
                }
                catch
                {
                    //The input may be a <select>
                    if (string.IsNullOrEmpty(strVal))
                    {
                        var currVal = await input.InputValueAsync();
                        if (!string.IsNullOrEmpty(currVal))
                        {
                            await input.SelectOptionAsync(new SelectOptionValue { Value = string.Empty });
                            if (verifyValue)
                                await Expect(input).ToBeEmptyAsync();
                        }
                    }
                    else
                    {
                        await input.SelectOptionAsync(new SelectOptionValue { Value = strVal });
                        if (verifyValue)
                            await Expect(input).ToHaveValueAsync(strVal);
                    }
                }
            }
        }

        protected virtual Task AssignValue1(object? value, bool verifyValue = true)
            => AssignValue("#Value1", value, verifyValue);

        protected virtual Task ExpectValue1Empty(bool? isSelect = null)
            => ExpectEmpties(new InputValue("Value1", isSelect: isSelect));

        protected virtual Task AssignValue2(object? value, bool verifyValue = true)
            => AssignValue("#Value2", value, verifyValue);

        protected virtual Task ExpectValue2Empty(bool? isSelect = null)
            => ExpectEmpties(new InputValue("Value2", isSelect: isSelect));

        protected virtual Task AssignValuePwn(object? value, bool verifyValue = true)
            => AssignValue("#ValuePwn", value, verifyValue);

        protected virtual Task ExpectValuePwnEmpty(bool? isSelect = null)
            => ExpectEmpties(new InputValue("ValuePwn", isSelect: isSelect));

        protected virtual Task AssignFieldValues(params InputValue[] fieldValues)
            => AssignFieldValues(fieldValues ?? [], true);

        protected virtual async Task AssignFieldValues(IEnumerable<InputValue> fieldValues, bool verifyValue = true)
        {
            foreach (var val in fieldValues)
                await AssignValue($"#{val.InputId}", val.Value, verifyValue);
        }

        protected virtual async Task ExpectValueEmpty(InputValue input)
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

        protected virtual async Task ExpectEmpties(params InputValue[] inputs)
        {
            foreach (var id in inputs)
                await ExpectValueEmpty(id);
        }

        protected virtual async Task ResetForm(params InputValue[] inputs)
        {
            var resetFormBtn = Page.GetByTestId($"btn-reset");
            await resetFormBtn.ClickAsync();

            await ExpectValue1Empty();
            await ExpectValue2Empty();
            await ExpectValuePwnEmpty();

            if(inputs is not null && inputs.Length > 0)
                await ExpectEmpties(inputs);
        }

        protected async Task CallClientValidation()
        {
            var clientValidationBtn = Page.GetByTestId($"btn-client");
            await clientValidationBtn.ClickAsync();
        }

        protected async Task CallServerValidation()
        {
            var serverValidationBtn = Page.GetByTestId($"btn-server");
            await Page.RunAndWaitForResponseAsync(
                async () => {
                    await serverValidationBtn.ClickAsync();
                }, 
                resp => resp.Ok 
                        && string.Equals(resp.Request.Method, "POST", StringComparison.OrdinalIgnoreCase)
                        && new Uri(resp.Url).GetLeftPart(UriPartial.Path).EndsWith("/validate")
            );
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

        protected class InputValue
        {
            public InputValue() { }

            public InputValue(string id, object? value = null, bool? isSelect = null)
            {
                this.InputId = id;
                this.Value = value;
                this.IsSelect = null;
            }

            public string InputId { get; set; } = string.Empty;

            public object? Value { get; set; }

            public bool? IsSelect { get; set; }
        }
    }
}