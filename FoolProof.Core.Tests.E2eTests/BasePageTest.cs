using System.Text.RegularExpressions;
using Microsoft.Playwright;
using Microsoft.Playwright.MSTest;

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

        [TestInitialize]
        public virtual async Task InitTest()
        {
            Browser = await Playwright.Chromium.LaunchAsync();
            Context = await Browser.NewContextAsync();
            
            Page = await Context.NewPageAsync();
            await Page.Context.AddCookiesAsync([ new Cookie {
                Url = PageUri().AbsoluteUri,
                Name = "UseInputTypes",
                Value = "false"
            } ]).ConfigureAwait(false);
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

        protected async Task AssignValue1(string value)
        {
            var textInput = Page.Locator("#Value1");
            await textInput.FillAsync(value);
            await Expect(textInput).ToHaveValueAsync(value);
        }

        protected async Task ExpectValue1Empty()
        {
            var textInput = Page.Locator("#Value1");
            await Expect(textInput).ToBeEmptyAsync();
        }

        protected async Task AssignValue2(string value)
        {
            var textInput = Page.Locator("#Value2");
            await textInput.FillAsync(value);
            await Expect(textInput).ToHaveValueAsync(value);
        }

        protected async Task AssignValuePwn(string value)
        {
            var textInput = Page.Locator("#ValuePwn");
            await textInput.FillAsync(value);
            await Expect(textInput).ToHaveValueAsync(value);
        }

        protected async Task ExpectValue2Empty()
        {
            var textInput = Page.Locator("#Value2");
            await Expect(textInput).ToBeEmptyAsync();
        }

        protected async Task ExpectValuePwnEmpty()
        {
            var textInput = Page.Locator("#ValuePwn");
            await Expect(textInput).ToBeEmptyAsync();
        }

        protected async Task ResetForm()
        {
            var resetFormBtn = Page.GetByTestId($"btn-reset");
            await resetFormBtn.ClickAsync();
            await Expect(Page.Locator("#Value1")).ToBeEmptyAsync();
            await Expect(Page.Locator("#Value2")).ToBeEmptyAsync();
            await Expect(Page.Locator("#ValuePwn")).ToBeEmptyAsync();
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
    }
}