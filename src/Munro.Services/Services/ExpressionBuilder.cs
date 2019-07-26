using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Munro.Common.Enums;
using Munro.Common.Models;

namespace Munro.Services.Services
{
    public class ExpressionBuilder
    {
        public ExpressionBuilder()
        {
        }

        public Expression<Func<T, bool>> CreateExpression<T>(IEnumerable<Condition> query)
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
    }
}