using System.Text.RegularExpressions;

namespace FoolProof.Core.Tests.E2eTests
{
    public class EqualToTest
    {
        [TestClass]
        public class Default : CompareBaseTest
        {
            protected override Regex PageTitleRegex() => new (@".+\s+[-]\s+EqualTo");

            protected override Uri PageUri() => new (new Uri(WebAppUrl), "equalto");

            protected override string Value2ValidationError => "Value2 must be equal to Value1";

            protected override string DataType => "string";

            protected override string GetNotValidValue()
            {
                return "Any value is a valid value.";
            }

            protected override (string Value1, string Value2) GetValues2PassCompare()
            {
                return ("Value one", "Value one");
            }

            protected override (string Value1, string Value2) GetValues2FailsCompare()
            {
                return ("Value one", "Value two");
            }

            [TestMethod]
            public override async Task EmptyValues()
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

            [Ignore]
            public override Task SameInvalidValues()
            {
                return base.SameInvalidValues();
            }

            public override async Task SameValues()
            {
                await LoadPage();

                var value = GetValues2PassCompare().Value1;
                await AssignValue1(value);
                await AssignValue2(value);

                await CallClientValidation();
                await ExpectValidationSucceed();

                await ResetForm();

                await AssignValue1(value);
                await AssignValue2(value);

                await CallServerValidation();
                await ExpectValidationSucceed();
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

                var value = GetValues2PassCompare().Value1;
                await ExpectValue1Empty();
                await AssignValue2(value);

                await CallClientValidation();
                await ExpectValidationSucceed();

                await ResetForm();
                await AssignValue2(value);

                await CallServerValidation();
                await ExpectValidationSucceed();
            }

            [TestMethod]
            public override async Task Value2Empty()
            {
                await LoadPage();

                var value = GetValues2PassCompare().Value1;
                await AssignValue1(value);
                await ExpectValue2Empty();

                await CallClientValidation();
                await ExpectValidationSucceed();

                await ResetForm();
                await AssignValue1(value);

                await CallServerValidation();
                await ExpectValidationSucceed();
            }
        }
    }
}