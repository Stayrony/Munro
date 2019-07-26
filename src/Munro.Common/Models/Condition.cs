using Munro.Common.Enums;

namespace Munro.Common.Models
{
    public class Condition
    {
        public string ColumnName { get; set; }

        public ConditionType Type { get; set; }

        public object[] Values { get; set; }
    }
}