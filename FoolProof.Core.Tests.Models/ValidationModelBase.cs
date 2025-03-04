namespace FoolProof.Core.Tests.Models
{
    public abstract class ValidationModelBase<T> where T: ModelAwareValidationAttribute
    {
        public T GetAttribute(string property) 
        {
            var custmAttrs = this.GetType().GetProperty(property)!.GetCustomAttributes(false);
            return custmAttrs
                    .Where(ca => typeof(ModelAwareValidationAttribute).IsAssignableFrom(ca.GetType()))
                    .OfType<T>()
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
