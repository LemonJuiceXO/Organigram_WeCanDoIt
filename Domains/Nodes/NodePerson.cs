namespace Org.Domains.Nodes;

public class NodePerson
{
    public Guid PersonId { get; set; } = Guid.NewGuid();
    public Guid RoleId { get; set; }
    public string Nom { get; set; }
    public string RoleName { get; set; }
}