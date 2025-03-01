
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace FoolProof.Core
{
    public enum LogicalOperator
    {
        And,
        Or,
        Not
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public abstract class PredicateAttribute : ModelAwareValidationAttribute
    {
        protected PredicateAttribute(
            LogicalOperator oper,
            params ModelAwareValidationAttribute[] operands
        ) : this(oper, "{0} is invalid", operands) { }

        protected PredicateAttribute(
            LogicalOperator oper,
            string defaultMessage,
            params ModelAwareValidationAttribute[] operands
        ) : base(defaultMessage ?? "{0} is invalid")
        {
            Operator = oper;
            Operands = operands ?? throw new ArgumentNullException(nameof(operands));
            if (!Operands.Any())
                throw new ArgumentException("Empty operands parameter.", nameof(operands));
        }

        public LogicalOperator Operator { get; protected set; }

        public IEnumerable<ModelAwareValidationAttribute> Operands { get; protected set; }

        public override string ClientTypeName => "predicate";

        public override bool IsValid(object value, object container)
        {
            return Operator switch
            {
                LogicalOperator.And => Operands.FirstOrDefault(oprnd => !oprnd.IsValid(value, container)) is null,
                LogicalOperator.Or => Operands.FirstOrDefault(oprnd => oprnd.IsValid(value, container)) is not null,
                LogicalOperator.Not => !Operands.First().IsValid(value, container),
                _ => throw new NotSupportedException()
            };
        }

        public override Dictionary<string, object> ClientValidationParameters(ClientModelValidationContext validationContext)
        {
            var clientParams = base.ClientValidationParameters(validationContext);
            clientParams.Add("logicaloperator", Operator.ToString());
            clientParams.Add("operands", Operands.Select(op => new {
                method = op.ClientTypeName.ToLowerInvariant(),
                @params = op.ClientValidationParameters(validationContext)
            }).ToArray());
            return clientParams;
        }

        protected static T CreateOperand<T>(object[] constParams) where T: ModelAwareValidationAttribute
            => Activator.CreateInstance(typeof(T), constParams) as T;
    }
}
