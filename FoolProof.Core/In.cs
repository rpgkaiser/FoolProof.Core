
using System;
using System.Collections.Generic;
using System.Linq;

namespace FoolProof.Core
{
    public class InAttribute : IsAttribute
    {
        public InAttribute(string dependentProperty) 
            : base(Operator.In, dependentProperty) { }

        public InAttribute(
            string dependentProperty, 
            string defaultMessage
        ) : base(Operator.In, dependentProperty, defaultMessage) { }
    }

    public class InAttribute<T> : IsAttribute<T[]>
    {
        public InAttribute(params T[] dependentValues)
            : base(Operator.In, dependentValues ?? Array.Empty<T>()) 
        {
            if (dependentValues is null || dependentValues.Length == 0)
                throw new ArgumentException("Dependent value list must not be empty.", nameof(dependentValues));
        }

        public InAttribute(
            string defaultMessage,
            T[] dependentValues
        ) : base(Operator.In, dependentValues ?? Array.Empty<T>(), defaultMessage) 
        {
            if (dependentValues is null || dependentValues.Length == 0)
                throw new ArgumentException("Dependent value list must not be empty.", nameof(dependentValues));
        }

        public InAttribute(params string[] dependentValues)
            : base(Operator.In, Array.Empty<T>())
        {
            if (dependentValues is null || dependentValues.Length == 0)
                throw new ArgumentException("Dependent value list must not be empty.", nameof(dependentValues));

            AssignValues(dependentValues);
        }

        public InAttribute(
            string[] dependentValues,
            string defaultMessage
        ) : base(Operator.In, Array.Empty<T>(), defaultMessage)
        {
            if (dependentValues is null || dependentValues.Length == 0)
                throw new ArgumentException("Dependent value list must not be empty.", nameof(dependentValues));

            AssignValues(dependentValues);
        }

        protected virtual void AssignValues(string[] dependentValues)
        {
            if (dependentValues is null)
                DependentValue = Array.Empty<T>();
            else if (typeof(T) == typeof(string))
                DependentValue = (T[])(object)(dependentValues);
            else
                DependentValue = dependentValues.Select(strVal => ConvertValue<T>(strVal)).ToArray();
        }

        protected override ClientDataType GetDataType(Type modelType)
            => base.GetDataType(typeof(T));
    }

    //Use this class for string values instead of InAttribute<string>, to avoid ambiguity during construction
    public class InTextsAttribute: InAttribute<string>
    {
        public InTextsAttribute(params string[] dependentValues)
            : base(null, dependentValues) { }

        public InTextsAttribute(
            string[] dependentValues,
            string defaultMessage
        ) : base(defaultMessage, dependentValues) { }
    }
}
