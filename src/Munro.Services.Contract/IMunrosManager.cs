using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Munro.Common.Enums;
using Munro.Common.Invoke;
using Munro.Models.Enums;
using Munro.Models.Models;

namespace Munro.Services.Contract
{
    public interface IMunrosManager
    {
        Task<InvokeResult<object>> UploadMunrosDataAsync(IFormFile file);

        InvokeResult<IEnumerable<MunroModel>> GetMunrosByQuery(
            IEnumerable<HillCategory> hillCategories = null,
            SortDirectionType heightSortDirectionType = SortDirectionType.None,
            SortDirectionType nameSortDirectionType = SortDirectionType.None,
            double? heightMinMetres = null,
            double? heightMaxMetres = null,
            int? limit = null);
    }
}