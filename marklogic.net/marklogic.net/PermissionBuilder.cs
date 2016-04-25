using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace marklogic.net
{
    public static class PermissionBuilder
    {
        public static string CreatePermissionsTable(List<Permission> permissions)
        {
            if (!permissions.Any() || permissions.Any(x => x == Permission.Default))
            {
                return "xdmp.defaultPermissions()";
            }

            var sb = new StringBuilder("[");
            foreach (var permission in permissions)
            {
                sb.Append(String.Format("xdmp.permission('{0}', '{1}'),", permission.Role, permission.Capability));
            }
            var result = sb.ToString().TrimEnd(',') + "]";

            return result;
        }
    }
}