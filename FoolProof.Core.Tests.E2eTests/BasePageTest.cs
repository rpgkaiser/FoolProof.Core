using Microsoft.Playwright.MSTest;

namespace FoolProof.Core.Tests.E2eTests
{
    public abstract class BasePageTest: PageTest
    {
        public static string WebAppUrl => TestEnv.WebAppUrl ?? string.Empty;

        protected async Task ExpectValidationSucceed(
            string operatorPrefix,
            string alertValidationMsg
        )
        {
            var value1ValidationMessage = Page.GetByTestId($"{operatorPrefix}-val1-valid-msg");
            await Expect(value1ValidationMessage).ToBeEmptyAsync();
            
            var value2ValidationMessage = Page.GetByTestId($"{operatorPrefix}-val2-valid-msg");
            await Expect(value2ValidationMessage).ToBeEmptyAsync();

            var validAlertDiv = Page.GetByTestId($"{operatorPrefix}-valid-alert");
            await Expect(validAlertDiv).ToBeVisibleAsync();
            var cssClass = await validAlertDiv.GetAttributeAsync("class");
            Assert.IsTrue(cssClass is not null && cssClass.Contains("alert-success"));
            await Expect(validAlertDiv).ToContainTextAsync(alertValidationMsg);
        }

        protected async Task ExpectValidationFailed(
            string operatorPrefix, 
            string value2ErrorMsg,
            string alertValidationMsg,
            string value1ErrorMsg = ""
        )
        {
            var value1ValidationMessage = Page.GetByTestId($"{operatorPrefix}-val1-valid-msg");
            if(string.IsNullOrEmpty(value1ErrorMsg))
                await Expect(value1ValidationMessage).ToBeEmptyAsync();
            else
                await Expect(value1ValidationMessage).ToContainTextAsync(value1ErrorMsg);

            var value2ValidationMessage = Page.GetByTestId($"{operatorPrefix}-val2-valid-msg");
            if (string.IsNullOrEmpty(value2ErrorMsg))
                await Expect(value2ValidationMessage).ToBeEmptyAsync();
            else
                await Expect(value2ValidationMessage).ToContainTextAsync(value2ErrorMsg);

            var validAlertDiv = Page.GetByTestId($"{operatorPrefix}-valid-alert");
            await Expect(validAlertDiv).ToBeVisibleAsync();
            var cssClass = await validAlertDiv.GetAttributeAsync("class");
            Assert.IsTrue(cssClass is not null && cssClass.Contains("alert-danger"));
            await Expect(validAlertDiv).ToContainTextAsync(alertValidationMsg);
        }
    }
}