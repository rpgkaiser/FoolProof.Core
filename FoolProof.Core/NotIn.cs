
using System;
using System.Linq;

namespace FoolProof.Core
{
    public class NotInAttribute : IsAttribute
    {
        public NotInAttribute(string dependentProperty) 
            : base(Operator.NotIn, dependentProperty) { }

        public NotInAttribute(
            string dependentProperty, 
            string defaultMessage
        ) : base(Operator.NotIn, dependentProperty) { }
    }

    public class NotInAttribute<T> : IsAttribute<T[]>
    {
        public NotInAttribute(params T[] dependentValues)
            : base(Operator.NotIn, dependentValues ?? Array.Empty<T>()) 
        {
            if (dependentValues is null || dependentValues.Length == 0)
                throw new ArgumentException("Dependent value list must not be empty.", nameof(dependentValues));
        }

        public NotInAttribute(
            string defaultMessage,
            T[] dependentValues
        ) : base(Operator.NotIn, dependentValues ?? Array.Empty<T>(), defaultMessage) 
        {
            if (dependentValues is null || dependentValues.Length == 0)
                throw new ArgumentException("Dependent value list must not be empty.", nameof(dependentValues));
        }

        public NotInAttribute(params string[] dependentValues)
            : base(Operator.NotIn, Array.Empty<T>())
        {
            if (dependentValues is null || dependentValues.Length == 0)
                throw new ArgumentException("Dependent value list must not be empty.", nameof(dependentValues));

            DependentValue = dependentValues.Select(strVal => ConvertValue<T>(strVal)).ToArray();
        }

        public NotInAttribute(
            string[] dependentValues,
            string defaultMessage
        ) : base(Operator.NotIn, Array.Empty<T>(), defaultMessage)
        {
            if (dependentValues is null || dependentValues.Length == 0)
                throw new ArgumentException("Dependent value list must not be empty.", nameof(dependentValues));

            DependentValue = dependentValues.Select(strVal => ConvertValue<T>(strVal)).ToArray();
        }

        protected override ClientDataType GetDataType(Type modelType)
            => base.GetDataType(typeof(T));
    }

    //Use this class for string values instead of NotInAttribute<string>, to avoid ambiguity during construction
    public class NotInTextsAttribute : NotInAttribute<string>
    {
        public NotInTextsAttribute(params string[] dependentValues)
            : base(null, dependentValues) { }

        public NotInTextsAttribute(
            string[] dependentValues,
            string defaultMessage
        ) : base(defaultMessage, dependentValues) { }
    }
}
