
namespace FoolProof.Core.Tests.E2eTests
{
    public abstract class CompareBaseTest : BasePageTest
    {
        protected virtual string Value1ValidMsgId => "value1-valid-msg";

        protected virtual string Value2ValidMsgId => "value2-valid-msg";

        protected virtual string ValuePwnValidMsgId => "valuepwn-valid-msg";

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

        protected virtual Task AssignValue1(object? value, bool verifyValue = true)
            => AssignValue("#Value1", value, verifyValue);

        protected virtual Task ExpectValue1Empty(bool? isSelect = null)
            => ExpectEmpties(new InputTestValue("Value1", isSelect: isSelect));

        protected virtual Task AssignValue2(object? value, bool verifyValue = true)
            => AssignValue("#Value2", value, verifyValue);

        protected virtual Task ExpectValue2Empty(bool? isSelect = null)
            => ExpectEmpties(new InputTestValue("Value2", isSelect: isSelect));

        protected virtual Task AssignValuePwn(object? value, bool verifyValue = true)
            => AssignValue("#ValuePwn", value, verifyValue);

        protected virtual Task ExpectValuePwnEmpty(bool? isSelect = null)
            => ExpectEmpties(new InputTestValue("ValuePwn", isSelect: isSelect));

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

        protected abstract CompareTestValues GetValues2PassValidation();

        protected abstract CompareTestValues GetValues2FailsValidation();

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

        protected override async Task ExpectValidationSucceed(
            string alertValidationMsg = "Model validation succeed"
        )
        {
            var value1ValidationMessage = Page.GetByTestId(Value1ValidMsgId);
            await Expect(value1ValidationMessage).ToBeEmptyAsync();

            var value2ValidationMessage = Page.GetByTestId(Value2ValidMsgId);
            await Expect(value2ValidationMessage).ToBeEmptyAsync();

            var valuePwnValidationMessage = Page.GetByTestId(ValuePwnValidMsgId);
            await Expect(valuePwnValidationMessage).ToBeEmptyAsync();

            await base.ExpectValidationSucceed(alertValidationMsg);
        }

        protected virtual async Task ExpectValidationFailed(
            string? value2ErrorMsg = null,
            string? valuePwnErrorMsg = null,
            string? value1ErrorMsg = null,
            params string[] alertValidationMsgs
        )
        {
            var value1ValidationMessage = Page.GetByTestId(Value1ValidMsgId);
            if (value1ErrorMsg == string.Empty)
                await Expect(value1ValidationMessage).ToBeEmptyAsync();
            else if (value1ErrorMsg is null)
                await Expect(value1ValidationMessage).Not.ToBeEmptyAsync();
            else
                await Expect(value1ValidationMessage).ToContainTextAsync(value1ErrorMsg);

            var value2ValidationMessage = Page.GetByTestId(Value2ValidMsgId);
            if (value2ErrorMsg == string.Empty)
                await Expect(value2ValidationMessage).ToBeEmptyAsync();
            else if (value2ErrorMsg is null)
                await Expect(value2ValidationMessage).Not.ToBeEmptyAsync();
            else
                await Expect(value2ValidationMessage).ToContainTextAsync(value2ErrorMsg);

            var valuePwnValidationMessage = Page.GetByTestId(ValuePwnValidMsgId);
            if (valuePwnErrorMsg == string.Empty)
                await Expect(valuePwnValidationMessage).ToBeEmptyAsync();
            else if (valuePwnErrorMsg is null)
                await Expect(valuePwnValidationMessage).Not.ToBeEmptyAsync();
            else
                await Expect(valuePwnValidationMessage).ToContainTextAsync(valuePwnErrorMsg);

            await ExpectValidationFailed(alertValidationMsgs);
        }

        protected override async Task ResetForm(params InputTestValue[] inputs)
        {
            await base.ResetForm(inputs);

            await ExpectValue1Empty();
            await ExpectValue2Empty();
            await ExpectValuePwnEmpty();
        }

        protected class CompareTestValues: TestValues
        {
            protected readonly InputTestValue _value1;
            protected readonly InputTestValue _value2;
            protected readonly InputTestValue _valuePwn;
            protected readonly List<InputTestValue> _otherValues;

            public CompareTestValues(
                object? value1 = default,
                object? value2 = default,
                object? valuePwn = default,
                params InputTestValue[] otherValues
            )
            {
                _value1 = value1 is InputTestValue inpVal1 ? inpVal1 : new InputTestValue("Value1", value1);
                _value2 = value2 is InputTestValue inpVal2 ? inpVal2 : new InputTestValue("Value2", value2);
                _valuePwn = valuePwn is InputTestValue inpValPwn ? inpValPwn : new InputTestValue("ValuePwn", valuePwn);

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

            public List<InputTestValue> OtherValues => _otherValues;

            public override IEnumerable<InputTestValue> AllValues()
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
