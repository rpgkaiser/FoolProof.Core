
namespace FoolProof.Core
{
    public class LessThanOrEqualToAttribute : IsAttribute
    {
        public LessThanOrEqualToAttribute(string dependentProperty) 
            : base(Operator.LessThanOrEqualTo, dependentProperty) { }

        public LessThanOrEqualToAttribute(
            string dependentProperty, 
            string defaultMessage
        ) : base(Operator.LessThanOrEqualTo, dependentProperty, defaultMessage) { }
    }

    public class LessThanOrEqualToAttribute<T> : IsAttribute<T>
    {
        public LessThanOrEqualToAttribute(T dependentValue)
            : base(Operator.LessThanOrEqualTo, dependentValue) { }

        public LessThanOrEqualToAttribute(
            T dependentValue,
            string defaultMessage
        ) : base(Operator.LessThanOrEqualTo, dependentValue, defaultMessage) { }
    }
}
