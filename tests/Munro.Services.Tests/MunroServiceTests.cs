using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Moq;
using Munro.Common.Enums;
using Munro.Common.Invoke;
using Munro.Common.Models;
using Munro.Models.Enums;
using Munro.Models.Models;
using Munro.Services.Contract.Helpers;
using Munro.Services.Helpers;
using Munro.Services.Services;
using NUnit.Framework;

namespace Munro.Services.Tests
{
    public class MunroServiceTests
    {
        private readonly Mock<ILogger<MunroService>> _mockLogger;
        private readonly IInvokeHandler<MunroService> _invokeHandler;
        private readonly IExpressionBuilder _expressionBuilder;
        private IQueryable<MunroModel> _munros;
        
        public MunroServiceTests()
        {
            _mockLogger = new Mock<ILogger<MunroService>>();
            _invokeHandler = new BaseInvokeHandler<MunroService>(new InvokeResultSettings(), _mockLogger.Object);
            _expressionBuilder = new ExpressionBuilder();
        }
        
        [SetUp]
        public void Setup()
        {
            _munros = new List<MunroModel>
            {
                new MunroModel
                {
                    Name = "Ben Chonzie", HillCategory = HillCategory.MUN, GridReference = "NN773308",
                    HeightMetres = 931
                },
                new MunroModel
                {
                    Name = "Stob Binnein - Stob Coire an Lochain", HillCategory = HillCategory.TOP,
                    GridReference = "NN438220", HeightMetres = 1068
                },
                new MunroModel
                {
                    Name = "Ben Vorlich North Top", HillCategory = HillCategory.TOP, GridReference = "NN294130",
                    HeightMetres = 931
                },
                new MunroModel
                {
                    Name = "Beinn a' Chreachain", HillCategory = HillCategory.MUN, GridReference = "NN373440",
                    HeightMetres = 1080.6
                },
                new MunroModel
                {
                    Name = "Stob a' Choire Odhair", HillCategory = HillCategory.MUN, GridReference = "NN257459",
                    HeightMetres = 945
                }
            }.AsQueryable();
        }
        
        [Test]
        public void ShouldReturnErrorIfEmptyMunrosList()
        {
            var munrosService = new MunroService(_invokeHandler, _expressionBuilder);

            var result = munrosService.GetMunrosByQuery(null, null, null, null);
            
            Assert.Null(result.Exception);
            Assert.False(result.IsSuccess);
            Assert.Null(result.Result);
            Assert.AreEqual(ResultCode.ObjectMissing, result.Code);
        }

        [Test]
        public void ShouldReturnGreaterThanOrEqualOfHeightMetresMunros()
        {
            var munrosService = new MunroService(_invokeHandler, _expressionBuilder);
            
            var conditions = new List<Condition>
            {
                new Condition
                {
                    ColumnName = nameof(MunroModel.HeightMetres),
                    Type = ConditionType.GreaterThanOrEqual, Values = new object[] {1000.0}
                }
            };
            
            var result = munrosService.GetMunrosByQuery(_munros, conditions, null, null);
            
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Result);
            Assert.IsNotEmpty(result.Result);
            Assert.AreEqual(result.Result.Count(), 2);
        }
        
        [Test]
        public void ShouldReturnGreaterThanOrEqualOfHeightMetres_And_SortHeightMetresDescending_Munros()
        {
            var munrosService = new MunroService(_invokeHandler, _expressionBuilder);
            
            var conditions = new List<Condition>
            {
                new Condition
                {
                    ColumnName = nameof(MunroModel.HeightMetres),
                    Type = ConditionType.GreaterThanOrEqual, Values = new object[] {1000.0}
                }
            };
            
            var sorts = new List<Sort>
            {
                new Sort
                {
                    ColumnName = nameof(MunroModel.HeightMetres),
                    Type = SortDirectionType.Descending
                }
            };
            
            var result = munrosService.GetMunrosByQuery(_munros, conditions, sorts, null);
            
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Result);
            Assert.IsNotEmpty(result.Result);
            Assert.AreEqual(result.Result.Count(), 2);
            Assert.That( result.Result, Is.Ordered.Descending.By("HeightMetres") );
        }
        
        [Test]
        public void ShouldReturnGreaterThanOrEqualOfHeightMetres_And_SortDescendingHeightMetres_SortAscendingByName_Munros()
        {
            var munrosService = new MunroService(_invokeHandler, _expressionBuilder);
            
            var conditions = new List<Condition>
            {
                new Condition
                {
                    ColumnName = nameof(MunroModel.HeightMetres),
                    Type = ConditionType.GreaterThanOrEqual, Values = new object[] {1000.0}
                }
            };
            
            var sorts = new List<Sort>
            {
                new Sort
                {
                    ColumnName = nameof(MunroModel.HeightMetres),
                    Type = SortDirectionType.Descending
                },
                new Sort
                {
                    ColumnName = nameof(MunroModel.Name),
                    Type = SortDirectionType.Ascending
                }
            };
            
            var result = munrosService.GetMunrosByQuery(_munros, conditions, sorts, null);
            
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Result);
            Assert.IsNotEmpty(result.Result);
            Assert.AreEqual(result.Result.Count(), 2);
            Assert.That( result.Result, Is.Ordered.Descending.By("HeightMetres").Then.Ascending.By("Name") );
        }
        
        [Test]
        public void ShouldReturnEqualOfHeightMetres_And_SortDescendingHeightMetres_SortAscendingByName_Munros()
        {
            var munrosService = new MunroService(_invokeHandler, _expressionBuilder);
            
            var conditions = new List<Condition>
            {
                new Condition
                {
                    ColumnName = nameof(MunroModel.HeightMetres),
                    Type = ConditionType.Equal, Values = new object[] {931.0}
                }
            };
            
            var sorts = new List<Sort>
            {
                new Sort
                {
                    ColumnName = nameof(MunroModel.HeightMetres),
                    Type = SortDirectionType.Descending
                },
                new Sort
                {
                    ColumnName = nameof(MunroModel.Name),
                    Type = SortDirectionType.Ascending
                }
            };
            
            var result = munrosService.GetMunrosByQuery(_munros, conditions, sorts, null);
            
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Result);
            Assert.IsNotEmpty(result.Result);
            Assert.AreEqual(result.Result.Count(), 2);
            Assert.That( result.Result, Is.Ordered.Descending.By("HeightMetres").Then.Ascending.By("Name") );
        }
        
        [Test]
        public void ShouldReturnEqualHillCategoryMUN_And_EqualHillCategoryTOP_And_RangeOfHeightMetres_SortAscendingByName_Munros()
        {
            var munrosService = new MunroService(_invokeHandler, _expressionBuilder);
            
            var conditions = new List<Condition>
            {
                new Condition
                {
                    ColumnName = nameof(MunroModel.HillCategory), Type = ConditionType.Equal,
                    Values = new object[] { HillCategory.MUN }
                },
                new Condition
                {
                    ColumnName = nameof(MunroModel.HillCategory), Type = ConditionType.Equal,
                    Values = new object[] { HillCategory.TOP }
                },
                new Condition
                {
                    ColumnName = nameof(MunroModel.HeightMetres), Type = ConditionType.Range,
                    Values = new object[] {940.0, 2000.0}
                }
            };
            
            var sorts = new List<Sort>
            {
                new Sort
                {
                    ColumnName = nameof(MunroModel.HeightMetres),
                    Type = SortDirectionType.Descending
                },
                new Sort
                {
                    ColumnName = nameof(MunroModel.Name),
                    Type = SortDirectionType.Ascending
                }
            };
            
            var result = munrosService.GetMunrosByQuery(_munros, conditions, sorts, null);
            
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Result);
            Assert.IsNotEmpty(result.Result);
            Assert.AreEqual(result.Result.Count(), 3);
            Assert.That( result.Result, Is.Ordered.Descending.By("HeightMetres").Then.Ascending.By("Name") );
        }
        
        [Test]
        public void ShouldReturnEqualHillCategoryMUN_And_EqualHillCategoryTOP_And_RangeOfHeightMetres_SortAscendingByName_And_Limit_Munros()
        {
            var munrosService = new MunroService(_invokeHandler, _expressionBuilder);
            
            var conditions = new List<Condition>
            {
                new Condition
                {
                    ColumnName = nameof(MunroModel.HillCategory), Type = ConditionType.Equal,
                    Values = new object[] { HillCategory.MUN }
                },
                new Condition
                {
                    ColumnName = nameof(MunroModel.HillCategory), Type = ConditionType.Equal,
                    Values = new object[] { HillCategory.TOP }
                },
                new Condition
                {
                    ColumnName = nameof(MunroModel.HeightMetres), Type = ConditionType.Range,
                    Values = new object[] {940.0, 2000.0}
                }
            };
            
            var sorts = new List<Sort>
            {
                new Sort
                {
                    ColumnName = nameof(MunroModel.HeightMetres),
                    Type = SortDirectionType.Descending
                },
                new Sort
                {
                    ColumnName = nameof(MunroModel.Name),
                    Type = SortDirectionType.Ascending
                }
            };
            
            var result = munrosService.GetMunrosByQuery(_munros, conditions, sorts, 2);
            
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Result);
            Assert.IsNotEmpty(result.Result);
            Assert.AreEqual(result.Result.Count(), 2);
            Assert.That( result.Result, Is.Ordered.Descending.By("HeightMetres").Then.Ascending.By("Name") );
        }
        
        [TestCase(1)]
        [TestCase(5)]
        [TestCase(3)]
        public void ShouldReturn_Limit_Munros(int limit)
        {
            var munrosService = new MunroService(_invokeHandler, _expressionBuilder);
            
            var result = munrosService.GetMunrosByQuery(_munros, null, null, limit);
            
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Result);
            Assert.IsNotEmpty(result.Result);
            Assert.AreEqual(result.Result.Count(), limit);
        }
    }
}