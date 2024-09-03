using System.Text.RegularExpressions;

namespace FoolProof.Core.Tests.E2eTests
{
    public class NotEqualToTest
    {
        [TestClass]
        public class Default : BasePageTest
        {
            protected override Regex PageTitleRegex() => new (@".+\s+[-]\s+NotEqualTo");

            protected override Uri PageUri() => new (new Uri(WebAppUrl), "notequalto");

            [TestMethod]
            public virtual async Task EmptyValues_ER()
            {
                await LoadPage();

                await ExpectValue1Empty();
                await ExpectValue2Empty();
                
                await CallClientValidation();
                await ExpectValidationFailed(
                    value2ErrorMsg: "Value2 must be not equal to Value1",
                    alertValidationMsg: "Model validation failed"
                );

                await ResetForm();

                await CallServerValidation();
                await ExpectValidationFailed("Value2 must be not equal to Value1");
            }

            [TestMethod]
            public virtual async Task Value1Empty_OK()
            {
                await LoadPage();

                await ExpectValue1Empty();
                await AssignValue2("Value two.");

                await CallClientValidation();
                await ExpectValidationSucceed();

                await ResetForm();
                await AssignValue2("Value two.");

                await CallServerValidation();
                await ExpectValidationSucceed();
            }

            [TestMethod]
            public virtual async Task Value2Empty_OK()
            {
                await LoadPage();

                await AssignValue1("Value one");
                await ExpectValue2Empty();

                await CallClientValidation();
                await ExpectValidationSucceed();

                await ResetForm();
                await AssignValue1("Value one");

                await CallServerValidation();
                await ExpectValidationSucceed();
            }

            [TestMethod]
            public virtual async Task SameValues_ER()
            {
                await LoadPage();

                await AssignValue1("Same value.");
                await AssignValue2("Same value.");

                await CallClientValidation();
                await ExpectValidationFailed(
                    value2ErrorMsg: "Value2 must be not equal to Value1",
                    alertValidationMsg: "Model validation failed"
                );

                await ResetForm();

                await AssignValue1("Same value.");
                await AssignValue2("Same value.");

                await CallServerValidation();
                await ExpectValidationFailed("Value2 must be not equal to Value1");
            }

            [TestMethod]
            public virtual async Task DifferentValues_OK()
            {
                await LoadPage();

                await AssignValue1("Value one.");
                await AssignValue2("Value two.");

                await CallClientValidation();
                await ExpectValidationSucceed();

                await ResetForm();

                await AssignValue1("Value one.");
                await AssignValue2("Value two.");

                await CallServerValidation();
                await ExpectValidationSucceed();
            }
        }
    }
}