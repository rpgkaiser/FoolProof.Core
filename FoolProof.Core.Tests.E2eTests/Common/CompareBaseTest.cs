
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
                value2ErrorMsg: Value2ValidationError,
                alertValidationMsgs: "Model validation failed"
            );

            await AssignTestValues(testValues, true, true, ["Value1"]);
            await ExpectValue1Empty();

            await CallServerValidation();
            await ExpectValidationFailed(
                value2ErrorMsg: Value2ValidationError,
                alertValidationMsgs: [Value2ValidationError]
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
                value2ErrorMsg: Value2ValidationError,
                alertValidationMsgs: "Model validation failed"
            );

            await AssignTestValues(testValues, true, true, ["Value2"]);
            await ExpectValue2Empty();

            await CallServerValidation();
            await ExpectValidationFailed(
                value2ErrorMsg: Value2ValidationError,
                alertValidationMsgs: [Value2ValidationError]
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

        protected virtual async Task AssignTestValues(
            TestValues tesValues, 
            bool resetFirt = false,
            bool verifyValues = true,
            params string[] ignoreInputIds
        )
        {
            if(resetFirt)
                await ResetForm([.. tesValues.AllValues()]);

            var fieldsToAssign = tesValues.AllValues();
            if(ignoreInputIds is not null && ignoreInputIds.Length > 0)
                fieldsToAssign = fieldsToAssign.Where(iv => !ignoreInputIds.Contains(iv.InputId));

            await AssignFieldValues([.. fieldsToAssign], verifyValues);
        }

        protected class TestValues
        {
            private readonly InputValue _value1;
            private readonly InputValue _value2;
            private readonly InputValue _valuePwn;
            private readonly List<InputValue> _otherValues;
            
            public TestValues(
                object? value1 = default,
                object? value2 = default,
                object? valuePwn = default, 
                params InputValue[] otherValues
            ) 
            {
                _value1 = new ("Value1", value1);
                _value2 = new("Value2", value2);
                _valuePwn = new("ValuePwn", valuePwn);

                if (otherValues is not null && otherValues.Length > 0)
                    _otherValues = [.. otherValues];
                else
                    _otherValues = [];
            }

            public object? Value1 
            { 
                get => _value1.Value; 
                set => _value1.Value = value; 
            }

            public object? Value2 
            { 
                get => _value2.Value; 
                set => _value2.Value = value; 
            }

            public object? ValuePwn 
            { 
                get => _valuePwn.Value; 
                set => _valuePwn.Value = value; 
            }

            public List<InputValue> OtherValues => _otherValues;

            public IEnumerable<InputValue> AllValues()
            {
                yield return _value1;
                yield return _value2;
                yield return _valuePwn;

                foreach (var val in _otherValues)
                    yield return val;
            }
        }
    }
}
