using System.Collections.Generic;
using NUnit.Framework;

namespace marklogic.net.tests
{
    [TestFixture]
    public class CollectionBuilderTests
    {
        [Test]
        public void BuildEmptyCollections()
        {
            var collections = new List<string>();

            var result = CollectionBuilder.CreateCollectionsTable(collections);

            Assert.AreEqual(result, "xdmp.defaultCollections()");
        }
        [Test]
        public void BuildSimpleCollections()
        {
            var collections = new List<string>() { "one", "two", "three" };

            var result = CollectionBuilder.CreateCollectionsTable(collections);

            Assert.AreEqual(result, "[\"one\",\"two\",\"three\"]");
        }

        [Test]
        public void BuildOneElementCollections()
        {
            var collections = new List<string>() { "one"};

            var result = CollectionBuilder.CreateCollectionsTable(collections);

            Assert.AreEqual(result, "[\"one\"]");
        }
    }

}
