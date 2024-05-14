using System.Text.RegularExpressions;
using Microsoft.Playwright;
using Microsoft.Playwright.MSTest;

namespace FoolProof.Core.Tests.E2eTests
{   
    public class EqualToTest
    {
        [TestClass]
        public class Default : BasePageTest
        {
            protected virtual string TestIDsPrefix => "eq2";

            protected virtual Uri PageUri => new(new Uri(WebAppUrl), "equalto");

            [TestMethod]
            public virtual async Task EmptyFields()
            {
                await Page.GotoAsync(PageUri.AbsoluteUri);

                await Expect(Page).ToHaveTitleAsync(new Regex(@".+\s+[-]\s+EqualTo"));
                
                var value1TextInput = Page.Locator("#Value1");
                await Expect(value1TextInput).ToBeEmptyAsync();

                var value2TextInput = Page.Locator("#Value2");
                await Expect(value2TextInput).ToBeEmptyAsync();

                var clientValidationBtn = Page.GetByTestId($"{TestIDsPrefix}-btn-client");
                await clientValidationBtn.ClickAsync();

                await ExpectValidationSucceed(TestIDsPrefix, "Model validation succeed");

                var resetFormBtn = Page.GetByTestId($"{TestIDsPrefix}-btn-reset");
                await resetFormBtn.ClickAsync();

                var serverValidationBtn = Page.GetByTestId($"{TestIDsPrefix}-btn-server");
                await Page.RunAndWaitForResponseAsync(async () =>
                {
                    await serverValidationBtn.ClickAsync();
                }, resp => new Uri(resp.Url).GetLeftPart(UriPartial.Path).EndsWith("/validate"));

                await ExpectValidationSucceed(TestIDsPrefix, "Model validation succeed");
            }

            [TestMethod]
            public virtual async Task Value2Empty()
            {
                var resp = await Page.GotoAsync(PageUri.AbsoluteUri);

                var body = await resp!.TextAsync();

                await Expect(Page).ToHaveTitleAsync(new Regex(@".+\s+[-]\s+EqualTo"));

                var value1TextInput = Page.Locator("#Value1");
                await value1TextInput.TypeAsync("Value one.");
                await Expect(value1TextInput).ToHaveValueAsync("Value one.");

                var value2TextInput = Page.Locator("#Value2");
                await Expect(value2TextInput).ToBeEmptyAsync();

                var clientValidationBtn = Page.GetByTestId($"{TestIDsPrefix}-btn-client");
                await clientValidationBtn.ClickAsync();
                await ExpectValidationFailed(
                    operatorPrefix: TestIDsPrefix,
                    value2ErrorMsg: "Value2 must be equal to Value1",
                    alertValidationMsg: "Model validation failed"
                );

                var resetFormBtn = Page.GetByTestId($"{TestIDsPrefix}-btn-reset");
                await resetFormBtn.ClickAsync();
                await Expect(value1TextInput).ToBeEmptyAsync();
                await Expect(value2TextInput).ToBeEmptyAsync();

                await value1TextInput.TypeAsync("Value two.");
                await Expect(value1TextInput).ToHaveValueAsync("Value two.");

                var serverValidationBtn = Page.GetByTestId($"{TestIDsPrefix}-btn-server");
                await Page.RunAndWaitForResponseAsync(async () =>
                {
                    await serverValidationBtn.ClickAsync();
                }, resp => new Uri(resp.Url).GetLeftPart(UriPartial.Path).EndsWith("/validate"));

                await ExpectValidationFailed(
                    operatorPrefix: TestIDsPrefix,
                    value2ErrorMsg: "Value2 must be equal to Value1",
                    alertValidationMsg: "Value2 must be equal to Value1"
                );
            }

            [TestMethod]
            public virtual async Task SameValues()
            {
                await Page.GotoAsync(PageUri.AbsoluteUri);

                await Expect(Page).ToHaveTitleAsync(new Regex(@".+\s+[-]\s+EqualTo"));

                var value1TextInput = Page.Locator("#Value1");
                await value1TextInput.TypeAsync("Value one.");
                await Expect(value1TextInput).ToHaveValueAsync("Value one.");

                var value2TextInput = Page.Locator("#Value2");
                await value2TextInput.TypeAsync("Value one.");
                await Expect(value2TextInput).ToHaveValueAsync("Value one.");

                var clientValidationBtn = Page.GetByTestId($"{TestIDsPrefix}-btn-client");
                await clientValidationBtn.ClickAsync();
                await ExpectValidationSucceed(TestIDsPrefix, "Model validation succeed");

                var resetFormBtn = Page.GetByTestId($"{TestIDsPrefix}-btn-reset");
                await resetFormBtn.ClickAsync();
                await Expect(value1TextInput).ToBeEmptyAsync();
                await Expect(value2TextInput).ToBeEmptyAsync();

                await value1TextInput.TypeAsync("Value one.");
                await Expect(value1TextInput).ToHaveValueAsync("Value one.");

                await value2TextInput.TypeAsync("Value one.");
                await Expect(value2TextInput).ToHaveValueAsync("Value one.");

                var serverValidationBtn = Page.GetByTestId($"{TestIDsPrefix}-btn-server");
                await Page.RunAndWaitForResponseAsync(async () =>
                {
                    await serverValidationBtn.ClickAsync();
                }, resp => new Uri(resp.Url).GetLeftPart(UriPartial.Path).EndsWith("/validate"));

                await ExpectValidationSucceed(TestIDsPrefix, "Model validation succeed");
            }

            [TestMethod]
            public virtual async Task DifferentValues()
            {
                await Page.GotoAsync(PageUri.AbsoluteUri);

                await Expect(Page).ToHaveTitleAsync(new Regex(@".+\s+[-]\s+EqualTo"));

                var value1TextInput = Page.Locator("#Value1");
                await value1TextInput.TypeAsync("Value one.");
                await Expect(value1TextInput).ToHaveValueAsync("Value one.");

                var value2TextInput = Page.Locator("#Value2");
                await value2TextInput.TypeAsync("Value two.");
                await Expect(value2TextInput).ToHaveValueAsync("Value two.");

                var clientValidationBtn = Page.GetByTestId($"{TestIDsPrefix}-btn-client");
                await clientValidationBtn.ClickAsync();
                await ExpectValidationFailed(
                    operatorPrefix: TestIDsPrefix,
                    value2ErrorMsg: "Value2 must be equal to Value1",
                    alertValidationMsg: "Model validation failed"
                );

                var resetFormBtn = Page.GetByTestId($"{TestIDsPrefix}-btn-reset");
                await resetFormBtn.ClickAsync();
                await Expect(value1TextInput).ToBeEmptyAsync();
                await Expect(value2TextInput).ToBeEmptyAsync();

                await value1TextInput.TypeAsync("Value one.");
                await Expect(value1TextInput).ToHaveValueAsync("Value one.");

                await value2TextInput.TypeAsync("Value two.");
                await Expect(value2TextInput).ToHaveValueAsync("Value two.");

                var serverValidationBtn = Page.GetByTestId($"{TestIDsPrefix}-btn-server");
                await Page.RunAndWaitForResponseAsync(async () =>
                {
                    await serverValidationBtn.ClickAsync();
                }, resp => new Uri(resp.Url).GetLeftPart(UriPartial.Path).EndsWith("/validate"));

                await ExpectValidationFailed(
                    operatorPrefix: TestIDsPrefix,
                    value2ErrorMsg: "Value2 must be equal to Value1",
                    alertValidationMsg: "Value2 must be equal to Value1"
                );
            }
        }

        [TestClass]
        public class PassWithNull : Default
        {
            protected override string TestIDsPrefix => "eq2wn";

            protected override Uri PageUri => new(new Uri(WebAppUrl), "equalto-pwn");

            [TestMethod]
            public override async Task Value2Empty()
            {
                var resp = await Page.GotoAsync(PageUri.AbsoluteUri);

                var body = await resp!.TextAsync();

                await Expect(Page).ToHaveTitleAsync(new Regex(@".+\s+[-]\s+EqualTo"));

                var value1TextInput = Page.Locator("#Value1");
                await value1TextInput.TypeAsync("Value one.");
                await Expect(value1TextInput).ToHaveValueAsync("Value one.");

                var value2TextInput = Page.Locator("#Value2");
                await Expect(value2TextInput).ToBeEmptyAsync();

                var clientValidationBtn = Page.GetByTestId($"{TestIDsPrefix}-btn-client");
                await clientValidationBtn.ClickAsync();
                await ExpectValidationSucceed(
                    operatorPrefix: TestIDsPrefix,
                    alertValidationMsg: "Model validation succeed."
                );

                var resetFormBtn = Page.GetByTestId($"{TestIDsPrefix}-btn-reset");
                await resetFormBtn.ClickAsync();
                await Expect(value1TextInput).ToBeEmptyAsync();
                await Expect(value2TextInput).ToBeEmptyAsync();

                await value1TextInput.TypeAsync("Value two.");
                await Expect(value1TextInput).ToHaveValueAsync("Value two.");

                var serverValidationBtn = Page.GetByTestId($"{TestIDsPrefix}-btn-server");
                await Page.RunAndWaitForResponseAsync(async () =>
                {
                    await serverValidationBtn.ClickAsync();
                }, resp => new Uri(resp.Url).GetLeftPart(UriPartial.Path).EndsWith("/validate"));

                await ExpectValidationSucceed(
                    operatorPrefix: TestIDsPrefix,
                    alertValidationMsg: "Model validation succeed."
                );
            }
        }
    }
}