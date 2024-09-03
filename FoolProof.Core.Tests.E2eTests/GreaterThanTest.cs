using System.Text.RegularExpressions;

namespace FoolProof.Core.Tests.E2eTests
{
    public class GreaterThanTest
    {
        [TestClass]
        public class DateValues : BasePageTest
        {
            protected override Regex PageTitleRegex { get; } = new(@".+\s+[-]\s+GreaterThan\s+\(Date\)");

            protected override Uri PageUri { get; } = new(new Uri(WebAppUrl), "greaterthan-date");

            [TestMethod]
            public virtual async Task EmptyValues_ER()
            {
                await LoadPage();

                await ExpectValue1Empty();
                await ExpectValue2Empty();

                await CallClientValidation();
                await ExpectValidationFailed(
                    value2ErrorMsg: "Value2 must be greater than Value1",
                    alertValidationMsg: "Model validation failed"
                );

                await ResetForm();

                await CallServerValidation();
                await ExpectValidationFailed("Value2 must be greater than Value1");
            }

            [TestMethod]
            public virtual async Task SameNotDateValues_ER()
            {
                await LoadPage();

                await AssignValue1("Not date value.");
                await AssignValue2("Not date value.");

                await CallClientValidation();
                await ExpectValidationFailed(
                    value2ErrorMsg: "Value2 must be greater than Value1",
                    alertValidationMsg: "Model validation failed"
                );

                await ResetForm();

                await AssignValue1("Not date value.");
                await AssignValue2("Not date value.");

                await CallServerValidation();
                await ExpectValidationFailed("Value2 must be greater than Value1", "The value 'Not date value.' is not valid for Value1");
                await ExpectValidationFailed("Value2 must be greater than Value1", "The value 'Not date value.' is not valid for Value2");
            }

            [TestMethod]
            public virtual async Task Value1Empty()
            {
                await LoadPage();

                await ExpectValue1Empty();
                await AssignValue2("10/10/2024");

                await CallClientValidation();
                await ExpectValidationFailed(
                    value2ErrorMsg: "Value2 must be greater than Value1",
                    alertValidationMsg: "Model validation failed"
                );

                await ResetForm();
                await AssignValue2("10/10/2024");

                await CallServerValidation();
                await ExpectValidationFailed("Value2 must be greater than Value1");
            }

            [TestMethod]
            public virtual async Task Value2Empty()
            {
                await LoadPage();

                await AssignValue1("10/10/2024");
                await ExpectValue2Empty();

                await CallClientValidation();
                await ExpectValidationFailed(
                    value2ErrorMsg: "Value2 must be greater than Value1",
                    alertValidationMsg: "Model validation failed"
                );

                await ResetForm();
                await AssignValue1("10/10/2024");

                await CallServerValidation();
                await ExpectValidationFailed("Value2 must be greater than Value1");
            }

            [TestMethod]
            public virtual async Task SameValues_ER()
            {
                await LoadPage();

                await AssignValue1("10/10/2024");
                await AssignValue2("10/10/2024");

                await CallClientValidation();
                await ExpectValidationFailed(
                    value2ErrorMsg: "Value2 must be greater than Value1",
                    alertValidationMsg: "Model validation failed"
                );

                await ResetForm();

                await AssignValue1("10/10/2024");
                await AssignValue2("10/10/2024");

                await CallServerValidation();
                await ExpectValidationFailed("Value2 must be greater than Value1");
            }

            [TestMethod]
            public virtual async Task Value2GreaterThanValueOne_OK()
            {
                await LoadPage();

                await AssignValue1("5/10/2020");
                await AssignValue2("10/20/2024");

                await CallClientValidation();
                await ExpectValidationSucceed();

                await ResetForm();

                await AssignValue1("5/10/2020");
                await AssignValue2("10/20/2024");

                await CallServerValidation();
                await ExpectValidationSucceed();
            }

            [TestMethod]
            public virtual async Task Value1GreaterThanValue2_ER()
            {
                await LoadPage();

                await AssignValue1("10/20/2024");
                await AssignValue2("5/10/2020");

                await CallClientValidation();
                await ExpectValidationFailed(
                    value2ErrorMsg: "Value2 must be greater than Value1",
                    alertValidationMsg: "Model validation failed"
                );

                await ResetForm();

                await AssignValue1("10/20/2024");
                await AssignValue2("5/10/2020");

                await CallServerValidation();
                await ExpectValidationFailed("Value2 must be greater than Value1");
            }
        }

        [TestClass]
        public class DateValues_PassWithNull : DateValues
        {
            protected override Uri PageUri { get; } = new(new Uri(WebAppUrl), "greaterthan-date-pwn");

            [TestMethod]
            public override async Task Value1Empty()
            {
                await LoadPage();

                await ExpectValue1Empty();
                await AssignValue2("10/20/2024");

                await CallClientValidation();
                await ExpectValidationSucceed();

                await ResetForm();
                await AssignValue2("10/20/2024");

                await CallServerValidation();
                await ExpectValidationSucceed();
            }

            [TestMethod]
            public override async Task Value2Empty()
            {
                await LoadPage();

                await AssignValue1("10/20/2024");
                await ExpectValue2Empty();

                await CallClientValidation();
                await ExpectValidationSucceed();

                await ResetForm();
                await AssignValue1("10/20/2024");

                await CallServerValidation();
                await ExpectValidationSucceed();
            }
        }
    }
}