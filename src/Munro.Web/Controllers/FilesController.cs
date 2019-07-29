using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Munro.Common.Invoke;
using Munro.Services.Contract;

namespace Munro.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IMunrosManager _munrosManager;
        
        public FilesController(IMunrosManager munrosManager)
        {
            _munrosManager = munrosManager;
        }

        [HttpPost("UploadFile")]
        public async Task<InvokeResult<object>> UploadFileAsync(IFormFile file)
        {
            return await _munrosManager.UploadMunrosDataAsync(file);
        }
    }
}