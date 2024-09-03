using System.ComponentModel.DataAnnotations;

namespace FoolProof.Core.Tests.Models
{
    public class GreaterThan
    {
        public class DateModel : ValidationModelBase<GreaterThanAttribute>
        {
            public DateTime? Value1 { get; set; }

            [GreaterThan("Value1")]
            public DateTime? Value2 { get; set; }
        }

        public class DateModelWithPassOnNull : ValidationModelBase<GreaterThanAttribute>
        {
            public DateTime? Value1 { get; set; }

            [GreaterThan("Value1", PassOnNull = true)]
            public DateTime? Value2 { get; set; }
        }

        public class Int16Model : ValidationModelBase<GreaterThanAttribute>
        {
            public Int16? Value1 { get; set; }

            [GreaterThan("Value1")]
            public Int16? Value2 { get; set; }
        }
    }
}
