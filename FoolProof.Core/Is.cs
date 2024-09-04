﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FoolProof.Core
{
    public enum ClientDataType
    {
        String = 0,
        Number,
        Bool,
        Date,
        Time,
        DateTime
    }

    public class IsAttribute : ContingentValidationAttribute
    {
        public Operator Operator { get; private set; }

        public bool PassOnNull { get; set; }

        public ClientDataType? DataType { get; set; }

        private OperatorMetadata _metadata;

        public IsAttribute(Operator @operator, string dependentProperty)
            : base(dependentProperty)
        {
            Operator = @operator;
            PassOnNull = false;
            _metadata = OperatorMetadata.Get(Operator);
        }

        public override string ClientTypeName
        {
            get { return "Is"; }
        }

        protected override IEnumerable<KeyValuePair<string, object>> GetClientValidationParameters(ModelMetadata modelMetadata)
        {
            var dataTypeStr = (DataType ?? IsAttribute.GetDataType(modelMetadata.ModelType)).ToString();
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

        public override string DefaultErrorMessage
        {
            get { return "{0} must be " + _metadata.ErrorMessage + " {1}."; }
        }

        public static ClientDataType GetDataType(Type valueType)
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

        private static readonly HashSet<Type> NumericTypes = new HashSet<Type>
        {
            typeof(int),  typeof(double),  typeof(decimal),
            typeof(long), typeof(short),   typeof(sbyte),
            typeof(byte), typeof(ulong),   typeof(ushort),
            typeof(uint), typeof(float)
        };

        private static bool IsNumeric(Type myType)
        {
            return NumericTypes.Contains(myType);
        }
    }
}
