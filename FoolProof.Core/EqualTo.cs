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

        public EqualToAttribute(string dependentValue)
            : base(dependentValue, Operator.EqualTo) { }

        public EqualToAttribute(
            string dependentValue,
            string defaultMessage
        ) : base(dependentValue, Operator.EqualTo, defaultMessage) { }
    }

    //Use this class for string values instead of EqualToAttribute<string>, to avoid ambiguity during construction
    public class SameTextAttribute : IsAttribute<string>
    {
        public SameTextAttribute(string compareText)
            : base(compareText, Operator.EqualTo) { }

        public SameTextAttribute(
            string compareText,
            string defaultMessage
        ) : base(compareText, Operator.EqualTo, defaultMessage) { }
    }

    public class IsEmptyAttribute : SameTextAttribute
    {
        public IsEmptyAttribute()
            : this("{0} must be empty.") 
        {
            PassOnNull = false;
        }

        public IsEmptyAttribute(string defaultMessage)
            : base(string.Empty, defaultMessage) 
        {
            PassOnNull = false;
        }
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
