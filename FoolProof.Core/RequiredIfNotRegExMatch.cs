
namespace FoolProof.Core
{
    public class RequiredIfNotRegExMatchAttribute : RequiredIfAttribute
    {
        public RequiredIfNotRegExMatchAttribute(string dependentValue, string pattern) 
            : base(dependentValue, Operator.NotRegExMatch, pattern as object) 
        {
            DataType = ClientDataType.String;
        }

        public RequiredIfNotRegExMatchAttribute(
            string dependentValue,
            string pattern,
            string defaultMessage
        ) : base(dependentValue, Operator.NotRegExMatch, pattern as object, defaultMessage)
        {
            DataType = ClientDataType.String;
        }

        public RequiredIfNotRegExMatchAttribute(
            string dependentValue, 
            string pattern,
            string defaultMessage,
            string targetPropName
        ) : base(dependentValue, Operator.NotRegExMatch, pattern, defaultMessage, targetPropName)
        {
            DataType = ClientDataType.String;
        }
    }
}
