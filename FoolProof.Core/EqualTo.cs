namespace FoolProof.Core
{
    public class EqualToAttribute : IsAttribute
    {
        public EqualToAttribute(string dependentProperty) 
            : base(Operator.EqualTo, dependentProperty) { }

        public EqualToAttribute(
            string dependentProperty, 
            string defaultMessage
        ) : base(Operator.EqualTo, dependentProperty, defaultMessage) { }
    }

    public class EqualToAttribute<T> : IsAttribute<T>
    {
        public EqualToAttribute(T dependentValue)
            : base(Operator.EqualTo, dependentValue) { }

        public EqualToAttribute(
            T dependentValue,
            string defaultMessage
        ) : base(Operator.EqualTo, dependentValue, defaultMessage) { }
    }

    public class IsEmptyAttribute : EqualToAttribute<string>
    {
        public IsEmptyAttribute()
            : this("{0} must be empty.") { }

        public IsEmptyAttribute(string defaultMessage)
            : base(string.Empty, defaultMessage) { }
    }

    public class IsTrueAttribute : EqualToAttribute<bool>
    {
        public IsTrueAttribute()
            : this("{0} must be true.") { }

        public IsTrueAttribute(string defaultMessage)
            : base(true, defaultMessage) { }
    }

    public class IsFalseAttribute : EqualToAttribute<bool>
    {
        public IsFalseAttribute()
            : this("{0} must be false.") { }

        public IsFalseAttribute(string defaultMessage)
            : base(false, defaultMessage) { }
    }
}
