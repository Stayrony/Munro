using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Munro.Common.Invoke;
using Munro.Models.Models;

namespace Munro.Services.Contract.Services
{
    public interface IFileReaderService
    {
        Task<InvokeResult<IEnumerable<MunroFullModel>>> UploadMunrosFileAsync(IFormFile file);
    }
}