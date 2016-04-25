using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Newtonsoft.Json;

namespace marklogic.net.Linq
{
    public class MlQueryProvider : QueryProvider
    {
        readonly MarkLogicConnection _connection;
        private readonly string _collection;

        public MlQueryProvider(MarkLogicConnection connection, string collection)
        {
            _connection = connection;
            _collection = collection;
        }

        public override string GetQueryText(Expression expression)
        {
            return Translate(expression);
        }

        public override object Execute(Expression expression)
        {
            var session = _connection.OpenSession();

            var result = session.QueryString(Translate(expression));
            //            var result = session.QueryString("var result = [];result.push(fn.doc('brrrr.json'));result");

            var elementType = TypeSystem.GetElementType(expression.Type);

            var listElementType = typeof(List<>).MakeGenericType(elementType);

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
            return new QueryTranslator().Translate(expression, _collection);
        }
    }
}