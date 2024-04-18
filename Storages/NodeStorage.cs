using Microsoft.Data.SqlClient;
using Org.Domains.Nodes;

namespace Org.Storages;

public class NodeStorage
{
    private const string insertNodeCommand = "INSERT dbo.NODES VALUES(@aId, @aTypeId, @aCode, @aName)";
    private const string insertNodePersonCommand = "INSERT dbo.POSTES VALUES(@aNodeId, @aPersonId, @aRoleId)";
    private readonly string connectionString;

    public NodeStorage(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public async ValueTask<bool> InsertNode(Node node)
    {
        await using var connection = new SqlConnection(connectionString);
        SqlCommand cmd = new(insertNodeCommand, connection);

        cmd.Parameters.AddWithValue("@aId", node.NodeId);
        cmd.Parameters.AddWithValue("@aTypeId", node.TypeId);
        cmd.Parameters.AddWithValue("@aName", node.Name);
        cmd.Parameters.AddWithValue("@aCode", node.Code);

        await connection.OpenAsync();
        var insertedRows = await cmd.ExecuteNonQueryAsync();

        return insertedRows != 0;
    }

    public async ValueTask<bool> AddPersonToNode(Guid nodeId, NodePerson person)
    {
        await using var connection = new SqlConnection(connectionString);
        SqlCommand cmd = new(insertNodePersonCommand, connection);

        cmd.Parameters.AddWithValue("@aNodeId", nodeId);
        cmd.Parameters.AddWithValue("@aPersonId", person.PersonId);
        cmd.Parameters.AddWithValue("@aRoleId", person.RoleId);

        await connection.OpenAsync();
        var insertedRows = await cmd.ExecuteNonQueryAsync();

        return insertedRows != 0;
    }
}