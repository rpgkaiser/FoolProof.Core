namespace FoolProof.Core.Tests.E2eTests
{
    public abstract class CompareBaseTest : BasePageTest
    {
        protected abstract string DataType { get; }

        protected abstract string Value2ValidationError { get; }

        [TestMethod("Empty Values : Invalid")]
        public virtual async Task EmptyValues()
        {
            await LoadPage();

            await ExpectValue1Empty();
            await ExpectValue2Empty();

            await CallClientValidation();
            await ExpectValidationFailed(
                value2ErrorMsg: Value2ValidationError,
                alertValidationMsg: "Model validation failed"
            );

            await ResetForm();

            await CallServerValidation();
            await ExpectValidationFailed(Value2ValidationError);
        }

        [TestMethod("Not Valid Values : Invalid")]
        public virtual async Task InvalidValues()
        {
            await LoadPage();

            var invalidValue = GetNotValidValue();
            await AssignValue1(invalidValue);
            await AssignValue2(invalidValue);

            await CallClientValidation();
            await ExpectValidationFailed(
                value2ErrorMsg: Value2ValidationError,
                alertValidationMsg: "Model validation failed"
            );

            await ResetForm();

            await AssignValue1(invalidValue);
            await AssignValue2(invalidValue);

            await CallServerValidation();
            await ExpectValidationFailed(Value2ValidationError, $"The value '{invalidValue}' is not valid for Value1");
            await ExpectValidationFailed(Value2ValidationError, $"The value '{invalidValue}' is not valid for Value2");
        }

        [TestMethod("Value1 is Empty : Invalid")]
        public virtual async Task Value1Empty()
        {
            await LoadPage();

            var value = GetValues2PassCompare().Value1;
            await ExpectValue1Empty();
            await AssignValue2(value);

            await CallClientValidation();
            await ExpectValidationFailed(
                value2ErrorMsg: Value2ValidationError,
                alertValidationMsg: "Model validation failed"
            );

            await ResetForm();
            await AssignValue2(value);

            await CallServerValidation();
            await ExpectValidationFailed(Value2ValidationError);
        }

        [TestMethod("Value2 is Empty : Invalid")]
        public virtual async Task Value2Empty()
        {
            await LoadPage();

            var value = GetValues2PassCompare().Value1;
            await AssignValue1(value);
            await ExpectValue2Empty();

            await CallClientValidation();
            await ExpectValidationFailed(
                value2ErrorMsg: Value2ValidationError,
                alertValidationMsg: "Model validation failed"
            );

            await ResetForm();
            await AssignValue1(value);

            await CallServerValidation();
            await ExpectValidationFailed(Value2ValidationError);
        }

        [TestMethod]
        public virtual async Task CompareValuesPass()
        {
            await LoadPage();

            var (value1, value2) = GetValues2PassCompare();
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
        public virtual async Task CompareValuesFails()
        {
            await LoadPage();

            var (value1, value2) = GetValues2FailsCompare();
            await AssignValue1(value1);
            await AssignValue2(value2);

            await CallClientValidation();
            await ExpectValidationFailed(
                value2ErrorMsg: Value2ValidationError,
                alertValidationMsg: "Model validation failed"
            );

            await ResetForm();

            await AssignValue1(value1);
            await AssignValue2(value2);

            await CallServerValidation();
            await ExpectValidationFailed(Value2ValidationError);
        }

        protected virtual string GetNotValidValue() => $"Not {DataType} value.";

        protected abstract (string Value1, string Value2) GetValues2PassCompare();

        protected abstract (string Value1, string Value2) GetValues2FailsCompare();
    }

    public abstract class CompareBaseTest_PassWithNull : CompareBaseTest
    {
        protected override Uri PageUri() => new(new Uri(WebAppUrl), $"lessthan/{DataType}?pwn=true");

        [TestMethod("Value1 is Empty : Valid")]
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

        [TestMethod("Value2 is Empty : Valid")]
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
