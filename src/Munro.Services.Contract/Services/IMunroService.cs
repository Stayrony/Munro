using System.Collections.Generic;
using Munro.Common.Invoke;
using Munro.Common.Models;
using Munro.Models.Models;

namespace Munro.Services.Contract.Services
{
    public interface IMunroService
    {
        InvokeResult<IEnumerable<MunroModel>> GetMunrosByQuery(
            IEnumerable<MunroModel> munros,
            IEnumerable<Condition> conditions,
            IEnumerable<Sort> sorts,
            int? limit);

        InvokeResult<IEnumerable<MunroModel>> ConvertMunrosFullModelToMunrosModel(
            IEnumerable<MunroFullModel> munroFullModels);
    }
}