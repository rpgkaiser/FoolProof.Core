using System.Collections.Generic;
using System.Linq;

namespace FoolProof.Core
{
    public class IsAttribute : ContingentValidationAttribute
    {
        public Operator Operator { get; private set; }
        public bool PassOnNull { get; set; }
        private OperatorMetadata _metadata;

        public IsAttribute(Operator @operator, string dependentProperty)
            : base(dependentProperty)
        {
            Operator = @operator;
            PassOnNull = false;
            _metadata = OperatorMetadata.Get(Operator);
        }

        public override string ClientTypeName
        {
            get { return "Is"; }
        }

        protected override IEnumerable<KeyValuePair<string, object>> GetClientValidationParameters()
        {
            return base.GetClientValidationParameters()
                .Union(new []
                       {
                           new KeyValuePair<string, object>("Operator", Operator.ToString()),
                           new KeyValuePair<string, object>("PassOnNull", PassOnNull)
                       });
        }

        public override bool IsValid(object value, object dependentValue, object container)
        {
            if (PassOnNull && (value == null || dependentValue == null) && (value != null || dependentValue != null))
                return true;

            return _metadata.IsValid(value, dependentValue);
        }

        public override string DefaultErrorMessage
        {
            get { return "{0} must be " + _metadata.ErrorMessage + " {1}."; }
        }
    }
}
