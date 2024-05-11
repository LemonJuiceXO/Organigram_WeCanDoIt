using Org.Domains.NodeTypes;
using Org.Domains.Persons;
using Org.Domains.Shared;

namespace Org.Apps;

public interface IRoleService
{
    ValueTask<List<Role>> GetRoles();

    ValueTask<Role?> GetRoleById(Guid roleRoleId);

    ValueTask<Result> CreateRole(NodeRole role);
}