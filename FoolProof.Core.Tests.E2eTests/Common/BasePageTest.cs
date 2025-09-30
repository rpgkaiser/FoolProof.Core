using System.Collections;
using System.Diagnostics;
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

        protected int Wait4MsgTimeout { get; set; } = 30000;

        protected int CallServerRetryCount { get; set; } = 1;

        private static readonly string[] logTypes = new string[] { "log", "info", "debug", "trace", "error", "warning" };

        [TestInitialize]
        public virtual async Task InitTest()
        {
            Browser = await Playwright.Chromium.LaunchAsync();
            Context = await Browser.NewContextAsync();
            Page = await Context.NewPageAsync();

            UseJQuery = TestEnv.UseJQuery;
            Wait4MsgTimeout = TestEnv.Wait4MsgTimeout;
            CallServerRetryCount = TestEnv.CallServerRetryCount;
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

            var rbtn = UseJQuery ?? true
                        ? Page.GetByTestId("use-jquery-rbtn")
                        : Page.GetByTestId("use-aspnetclient-rbtn");
            await Expect(rbtn).ToBeCheckedAsync();
        }

        protected virtual async Task VerifyValidationResult(InputTestValue input)
        {
            var inputValidMsg = Page.GetByTestId(input.ValidMsgElemTestId);
            //var textCont = await inputValidMsg.TextContentAsync();
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
            var validAlertDiv = Page.GetByTestId($"valid-alert");
            await Expect(validAlertDiv).ToBeVisibleAsync();
            var cssClass = await validAlertDiv.GetAttributeAsync("class");
            var content = await Page.ContentAsync();
            Assert.IsTrue(cssClass is not null && cssClass.Contains("alert-success"));
            await Expect(validAlertDiv).ToContainTextAsync(alertValidationMsg);
        }

        protected virtual async Task ExpectValidationFailed(params string[] alertValidationMsgs)
        {
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
            var input = Page.Locator(inputSeltor);
            var isCheckbox = await input.And(Page.Locator("input[type=checkbox]")).CountAsync() > 0;
            var isRadio = await input.And(Page.Locator("input[type=radio]")).CountAsync() > 0;
            if ((isCheckbox || isRadio) && value is bool boolValue)
            {
                //The input is a checkbox or radio
                if (boolValue && (!await input.IsCheckedAsync()))
                {
                    await input.CheckAsync(new() { Force = !verifyValue });
                    return;
                }

                if (!boolValue && await input.IsCheckedAsync())
                    await input.UncheckAsync(new() { Force = !verifyValue });

                return;
            }            
            
            var isSelect = await input.And(Page.Locator("select")).CountAsync() > 0;
            if(isSelect)
            {
                //The input is a select
                if (value is IEnumerable vals && value is not string)
                {
                    var strVals = vals.Cast<object>().Select(o => ConvertToString(o)).ToArray();
                    await input.SelectOptionAsync(strVals.Select(v => new SelectOptionValue { Value = v }));
                    if (verifyValue)
                        try { await Expect(input).ToHaveValuesAsync(strVals); }
                        catch (Exception ex)
                        {
                            if (strVals.Length > 1)
                                throw new Exception($"Multi-Value could not be assigned and/or verified. Field: {inputSeltor}; Value: {string.Join(';', strVals)}", ex);

                            await Expect(input).ToHaveValueAsync(strVals[0]);
                        }

                    return;
                }

                var strVal = ConvertToString(value);
                if (string.IsNullOrEmpty(strVal))
                {
                    var currVal = await input.InputValueAsync();
                    if (!string.IsNullOrEmpty(currVal))
                    {
                        await input.SelectOptionAsync(new SelectOptionValue { Value = string.Empty });
                        if (verifyValue)
                            await Expect(input).ToBeEmptyAsync();
                    }

                    return;
                }
                else
                {
                    await input.SelectOptionAsync(new SelectOptionValue { Value = strVal });
                    if (verifyValue)
                        await Expect(input).ToHaveValueAsync(strVal);

                    return;
                }
            }

            var isText = await input.And(Page.Locator("input, textarea")).CountAsync() > 0;
            if (isText)
            {
                //The input is a text or textarea
                var strVal = ConvertToString(value);
                await input.FillAsync(strVal);
                if (verifyValue)
                    await Expect(input).ToHaveValueAsync(strVal);

                return;
            }

            throw new Exception($"Value could not be assigned and/or verified. Field: {inputSeltor}; Value: {value}");
        }

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

            if(inputs is not null && inputs.Length > 0)
                await ExpectEmpties(inputs);
        }

        protected virtual async Task ResetForm(TestValues testValues)
        {
            var resetFormBtn = Page.GetByTestId($"btn-reset");
            await resetFormBtn.ClickAsync();

            var expEmpties = testValues.AllValues().Where(tv => tv.ResetAsEmpty == true);
            await ExpectEmpties([.. expEmpties]);
        }

        protected async Task<bool> CallClientValidation(TestValues? testValues = null, bool resetFirst = false, bool verifyValidResults = false)
        {
            if(testValues is not null)
                await AssignTestValues(testValues, resetFirst);

            var validMsgTask = ExpectValidationCompletedMessage();

            var clientValidationBtn = Page.GetByTestId($"btn-client");
            await clientValidationBtn.ClickAsync();

            //Wait for the JavaScript processing to finish before continue with the test
            var result = await validMsgTask;

            if (verifyValidResults && testValues is not null)
                foreach (var testVal in testValues.AllValues())
                    await VerifyValidationResult(testVal);

            return result;
        }

        protected async Task<bool> CallServerValidation(TestValues? testValues = null, bool resetFirst = false, bool verifyValidResults = false)
        {
            if (testValues is not null)
                await AssignTestValues(testValues, resetFirst);

            Task<bool> validMsgTask = Task.FromResult(false);

            var serverValidationBtn = Page.GetByTestId($"btn-server");

            for (int i = 0; i < CallServerRetryCount; i++)
            {
                try
                {
                    validMsgTask = ExpectValidationCompletedMessage();

                    await Page.RunAndWaitForResponseAsync(
                        async () => {
                            await serverValidationBtn.ClickAsync();
                        },
                        resp => resp.Ok
                                && string.Equals(resp.Request.Method, "POST", StringComparison.OrdinalIgnoreCase)
                                && new Uri(resp.Url).GetLeftPart(UriPartial.Path).EndsWith("/validate")
                    );
                    break;
                }
                catch (TimeoutException ex)
                {
                    if (i + 1 < CallServerRetryCount)
                        Trace.WriteLine($"Server response not received in the {i+1}th intent. Trying again");
                    else
                        throw new Exception($"Server response not received after {CallServerRetryCount} intents.", ex);
                }
            }

            //Wait for the JavaScript processing to finish before continue with the test
            var result = await validMsgTask;

            if (verifyValidResults && testValues is not null)
                foreach (var testVal in testValues.AllValues())
                    await VerifyValidationResult(testVal);

            return result;
        }

        protected async Task<bool> ExpectValidationCompletedMessage(bool? succeed = null)
        {
            var consoleMsg = await Page.WaitForConsoleMessageAsync(new() {
                Predicate = msgObj => logTypes.Contains(msgObj.Type)
                                      && msgObj.Text.Contains("Validation completed", StringComparison.OrdinalIgnoreCase),
                Timeout = Wait4MsgTimeout
            });

            var result = consoleMsg.Args.Count > 1 && await consoleMsg.Args[1].JsonValueAsync<bool>();
            if(succeed.HasValue)
                Assert.AreEqual(succeed.Value, result);

            return result;
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
                var result = new InputTestValue(InputId, Value, IsSelect, ValidResultText, ValidMsgElemTestId, ResetAsEmpty);
                
                modify?.Invoke(result);

                return result;
            }
        }

        protected abstract class TestValues
        {
            public abstract IEnumerable<InputTestValue> AllValues();
        }
    }
}