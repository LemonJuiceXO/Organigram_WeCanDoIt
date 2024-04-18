using Org.Domains.Shared;

namespace Org.Domains.Persons;

public static class RoleErrors
{
    public static ErrorCode RoleAlreadyExists => new("RoleError.AlreadyExists", "Role Already Exists");
}