using System.Collections.Generic;
using NUnit.Framework;

namespace marklogic.net.tests
{
    [TestFixture]
    public class PermissionBuilderTests
    {
        [Test]
        public void BuildEmptyPermissions()
        {
            var permissions = new List<Permission>() {Permission.Default};

            var result = PermissionBuilder.CreatePermissionsTable(permissions);

            Assert.AreEqual(result, "xdmp.defaultPermissions()");
        }
    }
}
