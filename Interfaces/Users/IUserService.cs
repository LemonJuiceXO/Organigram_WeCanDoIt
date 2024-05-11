using System.Security.Claims;
using Domains.User;

namespace Org.Apps.Users;

public interface IUserService
{
    public Task<User.UserState> LoginUser(string userName, string userPassword);
    public Task<bool> verifyUserCredentials(string userName, string userPassword);
    public Task<User> getUserByEmail(string userEmail);
    
    public Task<bool> verifyUserPassword(string userEmail,string enteredPassword);
    public Task<Guid> getUserIdByEmail(string userEmail);
    public Task<bool> createNewUser(string userEmail, string userPassword);
    
    public Task<User> createNewAdmin(string adminEmail);

    Task<bool> confirmUserRegistration(Guid userId,string userPassword);
}