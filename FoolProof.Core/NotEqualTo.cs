
namespace FoolProof.Core
{
    public class NotEqualToAttribute : IsAttribute
    {
        public NotEqualToAttribute(string dependentProperty) 
            : base(Operator.NotEqualTo, dependentProperty) { }

        public NotEqualToAttribute(
            string dependentProperty, 
            string defultMessage
        ) : base(Operator.NotEqualTo, dependentProperty, defultMessage) { }
    }

    public class NotEqualToAttribute<T> : IsAttribute<T>
    {
        public NotEqualToAttribute(T dependentValue)
            : base(Operator.NotEqualTo, dependentValue) { }

        public NotEqualToAttribute(
            T dependentValue,
            string defultMessage
        ) : base(Operator.NotEqualTo, dependentValue, defultMessage) { }

        public NotEqualToAttribute(string dependentValue)
            : base(dependentValue, Operator.NotEqualTo) { }

        public NotEqualToAttribute(
            string dependentValue,
            string defultMessage
        ) : base(dependentValue, Operator.NotEqualTo, defultMessage) { }
    }

    //Use this class for string values instead of NotEqualToAttribute<string>, to avoid ambiguity during construction
    public class DifferentTextAttribute : IsAttribute<string>
    {
        public DifferentTextAttribute(string compareText)
            : base(compareText, Operator.NotEqualTo) { }

        public DifferentTextAttribute(
            string compareText,
            string defultMessage
        ) : base(compareText, Operator.NotEqualTo, defultMessage) { }
    }

    public class IsNotEmptyAttribute : DifferentTextAttribute
    {
        public IsNotEmptyAttribute()
            : this("{0} must not be empty.") { }

        public IsNotEmptyAttribute(string defaultMessage)
            : base(string.Empty, defaultMessage) 
        {
            PassOnNull = false;
        }

        public override bool IsValid(object value, object container)
            => !string.IsNullOrEmpty(value as string) && _metadata.IsValid(value, DependentValue);
    }
}
