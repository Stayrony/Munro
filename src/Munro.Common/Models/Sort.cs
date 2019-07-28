using Munro.Common.Enums;

namespace Munro.Common.Models
{
    public class Sort
    {
        public string ColumnName { get; set; }

        public SortDirectionType Type { get; set; }
    }
}