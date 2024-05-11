using System.Data;
using System.Data.Common;
using System.Security.Claims;
using Domains.User;
using IAGE.Shared;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Org.Storages.UserStorage;

public  interface IUserRolesStorage
{
    Task<List<Claim>> selectUserRoles(Guid userId);
    Task<bool> insertUserRole(Guid userId, User.UserRole userRole);
    Task<Guid> selectRoleIdFromRoleName(User.UserRole roleName);
}
public class UserRolesStorage:IUserRolesStorage
{
    private readonly string connectionString;
    
    public UserRolesStorage(IConfiguration configuration)
    {
        connectionString = configuration.GetConnectionString("ORGDB");
    }
    
    
    private readonly string selectUserRolesQuery =
        "select u.UserId,u.UserEmail,ar.RoleName from dbo.Users as u " +
        "left join dbo.UserRoles as ur on ur.UserId=u.UserId left join dbo.AvailableRoles as ar " +
        "on ar.RoleId=ur.RoleId where u.UserId=@aUserId";
    
    private readonly string insertUserRoleQuery = "dbo.insertUserRole";
    
    private readonly string selectRoleIdFromRoleNameQuery = "select RoleId from dbo.AvailableRoles where " +
                                                            "RoleName=@aRoleName";
    
    public async Task<List<Claim>> selectUserRoles(Guid userId)
    {
        List<Claim> userClaims = new();
        
        
        using (SqlConnection connection=new SqlConnection(connectionString))
        {
            SqlCommand selectUserRolesCommand = new SqlCommand(selectUserRolesQuery,connection);
            selectUserRolesCommand.Parameters.AddWithValue("@aUserId", userId.ToString());
            await connection.OpenAsync();
            
            DataTable userClaimsTable = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(selectUserRolesCommand);
            adapter.Fill(userClaimsTable);

            foreach (DataRow userClaimRow in userClaimsTable.Rows)
            {
                var userRole= userClaimRow["RoleName"].AsString();
               
               
                userClaims.Add(new Claim(ClaimTypes.Role,userRole));
            }
            
        }

        return userClaims;
    }
    
    public async Task<bool> insertUserRole(Guid userId, User.UserRole userRole)
    {
       
        
        using (SqlConnection connection =new SqlConnection(connectionString))
        {
            SqlCommand insertUserRoleCommand = new SqlCommand(insertUserRoleQuery, connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            
            insertUserRoleCommand.Parameters.AddWithValue("@UserId", userId);
            insertUserRoleCommand.Parameters.AddWithValue("@RoleName", userRole.ToString());
            await connection.OpenAsync();
            
            var effectedRows= await insertUserRoleCommand.ExecuteNonQueryAsync()>0;
            await connection.CloseAsync();
            
            return effectedRows;

        }
    }

  

    public async Task<Guid> selectRoleIdFromRoleName(User.UserRole roleName)
    {
        using (SqlConnection connection=new SqlConnection(connectionString))
        {
            SqlCommand selectRoleIdFromRoleNameCommand = new SqlCommand(selectRoleIdFromRoleNameQuery, connection);
            selectRoleIdFromRoleNameCommand.Parameters.AddWithValue("@aRoleName", roleName);
            await connection.OpenAsync();
            
            DataTable roleIdTable = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.Fill(roleIdTable);

            if (roleIdTable.Rows.Count != 0)
            {
                return roleIdTable.Rows[0]["RoleId"].AsGuid();
            } 
            
            return Guid.Empty;
        }
    }
}