using System.Collections.Generic;
using System.Linq;
using Munro.Common.Enums;
using Munro.Common.Models;
using Munro.Models.Enums;
using Munro.Services.Services;
using NUnit.Framework;

namespace Munro.Services.Tests
{
    public class ExpressionBuilderTests
    {
        private IQueryable<Munro.Models.Models.Munro> Munros;

        [SetUp]
        public void Setup()
        {
            Munros = new List<Munro.Models.Models.Munro>
            {
                new Munro.Models.Models.Munro
                {
                    Name = "Ben Chonzie", HillCategory = HillCategory.MUN, GridReference = "NN773308",
                    HeightMetres = 931
                },
                new Munro.Models.Models.Munro
                {
                    Name = "Stob Binnein - Stob Coire an Lochain", HillCategory = HillCategory.TOP,
                    GridReference = "NN438220", HeightMetres = 1068
                },
                new Munro.Models.Models.Munro
                {
                    Name = "Ben Vorlich North Top", HillCategory = HillCategory.TOP, GridReference = "NN294130",
                    HeightMetres = 931
                },
                new Munro.Models.Models.Munro
                {
                    Name = "Beinn a' Chreachain", HillCategory = HillCategory.MUN, GridReference = "NN373440",
                    HeightMetres = 1080.6
                },
                new Munro.Models.Models.Munro
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
                    ColumnName = nameof(Munro.Models.Models.Munro.HeightMetres),
                    Type = ConditionType.GreaterThanOrEqual, Values = new object[] {1000.0}
                }
            };

            var exp = expressionBuilder.CreateExpression<Munro.Models.Models.Munro>(conditions);

            var actual = Munros.Where(exp).ToList();

            Assert.AreEqual(actual.Count, 2);
        }

        [Test]
        public void ShouldReturnRangeOfHeightMetresMunros()
        {
            ExpressionBuilder expressionBuilder = new ExpressionBuilder();

            var conditions = new List<Condition>
            {
                new Condition
                {
                    ColumnName = nameof(Munro.Models.Models.Munro.HeightMetres), Type = ConditionType.Range,
                    Values = new object[] {1000.0, 2000.0}
                },
            };

            var exp = expressionBuilder.CreateExpression<Munro.Models.Models.Munro>(conditions);

            var actual = Munros.Where(exp).ToList();

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
                    ColumnName = nameof(Munro.Models.Models.Munro.HillCategory), Type = ConditionType.Equal,
                    Values = new object[] { HillCategory.TOP }
                },
            };

            var exp = expressionBuilder.CreateExpression<Munro.Models.Models.Munro>(conditions);

            var actual = Munros.Where(exp).ToList();

            Assert.AreEqual(actual.Count, 2);
        }
        
        [Test]
        public void ShouldReturnEqualHillCategoryMUNMunros()
        {
            ExpressionBuilder expressionBuilder = new ExpressionBuilder();

            var conditions = new List<Condition>
            {
                new Condition
                {
                    ColumnName = nameof(Munro.Models.Models.Munro.HillCategory), Type = ConditionType.Equal,
                    Values = new object[] { HillCategory.MUN }
                },
            };

            var exp = expressionBuilder.CreateExpression<Munro.Models.Models.Munro>(conditions);

            var actual = Munros.Where(exp).ToList();

            Assert.AreEqual(actual.Count, 3);
        }
    }
}