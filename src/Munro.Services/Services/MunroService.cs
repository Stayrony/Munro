using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Munro.Common.Invoke;
using Munro.Common.Models;
using Munro.Services.Contract.Helpers;
using Munro.Services.Contract.Services;
using Munro.Models.Enums;
using Munro.Models.Models;

namespace Munro.Services.Services
{
    public class MunroService : IMunroService
    {
        private readonly IInvokeHandler<MunroService> _invokeHandler;
        private readonly IExpressionBuilder _expressionBuilder;

        public MunroService(IInvokeHandler<MunroService> invokeHandler,
            IExpressionBuilder expressionBuilder)
        {
            _invokeHandler = invokeHandler;
            _expressionBuilder = expressionBuilder;
        }

        public InvokeResult<IEnumerable<MunroModel>> ConvertMunrosFullModelToMunrosModel(
            IEnumerable<MunroFullModel> munroFullModels)
            => _invokeHandler.Invoke(() =>
            {
                var result = munroFullModels
                    .Where(x => !string.IsNullOrEmpty(x.HillCategoryPost1997))
                    .Select(x => new MunroModel
                    {
                        Name = x.Name,
                        GridReference = x.GridRef,
                        HeightMetres = double.TryParse(x.Heightm, out double height) ? height : 0,
                        HillCategory = Enum.TryParse(x.HillCategoryPost1997, out HillCategory category)
                            ? category
                            : HillCategory.NONE,
                    });

                return InvokeResult<IEnumerable<MunroModel>>.Ok(result);
            });

        public InvokeResult<IEnumerable<MunroModel>> GetMunrosByQuery(
            IEnumerable<MunroModel> munros,
            IEnumerable<Condition> conditions,
            IEnumerable<Sort> sorts,
            int? limit)
            => _invokeHandler.Invoke(() =>
            {
                if (munros == null || !munros.Any())
                {
                    return InvokeResult<IEnumerable<MunroModel>>.Fail(ResultCode.ObjectMissing, "Munros list is empty!");
                }
                
                IEnumerable<MunroModel> sortMunros = munros;
                IEnumerable<MunroModel> filterMunros = munros;
                
                if (conditions != null && conditions.Any())
                {
                    var expressionQuery = _expressionBuilder.CreateConditionExpression<MunroModel>(conditions);

                    filterMunros = munros.AsQueryable().Where(expressionQuery);
                }

                if (sorts != null && sorts.Any())
                {
                    sortMunros = _expressionBuilder.OrderByColumns(filterMunros.AsQueryable(), sorts);
                }
                else
                {
                    sortMunros = filterMunros;
                }

                if (limit != null && limit > 0)
                {
                    sortMunros = sortMunros.Take(limit.Value).ToList();
                }
                else
                {
                    sortMunros = sortMunros.ToList();
                }

                return InvokeResult<IEnumerable<MunroModel>>.Ok(sortMunros);
            });
    }
}