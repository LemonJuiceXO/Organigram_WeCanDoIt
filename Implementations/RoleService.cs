using Microsoft.Extensions.Configuration;
using Org.Apps;
using Org.Domains.NodeTypes;
using Org.Domains.Persons;
using Org.Domains.Shared;
using Org.Storages;

namespace Org.Impl;

public class RoleService : IRoleService
{
    private readonly RoleStorage roleStorage;

    public RoleService(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ORGDB");
        roleStorage = new RoleStorage(connectionString);
    }

    public async ValueTask<List<Role>> GetRoles()
    {
        return await roleStorage.SelectRoles();
    }

    public async ValueTask<Role?> GetRoleById(Guid roleId)
    {
        return await roleStorage.SelectRoleById(roleId);
    }

    public async ValueTask<Result> CreateRole(NodeRole role)
    {
        if (await roleStorage.RoleCodeExists(role.RoleCode))
            return Result.Failure(new List<ErrorCode>
            {
                RoleErrors.RoleAlreadyExists
            });

        await roleStorage.InsertRole(role);
        return Result.Succes;
    }
}