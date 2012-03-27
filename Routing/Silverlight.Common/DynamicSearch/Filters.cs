using System;
using System.Net;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace Silverlight.Common.DynamicSearch
{
    [DataContract]
    public enum FilterOperator
    {
        [EnumMember]
        Equals,
        [EnumMember]
        GreaterThan,
        [EnumMember]
        LessThan,
        [EnumMember]
        NotEquals,
        [EnumMember]
        Contains
    }

    //[DataContract(IsReference = true)]
    //[KnownType(typeof(FilterOperator))]
    //public class Filter<TEntity> : AbstractFilter, INotifyPropertyChanged
    //{
    //    public override void Accept<TEntity>(IFilterVisitor<TEntity> visitor)
    //    {
    //        visitor.Visit(this);
    //    }
    //    public override Expression<Func<TEntity, bool>> Accept<TEntity, TResult>(IFilterVisitor<TEntity, Expression<Func<TEntity, bool>>> visitor)
    //    {
    //        return visitor.Visit(this);
    //    }

    //    private Expression<Func<TEntity,bool>> _expression;

    //    public Expression<Func<TEntity,bool>> Expression
    //    {
    //        get { return _expression; }
    //        set { _expression = value; }
    //    }


        

    //    [DataMember]
    //    public string EntityName { get; set; }

    //    [DataMember]
    //    public string PropertyName { get; set; }

    //    [DataMember]
    //    public FilterOperator Operator { get; set; }

    //    protected object _value;
    //    [DataMember]
    //    public object Value { get { return _value; } set { _value = value; OnPropertyChanged("Value"); } }

    //    public override int GetHashCode()
    //    {
    //        int hash = 37;
    //        hash = hash + 19 * (string.IsNullOrEmpty(PropertyName) ? 0 : PropertyName.GetHashCode());
    //        hash = hash + 19 * Operator.GetHashCode();
    //        return hash;
    //    }

    //    public override bool Equals(object obj)
    //    {
    //        var other = obj as Filter<TEntity>;
    //        if (other == null) return false;

    //        bool equals = true;
    //        equals = equals && PropertyName == other.PropertyName;
    //        equals = equals && Operator == other.Operator;
    //        return equals;
    //    }

    //    public string OperatorAsString
    //    {
    //        get
    //        {
    //            switch (Operator)
    //            {
    //                case FilterOperator.Equals: return "=";
    //                case FilterOperator.GreaterThan: return ">";
    //                case FilterOperator.LessThan: return "<";
    //                case FilterOperator.NotEquals: return "!=";
    //                //case FilterOperator.Contains: return "LIKE";
    //                case FilterOperator.Contains: return "==";
    //            }
    //            return string.Empty;
    //        }
    //    }


    //    public object Clone()
    //    {
    //        return new Filter<TEntity>() { EntityName = this.EntityName, PropertyName = this.PropertyName, Operator = this.Operator, Value = this.Value };
    //    }

    //    public event PropertyChangedEventHandler PropertyChanged;
    //    protected void OnPropertyChanged(string propertyName)
    //    {
    //        if (PropertyChanged != null)
    //            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    //    }
    //}


    [DataContract(IsReference = true)]
    public abstract class AbstractFilter
    {
        public virtual void Accept<TEntity>(IFilterVisitor<TEntity> visitor)
        {
            visitor.Visit(this);
        }
        public virtual Expression<Func<TEntity, bool>> Accept<TEntity, TResult>(IFilterVisitor<TEntity, Expression<Func<TEntity, bool>>> visitor)
        {
            return visitor.Visit(this);
        }

        #region Operators

        public static AbstractFilter operator &(AbstractFilter leftSideSpecification, AbstractFilter rightSideSpecification)
        {
            return new AndFilter(leftSideSpecification, rightSideSpecification);
        }
        public static AbstractFilter operator |(AbstractFilter leftSideSpecification, AbstractFilter rightSideSpecification)
        {
            return new OrFilter(leftSideSpecification, rightSideSpecification);
        }
        public static AbstractFilter operator !(AbstractFilter specification)
        {
            return new NotFilter(specification);
        }
        public static bool operator false(AbstractFilter specification)
        {
            return false;
        }
        public static bool operator true(AbstractFilter specification)
        {
            return true;
        }

        #endregion
    }

    [DataContract(IsReference = true)]
    public class CompositeFilter : AbstractFilter
    {
        [DataMember]
        public AbstractFilter RightFilter { get; set; }

        [DataMember]
        public AbstractFilter LeftFilter { get; set; }

        public CompositeFilter(AbstractFilter left, AbstractFilter right) { LeftFilter = left; RightFilter = right; }


        public override void Accept<TEntity>(IFilterVisitor<TEntity> visitor)
        {
            visitor.Visit(this);
        }
        public override Expression<Func<TEntity, bool>> Accept<TEntity, TResult>(IFilterVisitor<TEntity, Expression<Func<TEntity, bool>>> visitor)
        {
            //throw new NotImplementedException();
            return visitor.Visit(this);
        }
    }

    [DataContract(IsReference = true)]
    public class AndFilter : CompositeFilter
    {
        public AndFilter(AbstractFilter left, AbstractFilter right) : base(left, right) { }

        public override void Accept<TEntity>(IFilterVisitor<TEntity> visitor)
        {
            visitor.Visit(this);
        }
        public override Expression<Func<TEntity, bool>> Accept<TEntity, TResult>(IFilterVisitor<TEntity, Expression<Func<TEntity, bool>>> visitor)
        {
            return visitor.Visit(this);
        }

    }

    [DataContract(IsReference = true)]
    public class NotFilter : AbstractFilter
    {
        [DataMember]
        public AbstractFilter Body { get; set; }

        public NotFilter(AbstractFilter body) { Body = body; }


        public override void Accept<TEntity>(IFilterVisitor<TEntity> visitor)
        {
            visitor.Visit(this);
        }
        public override Expression<Func<TEntity, bool>> Accept<TEntity, TResult>(IFilterVisitor<TEntity, Expression<Func<TEntity, bool>>> visitor)
        {
            return visitor.Visit(this);
        }

    }

    [DataContract(IsReference = true)]
    public class OrFilter : CompositeFilter
    {
        public OrFilter(AbstractFilter left, AbstractFilter right) : base(left, right) { }

        public override void Accept<TEntity>(IFilterVisitor<TEntity> visitor)
        {
            visitor.Visit(this);
        }
        public override Expression<Func<TEntity, bool>> Accept<TEntity, TResult>(IFilterVisitor<TEntity, Expression<Func<TEntity, bool>>> visitor)
        {
            return visitor.Visit(this);
        }


    }


    [DataContract(IsReference = true)]
    [KnownType(typeof(FilterOperator))]
    public class DataBindableFilter : AbstractFilter, INotifyPropertyChanged
    {
        public DataBindableFilter()
        {
            IsVisible = true;
        }

        [DataMember]
        public Type PropertyType { get; set; }

        [DataMember]
        public string DisplayName { get; set; }

        protected object _value;
        [DataMember]
        public object Value { get { return _value; } set { _value = value; OnPropertyChanged("Value"); } }

        public bool IsVisible { get; set; }


        public override void Accept<TEntity>(IFilterVisitor<TEntity> visitor)
        {
            visitor.Visit(this);
        }
        public override Expression<Func<TEntity, bool>> Accept<TEntity, TResult>(IFilterVisitor<TEntity, Expression<Func<TEntity, bool>>> visitor)
        {
            return visitor.Visit(this);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }

   

    [DataContract(IsReference=true)]
    [KnownType(typeof(FilterOperator))]
    public class ExpressionDataBindableFilter : DataBindableFilter, INotifyPropertyChanged
    {
        [DataMember]
        public string PropertyName { get; set; }
        [DataMember]
        public FilterOperator Operator { get; set; }
        [DataMember]
        public bool IsCaseSensitive { get; set; }

        public Expression Expression { get; set; }

        //public Type ValueType { get; set; }

        private IEnumerable<object> _values;
        public IEnumerable<object> Values
        {
            get { return _values; }
            set { _values = value; OnPropertyChanged("Values"); }
        }

        public override void Accept<TEntity>(IFilterVisitor<TEntity> visitor)
        {
            visitor.Visit(this);
        }
        public override Expression<Func<TEntity, bool>> Accept<TEntity, TResult>(IFilterVisitor<TEntity, Expression<Func<TEntity, bool>>> visitor)
        {
            return visitor.Visit(this);
        }

    }

    public class AdapterExpressionFilter : AbstractFilter
    {
        public DataBindableFilter ValueFilter { get; set; }
        public Func<object, System.Linq.Expressions.Expression> CreateExpression;

        public override void Accept<TEntity>(IFilterVisitor<TEntity> visitor)
        {
            visitor.Visit(this);
        }
        public override Expression<Func<TEntity, bool>> Accept<TEntity, TResult>(IFilterVisitor<TEntity, Expression<Func<TEntity, bool>>> visitor)
        {
            return visitor.Visit(this);
        }
    }


    public class ExpressionFilter : AbstractFilter
    {
        public System.Linq.Expressions.Expression Predicate { get; set; }

        public static ExpressionFilter BuildFilter<TEntity>(Expression<Func<TEntity, bool>> predicate)
        {
            var filter = new ExpressionFilter();
            filter.Predicate = predicate;
            return filter;
        }

        public override void Accept<TEntity>(IFilterVisitor<TEntity> visitor)
        {
            visitor.Visit(this);
        }
        public override Expression<Func<TEntity, bool>> Accept<TEntity, TResult>(IFilterVisitor<TEntity, Expression<Func<TEntity, bool>>> visitor)
        {
            return visitor.Visit(this);
        }
    }



    public class FilterExpressionVisitor : ExpressionVisitor
    {
        private string PropertyName = "";
        public Type PropertyType { get; protected set; }

        public string GetPropertyName(System.Linq.Expressions.Expression expression)
        {
            Visit(expression);
            return PropertyName.Trim('.');
        }

        protected override System.Linq.Expressions.Expression VisitMember(MemberExpression node)
        {
            PropertyName = node.Member.Name + "." + PropertyName;
            return Visit(node.Expression);
        }
    }

}
