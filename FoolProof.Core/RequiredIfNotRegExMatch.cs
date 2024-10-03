
namespace FoolProof.Core
{
    public class RequiredIfNotRegExMatchAttribute : RequiredIfAttribute
    {
        public RequiredIfNotRegExMatchAttribute(string dependentValue, string pattern) 
            : base(dependentValue, Operator.NotRegExMatch, pattern) 
        {
            DataType = ClientDataType.String;
        }
    }
}
