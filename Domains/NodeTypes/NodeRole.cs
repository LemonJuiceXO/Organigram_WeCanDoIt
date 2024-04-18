namespace Org.Domains.NodeTypes;

public class NodeRole : IOccurence
{
    public NodeRole(Guid roleId, int minValue, int maxValue)
    {
        RoleId = roleId;
        MinValue = minValue;
        MaxValue = maxValue;
    }

    private NodeRole()
    {
        RoleId = Guid.NewGuid();
        MinValue = 0;
        MaxValue = 1;
    }

    private NodeRole(Guid roleId,string roleName , string roleCode, int minValue, int maxValue)
    {
        RoleId = roleId;
        MinValue = minValue;
        MaxValue = maxValue;
        RoleName = roleName;
        RoleCode = roleCode;
    }

    public Guid RoleId { get; set; }
    public string RoleName { get; set; }
    public string RoleCode { get; set; }
    public int MinValue { get; set; }
    public int MaxValue { get; set; }

    public static NodeRole Create(Guid roleId, int min, int max)
    {
        return new NodeRole(roleId, min, max);
    }
    
    public static NodeRole Create(Guid roleId,string roleName,string roleCode ,int min, int max)
    {
        return new NodeRole(roleId,roleName,roleCode, min, max);
    }


    public static NodeRole Create()
    {
        return new NodeRole();
    }
}