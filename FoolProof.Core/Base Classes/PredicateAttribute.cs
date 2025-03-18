﻿
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace FoolProof.Core
{
    public enum LogicalOperator
    {
        And,
        Or,
        Not
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public abstract class PredicateAttribute : ModelAwareValidationAttribute
    {
        protected PredicateAttribute(
            LogicalOperator oper,
            params ValidationAttribute[] operands
        ) : this(oper, "{0} is invalid", operands) { }

        protected PredicateAttribute(
            LogicalOperator oper,
            string defaultMessage,
            params ValidationAttribute[] operands
        ) : base(defaultMessage ?? "{0} is invalid")
        {
            Operator = oper;
            Operands = operands ?? throw new ArgumentNullException(nameof(operands));
            if (!Operands.Any())
                throw new ArgumentException("Empty operands parameter.", nameof(operands));
        }

        public LogicalOperator Operator { get; protected set; }

        public IEnumerable<ValidationAttribute> Operands { get; protected set; }

        public override string ClientTypeName => "predicate";

        public override bool IsValid(object value, object container)
        {
            return Operator switch
            {
                LogicalOperator.And => Operands.FirstOrDefault(oprnd => !IsValid(oprnd, value, container)) is null,
                LogicalOperator.Or => Operands.FirstOrDefault(oprnd => IsValid(oprnd, value, container)) is not null,
                LogicalOperator.Not => !IsValid(Operands.First(), value, container),
                _ => throw new NotSupportedException()
            };
        }

        public override Dictionary<string, object> ClientValidationParameters(ClientModelValidationContext validationContext)
        {
            var clientParams = base.ClientValidationParameters(validationContext);
            clientParams.Add("logicaloperator", Operator.ToString());

            var opClientParams = Operands.Select(op => GetClientParams(op, validationContext))
                                 .Where(op => op is not null);
            clientParams.Add("operands", opClientParams.ToArray());
            return clientParams;
        }

        public static OperandParams GetClientParams(
            ValidationAttribute operand,
            ClientModelValidationContext validationContext,
            ModelMetadata modelMetadata = null
        )
        {
            modelMetadata ??= validationContext.ModelMetadata;

            OperandParams result;
            if (operand is ModelAwareValidationAttribute modelValid)
                result = new () {
                    Method = modelValid.ClientTypeName.ToLowerInvariant(),
                    Params = modelValid.ClientValidationParameters(validationContext),
                    Message = modelValid.FormatErrorMessage(validationContext)
                };
            else
            {
                var validContext = new ClientModelValidationContext(
                    validationContext.ActionContext,
                    modelMetadata,
                    validationContext.MetadataProvider,
                    new Dictionary<string, string>()
                );
                var adapterProvider = validationContext.ActionContext.HttpContext.RequestServices.GetService<IValidationAttributeAdapterProvider>();
                var stringLocalizer = validationContext.ActionContext.HttpContext.RequestServices.GetService<IStringLocalizer>();
                
                var attrAdapter = adapterProvider.GetAttributeAdapter(operand, stringLocalizer);
                if (attrAdapter is null)
                    return null;

                attrAdapter.AddValidation(validContext);

                var validMethod = validContext.Attributes.Where(at => at.Key.StartsWith("data-val-"))
                                  .OrderBy(at => at.Key.Length)
                                  .FirstOrDefault()
                                  .Key["data-val-".Length..]
                                  .ToLowerInvariant();
                var paramsPrefix = $"data-val-{validMethod}-";
                var validParams = validContext.Attributes.Where(at => at.Key.StartsWith(paramsPrefix))
                                  .ToDictionary(
                                      x => x.Key[paramsPrefix.Length..],
                                      x => x.Value as object
                                  );
                result = new() { 
                    Method = validMethod,
                    Params = validParams,
                    Message = validContext.Attributes.TryGetValue($"data-val-{validMethod}", out var msg)
                                ? msg : null
                };
            }

            return result;
        }

        public static bool IsValid(ValidationAttribute operand, object value, object container)
        {
            return operand is ModelAwareValidationAttribute modelValidator
                    ? modelValidator.IsValid(value, container)
                    : operand.IsValid(value);
        }

        public static T CreateOperand<T>(params object[] constParams) where T: ValidationAttribute
            => Activator.CreateInstance(typeof(T), constParams ?? Array.Empty<object>()) as T;
    }

    public class OperandParams
    {
        [JsonPropertyName("method")]
        public string Method { get; set; }

        [JsonPropertyName("params")]
        public Dictionary<string, object> Params { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}
