using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Touride.Framework.Utilities
{
    public static class LinqExpressionProvider
    {
        public static Expression<Func<T, bool>> strToFunc<T>(string propName, string opr, string value, Expression<Func<T, bool>> expr = null)
        {
            Expression<Func<T, bool>> func = null;

            try
            {
                var type = typeof(T);
                var prop = type.GetProperty(propName);

                ParameterExpression tpe = Expression.Parameter(typeof(T));
                Expression left = Expression.Property(tpe, prop);
                Expression right = Expression.Convert(ToExprConstant(prop, value), prop.PropertyType);
                Expression<Func<T, bool>> innerExpr = Expression.Lambda<Func<T, bool>>(ApplyFilter(opr, left, right), tpe);
                if (expr != null)
                    innerExpr = innerExpr.And(expr);
                func = innerExpr;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }

            return func;
        }

        public static Expression<Func<T, bool>> ExpressionJoin<T>(List<Expression<Func<T, bool>>> exprList = null)
        {
            Expression<Func<T, bool>> innerExpr = null;
            Expression<Func<T, bool>> predicate = null;

            try
            {
                int expressionCount = 0;
                foreach (var item in exprList)
                {
                    expressionCount++;
                    if (expressionCount > 1)
                        innerExpr = item.And(predicate);
                    predicate = item;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }

            return predicate;
        }


        private static Expression ToExprConstant(PropertyInfo prop, string value)
        {
            object val = null;

            try
            {
                switch (prop.Name)
                {
                    case "System.Guid":
                        val = Guid.NewGuid();
                        break;
                    default:
                        {
                            val = TypeDescriptor.GetConverter(prop.PropertyType).ConvertFrom(value);
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return Expression.Constant(val);
        }


        private static BinaryExpression ApplyFilter(string opr, Expression left, Expression right)
        {
            BinaryExpression InnerLambda = null;
            switch (opr)
            {
                case "==":
                case "=":
                    InnerLambda = Expression.Equal(left, right);
                    break;
                case "<":
                    InnerLambda = Expression.LessThan(left, right);
                    break;
                case ">":
                    InnerLambda = Expression.GreaterThan(left, right);
                    break;
                case ">=":
                    InnerLambda = Expression.GreaterThanOrEqual(left, right);
                    break;
                case "<=":
                    InnerLambda = Expression.LessThanOrEqual(left, right);
                    break;
                case "!=":
                    InnerLambda = Expression.NotEqual(left, right);
                    break;
                case "&&":
                    InnerLambda = Expression.And(left, right);
                    break;
                case "||":
                    InnerLambda = Expression.Or(left, right);
                    break;
            }
            return InnerLambda;
        }

        public static Expression<Func<T, TResult>> And<T, TResult>(this Expression<Func<T, TResult>> expr1, Expression<Func<T, TResult>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, TResult>>(Expression.AndAlso(expr1.Body, invokedExpr), expr1.Parameters);
        }

        public static Func<T, TResult> ExpressionToFunc<T, TResult>(this Expression<Func<T, TResult>> expr)
        {
            var res = expr.Compile();
            return res;
        }
    }
}
