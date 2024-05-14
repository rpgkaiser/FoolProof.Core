namespace FoolProof.Core.Tests.Models
{
    public abstract class ValidationModelBase<T> where T: ContingentValidationAttribute
    {
        public T GetAttribute(string property) 
        {
            return (T)this.GetType().GetProperty(property)!.GetCustomAttributes(typeof(T), false)[0];
        }

        public bool IsValid(string property) 
        {
            var attribute = this.GetAttribute(property);
            return attribute.IsValid(this.GetType().GetProperty(property)!.GetValue(this, null), this);
        }
    }
}
