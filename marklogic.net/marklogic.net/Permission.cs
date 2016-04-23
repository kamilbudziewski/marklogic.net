namespace marklogic.net
{
    public class Permission
    {
        public Permission(string role, Capability capability)
        {
            Role = role;
            Capability = capability;
        }

        public static Permission Default = new Permission("", Capability.read);
        public string Role { get; set; }
        public Capability Capability { get; set; }
    }
}