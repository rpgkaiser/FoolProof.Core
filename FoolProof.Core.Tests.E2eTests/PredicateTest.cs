using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace FoolProof.Core.Tests.E2eTests
{
    [TestClass]
    public class PredicateTest : BasePageTest
    {
        protected override Regex PageTitleRegex() => new(@".+\s+[-]\s+Predicate");

        protected override Uri PageUri() => new(new Uri(WebAppUrl), "predicate");

        protected override string Value1ValidMsgId => $"{nameof(Predicate.Model.FirstName).ToLowerInvariant()}-valid-msg";

        protected override string Value2ValidMsgId => $"{nameof(Predicate.Model.LastName).ToLowerInvariant()}-valid-msg";

        protected override string ValuePwnValidMsgId => $"{nameof(Predicate.Model.Email).ToLowerInvariant()}-valid-msg";

        [TestInitialize]
        public override Task InitTest()
        {
            UseInputTypes = true;
            return base.InitTest();
        }

        [CustomTestMethod]
        public async Task ModelValidationPass()
        {
            await LoadPage();

            foreach (var testValues in GetValues2PassValidation())
            {
                await CallClientValidation(testValues, true);
                await ExpectValidationSucceed();

                await CallServerValidation(testValues, true);
                await ExpectValidationSucceed();
            }
        }

        [CustomTestMethod]
        public async Task ModelValidationFails()
        {
            await LoadPage();

            foreach (var testValues in GetValues2FailsValidation())
            {
                await CallClientValidation(testValues, true);
                await ExpectValidationFailed(testValues, "Model validation failed");
                if(testValues.ModelWiseHandler is not null)
                    await VerifyValidationResult(testValues.ModelWiseHandler);

                await CallServerValidation(testValues, true);

                var validMsgs = testValues.AllValues()
                                .Where(tv => !string.IsNullOrEmpty(tv.ValidResultText))
                                .Select(tv => tv.ValidResultText!);
                await ExpectValidationFailed(testValues, [.. validMsgs]);
                if (testValues.ModelWiseHandler is not null)
                    await VerifyValidationResult(testValues.ModelWiseHandler);
            }
        }

        protected IEnumerable<PredicateTestValues> GetValues2PassValidation()
        {
            var testValues = new PredicateTestValues(
                "Jhonny",
                "McDonald Smith",
                "jhon.mcd@server.com",
                [
                    new(nameof(Predicate.Model.Age), 6),
                    new(nameof(Predicate.Model.YearsOfStudy), 2),
                    new(nameof(Predicate.Model.ElementarySchool), false, false, resetAsEmpty: false),
                    new(nameof(Predicate.Model.HighSchool), false, false, resetAsEmpty: false),
                    new(nameof(Predicate.Model.University), false, false, resetAsEmpty: false),
                    new(nameof(Predicate.Model.Country), "", true),
                    new(nameof(Predicate.Model.PhoneNumber), "")
                ]
            );
            yield return testValues;
            yield return testValues = testValues.Clone(tv => {
                tv.OtherValues.First(ov => ov.InputId == nameof(Predicate.Model.Age)).Value = 12;
                tv.OtherValues.First(ov => ov.InputId == nameof(Predicate.Model.YearsOfStudy)).Value = 6;
                tv.OtherValues.First(ov => ov.InputId == nameof(Predicate.Model.ElementarySchool)).Value = true;
            });
            yield return testValues = testValues.Clone(tv => {
                tv.OtherValues.First(ov => ov.InputId == nameof(Predicate.Model.Age)).Value = 18;
                tv.OtherValues.First(ov => ov.InputId == nameof(Predicate.Model.YearsOfStudy)).Value = 10;
                tv.OtherValues.First(ov => ov.InputId == nameof(Predicate.Model.HighSchool)).Value = true;
            });
            yield return testValues.Clone(tv => {
                tv.OtherValues.First(ov => ov.InputId == nameof(Predicate.Model.Age)).Value = 20;
                tv.OtherValues.First(ov => ov.InputId == nameof(Predicate.Model.YearsOfStudy)).Value = 14;
                tv.OtherValues.First(ov => ov.InputId == nameof(Predicate.Model.University)).Value = true;
            });
            yield return testValues.Clone(tv => {
                tv.OtherValues.First(ov => ov.InputId == nameof(Predicate.Model.Country)).Value = "US";
            });
            yield return testValues.Clone(tv => {
                tv.OtherValues.First(ov => ov.InputId == nameof(Predicate.Model.Country)).Value = "US";
                tv.OtherValues.First(ov => ov.InputId == nameof(Predicate.Model.PhoneNumber)).Value = "+13053054567";
            });
            yield return testValues.Clone(tv => {
                tv.OtherValues.First(ov => ov.InputId == nameof(Predicate.Model.Country)).Value = "ES";
                tv.OtherValues.First(ov => ov.InputId == nameof(Predicate.Model.PhoneNumber)).Value = "+34446234528";
            });
            yield return testValues.Clone(tv => {
                tv.OtherValues.First(ov => ov.InputId == nameof(Predicate.Model.Country)).Value = "CU";
                tv.OtherValues.First(ov => ov.InputId == nameof(Predicate.Model.PhoneNumber)).Value = "+5352048866";
            });
        }

        protected IEnumerable<PredicateTestValues> GetValues2FailsValidation()
        {
            var testValues = new PredicateTestValues(
                "To",
                "Small",
                "no.valid.email",
                [
                    new(nameof(Predicate.Model.Age), 4),
                    new(nameof(Predicate.Model.YearsOfStudy), 4),
                    new(nameof(Predicate.Model.ElementarySchool), true, false, validResText: "Years of study do not cover the elementary school duration.", resetAsEmpty: false),
                    new(nameof(Predicate.Model.HighSchool), true, false, validResText: "No elementary school or years of study do not cover the high school duration.", resetAsEmpty: false),
                    new(nameof(Predicate.Model.University), true, false, validResText: "No high school or years of study do not cover the university duration.", resetAsEmpty: false),
                    new(nameof(Predicate.Model.Country), "US", true),
                    new(nameof(Predicate.Model.PhoneNumber), "+340054652", false, validResText: "Invalid international phone number format.")
                ]
            ) {
                ModelWiseHandler = new("ModelValidationHandler", validResText: "Personal information validation failed")
            };

            yield return testValues;

            yield return testValues = testValues.Clone(tv => {
                tv.Value1 = "Jhonny";
            });
            yield return testValues = testValues.Clone(tv => {
                tv.Value2 = "McDonald Smith";
            });
            yield return testValues = testValues.Clone(tv => {
                tv.ValuePwn = "jhon.mcd@server.com";
            });
            yield return testValues = testValues.Clone(tv => {
                tv.OtherValues.First(ov => ov.InputId == nameof(Predicate.Model.Age)).Value = 22;
                tv.ModelWiseHandler!.ValidResultText = string.Empty;
            });
            yield return testValues = testValues.Clone(tv => {
                tv.OtherValues.First(ov => ov.InputId == nameof(Predicate.Model.Age)).Value = 12;
                tv.OtherValues.First(ov => ov.InputId == nameof(Predicate.Model.YearsOfStudy)).Value = 6;
                tv.OtherValues.First(ov => ov.InputId == nameof(Predicate.Model.ElementarySchool)).Value = true;
                tv.OtherValues.First(ov => ov.InputId == nameof(Predicate.Model.ElementarySchool)).ValidResultText = string.Empty;
            });
            yield return testValues = testValues.Clone(tv => {
                tv.OtherValues.First(ov => ov.InputId == nameof(Predicate.Model.Age)).Value = 16;
                tv.OtherValues.First(ov => ov.InputId == nameof(Predicate.Model.YearsOfStudy)).Value = 10;
                tv.OtherValues.First(ov => ov.InputId == nameof(Predicate.Model.HighSchool)).Value = true;
                tv.OtherValues.First(ov => ov.InputId == nameof(Predicate.Model.HighSchool)).ValidResultText = string.Empty;
            });
            yield return testValues = testValues.Clone(tv => {
                tv.OtherValues.First(ov => ov.InputId == nameof(Predicate.Model.Age)).Value = 22;
                tv.OtherValues.First(ov => ov.InputId == nameof(Predicate.Model.YearsOfStudy)).Value = 14;
                tv.OtherValues.First(ov => ov.InputId == nameof(Predicate.Model.University)).Value = true;
                tv.OtherValues.First(ov => ov.InputId == nameof(Predicate.Model.University)).ValidResultText = string.Empty;
            });
            yield return testValues.Clone(tv => {
                tv.OtherValues.First(ov => ov.InputId == nameof(Predicate.Model.Country)).Value = "";
                tv.OtherValues.First(ov => ov.InputId == nameof(Predicate.Model.PhoneNumber)).Value = "+1405305465";
            });
            yield return testValues.Clone(tv => {
                tv.OtherValues.First(ov => ov.InputId == nameof(Predicate.Model.Country)).Value = "US";
                tv.OtherValues.First(ov => ov.InputId == nameof(Predicate.Model.PhoneNumber)).Value = "+1405305465";
            });
            yield return testValues.Clone(tv => {
                tv.OtherValues.First(ov => ov.InputId == nameof(Predicate.Model.Country)).Value = "US";
                tv.OtherValues.First(ov => ov.InputId == nameof(Predicate.Model.PhoneNumber)).Value = "+344053054658";
            });
            yield return testValues.Clone(tv => {
                tv.OtherValues.First(ov => ov.InputId == nameof(Predicate.Model.Country)).Value = "ES";
                tv.OtherValues.First(ov => ov.InputId == nameof(Predicate.Model.PhoneNumber)).Value = "+13053054653";
            });
            yield return testValues.Clone(tv => {
                tv.OtherValues.First(ov => ov.InputId == nameof(Predicate.Model.Country)).Value = "ES";
                tv.OtherValues.First(ov => ov.InputId == nameof(Predicate.Model.PhoneNumber)).Value = "+340530546";
            });
            yield return testValues.Clone(tv => {
                tv.OtherValues.First(ov => ov.InputId == nameof(Predicate.Model.Country)).Value = "CU";
                tv.OtherValues.First(ov => ov.InputId == nameof(Predicate.Model.PhoneNumber)).Value = "+1305305465";
            });
            yield return testValues.Clone(tv => {
                tv.OtherValues.First(ov => ov.InputId == nameof(Predicate.Model.Country)).Value = "CU";
                tv.OtherValues.First(ov => ov.InputId == nameof(Predicate.Model.PhoneNumber)).Value = "+53530546";
            });
        }

        protected class PredicateTestValues : TestValues
        {
            public PredicateTestValues(
                string? firstName = default,
                string? lastName = default,
                string? email = default,
                params InputTestValue[] otherValues
            ) : base(
                    new InputTestValue(nameof(Predicate.Model.FirstName), firstName),
                    new InputTestValue(nameof(Predicate.Model.LastName), lastName),
                    new InputTestValue(nameof(Predicate.Model.Email), email), 
                    otherValues
                ) { }

            public InputTestValue? ModelWiseHandler { get; set; }

            public PredicateTestValues Clone(Action<PredicateTestValues>? modify = null)
            {
                var result = new PredicateTestValues(
                    Value1 as string,
                    Value2 as string,
                    ValuePwn as string,
                    [.. OtherValues]
                ) {
                    ModelWiseHandler = ModelWiseHandler?.Clone()
                };
                
                modify?.Invoke(result);

                return result;
            }
        }
    }
}