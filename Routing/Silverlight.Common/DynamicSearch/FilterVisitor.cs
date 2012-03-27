using System;
using System.Net;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Reflection;

namespace Silverlight.Common.DynamicSearch
{
    public class FilterVisitor<TEntity> : IFilterVisitor<TEntity, Expression<Func<TEntity, bool>>>
    {
        public Expression<Func<TEntity, bool>> Visit(AbstractFilter filter)
        {
            return filter.Accept<TEntity, Expression<Func<TEntity, bool>>>(this);
        }

        public Expression<Func<TEntity, bool>> Visit(CompositeFilter filter)
        {
            throw new NotImplementedException();
        }

        public Expression<Func<TEntity, bool>> Visit(AndFilter filter)
        {
            var left = Visit(filter.LeftFilter);
            var right = Visit(filter.RightFilter);

            if (left == null && right == null)
                return null;
            if (left == null && right != null)
                return right;
            if (right == null && left != null)
                return left;

            return left.And(right);
        }

        public Expression<Func<TEntity, bool>> Visit(OrFilter filter)
        {
            var left = Visit(filter.LeftFilter);
            var right = Visit(filter.RightFilter);

            if (left == null && right == null)
                return null;
            if (left == null && right != null)
                return right;
            if (right == null && left != null)
                return left;

            return left.Or(right);
        }

        public Expression<Func<TEntity, bool>> Visit(NotFilter filter)
        {
       
            throw new NotImplementedException();
        }

        //public Expression<Func<TEntity, bool>> Visit(Filter<TEntity> filter)
        //{
        //    throw new NotImplementedException();
        //}

        public Expression<Func<TEntity, bool>> Visit(ExpressionDataBindableFilter filter)
        {
            if (filter.Value == null || (filter.Value is string && string.IsNullOrEmpty(filter.Value as string)) || filter.Value.Equals(default(TEntity)))
                return null;

            var propName = filter.PropertyName;
            bool isNullable = filter.PropertyType.IsGenericType && filter.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>);

            object value = null;
            if(isNullable)
                value = Convert.ChangeType(filter.Value, filter.PropertyType.GetGenericArguments().First(), System.Threading.Thread.CurrentThread.CurrentUICulture);
            else
                value = Convert.ChangeType(filter.Value, filter.PropertyType, System.Threading.Thread.CurrentThread.CurrentUICulture);

            switch (filter.Operator)
            {
                case FilterOperator.Contains:

                    if (filter.IsCaseSensitive)
                        return System.Linq.Dynamic.DynamicExpression.ParseLambda<TEntity, bool>(string.Format("{0}.Contains(@0)", propName), filter.Value);
                    else
                        return System.Linq.Dynamic.DynamicExpression.ParseLambda<TEntity, bool>(string.Format("{0}.ToUpper().Contains(@0)", propName), filter.Value.ToString().ToUpper());

                case FilterOperator.Equals:

                    if (filter.PropertyType == typeof(string))
                        if (filter.IsCaseSensitive)
                            return System.Linq.Dynamic.DynamicExpression.ParseLambda<TEntity, bool>(string.Format("{0}.Equals(@0)", propName), filter.Value);
                        else
                            return System.Linq.Dynamic.DynamicExpression.ParseLambda<TEntity, bool>(string.Format("{0}.ToUpper().Equals(@0)", propName), filter.Value.ToString().ToUpper());

                    if(isNullable)
                        return System.Linq.Dynamic.DynamicExpression.ParseLambda<TEntity, bool>(string.Format("{0}.Value.Equals(@0)", propName), value);
                    
                    return System.Linq.Dynamic.DynamicExpression.ParseLambda<TEntity, bool>(string.Format("{0}.Equals(@0)", propName), value);


                case FilterOperator.GreaterThan:

                    return System.Linq.Dynamic.DynamicExpression.ParseLambda<TEntity, bool>(string.Format("{0} >= @0", propName), filter.Value);
                   

                case FilterOperator.LessThan:

                    return System.Linq.Dynamic.DynamicExpression.ParseLambda<TEntity, bool>(string.Format("{0} <= @0", propName), filter.Value);


                case FilterOperator.NotEquals:
                    throw new NotImplementedException("FilterOperator.NotEquals");

                    if (!filter.IsCaseSensitive && filter.Value != null && filter.Value is string)
                        return System.Linq.Dynamic.DynamicExpression.ParseLambda<TEntity, bool>(string.Format("!{0}.ToUpper().Equals(@0)", propName), filter.Value.ToString().ToUpper());
                    return System.Linq.Dynamic.DynamicExpression.ParseLambda<TEntity, bool>(string.Format("!{0}.Equals(@0)", propName), filter.Value);

                default:
                    break;
            }

            throw new NotImplementedException();
        }

        public Expression<Func<TEntity,bool>> Visit(ExpressionFilter filter)
        {
            return filter.Predicate as Expression<Func<TEntity, bool>>;
        }

        public Expression<Func<TEntity, bool>> Visit(AdapterExpressionFilter filter)
        {
            //throw new NotImplementedException();
            //var adapted = Visit(filter.ValueFilter);

            return filter.CreateExpression(filter.ValueFilter.Value) as Expression<Func<TEntity, bool>>;

        }

        public Expression<Func<TEntity, bool>> Visit(DataBindableFilter filter)
        {
            throw new NotImplementedException();
        }

        

    }


    public class IteratorFilterVisitor<TEntity, TNode> : IFilterVisitor<TEntity> where TNode : AbstractFilter
    {
        Action<TNode> Action;

        public IteratorFilterVisitor(Action<TNode> action)
        {
            Action = action;
        }


        public void Visit(AbstractFilter filter)
        {
            filter.Accept(this);
        }

        public void Visit(CompositeFilter filter)
        {
            Action(filter as TNode);
        }

        public void Visit(AndFilter filter)
        {
            Visit(filter.LeftFilter);
            Visit(filter.RightFilter);

            Action(filter as TNode);
            Action(filter as TNode);
        }

        public void Visit(OrFilter filter)
        {
            Visit(filter.LeftFilter);
            Visit(filter.RightFilter);

            Action(filter as TNode);
            Action(filter as TNode);
        }

        public void Visit(NotFilter filter)
        {
            Action(filter as TNode);
        }

        //public void Visit(Filter<TEntity> filter)
        //{
        //    Action(filter as TNode);
        //}

        public void Visit(ExpressionDataBindableFilter filter)
        {
            Action(filter as TNode);
        }

        public void Visit(ExpressionFilter filter)
        {
            Action(filter as TNode);
        }

        public void Visit(AdapterExpressionFilter filter)
        {
            Visit(filter.ValueFilter);
            Action(filter as TNode);

        }

        public void Visit(DataBindableFilter filter)
        {
            Action(filter as TNode);

        }


    }
}
