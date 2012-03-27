using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Silverlight.Common.DynamicSearch
{
    public interface IFilterVisitor<TEntity, TResult>
    {
        TResult Visit(AbstractFilter filter);

        TResult Visit(CompositeFilter filter);
        TResult Visit(AndFilter filter);
        TResult Visit(OrFilter filter);
        TResult Visit(NotFilter filter);
        //TResult Visit(Filter<TEntity> filter);

        TResult Visit(ExpressionDataBindableFilter filter);
        TResult Visit(ExpressionFilter filter);
        TResult Visit(AdapterExpressionFilter filter);
        TResult Visit(DataBindableFilter filter);
    }

    public interface IFilterVisitor<TEntity>
    {
        void Visit(AbstractFilter filter);

        void Visit(CompositeFilter filter);
        void Visit(AndFilter filter);
        void Visit(OrFilter filter);
        void Visit(NotFilter filter);
        //void Visit(Filter<TEntity> filter);

        void Visit(ExpressionDataBindableFilter filter);
        void Visit(ExpressionFilter filter);
        void Visit(AdapterExpressionFilter filter);
        void Visit(DataBindableFilter filter);
    }
}
