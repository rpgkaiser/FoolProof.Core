
namespace FoolProof.Core
{
    public class RequiredIfRegExMatchAttribute : RequiredIfAttribute
    {
        public RequiredIfRegExMatchAttribute(string dependentProperty, string pattern) 
            : base(dependentProperty, Operator.RegExMatch, pattern as object) 
        {
            DataType = ClientDataType.String;
        }

        public RequiredIfRegExMatchAttribute(
            string dependentProperty,
            string pattern,
            string defaultMessage
        ) : base(dependentProperty, Operator.RegExMatch, pattern as object, defaultMessage)
        {
            DataType = ClientDataType.String;
        }
    }
}
