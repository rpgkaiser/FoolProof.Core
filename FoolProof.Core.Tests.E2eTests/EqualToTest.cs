using System.ComponentModel;
using System.Text.RegularExpressions;

namespace FoolProof.Core.Tests.E2eTests
{
    public class EqualToTest
    {
        [TestClass]
        public class Default : CompareBaseTest
        {
            protected override Regex PageTitleRegex() => new (@".+\s+[-]\s+EqualTo");

            protected override Uri PageUri() => new (new Uri(WebAppUrl), "eq2");

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

            [CustomTestMethod("Empty Values : Valid")]
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
            public override Task InvalidValues()
            {
                return base.InvalidValues();
            }

            [CustomTestMethod("Value1 == Value2 : Valid")]
            public override Task CompareValuesPass()
            {
                return base.CompareValuesPass();
            }

            [CustomTestMethod("Value1 != Value2 : Invalid")]
            public override Task CompareValuesFails()
            {
                return base.CompareValuesFails();
            }
        }

        [TestClass]
        public class PassWithNull : CompareBaseTest_PassWithNull
        {
            protected override Regex PageTitleRegex() => new(@".+\s+[-]\s+EqualTo");

            protected override Uri PageUri() => new (new Uri(WebAppUrl), "eq2?pwn=true");

            protected override string DataType => "string";

            protected override string Value2ValidationError => "Value2 must be equal to Value1";

            [CustomTestMethod("Empty Values : Valid")]
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
            public override Task InvalidValues()
            {
                return base.InvalidValues();
            }

            [CustomTestMethod("Value1 is Empty : Valid")]
            public override Task Value1Empty()
            {
                return base.Value1Empty();
            }

            [CustomTestMethod("Value2 is Empty : Valid")]
            public override Task Value2Empty()
            {
                return base.Value2Empty();
            }

            [CustomTestMethod("Value1 == Value2 : Valid")]
            public override Task CompareValuesPass()
            {
                return base.CompareValuesPass();
            }

            [CustomTestMethod("Value1 != Value2 : Invalid")]
            public override Task CompareValuesFails()
            {
                return base.CompareValuesFails();
            }

            protected override (string Value1, string Value2) GetValues2PassCompare()
            {
                return ("Value one", "Value one");
            }

            protected override (string Value1, string Value2) GetValues2FailsCompare()
            {
                return ("Value one", "Value two");
            }
        }
    }
}