using System.Linq;
using System;
using System.ComponentModel.DataAnnotations;

namespace FoolProof.Core
{
    public class OrAttribute : PredicateAttribute
    {
        public OrAttribute(
            params ValidationAttribute[] operands
        ) : this("{0} is invalid", operands) { }

        public OrAttribute(
            string defaultMessage,
            params ValidationAttribute[] operands
        ) : base(LogicalOperator.Or, defaultMessage, operands) 
        {
            if (operands.Count() < 2)
                throw new ArgumentException("Must be at least two operands.", nameof(operands));
        }
    }

    public class OrAttribute<OP1, OP2> : OrAttribute
        where OP1 : ValidationAttribute
        where OP2 : ValidationAttribute
    {
        protected OrAttribute(
            OP1 leftPart,
            OP2 rightPart
        ) : this(leftPart, rightPart, "{0} is invalid") { }

        protected OrAttribute(
            OP1 leftPart,
            OP2 rightPart,
            string defaultMessage
        ) : base(defaultMessage, leftPart, rightPart) { }

        public OrAttribute(
            object[] leftPartParams,
            object[] rightPartParams
        ) : this(leftPartParams, rightPartParams, "{0} is invalid") { }

        public OrAttribute(
            object[] leftPartParams,
            object[] rightPartParams,
            string defaultMessage
        ) : this(
                CreateOperand<OP1>(leftPartParams), 
                CreateOperand<OP2>(rightPartParams),
                defaultMessage
            ) { }
    }

    public class OrAttribute<OP1, OP2, OP3> : OrAttribute
        where OP1 : ValidationAttribute
        where OP2 : ValidationAttribute
        where OP3 : ValidationAttribute
    {
        protected OrAttribute(
            OP1 operand1,
            OP2 operand2,
            OP3 operand3
        ) : this(operand1, operand2, operand3, "{0} is invalid") { }

        protected OrAttribute(
            OP1 operand1,
            OP2 operand2,
            OP3 operand3,
            string defaultMessage
        ) : base(defaultMessage, operand1, operand2, operand3) { }

        public OrAttribute(
            object[] operand1Params,
            object[] operand2Params,
            object[] operand3Params
        ) : this(operand1Params, operand2Params, operand3Params, "{0} is invalid") { }

        public OrAttribute(
            object[] operand1Params,
            object[] operand2Params,
            object[] operand3Params,
            string defaultMessage
        ) : this(
                CreateOperand<OP1>(operand1Params), 
                CreateOperand<OP2>(operand2Params), 
                CreateOperand<OP3>(operand3Params), 
                defaultMessage
            ) { }
    }

    public class OrAttribute<OP1, OP2, OP3, OP4> : OrAttribute
        where OP1 : ValidationAttribute
        where OP2 : ValidationAttribute
        where OP3 : ValidationAttribute
        where OP4 : ValidationAttribute
    {
        protected OrAttribute(
            OP1 operand1,
            OP2 operand2,
            OP3 operand3,
            OP4 operand4
        ) : this(operand1, operand2, operand3, operand4, "{0} is invalid") { }

        protected OrAttribute(
            OP1 operand1,
            OP2 operand2,
            OP3 operand3,
            OP4 operand4,
            string defaultMessage
        ) : base(defaultMessage, operand1, operand2, operand3, operand4) { }

        public OrAttribute(
            object[] operand1Params,
            object[] operand2Params,
            object[] operand3Params,
            object[] operand4Params
        ) : this(operand1Params, operand2Params, operand3Params, operand4Params, "{0} is invalid") { }

        public OrAttribute(
            object[] operand1Params,
            object[] operand2Params,
            object[] operand3Params,
            object[] operand4Params,
            string defaultMessage
        ) : this(
                CreateOperand<OP1>(operand1Params),
                CreateOperand<OP2>(operand2Params),
                CreateOperand<OP3>(operand3Params),
                CreateOperand<OP4>(operand4Params),
                defaultMessage
            )
        { }
    }
}
