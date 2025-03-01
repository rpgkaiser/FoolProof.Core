namespace FoolProof.Core
{
    public class NotAttribute : PredicateAttribute
    {
        public NotAttribute(
            ModelAwareValidationAttribute operand
        ) : this(operand, "{0} is invalid") { }

        public NotAttribute(
            ModelAwareValidationAttribute operand,
            string defaultMessage
        ) : base(LogicalOperator.Not, defaultMessage, operand) { }
    }

    public class NotAttribute<OPT> : NotAttribute
        where OPT : ModelAwareValidationAttribute
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
        ) : this(CreateOperand<OPT>(constParams), "{0} is invalid") { }

        public NotAttribute(
            string defaultMessage,
            params object[] constParams
        ) : base(CreateOperand<OPT>(constParams), defaultMessage) { }
    }
}
