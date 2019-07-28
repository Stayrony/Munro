using System.Collections.Generic;
using Munro.Infrastructure.Contract.Repositories;
using Munro.Models.Models;

namespace Munro.Infrastructure.Repositories
{
    public class MunrosRepository : IMunrosRepository
    {
        private List<MunroModel> _munros;
        
        public MunrosRepository()
        {
             _munros = new List<MunroModel>();   
        }
        
        public IEnumerable<MunroModel> GetAll()
        {
            return _munros;
        }

        public bool AddRange(IEnumerable<MunroModel> munroModels)
        {
            if (munroModels == null)
            {
                return false;
            }
            
            _munros.AddRange(munroModels);
            return true;

        }
    }
}