using System.Collections.Generic;
using System.Linq;
using Munro.Common.Enums;
using Munro.Common.Models;
using Munro.Models.Enums;
using Munro.Models.Models;
using Munro.Services.Helpers;
using NUnit.Framework;

namespace Munro.Services.Tests
{
    public class ExpressionBuilderTests
    {
        private IQueryable<MunroModel> _munros;

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
        public void ShouldReturnGreaterThanOrEqualOfHeightMetresMunros()
        {
            ExpressionBuilder expressionBuilder = new ExpressionBuilder();

            var conditions = new List<Condition>
            {
                new Condition
                {
                    ColumnName = nameof(MunroModel.HeightMetres),
                    Type = ConditionType.GreaterThanOrEqual, Values = new object[] {1000.0}
                }
            };

            var exp = expressionBuilder.CreateConditionExpression<MunroModel>(conditions);

            var actual = _munros.Where(exp).ToList();

            Assert.AreEqual(actual.Count, 2);
        }

        [Test]
        public void ShouldReturnLessThanOrEqualOfHeightMetresMunros()
        {
            ExpressionBuilder expressionBuilder = new ExpressionBuilder();

            var conditions = new List<Condition>
            {
                new Condition
                {
                    ColumnName = nameof(MunroModel.HeightMetres),
                    Type = ConditionType.LessThanOrEqual, Values = new object[] {1000.0}
                }
            };

            var exp = expressionBuilder.CreateConditionExpression<MunroModel>(conditions);

            var actual = _munros.Where(exp).ToList();

            Assert.AreEqual(actual.Count, 3);
        }
        
        [Test]
        public void ShouldReturnRangeOfHeightMetresMunros()
        {
            ExpressionBuilder expressionBuilder = new ExpressionBuilder();

            var conditions = new List<Condition>
            {
                new Condition
                {
                    ColumnName = nameof(MunroModel.HeightMetres), Type = ConditionType.Range,
                    Values = new object[] {1000.0, 2000.0}
                }
            };

            var exp = expressionBuilder.CreateConditionExpression<MunroModel>(conditions);

            var actual = _munros.Where(exp).ToList();

            Assert.AreEqual(actual.Count, 2);
        }
        
        [Test]
        public void ShouldReturnEqualHillCategoryTOPMunros()
        {
            ExpressionBuilder expressionBuilder = new ExpressionBuilder();

            var conditions = new List<Condition>
            {
                new Condition
                {
                    ColumnName = nameof(MunroModel.HillCategory), Type = ConditionType.Equal,
                    Values = new object[] { HillCategory.TOP }
                }
            };

            var exp = expressionBuilder.CreateConditionExpression<MunroModel>(conditions);

            var actual = _munros.Where(exp).ToList();

            Assert.AreEqual(actual.Count, 2);
        }
        
        [Test]
        public void ShouldReturnEqualHillCategoryTOP_And_MUN_Munros()
        {
            ExpressionBuilder expressionBuilder = new ExpressionBuilder();

            var conditions = new List<Condition>
            {
                new Condition
                {
                    ColumnName = nameof(MunroModel.HillCategory), Type = ConditionType.Equal,
                    Values = new object[] { HillCategory.TOP }
                },
                new Condition
                {
                    ColumnName = nameof(MunroModel.HillCategory), Type = ConditionType.Equal,
                    Values = new object[] { HillCategory.MUN }
                }
            };

            var exp = expressionBuilder.CreateConditionExpression<MunroModel>(conditions);

            var actual = _munros.Where(exp).ToList();

            Assert.AreEqual(actual.Count, 5);
        }
        
        [Test]
        public void ShouldReturnEqualHillCategoryMUNMunros()
        {
            ExpressionBuilder expressionBuilder = new ExpressionBuilder();

            var conditions = new List<Condition>
            {
                new Condition
                {
                    ColumnName = nameof(MunroModel.HillCategory), Type = ConditionType.Equal,
                    Values = new object[] { HillCategory.MUN }
                }
            };

            var exp = expressionBuilder.CreateConditionExpression<MunroModel>(conditions);

            var actual = _munros.Where(exp).ToList();

            Assert.AreEqual(actual.Count, 3);
        }
        
        [Test]
        public void ShouldReturnEqualHillCategoryMUN_And_RangeOfHeightMetres_Munros()
        {
            ExpressionBuilder expressionBuilder = new ExpressionBuilder();

            var conditions = new List<Condition>
            {
                new Condition
                {
                    ColumnName = nameof(MunroModel.HillCategory), Type = ConditionType.Equal,
                    Values = new object[] { HillCategory.MUN }
                },
                new Condition
                {
                    ColumnName = nameof(MunroModel.HeightMetres), Type = ConditionType.Range,
                    Values = new object[] {1000.0, 2000.0}
                }
                
            };

            var exp = expressionBuilder.CreateConditionExpression<MunroModel>(conditions);

            var actual = _munros.Where(exp).ToList();

            Assert.AreEqual(actual.Count, 1);
        }
        
        [Test]
        public void ShouldReturnEqualHillCategoryMUN_And_EqualHillCategoryTOP_And_RangeOfHeightMetres_Munros()
        {
            ExpressionBuilder expressionBuilder = new ExpressionBuilder();

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

            var exp = expressionBuilder.CreateConditionExpression<MunroModel>(conditions);

            var actual = _munros.Where(exp).ToList();

            Assert.AreEqual(actual.Count, 3);
        }
        
        [Test]
        public void ShouldReturnLessThanOrEqualOfHeightMetres_And_EqualHillCategoryTOP_Munros()
        {
            ExpressionBuilder expressionBuilder = new ExpressionBuilder();

            var conditions = new List<Condition>
            {
                new Condition
                {
                    ColumnName = nameof(MunroModel.HeightMetres),
                    Type = ConditionType.LessThanOrEqual, Values = new object[] {1000.0}
                },
                new Condition
                {
                    ColumnName = nameof(MunroModel.HillCategory), Type = ConditionType.Equal,
                    Values = new object[] { HillCategory.TOP }
                }
            };

            var exp = expressionBuilder.CreateConditionExpression<MunroModel>(conditions);

            var actual = _munros.Where(exp).ToList();

            Assert.AreEqual(actual.Count, 1);
        }
        
        [Test]
        public void ShouldReturnLessThanOrEqualOfHeightMetres_And_EqualHillCategoryMUN_Munros()
        {
            ExpressionBuilder expressionBuilder = new ExpressionBuilder();

            var conditions = new List<Condition>
            {
                new Condition
                {
                    ColumnName = nameof(MunroModel.HeightMetres),
                    Type = ConditionType.LessThanOrEqual, Values = new object[] {900.0}
                },
                new Condition
                {
                    ColumnName = nameof(MunroModel.HillCategory), Type = ConditionType.Equal,
                    Values = new object[] { HillCategory.TOP }
                }
            };

            var exp = expressionBuilder.CreateConditionExpression<MunroModel>(conditions);

            var actual = _munros.Where(exp).ToList();

            Assert.AreEqual(actual.Count, 0);
        }
        
        [Test]
        public void ShouldReturnSortDescendingHeightMetres_Munros()
        {
            ExpressionBuilder expressionBuilder = new ExpressionBuilder();

            var sorts = new List<Sort>
            {
                new Sort
                {
                    ColumnName = nameof(MunroModel.HeightMetres),
                    Type = SortDirectionType.Descending
                }
            };

            var actual = expressionBuilder.OrderByColumns(_munros, sorts);

            Assert.That( actual, Is.Ordered.Descending.By("HeightMetres") );
        }
        
        [Test]
        public void ShouldReturnSortDescendingByHeightMetres_And_SortAscendingByName_Munros()
        {
            ExpressionBuilder expressionBuilder = new ExpressionBuilder();

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

            var actual = expressionBuilder.OrderByColumns(_munros, sorts);

            Assert.That( actual, Is.Ordered.Descending.By("HeightMetres").Then.Ascending.By("Name") );
        }
        
        [Test]
        public void ShouldReturnSortDescendingByHeightMetres_And_SortDescendingByName_Munros()
        {
            ExpressionBuilder expressionBuilder = new ExpressionBuilder();

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
                    Type = SortDirectionType.Descending
                }
            };

            var actual = expressionBuilder.OrderByColumns(_munros, sorts);

            Assert.That( actual, Is.Ordered.Descending.By("HeightMetres").Then.Descending.By("Name") );
        }
    }
}