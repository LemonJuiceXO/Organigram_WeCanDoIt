using System.Data;
using System.Security.Claims;
using System.Transactions;
using Domains.User;
using IAGE.Shared;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Org.Storages.UserStorage;

public interface IUserStorage
{
    public Task<string> SelectUserPassword(string userId);
    public Task<User> SelectedUserByEmail(string userEmail);

    public Task<bool> insertNewUser(string userEmail, string userHashedPassword);


  

    Task<Guid> selectUserIdFromEmail(string userEmail);

    Task<bool> insertAdmin(User admin);
    Task<bool> confirmUserRegistration(Guid userId, string userHashedPassword);

  
}

public class UserStorage :IUserStorage
{
    private readonly string connectionString;
    
    public UserStorage(IConfiguration configuration)
    {
        connectionString = configuration.GetConnectionString("ORGDB");
    }
    
    private readonly string selectUserPasswordQuery = "select UserPassword from dbo.users where UserEmail=@aUserEmail";
    
    private readonly string selectUserbyEmailQuery = "select UserId , UserState from dbo.users where UserEmail=@aUserEmail";

    private readonly string insertNewUserQuery =
        "insert into dbo.users (UserId,UserEmail,UserPassword,UserState) values (@aUserId,@aUserEmail," +
        "@aUserPassword,@aUserState)";

   

    private readonly string selectUserIdQuery = "select UserId from dbo.Users where UserEmail=@aUserEmail";

    private readonly string insertAdminQuery = "insert into dbo.Users (UserId,UserEmail,UserPassword,UserState) values " +
                                               "((@aUserId),@aUserEmail,@aUserPassword,@aUSerState)";

    private readonly string confirmUserQuery =
        "update dbo.Users set UserPassword=@aUserPassword , UserState=@aUserState where " +
        "Userid=@aUserId";


    public async Task<string> SelectUserPassword(string userEmail)
    {
        string userHashedPassword;
        
        using (SqlConnection connection=new SqlConnection(connectionString))
        {
            SqlCommand selectUserPasswordCommand = new SqlCommand(selectUserPasswordQuery,connection);
            selectUserPasswordCommand.Parameters.AddWithValue("@aUserEmail", userEmail);
            await connection.OpenAsync();
            
            DataTable userPassword = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(selectUserPasswordCommand);
            adapter.Fill(userPassword);
           userHashedPassword= userPassword.Rows[0]["UserPassword"].AsString();
           
        }
       
        return userHashedPassword;
    }

    public async Task<User> SelectedUserByEmail(string userEmail)
    {
        User user=null;
        
        using (SqlConnection connection=new SqlConnection(connectionString))
        {
            SqlCommand selectUserByEmailCommand = new SqlCommand(selectUserbyEmailQuery,connection);
            selectUserByEmailCommand.Parameters.AddWithValue("@aUserEmail", userEmail);
            await connection.OpenAsync();
            
            
            DataTable userInfoTable = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(selectUserByEmailCommand);
         
           
            adapter.Fill(userInfoTable);
            
            if (userInfoTable.Rows.Count != 0)
            {
               
               var userRow = userInfoTable.Rows[0];
               user = User.createUser(userRow["UserId"].AsGuid(), userEmail, (User.UserState)userRow["UserState"].AsInt());
    
            }
        }

        return user;
    }

    public async Task<bool> insertNewUser(string userEmail, string userHashedPassword)
    {
        using (SqlConnection connection=new SqlConnection(connectionString))
        {
            SqlCommand insertNewUserCommand = new SqlCommand(insertNewUserQuery, connection);
            insertNewUserCommand.Parameters.AddWithValue("@aUserId", Guid.NewGuid().ToString());
            insertNewUserCommand.Parameters.AddWithValue("@aUserEmail", userEmail);
            insertNewUserCommand.Parameters.AddWithValue("@aUserPassword", userHashedPassword);
            insertNewUserCommand.Parameters.AddWithValue("@aUserState", User.UserState.CanLogin);
            await connection.OpenAsync();

           int operationSuccess= await insertNewUserCommand.ExecuteNonQueryAsync();
           if (operationSuccess > 0)
           {
               return true;
           }
        }

        return false;
    }
    
    

    public async Task<Guid> selectUserIdFromEmail(string userEmail)
    {
        Guid userId=Guid.Empty;
        
        using (SqlConnection connection=new SqlConnection(connectionString))
        {
            SqlCommand selectUserIdCommand = new SqlCommand(selectUserIdQuery,connection);
            selectUserIdCommand.Parameters.AddWithValue("@aUserEmail", userEmail);
            DataTable userTable = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(selectUserIdCommand);
            adapter.Fill(userTable);

            if (userTable.Rows.Count != 0)
            {
                userId = userTable.Rows[0]["UserID"].AsGuid();
            }
            
        }

        return userId;
    }

    public async Task<bool> insertAdmin(User admin)
    {
        
        using (SqlConnection connection=new SqlConnection(connectionString))
        {
            SqlCommand insertAdmin = new SqlCommand(insertAdminQuery,connection);
            insertAdmin.Parameters.AddWithValue("@aUserId", admin.UserId);
            insertAdmin.Parameters.AddWithValue("@aUserEmail", admin.UserEmail);
            insertAdmin.Parameters.AddWithValue("@aUserPassword", "NOT DEFINED");
            insertAdmin.Parameters.AddWithValue("@aUserState", admin.userState);
            await connection.OpenAsync();
            var effectedRows= await insertAdmin.ExecuteNonQueryAsync()>0;
            await connection.CloseAsync();
            
            return effectedRows;
            
        }
    }

    public async Task<bool> confirmUserRegistration(Guid userId, string userHashedPassword)
    {
        using (SqlConnection connection=new SqlConnection(connectionString))
        {
            SqlCommand confirmUserCommand = new SqlCommand(confirmUserQuery, connection);
            confirmUserCommand.Parameters.AddWithValue("@aUserPassword", userHashedPassword);
            confirmUserCommand.Parameters.AddWithValue("@aUserState", User.UserState.CanLogin);
            confirmUserCommand.Parameters.AddWithValue("@aUserId", userId);
            await connection.OpenAsync();
            return await confirmUserCommand.ExecuteNonQueryAsync() > 0;


        }
    }

  
}