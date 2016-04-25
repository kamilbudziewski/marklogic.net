using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace marklogic.net.Linq
{
    internal class QueryTranslator : ExpressionVisitor
    {
        StringBuilder _sb;

        internal QueryTranslator()
        {
        }

        internal string Translate(Expression expression)
        {
            this._sb = new StringBuilder();
            this.Visit(expression);
            return this._sb.ToString();
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
                _sb.Append("SELECT * FROM(");
                this.Visit(m.Arguments[0]);
                _sb.Append(") AS T WHERE ");
                var lambda = (LambdaExpression)StripQuotes(m.Arguments[1]);
                this.Visit(lambda.Body);
                return m;
            }
            throw new NotSupportedException(string.Format("The method '{ 0 }’ is not supported", m.Method.Name));
        }

        protected override Expression VisitUnary(UnaryExpression u)
        {
            switch (u.NodeType)
            {
                case ExpressionType.Not:
                    _sb.Append(" NOT ");
                    Visit(u.Operand);
                    break;
                default:
                    throw new NotSupportedException(string.Format("The unary operator '{ 0 }’ is not supported", u.NodeType));
            }
            return u;
        }

        protected override Expression VisitBinary(BinaryExpression b)
        {
            _sb.Append("(");
            this.Visit(b.Left);

            switch (b.NodeType)
            {
                case ExpressionType.And:
                    break;
                case ExpressionType.Or:
                    _sb.Append(" OR");
                    break;
                case ExpressionType.Equal:
                    _sb.Append(" = ");
                    break;
                case ExpressionType.NotEqual:
                    _sb.Append(" <> ");
                    break;
                case ExpressionType.LessThan:
                    _sb.Append(" < ");
                    break;
                case ExpressionType.LessThanOrEqual:
                    _sb.Append(" <= ");
                    break;
                case ExpressionType.GreaterThan:
                    _sb.Append(" > ");
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    _sb.Append(" >= ");
                    break;
                default:
                    throw new NotSupportedException(string.Format("The binary operator '{ 0 }’ is not supported", b.NodeType));
            }
            this.Visit(b.Right);
            _sb.Append(")");
            return b;

        }

        protected override Expression VisitConstant(ConstantExpression c)
        {
            IQueryable q = c.Value as IQueryable;
            if (q != null)
            {
                // assume constant nodes w/ IQueryables are table references
                _sb.Append("SELECT * FROM ");
                _sb.Append(q.ElementType.Name);
            }
            else if (c.Value == null)
            {
                _sb.Append("NULL");
            }
            else
            {
                switch (Type.GetTypeCode(c.Value.GetType()))
                {
                    case TypeCode.Boolean:
                        _sb.Append(((bool)c.Value) ? 1 : 0);
                        break;
                    case TypeCode.String:
                        _sb.Append("'");
                        _sb.Append(c.Value);
                        _sb.Append("'");
                        break;
                    case TypeCode.Object:
                        throw new NotSupportedException(string.Format("The constant for '{ 0}’ is not supported", c.Value));
                    default:
                        _sb.Append(c.Value);
                        break;
                }
            }
            return c;
        }

        protected override Expression VisitMember(MemberExpression m)
        {
            if (m.Expression != null && m.Expression.NodeType == ExpressionType.Parameter)
            {
                _sb.Append(m.Member.Name);
                return m;
            }

            throw new NotSupportedException(string.Format("The member ‘{ 0 }’ is not supported", m.Member.Name));
        }
    }

    internal class ObjectReader<T> : IEnumerable<T>, IEnumerable where T : class, new()
    {
        Enumerator _enumerator;

        internal ObjectReader(List<T> elements)
        {
            _enumerator = new Enumerator(elements);
        }

        public IEnumerator<T> GetEnumerator()
        {
            var e = this._enumerator;
            if (e == null)
            {
                throw new InvalidOperationException("Cannot enumerate more than once");
            }

            _enumerator = null;
            return e;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        class Enumerator : IEnumerator<T>, IEnumerator, IDisposable
        {
            private List<T>.Enumerator _enumerable;

            internal Enumerator(List<T> elements)
            {
                _enumerable = elements.GetEnumerator();
            }

            public T Current
            {
                get { return _enumerable.Current; }

            }

            object IEnumerator.Current
            {
                get { return _enumerable.Current; }
            }

            public bool MoveNext()
            {
                return _enumerable.MoveNext();
            }

            public void Reset()
            {
            }

            public void Dispose()
            {
                _enumerable.Dispose();
            }
        }
    }

    public class MlQueryProvider : QueryProvider
    {
        readonly MarkLogicConnection _connection;

        public MlQueryProvider(MarkLogicConnection connection)
        {
            _connection = connection;
        }

        public override string GetQueryText(Expression expression)
        {
            return Translate(expression);
        }

        public override object Execute(Expression expression)
        {
            var cmd = this._connection.OpenSession();

            var result = cmd.QueryString(Translate(expression));
//            var result = cmd.QueryString("var result = [];result.push(fn.doc('brrrr.json'));result");

            var elementType = TypeSystem.GetElementType(expression.Type);

            var listElementType = typeof (List<>).MakeGenericType(elementType);

            return Activator.CreateInstance(

                typeof(ObjectReader<>).MakeGenericType(elementType),

                BindingFlags.Instance | BindingFlags.NonPublic, null,

                new object[]
                {
                    JsonConvert.DeserializeObject(result.StringResult, listElementType)
                },

                null);
        }

        private string Translate(Expression expression)
        {
            return new QueryTranslator().Translate(expression);
        }
    }
}
