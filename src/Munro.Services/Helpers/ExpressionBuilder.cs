using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Munro.Common.Enums;
using Munro.Common.Models;
using Munro.Services.Contract.Helpers;

namespace Munro.Services.Helpers
{
    public class ExpressionBuilder : IExpressionBuilder
    {
        public Expression<Func<T, bool>> CreateConditionExpression<T>(
            IEnumerable<Condition> query)
        {
            var groups = query.GroupBy(c => c.ColumnName);

            Expression exp = null;
            var param = Expression.Parameter(typeof(T));

            foreach (var group in groups)
            {
                Expression groupExp = null;

                foreach (var condition in group)
                {
                    Expression con = Expression.Constant(true);

                    switch (condition.Type)
                    {
                        case ConditionType.Range:
                            con = Expression.AndAlso(
                                Expression.GreaterThanOrEqual(Expression.Property(param, condition.ColumnName),
                                    Expression.Constant(condition.Values[0])),
                                Expression.LessThanOrEqual(Expression.Property(param, condition.ColumnName),
                                    Expression.Constant(condition.Values[1])));
                            break;
                        case ConditionType.Equal:
                            con = Expression.AndAlso(con,
                                Expression.Equal(Expression.Property(param, condition.ColumnName),
                                    Expression.Constant(condition.Values[0])));
                            break;
                        case ConditionType.GreaterThanOrEqual:
                            con = Expression.AndAlso(con,
                                Expression.GreaterThanOrEqual(Expression.Property(param, condition.ColumnName),
                                    Expression.Constant(condition.Values[0])));
                            break;
                        case ConditionType.LessThanOrEqual:
                            con = Expression.AndAlso(con,
                                Expression.LessThanOrEqual(Expression.Property(param, condition.ColumnName),
                                    Expression.Constant(condition.Values[0])));
                            break;
                        default:
                            con = Expression.Constant(true);
                            break;
                    }

                    groupExp = groupExp == null ? con : Expression.OrElse(groupExp, con);
                }

                exp = exp == null ? groupExp : Expression.AndAlso(groupExp, exp);
            }

            return Expression.Lambda<Func<T, bool>>(exp, param);
        }

        public IQueryable<T> OrderByColumns<T>(
            IQueryable<T> collection,
            IEnumerable<Sort> query)
        {
            bool firstTime = true;

            var parameter = Expression.Parameter(typeof(T));

            foreach (var column in query)
            {
                var property = Expression.Property(parameter, column.ColumnName);

                // Build something like x => x.Name or x => x.HeightMetres
                var exp = Expression.Lambda(property, parameter);

                // Based on the sorting direction, get the right method
                string method = String.Empty;
                if (firstTime)
                {
                    method = column.Type == SortDirectionType.Ascending
                        ? "OrderBy"
                        : "OrderByDescending";

                    firstTime = false;
                }
                else
                {
                    method = column.Type == SortDirectionType.Ascending
                        ? "ThenBy"
                        : "ThenByDescending";
                }

                // typeof(T) is the type of the T
                // exp.Body.Type is the type of the property. Again, for Name, it's
                //     a String. For HeightMetres, it's a Double.
                Type[] types = {typeof(T), exp.Body.Type};

                // Build the call expression
                // It will look something like:
                //     OrderBy*(x => x.Name) or Order*(x => x.HeightMetres)
                //     ThenBy*(x => x.Name) or ThenBy*(x => x.HeightMetres)

                var mce = Expression.Call(typeof(Queryable), method, types,
                    collection.Expression, exp);

                // Now you can run the expression against the collection
                collection = collection.Provider.CreateQuery<T>(mce);
            }

            return collection;
        }
    }
}