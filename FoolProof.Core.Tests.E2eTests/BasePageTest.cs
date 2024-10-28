using System.Collections;
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

        protected bool? UseInputTypes { get; set; } = false;

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
            var value1ValidationMessage = Page.GetByTestId($"val1-valid-msg");
            await Expect(value1ValidationMessage).ToBeEmptyAsync();
            
            var value2ValidationMessage = Page.GetByTestId($"val2-valid-msg");
            await Expect(value2ValidationMessage).ToBeEmptyAsync();

            var valuePwnValidationMessage = Page.GetByTestId($"valPwn-valid-msg");
            await Expect(value2ValidationMessage).ToBeEmptyAsync();

            var validAlertDiv = Page.GetByTestId($"valid-alert");
            await Expect(validAlertDiv).ToBeVisibleAsync();
            var cssClass = await validAlertDiv.GetAttributeAsync("class");
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
            var value1ValidationMessage = Page.GetByTestId($"val1-valid-msg");
            if(string.IsNullOrEmpty(value1ErrorMsg))
                await Expect(value1ValidationMessage).ToBeEmptyAsync();
            else
                await Expect(value1ValidationMessage).ToContainTextAsync(value1ErrorMsg);

            var value2ValidationMessage = Page.GetByTestId($"val2-valid-msg");
            if (string.IsNullOrEmpty(value2ErrorMsg))
                await Expect(value2ValidationMessage).ToBeEmptyAsync();
            else
                await Expect(value2ValidationMessage).ToContainTextAsync(value2ErrorMsg);

            var valuePwnValidationMessage = Page.GetByTestId($"valPwn-valid-msg");
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

        protected virtual async Task AssignValue(string inputSeltor, object? value)
        {
            var input = Page.Locator(inputSeltor);
            if (value is string strVal)
            {
                await input.FillAsync(strVal);
                await Expect(input).ToHaveValueAsync(strVal);
            }
            else if (value is IEnumerable vals)
            {
                var strVals = vals.Cast<object>().Select(o => ConvertToString(o)).ToArray();
                await input.SelectOptionAsync(strVals);
                await Expect(input).ToHaveValuesAsync(strVals);
            }
            else
            {
                strVal = ConvertToString(value);
                await input.FillAsync(strVal);
                await Expect(input).ToHaveValueAsync(strVal);
            }
        }

        protected virtual Task AssignValue1(object? value)
            => AssignValue("#Value1", value);

        protected virtual async Task ExpectValue1Empty()
        {
            var textInput = Page.Locator("#Value1");
            await Expect(textInput).ToBeEmptyAsync();
        }

        protected virtual Task AssignValue2(object? value)
            => AssignValue("#Value2", value);

        protected virtual async Task ExpectValue2Empty()
        {
            var textInput = Page.Locator("#Value2");
            await Expect(textInput).ToBeEmptyAsync();
        }

        protected virtual Task AssignValuePwn(object? value)
            => AssignValue("#ValuePwn", value);

        protected virtual async Task ExpectValuePwnEmpty()
        {
            var textInput = Page.Locator("#ValuePwn");
            await Expect(textInput).ToBeEmptyAsync();
        }

        protected virtual async Task ResetForm()
        {
            var resetFormBtn = Page.GetByTestId($"btn-reset");
            await resetFormBtn.ClickAsync();
            await ExpectValue1Empty();
            await ExpectValue2Empty();
            await ExpectValuePwnEmpty();
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
                _ => value + ""
            };
        }
    }
}