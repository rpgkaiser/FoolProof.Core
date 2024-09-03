using System.Text.RegularExpressions;

namespace FoolProof.Core.Tests.E2eTests
{
    public class EqualToTest
    {
        [TestClass]
        public class Default : BasePageTest
        {
            protected override Regex PageTitleRegex() => new (@".+\s+[-]\s+EqualTo");

            protected override Uri PageUri() => new (new Uri(WebAppUrl), "equalto");

            [TestMethod]
            public virtual async Task EmptyValues_OK()
            {
                await LoadPage();

                await ExpectValue1Empty();
                await ExpectValue2Empty();
                
                await CallClientValidation();
                await ExpectValidationSucceed();

                await ResetForm();

                await CallServerValidation();
                await ExpectValidationSucceed();
            }

            [TestMethod]
            public virtual async Task Value1Empty()
            {
                await LoadPage();

                await ExpectValue1Empty();
                await AssignValue2("Value two.");

                await CallClientValidation();
                await ExpectValidationFailed(
                    value2ErrorMsg: "Value2 must be equal to Value1",
                    alertValidationMsg: "Model validation failed"
                );

                await ResetForm();
                await AssignValue2("Value two.");

                await CallServerValidation();
                await ExpectValidationFailed("Value2 must be equal to Value1");
            }

            [TestMethod]
            public virtual async Task Value2Empty()
            {
                await LoadPage();

                await AssignValue1("Value one");
                await ExpectValue2Empty();

                await CallClientValidation();
                await ExpectValidationFailed(
                    value2ErrorMsg: "Value2 must be equal to Value1",
                    alertValidationMsg: "Model validation failed"
                );

                await ResetForm();
                await AssignValue1("Value one");

                await CallServerValidation();
                await ExpectValidationFailed("Value2 must be equal to Value1");
            }

            [TestMethod]
            public virtual async Task SameValues_OK()
            {
                await LoadPage();

                await AssignValue1("Same value.");
                await AssignValue2("Same value.");

                await CallClientValidation();
                await ExpectValidationSucceed();

                await ResetForm();

                await AssignValue1("Same value.");
                await AssignValue2("Same value.");

                await CallServerValidation();
                await ExpectValidationSucceed();
            }

            [TestMethod]
            public virtual async Task DifferentValues_ER()
            {
                await LoadPage();

                await AssignValue1("Value one.");
                await AssignValue2("Value two.");

                await CallClientValidation();
                await ExpectValidationFailed(
                    value2ErrorMsg: "Value2 must be equal to Value1",
                    alertValidationMsg: "Model validation failed"
                );

                await ResetForm();

                await AssignValue1("Value one.");
                await AssignValue2("Value two.");

                await CallServerValidation();
                await ExpectValidationFailed("Value2 must be equal to Value1");
            }
        }

        [TestClass]
        public class PassWithNull : Default
        {
            protected override Uri PageUri() => new (new Uri(WebAppUrl), "equalto?pwn=true");

            [TestMethod]
            public override async Task Value1Empty()
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
            public override async Task Value2Empty()
            {
                await LoadPage();

                await AssignValue1("Value one.");
                await ExpectValue2Empty();

                await CallClientValidation();
                await ExpectValidationSucceed();

                await ResetForm();
                await AssignValue1("Value one.");

                await CallServerValidation();
                await ExpectValidationSucceed();
            }
        }
    }
}