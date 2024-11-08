using System.ComponentModel.DataAnnotations;

namespace FoolProof.Core.Tests.Models
{
    public class NotIn
    {
        public class SingleValueModel<T> : ValidationModelBase<NotInAttribute>
        {
            [DataType("StringList")]
            public T? Value1 { get; set; }

            [DataType(DataType.Text)]
            [NotIn(nameof(Value1))]
            public T? Value2 { get; set; }

            [DataType(DataType.Text)]
            [NotIn(nameof(Value1), PassOnNull = true)]
            public T? ValuePwn { get; set; }
        }

        public class DateTimeListModel : ValidationModelBase<NotInAttribute>
        {
            [DataType("DateTimeList")]
            public IEnumerable<DateTime>? Value1 { get; set; }

            [DataType(DataType.DateTime)]
            [NotIn(nameof(Value1))]
            public DateTime? Value2 { get; set; }

            [DataType(DataType.DateTime)]
            [NotIn(nameof(Value1), PassOnNull = true)]
            public DateTime? ValuePwn { get; set; }
        }

        public class In16ListModel : ValidationModelBase<NotInAttribute>
        {
            [DataType("Int16List")]
            public IEnumerable<Int16>? Value1 { get; set; }

            [NotIn(nameof(Value1))]
            public Int16? Value2 { get; set; }

            [NotIn(nameof(Value1), PassOnNull = true)]
            public Int16? ValuePwn { get; set; }
        }
    }
}
