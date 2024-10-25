namespace FoolProof.Core.Tests.E2eTests
{
    public abstract class CompareBaseTest : BasePageTest
    {
        protected abstract string DataType { get; }

        protected abstract string Value2ValidationError { get; }

        protected abstract string ValuePwnValidationError { get; }

        [CustomTestMethod("Empty Values : Invalid")]
        public virtual async Task EmptyValues()
        {
            await LoadPage();

            await ExpectValue1Empty();
            await ExpectValue2Empty();
            await ExpectValuePwnEmpty();

            await CallClientValidation();
            await ExpectClientValidationFailed();

            await ResetForm();

            await CallServerValidation();
            await ExpectServerValidationFailed();
        }

        [CustomTestMethod("Not Valid Values : Invalid")]
        public virtual async Task InvalidValues()
        {
            await LoadPage();

            var invalidValue = GetNotValidValue();
            await AssignValue1(invalidValue);
            await AssignValue2(invalidValue);
            await AssignValuePwn(invalidValue);

            await CallClientValidation();
            await ExpectClientValidationFailed();

            await ResetForm();

            await AssignValue1(invalidValue);
            await AssignValue2(invalidValue);
            await AssignValuePwn(invalidValue);

            await CallServerValidation();
            await ExpectValidationFailed(
                value2ErrorMsg: Value2ValidationError,
                valuePwnErrorMsg: ValuePwnValidationError,
                alertValidationMsgs: [InvalidValue1ValidationError(invalidValue), 
                    InvalidValue2ValidationError(invalidValue),
                    InvalidValuePwnValidationError(invalidValue)
                ]
            );
        }

        [CustomTestMethod("Value1 is Empty : Invalid")]
        public virtual async Task Value1Empty()
        {
            await LoadPage();

            var vals = GetValues2PassCompare();
            await ExpectValue1Empty();
            await AssignValue2(vals.Value2);
            await AssignValuePwn(vals.ValuePwn);

            await CallClientValidation();
            await ExpectValidationFailed(
                value2ErrorMsg: Value2ValidationError,
                valuePwnErrorMsg: "",
                alertValidationMsgs: "Model validation failed"
            );

            await ResetForm();
            await AssignValue2(vals.Value2);
            await AssignValuePwn(vals.ValuePwn);

            await CallServerValidation();
            await ExpectValidationFailed(
                value2ErrorMsg: Value2ValidationError,
                valuePwnErrorMsg: "",
                alertValidationMsgs: [Value2ValidationError]
            );
        }

        [CustomTestMethod("Value2 is Empty : Invalid")]
        public virtual async Task Value2Empty()
        {
            await LoadPage();

            var vals = GetValues2PassCompare();
            await AssignValue1(vals.Value1);
            await ExpectValue2Empty();
            await AssignValuePwn(vals.ValuePwn);

            await CallClientValidation();
            await ExpectValidationFailed(
                value2ErrorMsg: Value2ValidationError,
                valuePwnErrorMsg: "",
                alertValidationMsgs: "Model validation failed"
            );

            await ResetForm();
            await AssignValue1(vals.Value1);
            await AssignValuePwn(vals.ValuePwn);

            await CallServerValidation();
            await ExpectValidationFailed(
                value2ErrorMsg: Value2ValidationError,
                valuePwnErrorMsg: "",
                alertValidationMsgs: [Value2ValidationError]
            );
        }

        [TestMethod]
        public virtual async Task CompareValuesPass()
        {
            await LoadPage();

            var (value1, value2, valuePwn) = GetValues2PassCompare();
            await AssignValue1(value1);
            await AssignValue2(value2);
            await AssignValue2(valuePwn);

            await CallClientValidation();
            await ExpectValidationSucceed();

            await ResetForm();

            await AssignValue1(value1);
            await AssignValue2(value2);
            await AssignValue2(valuePwn);

            await CallServerValidation();
            await ExpectValidationSucceed();
        }

        [TestMethod]
        public virtual async Task CompareValuesFails()
        {
            await LoadPage();

            var (value1, value2, valuePwn) = GetValues2FailsCompare();
            await AssignValue1(value1);
            await AssignValue2(value2);
            await AssignValuePwn(valuePwn);

            await CallClientValidation();
            await ExpectClientValidationFailed();

            await ResetForm();

            await AssignValue1(value1);
            await AssignValue2(value2);
            await AssignValuePwn(valuePwn);

            await CallServerValidation();
            await ExpectServerValidationFailed();
        }

        protected virtual string GetNotValidValue() => $"Not {DataType} value.";

        protected virtual string InvalidValue1ValidationError(string invalidValue)
            => $"The value '{invalidValue}' is not valid for Value1.";

        protected virtual string InvalidValue2ValidationError(string invalidValue)
            => $"The value '{invalidValue}' is not valid for Value2.";

        protected virtual string InvalidValuePwnValidationError(string invalidValue)
            => $"The value '{invalidValue}' is not valid for ValuePwn.";

        protected abstract (string Value1, string Value2, string ValuePwn) GetValues2PassCompare();

        protected abstract (string Value1, string Value2, string ValuePwn) GetValues2FailsCompare();

        protected Task ExpectClientValidationFailed()
            => ExpectValidationFailed(
                    value2ErrorMsg: Value2ValidationError,
                    valuePwnErrorMsg: ValuePwnValidationError,
                    alertValidationMsgs: "Model validation failed"
                );

        protected Task ExpectServerValidationFailed()
            => ExpectValidationFailed(
                    value2ErrorMsg: Value2ValidationError,
                    valuePwnErrorMsg: ValuePwnValidationError,
                    alertValidationMsgs: [Value2ValidationError, ValuePwnValidationError]
                );
    }
}
