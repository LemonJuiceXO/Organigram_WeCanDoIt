using System.Security.Claims;
using System.Transactions;
using DevOne.Security.Cryptography.BCrypt;
using Domains.User;
using Org.Apps.Users;
using Org.Storages.UserStorage;

namespace Implementations.Users;

public class UserService:IUserService
{
    private IUserStorage userStorage;
    private readonly IUserRolesService userRolesService;

    public UserService(IUserStorage userStorage,IUserRolesService userRolesService)
    {
        this.userStorage = userStorage;
        this.userRolesService = userRolesService;
    }
    
    public async Task<User.UserState> LoginUser(string userName, string userPassword)
    {
        
        bool isVerified = await verifyUserCredentials(userName, userPassword);
        
        if (isVerified)
        {
          var user=  await userStorage.SelectedUserByEmail(userName);
          if (user is null || user.userState==User.UserState.Blocked)
          {
              return User.UserState.DoesntExist;
          }

        bool isPaswordCorrect= await  verifyUserPassword(userName, userPassword);
        
        if (isPaswordCorrect)
        {
            return User.UserState.CanLogin;
        }

        }

        return User.UserState.DoesntExist;

    }

    public async Task<bool> verifyUserCredentials(string userName, string userPassword)
    {
        if (userName == null || userPassword == null)
        {
            throw new Exception("Incorrect Information");
            return false;
        }

        return true;
    }

    public async Task<User> getUserByEmail(string userEmail)
    {
        return await userStorage.SelectedUserByEmail(userEmail);
    }
    
    
    public async Task<bool> verifyUserPassword(string userEmail,string enteredPassword)
    {
        string userHashedPassword=await userStorage.SelectUserPassword(userEmail);
        return BCryptHelper.CheckPassword(enteredPassword, userHashedPassword);
    }

    public async Task<Guid> getUserIdByEmail(string userEmail)
    {
        return await userStorage.selectUserIdFromEmail(userEmail);
    }

    public async Task<bool> createNewUser(string userEmail, string userPassword)
    {
        string hashedPassword = BCryptHelper.HashPassword(userPassword,BCryptHelper.GenerateSalt(10));
        return await userStorage.insertNewUser(userEmail, hashedPassword);
    }

    public async Task<User> createNewAdmin(string adminEmail)
    {
        using (TransactionScope scope=new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            User admin = User.createUser(Guid.NewGuid(),adminEmail,User.UserState.AwaitingConfirmation);
            
            bool adminCreationSuccess=  await userStorage.insertAdmin(admin);
            
            bool roleCreationSuccess=  await userRolesService.createUserRole(admin.UserId, User.UserRole.Admin);
           
            if (adminCreationSuccess && roleCreationSuccess)
            {
                scope.Complete();
                
                return admin; 
               
            }
        }
        
        return null;
    }

    public async Task<bool> confirmUserRegistration(Guid userId, string userPassword)
    {
        string hashedPassword = BCryptHelper.HashPassword(userPassword,BCryptHelper.GenerateSalt(10));
        
        return await userStorage.confirmUserRegistration(userId,hashedPassword);
    }
}