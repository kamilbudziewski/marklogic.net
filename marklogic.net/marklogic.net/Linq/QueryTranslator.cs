using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;

namespace marklogic.net.Linq
{
    internal class QueryTranslator : ExpressionVisitor
    {
        private MarkLogicQuery _query;
        private string _name;
        private string _value;
        private string _operand;

        internal string Translate(Expression expression, string collection)
        {
            _query = new MarkLogicQuery() { Collection = collection, Filters = new List<Filter>() };
            Visit(expression);
            return _query.ToString();
        }

        private static Expression StripQuotes(Expression e)
        {
            while (e.NodeType == ExpressionType.Quote)
            {
                e = ((UnaryExpression)e).Operand;
            }
            return e;
        }

        protected override Expression VisitMethodCall(MethodCallExpression m)
        {
            if (m.Method.DeclaringType == typeof(Queryable) && m.Method.Name == "Where")
            {
                Visit(m.Arguments[0]);
                var lambda = (LambdaExpression)StripQuotes(m.Arguments[1]);
                Visit(lambda.Body);
                return m;
            }
            throw new NotSupportedException(string.Format("The method '{ 0 }’ is not supported", m.Method.Name));
        }

        protected override Expression VisitUnary(UnaryExpression u)
        {
            switch (u.NodeType)
            {
                case ExpressionType.Not:
//                    _sb.Append(" NOT ");
                    Visit(u.Operand);
                    break;
                default:
                    throw new NotSupportedException(string.Format("The unary operator '{ 0 }’ is not supported", u.NodeType));
            }
            return u;
        }

        protected override Expression VisitBinary(BinaryExpression b)
        {
            //TODO: parsowac do struktury gdzie bedzie drzewko wyrazen i potem do stringa
            Visit(b.Left);

            switch (b.NodeType)
            {
                case ExpressionType.And:
                    break;
                case ExpressionType.Or:
//                    _sb.Append(" OR");
                    break;
                case ExpressionType.OrElse: // ||

                    break;
                case ExpressionType.Equal:
                    _operand = "=";
                    break;
                case ExpressionType.NotEqual:
                    break;
                case ExpressionType.LessThan:
                    break;
                case ExpressionType.LessThanOrEqual:
                    break;
                case ExpressionType.GreaterThan:
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    break;
                default:
                    throw new NotSupportedException(string.Format("The binary operator '{ 0 }’ is not supported", b.NodeType));
            }

            Visit(b.Right);

            _query.Filters.Add(new Filter(_name, new[] { _value }));

            return b;

        }

        protected override Expression VisitConstant(ConstantExpression c)
        {
            IQueryable q = c.Value as IQueryable;
            if (q != null)
            {
                // assume constant nodes w/ IQueryables are table references
                //                _sb.Append("SELECT * FROM ");
                //                _sb.Append(q.ElementType.Name);
            }
            else if (c.Value == null)
            {
//                _sb.Append("NULL");
            }
            else
            {
                switch (Type.GetTypeCode(c.Value.GetType()))
                {
                    case TypeCode.Boolean:
                        _value = c.Value.ToString();
                        break;
                    case TypeCode.String:
                        _value = c.Value.ToString();
                        break;
                    case TypeCode.Object:
                        throw new NotSupportedException(string.Format("The constant for '{ 0}’ is not supported", c.Value));
                    default:
                        _value = c.Value.ToString();
                        break;
                }
            }
            return c;
        }

        protected override Expression VisitMember(MemberExpression m)
        {
            if (m.Expression != null && m.Expression.NodeType == ExpressionType.Parameter)
            {
                _name = m.Member.Name;
                return m;
            }

            throw new NotSupportedException(string.Format("The member ‘{ 0 }’ is not supported", m.Member.Name));
        }
    }

    internal class MarkLogicQuery
    {
        public List<Filter> Filters { get; set; }
        public string Collection { get; set; }
        public override string ToString()
        {
            var format = "var result = []; for(var i of cts.search(cts.andQuery([{0}]))) result.push(i); result";

            var filters = new List<string>();
            foreach (var filter in Filters)
            {
                var sb = new StringBuilder();
                if (filter.Operator == Operator.And)
                {
                    sb.Append("cts.andQuery([");
                }
                if (filter.Operator == Operator.Or)
                {
                    sb.Append("cts.orQuery([");
                }

                foreach (var value in filter.Value)
                {
                    sb.AppendFormat("cts.jsonPropertyValueQuery('{0}', '{1}')", filter.Name, value);
                }
                sb.Append("])");
                filters.Add(sb.ToString());
            }
            return string.Format(format,
                string.Join(",", filters));
        }
    }

    internal class Filter
    {
        public Filter(string name, string[] value)
        {
            Name = name;
            Value = value;
            Operator = Operator.Or;
        }

        public Filter(string name, string[] value, Operator @operator)
        {
            Name = name;
            Value = value;
            Operator = @operator;
        }

        public string Name { get; set; }
        public string[] Value { get; set; }
        public Operator Operator { get; set; }
    }

    internal enum Operator
    {
        Or, And
    }
}
