using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FoolProof.Core
{
    public enum ClientDataType
    {
        Auto = 0,
        String,
        Number,
        Bool,
        Date,
        Time,
        DateTime
    }

    public class IsAttribute : ContingentValidationAttribute
    {
        private readonly OperatorMetadata _metadata;

        public Operator Operator { get; private set; }

        public bool PassOnNull { get; set; }

        public ClientDataType DataType { get; set; }

        public IsAttribute(
            Operator @operator, 
            string dependentProperty
        ) : this (@operator, dependentProperty, "{0} must be {2} {1}.") { }

        public IsAttribute(
            Operator @operator, 
            string dependentProperty, 
            string defaultMessage
        ) : base(dependentProperty, defaultMessage ?? "{0} must be {2} {1}.")
        {
            Operator = @operator;
            _metadata = OperatorMetadata.Get(Operator);
        }

        public override string ClientTypeName
        {
            get { return "Is"; }
        }

        protected override IEnumerable<KeyValuePair<string, object>> GetClientValidationParameters(ModelMetadata modelMetadata)
        {
            var dataTypeStr = GetDataType(modelMetadata.ModelType).ToString();
            var clientParams = new List<KeyValuePair<string, object>>() {
                new KeyValuePair<string, object>("Operator", Operator.ToString()),
                new KeyValuePair<string, object>("PassOnNull", PassOnNull),
                new KeyValuePair<string, object>("DataType", dataTypeStr)
            };
            return base.GetClientValidationParameters(modelMetadata).Union(clientParams);
        }

        public override bool IsValid(object value, object dependentValue, object container)
        {
            if (PassOnNull && (value == null || dependentValue == null) && (value != null || dependentValue != null))
                return true;

            return _metadata.IsValid(value, dependentValue);
        }

		public override string FormatErrorMessage(string name)
		{
			return string.Format(ErrorMessageString, name, DependentPropertyDisplayName ?? DependentProperty, _metadata.ErrorMessage);
		}

        public static ClientDataType GetClientDataType(Type valueType)
        {
            valueType = Nullable.GetUnderlyingType(valueType) ?? valueType;

            if (IsNumeric(valueType))
                return ClientDataType.Number;

            return valueType.Name switch {
                nameof(DateTime) => ClientDataType.DateTime,
                nameof(DateTimeOffset) => ClientDataType.DateTime,
                "DateOnly" => ClientDataType.Date,
                nameof(TimeSpan) => ClientDataType.Time,
                "TimeOnly" => ClientDataType.Time,
                nameof(Boolean) => ClientDataType.Bool,
                _ => ClientDataType.String
            };
        }

        protected virtual ClientDataType GetDataType(Type modelType)
            => DataType == ClientDataType.Auto
                ? GetClientDataType(modelType)
                : DataType;
    }

    public class IsAttribute<T>: ModelAwareValidationAttribute
    {
        protected readonly OperatorMetadata _metadata;

        public IsAttribute(
            Operator @operator,
            T dependentValue
        ) : this(@operator, dependentValue, "{0} must be {2} {1}.") { }

        public IsAttribute(
            Operator @operator,
            T dependentValue,
            string defaultMessage
        ) : base(defaultMessage ?? "{0} must be {2} {1}.")
        {
            Operator = @operator;
            DependentValue = dependentValue;
            _metadata = OperatorMetadata.Get(Operator);
        }

        public IsAttribute(
            string dependentValue,
            Operator @operator
        ) : this(dependentValue, @operator, "{0} must be {2} {1}.") {}

        public IsAttribute(
            string dependentValue,
            Operator @operator,
            string defaultMessage
        ) : base(defaultMessage ?? "{0} must be {2} {1}.")
        {
            Operator = @operator;
            DependentValue = ConvertValue<T>(dependentValue);
            _metadata = OperatorMetadata.Get(Operator);
        }

        public Operator Operator { get; private set; }

        protected bool PassOnNull { get; set; } = true;

        public ClientDataType DataType { get; set; }

        public T DependentValue { get; set; }

        public virtual string DependentValueText
        {
            get
            {
                if (DependentValue is null)
                    return string.Empty;

                if (IsNumeric(DependentValue.GetType()) || DependentValue is bool)
                    return DependentValue.ToString();

                return DependentValue switch {
                    string str => $"'{str}'",
                    IEnumerable list => $"[{string.Join(", ", list.Cast<object>())}]",
                    _ => JsonSerializer.Serialize(DependentValue)
                };
            }
        }

        public override string ClientTypeName
        {
            get { return "Is"; }
        }

        public override bool IsValid(object value, object container)
        {
            return value == null || _metadata.IsValid(value, DependentValue);
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(ErrorMessageString, name, $"{DependentValueText}", _metadata.ErrorMessage);
        }

        protected override IEnumerable<KeyValuePair<string, object>> GetClientValidationParameters(ModelMetadata modelMetadata)
        {
            var dataTypeStr = GetDataType(typeof(T)).ToString();
            object depValue = DependentValue is not null ? JsonSerializer.Serialize(DependentValue) : DependentValue;
            var clientParams = new List<KeyValuePair<string, object>>() {
                new KeyValuePair<string, object>("DependentValue", depValue),
                new KeyValuePair<string, object>("Operator", Operator.ToString()),
                new KeyValuePair<string, object>("PassOnNull", PassOnNull),
                new KeyValuePair<string, object>("DataType", dataTypeStr)
            };
            return base.GetClientValidationParameters(modelMetadata).Union(clientParams);
        }

        protected virtual VT ConvertValue<VT>(string strValue)
            => (VT)TypeDescriptor.GetConverter(typeof(VT)).ConvertFromString(strValue);

        protected virtual ClientDataType GetDataType(Type modelType)
            => DataType == ClientDataType.Auto
                ? IsAttribute.GetClientDataType(modelType)
                : DataType;
    }
}
