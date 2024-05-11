using System.Security.Claims;
using Domains.User;
using Org.Apps.Users;
using Org.Domains.Persons;
using Org.Storages.UserStorage;

namespace Implementations.Users;

public class UserRolesService:IUserRolesService
{
    private readonly IUserRolesStorage userRolesStorage;
    
    public UserRolesService(IUserRolesStorage userRolesStorage)
    {
    
        this.userRolesStorage = userRolesStorage;
    }
    
    public async Task<List<Claim>> getUserClaims(Guid userId)
    {
        return await userRolesStorage.selectUserRoles(userId);
    }

    public async Task<bool> createUserRole(Guid userId, User.UserRole userRole)
    {
       return await userRolesStorage.insertUserRole(userId, userRole);
    }

    public async Task<Guid> getRoleIdByRoleName(User.UserRole RoleName)
    {
        return await userRolesStorage.selectRoleIdFromRoleName(RoleName);
    }
}