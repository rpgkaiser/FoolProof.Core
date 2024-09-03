using System.Text.RegularExpressions;

namespace FoolProof.Core.Tests.E2eTests
{
    public class GreaterThanTest
    {
        public abstract class BaseTests : BasePageTest
        {
            protected override Regex PageTitleRegex() => new($@".+\s+[-]\s+GreaterThan\s+\({DataType}\)");

            protected override Uri PageUri() => new(new Uri(WebAppUrl), $"greaterthan/{DataType}");

            protected abstract string DataType { get; }

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
            public virtual async Task SameInvalidValues_ER()
            {
                await LoadPage();

                var invalidValue = GetNotValidValue();
                await AssignValue1(invalidValue);
                await AssignValue2(invalidValue);

                await CallClientValidation();
                await ExpectValidationFailed(
                    value2ErrorMsg: "Value2 must be greater than Value1",
                    alertValidationMsg: "Model validation failed"
                );

                await ResetForm();

                await AssignValue1(invalidValue);
                await AssignValue2(invalidValue);

                await CallServerValidation();
                await ExpectValidationFailed("Value2 must be greater than Value1", $"The value '{invalidValue}' is not valid for Value1");
                await ExpectValidationFailed("Value2 must be greater than Value1", $"The value '{invalidValue}' is not valid for Value2");
            }

            [TestMethod]
            public virtual async Task Value1Empty()
            {
                await LoadPage();

                var value = GetValidValues().Value1;
                await ExpectValue1Empty();
                await AssignValue2(value);

                await CallClientValidation();
                await ExpectValidationFailed(
                    value2ErrorMsg: "Value2 must be greater than Value1",
                    alertValidationMsg: "Model validation failed"
                );

                await ResetForm();
                await AssignValue2(value);

                await CallServerValidation();
                await ExpectValidationFailed("Value2 must be greater than Value1");
            }

            [TestMethod]
            public virtual async Task Value2Empty()
            {
                await LoadPage();

                var value = GetValidValues().Value1;
                await AssignValue1(value);
                await ExpectValue2Empty();

                await CallClientValidation();
                await ExpectValidationFailed(
                    value2ErrorMsg: "Value2 must be greater than Value1",
                    alertValidationMsg: "Model validation failed"
                );

                await ResetForm();
                await AssignValue1(value);

                await CallServerValidation();
                await ExpectValidationFailed("Value2 must be greater than Value1");
            }

            [TestMethod]
            public virtual async Task SameValues_ER()
            {
                await LoadPage();

                var value = GetValidValues().Value1;
                await AssignValue1(value);
                await AssignValue2(value);

                await CallClientValidation();
                await ExpectValidationFailed(
                    value2ErrorMsg: "Value2 must be greater than Value1",
                    alertValidationMsg: "Model validation failed"
                );

                await ResetForm();

                await AssignValue1(value);
                await AssignValue2(value);

                await CallServerValidation();
                await ExpectValidationFailed("Value2 must be greater than Value1");
            }

            [TestMethod]
            public virtual async Task Value2GreaterThanValue1_OK()
            {
                await LoadPage();

                var (value1, value2) = GetValidValues();
                await AssignValue1(value1);
                await AssignValue2(value2);

                await CallClientValidation();
                await ExpectValidationSucceed();

                await ResetForm();

                await AssignValue1(value1);
                await AssignValue2(value2);

                await CallServerValidation();
                await ExpectValidationSucceed();
            }

            [TestMethod]
            public virtual async Task Value1GreaterThanValue2_ER()
            {
                await LoadPage();

                var (value1, value2) = GetValidValues(true);
                await AssignValue1(value1);
                await AssignValue2(value2);

                await CallClientValidation();
                await ExpectValidationFailed(
                    value2ErrorMsg: "Value2 must be greater than Value1",
                    alertValidationMsg: "Model validation failed"
                );

                await ResetForm();

                await AssignValue1(value1);
                await AssignValue2(value2);

                await CallServerValidation();
                await ExpectValidationFailed("Value2 must be greater than Value1");
            }

            protected virtual string GetNotValidValue() => $"Not {DataType} value.";

            protected abstract (string Value1, string Value2) GetValidValues(bool firstBigger = false);
        }

        public abstract class BaseTests_PassWithNull : BaseTests
        {
            protected override Uri PageUri() => new(new Uri(WebAppUrl), $"greaterthan/{DataType}?pwn=true");

            [TestMethod]
            public override async Task Value1Empty()
            {
                await LoadPage();

                var value = GetValidValues().Value1;
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

                var value = GetValidValues().Value1;
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

        [TestClass]
        public class DateValues : BaseTests
        {
            protected override string DataType => "Date";

            protected override (string Value1, string Value2) GetValidValues(bool firstBigger = false)
            {
                if (firstBigger)
                    return ("10/10/2010", "5/5/2005");

                return ("5/5/2005", "10/10/2010");
            }
        }

        [TestClass]
        public class DateValues_PassWithNull : BaseTests_PassWithNull
        {
            protected override string DataType => "Date";

            protected override (string Value1, string Value2) GetValidValues(bool firstBigger = false)
            {
                if (firstBigger)
                    return ("10/10/2010", "5/5/2005");

                return ("5/5/2005", "10/10/2010");
            }
        }

        [TestClass]
        public class TimeValues : BaseTests
        {
            protected override string DataType => "Time";

            protected override (string Value1, string Value2) GetValidValues(bool firstBigger = false)
            {
                if (firstBigger)
                    return ("20:30", "10:00");

                return ("08:00", "22:00");
            }
        }

        [TestClass]
        public class TimeValues_PassWithNull : BaseTests_PassWithNull
        {
            protected override string DataType => "Time";

            protected override (string Value1, string Value2) GetValidValues(bool firstBigger = false)
            {
                if (firstBigger)
                    return ("20:30", "10:00");

                return ("08:00", "22:00");
            }
        }
    }
}