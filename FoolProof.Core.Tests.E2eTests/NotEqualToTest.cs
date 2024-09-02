using System.Text.RegularExpressions;

namespace FoolProof.Core.Tests.E2eTests
{
    public class NotEqualToTest
    {
        [TestClass]
        public class Default : BasePageTest
        {
            protected override Regex PageTitleRegex { get; } = new (@".+\s+[-]\s+NotEqualTo");

            protected override Uri PageUri { get; } = new (new Uri(WebAppUrl), "notequalto");

            [TestMethod]
            public virtual async Task EmptyValues()
            {
                await LoadPage();

                await ExpectValue1Empty();
                await ExpectValue2Empty();
                
                await ExecClientValidation();
                await ExpectValidationFailed(
                    value2ErrorMsg: "Value2 must be not equal to Value1",
                    alertValidationMsg: "Model validation failed"
                );

                await ResetForm();

                await ExecServerValidation();
                await ExpectValidationFailed("Value2 must be not equal to Value1");
            }

            [TestMethod]
            public virtual async Task Value1Empty()
            {
                await LoadPage();

                await ExpectValue1Empty();
                await AssignValue2("Value two.");

                await ExecClientValidation();
                await ExpectValidationSucceed();

                await ResetForm();
                await AssignValue2("Value two.");

                await ExecServerValidation();
                await ExpectValidationSucceed();
            }

            [TestMethod]
            public virtual async Task Value2Empty()
            {
                await LoadPage();

                await AssignValue1("Value one");
                await ExpectValue2Empty();

                await ExecClientValidation();
                await ExpectValidationSucceed();

                await ResetForm();
                await AssignValue1("Value one");

                await ExecServerValidation();
                await ExpectValidationSucceed();
            }

            [TestMethod]
            public virtual async Task SameValues()
            {
                await LoadPage();

                await AssignValue1("Same value.");
                await AssignValue2("Same value.");

                await ExecClientValidation();
                await ExpectValidationFailed(
                    value2ErrorMsg: "Value2 must be not equal to Value1",
                    alertValidationMsg: "Model validation failed"
                );

                await ResetForm();

                await AssignValue1("Same value.");
                await AssignValue2("Same value.");

                await ExecServerValidation();
                await ExpectValidationFailed("Value2 must be not equal to Value1");
            }

            [TestMethod]
            public virtual async Task DifferentValues()
            {
                await LoadPage();

                await AssignValue1("Value one.");
                await AssignValue2("Value two.");

                await ExecClientValidation();
                await ExpectValidationSucceed();

                await ResetForm();

                await AssignValue1("Value one.");
                await AssignValue2("Value two.");

                await ExecServerValidation();
                await ExpectValidationSucceed();
            }
        }

        [TestClass]
        public class PassWithNull : Default
        {
            protected override Uri PageUri { get; } = new (new Uri(WebAppUrl), "notequalto-pwn");

            [TestMethod]
            public override async Task Value1Empty()
            {
                await LoadPage();

                await ExpectValue1Empty();
                await AssignValue2("Value two.");

                await ExecClientValidation();
                await ExpectValidationSucceed();

                await ResetForm();
                await AssignValue2("Value two.");

                await ExecServerValidation();
                await ExpectValidationSucceed();
            }

            [TestMethod]
            public override async Task Value2Empty()
            {
                await LoadPage();

                await AssignValue1("Value one.");
                await ExpectValue2Empty();

                await ExecClientValidation();
                await ExpectValidationSucceed();

                await ResetForm();
                await AssignValue1("Value one.");

                await ExecServerValidation();
                await ExpectValidationSucceed();
            }
        }
    }
}