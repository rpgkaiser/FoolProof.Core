
namespace FoolProof.Core
{
    public class RequiredIfFalseAttribute : RequiredIfAttribute
    {
        public RequiredIfFalseAttribute(string dependentProperty) 
            : base(dependentProperty, Operator.EqualTo, false) 
        {
            DataType = ClientDataType.Bool;
        }

        public RequiredIfFalseAttribute(
            string dependentProperty,
            string defaultMessage
        ) : base(dependentProperty, Operator.EqualTo, false, defaultMessage)
        {
            DataType = ClientDataType.Bool;
        }
    }
}
