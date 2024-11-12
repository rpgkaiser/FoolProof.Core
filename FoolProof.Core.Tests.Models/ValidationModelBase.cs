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

        public bool IsValid(string property) 
        {
            var attribute = this.GetAttribute(property);
            return attribute.IsValid(this.GetType().GetProperty(property)!.GetValue(this, null), this);
        }
    }
}
