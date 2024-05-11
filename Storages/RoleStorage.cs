using System.Data;
using IAGE.Shared;
using Microsoft.Data.SqlClient;
using Org.Domains.NodeTypes;
using Org.Domains.Persons;

namespace Org.Storages;

public class RoleStorage
{
    private const string insertRoleCommand =
        "INSERT dbo.ROLES (RoleId,RoleCode,RoleName) VALUES(@aRoleId, @aRoleCode, @aRoleName)";

    private const string selectRolesQuery = "SELECT * FROM ROLES";

    private const string selectRoleByIdQuery = "SELECT * FROM ROLES WHERE RoleId = @aRoleId";

    private const string selectRoleCountByCodeQuery =
        "SELECT COUNT(*) FROM ROLES WHERE upper(RoleCode) = upper(@aRoleCode)";

    private readonly string connectionString;

    public RoleStorage(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public async ValueTask<bool> InsertRole(NodeRole role)
    {
        try
        {
            await using var connection = new SqlConnection(connectionString);
            SqlCommand cmd = new(insertRoleCommand, connection);

            cmd.Parameters.AddWithValue("@aRoleId", role.RoleId);
            cmd.Parameters.AddWithValue("@aRoleCode", role.RoleCode);
            cmd.Parameters.AddWithValue("@aRoleName", role.RoleName);

            connection.Open();

            var insertedRows = cmd.ExecuteNonQuery();
            return insertedRows != 0;
        }
        catch (Exception e)
        {
            throw new Exception(e.AsString());
        }
    }

    public async ValueTask<List<Role>> SelectRoles()
    {
        await using var connection = new SqlConnection(connectionString);
        SqlCommand cmd = new(selectRolesQuery, connection);
        SqlDataAdapter da = new(cmd);
        connection.Open();
        DataTable dt = new();
        da.Fill(dt);

        var roles = new List<Role>();
        foreach (DataRow row in dt.Rows)
            roles.Add(Role.Create(
                row["RoleId"].AsGuid(),
                row["RoleCode"].AsString(),
                row["RoleName"].AsString()));

        return roles;
    }

    public async ValueTask<Role?> SelectRoleById(Guid roleId)
    {
        await using var connection = new SqlConnection(connectionString);
        SqlCommand cmd = new(selectRoleByIdQuery, connection);
        cmd.Parameters.AddWithValue("@aRoleId", roleId);
        SqlDataAdapter da = new(cmd);
        connection.Open();
        DataTable dt = new();
        da.Fill(dt);

        return dt.Rows.Count == 0
            ? null
            : createRoleFromRow(dt.Rows[0]);
    }

    private static Role createRoleFromRow(DataRow row)
    {
        return Role.Create(
            row["RoleId"].AsGuid(),
            row["RoleCode"].AsString(),
            row["RoleName"].AsString());
    }

    public async Task<bool> RoleCodeExists(string roleCode)

    {
        await using var connection = new SqlConnection(connectionString);
        SqlCommand cmd = new(selectRoleCountByCodeQuery, connection);
        cmd.Parameters.AddWithValue("@aRoleCode", roleCode);
        SqlDataAdapter da = new(cmd);
        connection.Open();

        var result = (int)await cmd.ExecuteScalarAsync();
        return result > 0;
    }
}