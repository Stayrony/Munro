using System.Collections.Generic;
using Munro.Common.Enums;
using Munro.Models.Enums;

namespace Munro.Web.Requests
{
    public class MunroSearchRequest
    {
        public IEnumerable<HillCategory> HillCategories { get; set; }
        public SortDirectionType HeightSortDirectionType { get;  set; } = SortDirectionType.None;
        public SortDirectionType NameSortDirectionType { get; set; } = SortDirectionType.None;
        public double? HeightMinMetres { get;  set; }
        public double? HeightMaxMetres { get; set; }
        public int? Limit { get; set; }
    }
}