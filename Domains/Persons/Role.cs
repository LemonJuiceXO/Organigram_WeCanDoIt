namespace Org.Domains.Persons;

public record Role
{
    private Role()
    {
        RoleId = Guid.NewGuid();
    }

    private Role(Guid roleId, string roleCode, string roleName) : this()
    {
        RoleId = roleId;
        RoleCode = roleCode;
        RoleName = roleName;
    }

    public Guid RoleId { get; set; }
    public string RoleCode { get; set; } = null!;
    public string RoleName { get; set; } = null!;

    public static Role Create(Guid roleId, string roleCode, string roleName)
    {
        return new Role(roleId, roleCode, roleName);
    }

    public static Role Create(string roleCode, string roleName)
    {
        return new Role(Guid.NewGuid(), roleCode, roleName);
    }

    public static Role Create()
    {
        return new Role();
    }
}