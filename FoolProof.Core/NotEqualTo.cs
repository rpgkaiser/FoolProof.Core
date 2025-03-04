
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
    }

    public class IsNotEmptyAttribute : NotEqualToAttribute<string>
    {
        public IsNotEmptyAttribute()
            : this("{0} must not be empty.") { }

        public IsNotEmptyAttribute(string defaultMessage)
            : base(string.Empty, defaultMessage) { }
    }
}
