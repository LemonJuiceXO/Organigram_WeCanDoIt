using System.Transactions;
using Microsoft.Data.SqlClient;
using Org.Domains.Nodes;

namespace Org.Storages;

public class NodeStorage
{
    private const string insertNodeCommand = "INSERT dbo.NODES VALUES(@aId, @aTypeId, @aCode, @aName)";
    private const string insertNodePersonCommand = "INSERT dbo.POSTES VALUES(@aNodeId, @aPersonId, @aRoleId)";
    private const string linkSubNodeCommand = "insert dbo.PARENTS Values(@aNodeId,@aParentId)";
    private readonly string connectionString;

    public NodeStorage(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public async ValueTask<bool> InsertNode(Node node)
    {
        using (TransactionScope transactionScope=new TransactionScope(TransactionScopeOption.Required))
        {
            await using var connection = new SqlConnection(connectionString);
            SqlCommand cmd = new(insertNodeCommand, connection);

            cmd.Parameters.AddWithValue("@aId", node.NodeId);
            cmd.Parameters.AddWithValue("@aTypeId", node.TypeId);
            cmd.Parameters.AddWithValue("@aName", node.Name);
            cmd.Parameters.AddWithValue("@aCode", node.Code);
        
            await connection.OpenAsync();
            
            foreach (Node subNode in node.SubNodes)
            {
               await InsertSubNode(node.NodeId, subNode);
            }

            foreach (NodePerson person in node.Persons)
            {
                await AddPersonToNode(node.NodeId, person);
            }
            
            var insertedRows = await cmd.ExecuteNonQueryAsync();
            
            transactionScope.Dispose();
            return insertedRows != 0;
            
            
        }
        
        
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

    public async Task InsertSubNode(Guid nodeId, Node subNode)
    {
        
            await InsertNode(subNode);
            
            using (SqlConnection connection=new SqlConnection(connectionString))
            {
                SqlCommand linkSubNode = new SqlCommand(linkSubNodeCommand, connection);
                linkSubNode.Parameters.AddWithValue("@aNodeId", subNode.NodeId);
                linkSubNode.Parameters.AddWithValue("@aParent", nodeId);
               await connection.OpenAsync();
               
               await linkSubNode.ExecuteNonQueryAsync();
            } 
        
       
    }
}