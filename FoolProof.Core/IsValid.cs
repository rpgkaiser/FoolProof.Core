using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace FoolProof.Core
{
    public class IsValidAttribute<T> : ModelAwareValidationAttribute
        where T : ValidationAttribute
    {
        public string ModelPropertyName { get; protected set; }

        public T Validator { get; set; }

        public IsValidAttribute(
            string modelPropName,
            T validationRule
        ) : this(modelPropName, validationRule, "{0} is invalid") { }

        public IsValidAttribute(
            string modelPropName,
            T validator,
            string defaultMessage
        ) : base(defaultMessage) 
        {
            this.ModelPropertyName = modelPropName ?? throw new ArgumentNullException(nameof(modelPropName));
            this.Validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        public IsValidAttribute(
            string modelPropName,
            params object[] validatorParams
        ) : this(modelPropName, validatorParams, "{0} is invalid") { }

        public IsValidAttribute(
            string modelPropName,
            object[] validatorParams,
            string defaultMessage
        ) : this(modelPropName, Activator.CreateInstance(typeof(T), validatorParams) as T, defaultMessage) { }

        public override string ClientTypeName => "IsValid";

        public override bool IsValid(object value, object container)
        {
            value = GetPropertyValue(ModelPropertyName, container);
            return PredicateAttribute.IsValid(Validator, value, container);
        }

        public override Dictionary<string, object> ClientValidationParameters(ClientModelValidationContext validationContext)
        {
            var clientParams = base.ClientValidationParameters(validationContext.ModelMetadata);
            
            var modelPropMetadata = validationContext.MetadataProvider.GetMetadataForProperty(
                validationContext.ModelMetadata.ContainerType ?? validationContext.ModelMetadata.ModelType, 
                ModelPropertyName
            );

            if (modelPropMetadata is not null)
            {
                var validParams = PredicateAttribute.GetClientParams(Validator, validationContext, modelPropMetadata);
                if (validParams is not null)
                {
                    clientParams.Add("modelpropertyname", ModelPropertyName);
                    clientParams.Add("validationparams", new {
                        method = validParams.Method.ToLowerInvariant(),
                        @params = validParams.Params,
                        message = validParams.Message
                    });
                }
            }

            return clientParams;
        }

        public override string FormatErrorMessage(ClientModelValidationContext validationContext)
        {
            var modelPropMetadata = validationContext.MetadataProvider.GetMetadataForProperty(
                validationContext.ModelMetadata.ContainerType ?? validationContext.ModelMetadata.ModelType,
                ModelPropertyName
            );
            return base.FormatErrorMessage(new ClientModelValidationContext(
                validationContext.ActionContext,
                modelPropMetadata,
                validationContext.MetadataProvider,
                validationContext.Attributes
            ));
        }
    }

    public class IsValidAttribute : IsValidAttribute<ValidationAttribute>
    {
        public IsValidAttribute(
            string modelPropName,
            ValidationAttribute validationRule
        ) : this(modelPropName, validationRule, "{0} is invalid") { }

        public IsValidAttribute(
            string modelPropName,
            ValidationAttribute validationRule,
            string defaultMessage
        ) : base(modelPropName, validationRule, defaultMessage) { }
    }
}
