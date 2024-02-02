namespace FoolProof.Core.Tests.Models
{
    public class RequiredIfFalse
    {
        public class Model : ValidationModelBase<RequiredIfFalseAttribute>
        {
            public bool? Value1 { get; set; }

            [RequiredIfFalse("Value1")]
            public string? Value2 { get; set; }
        }
    }
}
