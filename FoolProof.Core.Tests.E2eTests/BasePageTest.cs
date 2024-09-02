using System.Text.RegularExpressions;
using Microsoft.Playwright.MSTest;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;

namespace FoolProof.Core.Tests.E2eTests
{
    public abstract class BasePageTest: PageTest
    {
        public static string WebAppUrl => TestEnv.WebAppUrl ?? string.Empty;

        protected abstract Regex PageTitleRegex { get; }

        protected abstract Uri PageUri { get; }

        protected virtual async Task LoadPage()
        {
            await Page.GotoAsync(PageUri.AbsoluteUri);
            await Expect(Page).ToHaveTitleAsync(PageTitleRegex);
        }

        protected async Task ExpectValidationSucceed(string alertValidationMsg = "Model validation succeed")
        {
            var value1ValidationMessage = Page.GetByTestId($"val1-valid-msg");
            await Expect(value1ValidationMessage).ToBeEmptyAsync();
            
            var value2ValidationMessage = Page.GetByTestId($"val2-valid-msg");
            await Expect(value2ValidationMessage).ToBeEmptyAsync();

            var validAlertDiv = Page.GetByTestId($"valid-alert");
            await Expect(validAlertDiv).ToBeVisibleAsync();
            var cssClass = await validAlertDiv.GetAttributeAsync("class");
            Assert.IsTrue(cssClass is not null && cssClass.Contains("alert-success"));
            await Expect(validAlertDiv).ToContainTextAsync(alertValidationMsg);
        }

        protected async Task ExpectValidationFailed(
            string value2ErrorMsg,
            string? alertValidationMsg = null,
            string? value1ErrorMsg = null
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

            var validAlertDiv = Page.GetByTestId($"valid-alert");
            await Expect(validAlertDiv).ToBeVisibleAsync();
            var cssClass = await validAlertDiv.GetAttributeAsync("class");
            Assert.IsTrue(cssClass is not null && cssClass.Contains("alert-danger"));
            await Expect(validAlertDiv).ToContainTextAsync(alertValidationMsg ?? value2ErrorMsg);
        }

        protected async Task AssignValue1(string value)
        {
            var textInput = Page.Locator("#Value1");
            await textInput.TypeAsync(value);
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
            await textInput.TypeAsync(value);
            await Expect(textInput).ToHaveValueAsync(value);
        }

        protected async Task ExpectValue2Empty()
        {
            var textInput = Page.Locator("#Value2");
            await Expect(textInput).ToBeEmptyAsync();
        }

        protected async Task ResetForm()
        {
            var resetFormBtn = Page.GetByTestId($"btn-reset");
            await resetFormBtn.ClickAsync();
            await Expect(Page.Locator("#Value1")).ToBeEmptyAsync();
            await Expect(Page.Locator("#Value2")).ToBeEmptyAsync();
        }

        protected async Task ExecClientValidation()
        {
            var clientValidationBtn = Page.GetByTestId($"btn-client");
            await clientValidationBtn.ClickAsync();
        }

        protected async Task ExecServerValidation()
        {
            var serverValidationBtn = Page.GetByTestId($"btn-server");
            await Page.RunAndWaitForResponseAsync(async () =>
            {
                await serverValidationBtn.ClickAsync();
            }, resp => new Uri(resp.Url).GetLeftPart(UriPartial.Path).EndsWith("/validate"));
        }
    }
}