using System;
using System.Collections;
using System.Collections.Generic;

namespace marklogic.net.Linq
{
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
}