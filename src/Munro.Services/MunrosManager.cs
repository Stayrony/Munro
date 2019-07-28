using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Munro.Common.Enums;
using Munro.Common.Invoke;
using Munro.Common.Models;
using Munro.Infrastructure.Contract.Repositories;
using Munro.Models.Enums;
using Munro.Models.Models;
using Munro.Services.Contract;
using Munro.Services.Contract.Services;

namespace Munro.Services
{
    public class MunrosManager : IMunrosManager
    {
        private readonly IMunrosRepository _munrosRepository;
        private readonly IInvokeHandler<MunrosManager> _invokeHandler;
        private readonly IFileReaderService _fileReaderService;
        private readonly IMunroService _munroService;

        public MunrosManager(
            IInvokeHandler<MunrosManager> invokeHandler,
            IMunrosRepository munrosRepository,
            IFileReaderService fileReaderService,
            IMunroService munroService)
        {
            _munrosRepository = munrosRepository;
            _invokeHandler = invokeHandler;
            _fileReaderService = fileReaderService;
            _munroService = munroService;
        }

        public async Task<InvokeResult<object>> UploadMunrosDataAsync(IFormFile file)
            => await _invokeHandler.InvokeAsync(async () =>
            {
                var uploadResult = await _fileReaderService.UploadMunrosFileAsync(file);

                if (!uploadResult.IsSuccess)
                {
                    return InvokeResult<object>.Fail(uploadResult.Code);
                }

                var munrosModelResult = _munroService.ConvertMunrosFullModelToMunrosModel(uploadResult.Result);
                if (!munrosModelResult.IsSuccess)
                {
                    return InvokeResult<object>.Fail(munrosModelResult.Code);
                }

                _munrosRepository.AddRange(munrosModelResult.Result);

                return InvokeResult<object>.Ok();
            });

        public InvokeResult<IEnumerable<MunroModel>> GetMunrosByQuery(
            IEnumerable<HillCategory> hillCategories = null,
            SortDirectionType heightSortDirectionType = SortDirectionType.None,
            SortDirectionType nameSortDirectionType = SortDirectionType.None,
            double? heightMinMetres = null,
            double? heightMaxMetres = null,
            int? limit = null)
            => _invokeHandler.Invoke(() =>
            {
                var queryResult = this.CreateMunrosQuery(
                    hillCategories,
                    heightSortDirectionType,
                    nameSortDirectionType,
                    heightMinMetres,
                    heightMaxMetres);

                if (!queryResult.IsSuccess)
                {
                    return InvokeResult<IEnumerable<MunroModel>>.Fail(queryResult.Code);
                }

                var munros = _munrosRepository.GetAll();
               
                var result =
                    _munroService.GetMunrosByQuery(munros, queryResult.Result.conditions, queryResult.Result.sorts,
                        limit);
                return result;
            });

        #region -- Private methods --

        private InvokeResult<(IEnumerable<Condition> conditions, IEnumerable<Sort> sorts)> CreateMunrosQuery(
            IEnumerable<HillCategory> hillCategories = null,
            SortDirectionType heightSortDirectionType = SortDirectionType.None,
            SortDirectionType nameSortDirectionType = SortDirectionType.None,
            double? heightMinMetres = null,
            double? heightMaxMetres = null)
            => _invokeHandler.Invoke(() =>
            {
                var conditions = new List<Condition>();
                var sorts = new List<Sort>();

                if (heightMaxMetres != null && heightMinMetres != null)
                {
                    if (heightMinMetres > heightMaxMetres)
                    {
                        return InvokeResult<(IEnumerable<Condition> conditions, IEnumerable<Sort> sorts)>.Fail(
                            ResultCode.ValidationError,
                            "Maximum height should be more than the minimum height");
                    }

                    conditions.Add(new Condition
                    {
                        ColumnName = nameof(MunroModel.HeightMetres), Type = ConditionType.Range,
                        Values = new object[] {heightMinMetres, heightMaxMetres}
                    });
                }
                else if (heightMaxMetres != null)
                {
                    conditions.Add(new Condition
                    {
                        ColumnName = nameof(MunroModel.HeightMetres), Type = ConditionType.GreaterThanOrEqual,
                        Values = new object[] {heightMaxMetres}
                    });
                }
                else if (heightMinMetres != null)
                {
                    conditions.Add(new Condition
                    {
                        ColumnName = nameof(MunroModel.HeightMetres), Type = ConditionType.LessThanOrEqual,
                        Values = new object[] {heightMinMetres}
                    });
                }

                if (hillCategories != null && hillCategories.Any())
                {
                    conditions.AddRange(hillCategories.Select(x => new Condition
                    {
                        ColumnName = nameof(MunroModel.HillCategory), Type = ConditionType.Equal,
                        Values = new object[] {x}
                    }));
                }

                if (heightSortDirectionType != SortDirectionType.None)
                {
                    sorts.Add(new Sort
                    {
                        ColumnName = nameof(MunroModel.HeightMetres),
                        Type = heightSortDirectionType
                    });
                }

                if (nameSortDirectionType != SortDirectionType.None)
                {
                    sorts.Add(new Sort
                    {
                        ColumnName = nameof(MunroModel.Name),
                        Type = heightSortDirectionType
                    });
                }

                return InvokeResult<(IEnumerable<Condition> conditions, IEnumerable<Sort> sorts)>
                    .Ok((conditions: conditions, sorts: sorts));
            });

        #endregion
    }
}