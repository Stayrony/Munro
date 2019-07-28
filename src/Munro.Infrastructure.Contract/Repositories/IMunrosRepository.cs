using System.Collections.Generic;
using Munro.Models.Models;

namespace Munro.Infrastructure.Contract.Repositories
{
    public interface IMunrosRepository
    {
        IEnumerable<MunroModel> GetAll();
        bool AddRange(IEnumerable<MunroModel> munroModels);
    }
}