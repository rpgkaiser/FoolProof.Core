
namespace FoolProof.Core.Tests.E2eTests
{
    public abstract class CompareBaseTest : BasePageTest
    {
        protected abstract string DataType { get; }

        protected abstract string Value2ValidationError { get; }

        protected abstract string ValuePwnValidationError { get; }

        [CustomTestMethod("Empty Values : Valid")]
        public virtual async Task EmptyValues()
        {
            await EmptyValues(true);
        }

        [CustomTestMethod("Value1 is Empty : Invalid")]
        public virtual async Task Value1Empty()
        {
            await LoadPage();

            var testValues = GetValues2PassValidation();
            await AssignTestValues(testValues, false, true, ["Value1"]);
            await ExpectValue1Empty();

            await CallClientValidation();
            await ExpectValidationFailed(
                Value2ValidationError,
                "",
                "",
                "Model validation failed"
            );

            await AssignTestValues(testValues, true, true, ["Value1"]);
            await ExpectValue1Empty();

            await CallServerValidation();
            await ExpectValidationFailed(
                Value2ValidationError,
                "",
                "",
                Value2ValidationError
            );
        }

        [CustomTestMethod("Value2 is Empty : Invalid")]
        public virtual async Task Value2Empty()
        {
            await LoadPage();

            var testValues = GetValues2PassValidation();
            await AssignTestValues(testValues, false, true, ["Value2"]);
            await ExpectValue2Empty();

            await CallClientValidation();
            await ExpectValidationFailed(
                Value2ValidationError,
                "",
                "", 
                "Model validation failed"
            );

            await AssignTestValues(testValues, true, true, ["Value2"]);
            await ExpectValue2Empty();

            await CallServerValidation();
            await ExpectValidationFailed(
                Value2ValidationError,
                "",
                "", 
                Value2ValidationError
            );
        }

        [CustomTestMethod("ValuePwn is Empty : Valid")]
        public virtual async Task ValuePwnEmpty()
        {
            await LoadPage();

            var testValues = GetValues2PassValidation();
            await AssignTestValues(testValues, false, true, ["ValuePwn"]);
            await ExpectValuePwnEmpty();

            await CallClientValidation();
            await ExpectValidationSucceed();

            await AssignTestValues(testValues, true, true, ["ValuePwn"]);
            await ExpectValuePwnEmpty();

            await CallServerValidation();
            await ExpectValidationSucceed();
        }

        [TestMethod]
        public virtual async Task FormValidationSuccess()
        {
            await LoadPage();

            var testValues = GetValues2PassValidation();
            await AssignTestValues(testValues);

            await CallClientValidation();
            await ExpectValidationSucceed();

            await AssignTestValues(testValues, true);

            await CallServerValidation();
            await ExpectValidationSucceed();
        }

        [TestMethod]
        public virtual async Task FormValidationFailure()
        {
            await LoadPage();

            var testValues = GetValues2FailsValidation();
            await AssignTestValues(testValues);

            await CallClientValidation();
            await ExpectClientValidationFailed();

            await AssignTestValues(testValues, true);

            await CallServerValidation();
            await ExpectServerValidationFailed();
        }

        protected virtual async Task EmptyValues(bool valid)
        {
            await LoadPage();

            var otherInputIds = GetValues2PassValidation().OtherValues.ToArray();
            await ResetForm(otherInputIds);

            await CallClientValidation();
            if (valid)
                await ExpectValidationSucceed();
            else
                await ExpectClientValidationFailed();

            await CallServerValidation();
            if (valid)
                await ExpectValidationSucceed();
            else
                await ExpectServerValidationFailed();
        }

        protected abstract TestValues GetValues2PassValidation();

        protected abstract TestValues GetValues2FailsValidation();

        protected Task ExpectClientValidationFailed()
            => ExpectValidationFailed(
                    Value2ValidationError,
                    ValuePwnValidationError,
                    "",
                    "Model validation failed"
                );

        protected Task ExpectServerValidationFailed()
            => ExpectValidationFailed(
                    Value2ValidationError,
                    ValuePwnValidationError,
                    "",
                    Value2ValidationError, ValuePwnValidationError
                );
    }
}
