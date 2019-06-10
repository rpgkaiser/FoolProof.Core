
namespace FoolProof.Core
{
    public class InAttribute : IsAttribute
    {
        public InAttribute(string dependentProperty) : base(Operator.In, dependentProperty) { }
    }
}
