
namespace FoolProof.Core
{
    public class RequiredIfTrueAttribute : RequiredIfAttribute
    {
        public RequiredIfTrueAttribute(string dependentProperty) 
            : base(dependentProperty, Operator.EqualTo, true) 
        {
            DataType = ClientDataType.Bool;
        }

        public RequiredIfTrueAttribute(
            string dependentProperty,
            string defaultMessage
        ) : base(dependentProperty, Operator.EqualTo, true, defaultMessage)
        {
            DataType = ClientDataType.Bool;
        }

        public RequiredIfTrueAttribute(
            string dependentProperty, 
            string defaultMessage, 
            string targetPropName
        ) : base(dependentProperty, Operator.EqualTo, true, defaultMessage, targetPropName)
        {
            DataType = ClientDataType.Bool;
        }
    }
}
