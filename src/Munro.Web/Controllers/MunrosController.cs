using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Munro.Common.Invoke;
using Munro.Models.Models;
using Munro.Services.Contract;
using Munro.Web.Requests;

namespace Munro.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MunrosController : ControllerBase
    {
        private readonly IMunrosManager _munrosManager;

        public MunrosController(
            IMunrosManager munrosManager)
        {
            _munrosManager = munrosManager;
        }

        [HttpPost]
        public InvokeResult<IEnumerable<MunroModel>> GetMunros([FromBody] MunroSearchRequest request)
        {
            return _munrosManager.GetMunrosByQuery(
                request.HillCategories,
                request.HeightSortDirectionType,
                request.NameSortDirectionType,
                request.HeightMinMetres,
                request.HeightMaxMetres,
                request.Limit);
        }
    }
}