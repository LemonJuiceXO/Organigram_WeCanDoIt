namespace Domains.User;

public class User
{
    public Guid UserId { get; set; }
    public string UserEmail { get; set; }
   
    public UserState userState { get; set; }

    private User()
    {
        
    }
    private User(Guid userId , string userEmail , UserState userState) 
    {
        this.UserId = userId;
        this.UserEmail = userEmail;
        this.userState = userState;
    }
    

    public static User createUser(Guid userId, string userEmail, UserState userState)
    {
        return new User( userId,  userEmail,  userState);
    }

    public static User createUser()
    {
        return new User();
    }
    public enum UserState
    {
        CanLogin=1,
        Blocked=0,
        DoesntExist=-1,
        AwaitingConfirmation=2
    }
    
    public enum UserRole
    {
        Admin,
        NormalUser,
        Manager,
        
    }
}