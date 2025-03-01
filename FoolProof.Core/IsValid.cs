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
            T validationRule
        ) : this(null, validationRule, "{0} is invalid") { }

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
            this.ModelPropertyName = modelPropName;
            this.Validator = validator;
        }

        public IsValidAttribute(
            params object[] validatorParams
        ) : this(null, Activator.CreateInstance(typeof(T), validatorParams) as T, "{0} is invalid") { }

        public IsValidAttribute(
            string modelPropName,
            params object[] validatorParams
        ) : this(modelPropName, Activator.CreateInstance(typeof(T), validatorParams) as T, "{0} is invalid") { }

        public IsValidAttribute(
            string modelPropName,
            object[] validatorParams,
            string defaultMessage
        ) : this(modelPropName, Activator.CreateInstance(typeof(T), validatorParams) as T, defaultMessage) 
        {
            this.ModelPropertyName = modelPropName;
        }

        public override string ClientTypeName => "IsValid";

        public override bool IsValid(object value, object container)
        {
            if(!string.IsNullOrWhiteSpace(ModelPropertyName))
                value = GetPropertyValue(ModelPropertyName, container);

            return Validator is ModelAwareValidationAttribute modelValidator
                    ? modelValidator.IsValid(value, container)
                    : Validator.IsValid(value);
        }

        public override Dictionary<string, object> ClientValidationParameters(ClientModelValidationContext validationContext)
        {
            var clientParams = base.ClientValidationParameters(validationContext.ModelMetadata);
            
            if(!string.IsNullOrWhiteSpace(ModelPropertyName))
                clientParams.Add("modelpropertyname", ModelPropertyName);

            string validMethod = "";
            Dictionary<string, object> validParams = new Dictionary<string, object>();
            if (Validator is ModelAwareValidationAttribute modelValidator)
            {
                validMethod = modelValidator.ClientTypeName.ToLowerInvariant(); 
                validParams = modelValidator.ClientValidationParameters(validationContext.ModelMetadata);
            }
            else
            {
                var modelPropMetadata = validationContext.MetadataProvider.GetMetadataForProperty(validationContext.ModelMetadata.ModelType, ModelPropertyName);
                var validContext = new ClientModelValidationContext(
                    validationContext.ActionContext,
                    modelPropMetadata,
                    validationContext.MetadataProvider,
                    new Dictionary<string, string>()
                );
                var adapterProvider = validationContext.ActionContext.HttpContext.RequestServices.GetService<IValidationAttributeAdapterProvider>();
                var stringLocalizer = validationContext.ActionContext.HttpContext.RequestServices.GetService<IStringLocalizer>();
                var attrAdapter = adapterProvider.GetAttributeAdapter(Validator, stringLocalizer);
                attrAdapter.AddValidation(validContext);

                validMethod = validContext.Attributes.Where(at => at.Key.StartsWith("data-val-"))
                              .OrderBy(at => at.Key.Length)
                              .FirstOrDefault()
                              .Key["data-val-".Length..]
                              .ToLowerInvariant();
                var paramsPrefix = $"data-val-{validMethod}-";
                validParams = validContext.Attributes.Where(at => at.Key.StartsWith(paramsPrefix))
                              .ToDictionary(
                                  x => x.Key[paramsPrefix.Length..], 
                                  x => x.Value as object
                              );
            }

            clientParams.Add("validationparams", new {
                method = validMethod.ToLowerInvariant(),
                @params = validParams
            });

            return clientParams;
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
