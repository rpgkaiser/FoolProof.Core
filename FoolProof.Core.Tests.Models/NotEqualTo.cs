using System.ComponentModel.DataAnnotations;

namespace FoolProof.Core.Tests.Models
{
    public class NotEqualTo
    {
        public class Model : ValidationModelBase<NotEqualToAttribute>
        {
            public string? Value1 { get; set; }

            [NotEqualTo(nameof(Value1))]
            public string? Value2 { get; set; }

            [NotEqualTo(nameof(Value1), PassOnNull = true)]
            public string? ValuePwn { get; set; }
        }
    }
}
