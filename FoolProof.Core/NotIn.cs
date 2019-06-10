
namespace FoolProof.Core
{
    public class NotInAttribute : IsAttribute
    {
        public NotInAttribute(string dependentProperty) : base(Operator.NotIn, dependentProperty) { }
    }
}
