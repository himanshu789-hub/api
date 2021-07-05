using System;
using Shambala.Domain;
using System.Linq.Expressions;
namespace Shambala.Helpher
{
    public class InvoiceTolerance
    {
        class BalanceInfo
        {
            public decimal Price { get; set; }
            public decimal Debit { get; set; }
        }
        public static decimal ToleranceValue = 0.0M;
        public static bool IsCleared(decimal Price, decimal debit)
        {
            BalanceInfo balanceInfo = new BalanceInfo { Debit = debit, Price = Price };
            return ExpressionMethods.IsCleared<BalanceInfo>("Price","Debit").Compile().Invoke(balanceInfo);
        }
    }
    public class ExpressionMethods
    {
        public static Expression<Func<T, bool>> IsCleared<T>(string totalPrice, string duePrice) where T : class
        {

            var invoice = ParameterExpression.Parameter(typeof(T));
            var leftParameter = Expression.PropertyOrField(invoice, totalPrice);
            var rightParameter = Expression.PropertyOrField(invoice, duePrice);

            Expression binaryExpression = Expression.LessThanOrEqual(Expression.Subtract(leftParameter, rightParameter), ParameterExpression.Constant(InvoiceTolerance.ToleranceValue));
            return Expression.Lambda<Func<T, bool>>(binaryExpression, invoice);
        }

        public static Expression<TFunc> Negate<TFunc>(Expression<TFunc> expression)
        {
            var param = expression.Parameters;
            var body = expression.Body;
            return Expression.Lambda<TFunc>(body, param);
        }

    }
    public static class ExpressionExtension
    {
        public static Expression<Func<T, bool>> OrElse<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            if (expr1 == null)
                return expr2;

            var parameter = Expression.Parameter(typeof(T));

            var leftVisitor = new ReplaceExpressionVisitor(expr1.Parameters[0], parameter);
            var left = leftVisitor.Visit(expr1.Body);
            var rightVisitor = new ReplaceExpressionVisitor(expr2.Parameters[0], parameter);
            var right = rightVisitor.Visit(expr2.Body);

            return Expression.Lambda<Func<T, bool>>(Expression.OrElse(left, right), parameter);

        }
        public static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2) where T : class
        {
            if (expr1 == null)
                return expr2;
            var parameter = Expression.Parameter(typeof(T));

            var leftVisitor = new ReplaceExpressionVisitor(expr1.Parameters[0], parameter);
            var left = leftVisitor.Visit(expr1.Body);
            var rightVisitor = new ReplaceExpressionVisitor(expr2.Parameters[0], parameter);
            var right = rightVisitor.Visit(expr2.Body);

            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(left, right), parameter);
        }
        private class ReplaceExpressionVisitor
        : ExpressionVisitor
        {
            private readonly Expression _oldValue;
            private readonly Expression _newValue;

            public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
            {
                _oldValue = oldValue;
                _newValue = newValue;
            }

            public override Expression Visit(Expression node)
            {
                if (node == _oldValue)
                    return _newValue;
                return base.Visit(node);
            }
        }
    }
}
