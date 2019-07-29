using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Munro.Common.Models;

namespace Munro.Services.Contract.Helpers
{
    public interface IExpressionBuilder
    {
        Expression<Func<T, bool>> CreateConditionExpression<T>(IEnumerable<Condition> query);

        IQueryable<T> OrderByColumns<T>(IQueryable<T> collection, IEnumerable<Sort> query);
    }
}