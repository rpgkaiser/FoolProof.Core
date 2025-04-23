using System.ComponentModel.DataAnnotations;

namespace FoolProof.Core.Tests.Models
{
    public abstract class ValidationModelBase
    {
        public const string Value1Description = "Value1: Dependent Value";
        public const string Value2Description = "Value2: Value To Compare";
        public const string ValuePwnDescription = "ValuePwn: Value To Compare If Not NULL";

        public ModelAwareValidationAttribute GetAttribute(string property)
        {
            var custmAttrs = this.GetType().GetProperty(property)!.GetCustomAttributes(false);
            return custmAttrs
                    .Where(ca => typeof(ModelAwareValidationAttribute).IsAssignableFrom(ca.GetType()))
                    .OfType<ModelAwareValidationAttribute>()
                    .First();
        }

        public PredicateAttribute GetAttribute()
        {
            var custmAttrs = this.GetType().GetCustomAttributes(false);
            return custmAttrs
                    .Where(ca => typeof(PredicateAttribute).IsAssignableFrom(ca.GetType()))
                    .OfType<PredicateAttribute>()
                    .First();
        }

        public bool IsValid(string property)
        {
            var attribute = this.GetAttribute(property);
            return attribute.IsValid(this.GetType().GetProperty(property)!.GetValue(this, null), this);
        }

        public bool IsModelValid()
        {
            var attribute = this.GetAttribute();
            return attribute.IsValid(null, this);
        }
    }
}
