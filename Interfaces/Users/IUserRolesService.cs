using System.Security.Claims;
using Domains.User;

namespace Org.Apps.Users;

public interface IUserRolesService
{
    public Task<List<Claim>> getUserClaims(Guid userId);
    
    public Task<bool> createUserRole(Guid userId, User.UserRole userRole);

    public Task<Guid> getRoleIdByRoleName(User.UserRole RoleName);
}