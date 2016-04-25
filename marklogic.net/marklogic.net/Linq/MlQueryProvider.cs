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