using System.ComponentModel.DataAnnotations;

namespace FoolProof.Core
{
    public class NotAttribute : PredicateAttribute
    {
        public NotAttribute(
            ValidationAttribute operand
        ) : this(operand, "{0} is invalid") { }

        public NotAttribute(
            ValidationAttribute operand,
            string defaultMessage
        ) : base(LogicalOperator.Not, defaultMessage, operand) { }
    }

    public class NotAttribute<OPT> : NotAttribute
        where OPT : ValidationAttribute
    {
        protected NotAttribute(
            OPT operand
        ) : this(operand, "{0} is invalid") { }

        protected NotAttribute(
            OPT operand,
            string defaultMessage
        ) : base(operand, defaultMessage) { }

        public NotAttribute(
            params object[] constParams
        ) : this("{0} is invalid", constParams) { }

        public NotAttribute(
            string defaultMessage,
            params object[] constParams
        ) : this(CreateOperand<OPT>(constParams), defaultMessage) { }
    }
}
