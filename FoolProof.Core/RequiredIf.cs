using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FoolProof.Core
{
    public class RequiredIfAttribute : ContingentValidationAttribute
    {
        public Operator Operator { get; private set; }

        public object DependentValue { get; private set; }

        public ClientDataType DataType { get; set; }

        protected OperatorMetadata Metadata { get; private set; }

        public RequiredIfAttribute(
            string dependentProperty, 
            Operator @operator, 
            object dependentValue
        ) : this(dependentProperty, @operator, dependentValue, "{0} is required due to {1} being {3} {2}")
        {}

        public RequiredIfAttribute(
            string dependentProperty,
            Operator @operator,
            object dependentValue,
            string defaultMessage
        ) : base(dependentProperty, defaultMessage ?? "{0} is required due to {1} being {3} {2}")
        {
            Operator = @operator;
            DependentValue = dependentValue;
            Metadata = OperatorMetadata.Get(Operator);
        }

        public RequiredIfAttribute(
            string dependentProperty, 
            object dependentValue
        ) : this(dependentProperty, Operator.EqualTo, dependentValue) 
        {}

        public RequiredIfAttribute(
            string dependentProperty,
            object dependentValue,
            string defaultMessage
        ) : this(dependentProperty, Operator.EqualTo, dependentValue, defaultMessage)
        { }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(ErrorMessageString, name, DependentPropertyDisplayName ?? DependentProperty, DependentValue, Metadata.ErrorMessage);
        }

        public override string ClientTypeName
        {
            get { return "RequiredIf"; }
        }

        protected override IEnumerable<KeyValuePair<string, object>> GetClientValidationParameters(ModelMetadata modelMetadata)
        {
            var dependentProperty = GetModelProperty(modelMetadata.ContainerType, DependentProperty);
            var dataTypeStr = GetDataType(dependentProperty.PropertyType).ToString();
            object depValue = DependentValue is not null ? JsonSerializer.Serialize(DependentValue) : DependentValue;
            var clientParams = new List<KeyValuePair<string, object>>() {
                new KeyValuePair<string, object>("Operator", Operator.ToString()),
                new KeyValuePair<string, object>("DependentValue", depValue),
                new KeyValuePair<string, object>("DataType", dataTypeStr)
            };
            return base.GetClientValidationParameters(modelMetadata).Union(clientParams);
        }

        public override bool IsValid(object value, object dependentValue, object container)
        {
            if (Metadata.IsValid(dependentValue, DependentValue))
                return value != null && !string.IsNullOrEmpty(value.ToString().Trim());

            return true;
        }

        protected virtual ClientDataType GetDataType(Type modelType)
            => DataType == ClientDataType.Auto
                ? IsAttribute.GetClientDataType(modelType)
                : DataType;
    }
}
