using System.Data;
using IAGE.Shared;
using Microsoft.Data.SqlClient;
using Org.Domains.Nodes;
using Org.Domains.NodeTypes;

namespace Org.Storages;

public class OrgTypeStorage
{
    private const string insertNodeTypeCommand = "INSERT orgTypes.NODES VALUES(@aId, @aCode, @aName)";

    private const string addRoleToNodeTypeCommand =
        "INSERT orgTypes.ROLES VALUES(@aNodeId, @aRoleId, @aMinValue, @aMaxValue)";

    private const string addSubNodeToNodeTypeCommand =
        "INSERT orgTypes.SUBNODES VALUES(@aNodeId, @aSubNodeId, @aMinValue, @aMaxValue)";

    private const string selectNodeTypeByIdQuery = "orgTypes.GetNodeType";

    private const string selectNodeTypeByCodeQuery = "SELECT * FROM OrgTypes.NODES WHERE Code = @aCode";

    private const string selectNodeTypesQuery = "getNodeTypes";
    private readonly string connectionString;

    public OrgTypeStorage(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public async ValueTask<bool> InsertNodeType(Guid id, string code, string name)
    {
        await using var connection = new SqlConnection(connectionString);
        SqlCommand cmd = new(insertNodeTypeCommand, connection);

        cmd.Parameters.AddWithValue("@aId", id);
        cmd.Parameters.AddWithValue("@aCode", code);
        cmd.Parameters.AddWithValue("@aName", name);

        await connection.OpenAsync();
        var insertedRows = await cmd.ExecuteNonQueryAsync();

        return insertedRows != 0;
    }

    public async ValueTask<bool> AddRoleToNodeType(Guid nodeId, NodeRole nodeRole)
    {
        await using var connection = new SqlConnection(connectionString);
        SqlCommand cmd = new(addRoleToNodeTypeCommand, connection);

        cmd.Parameters.AddWithValue("@aNodeId", nodeId);
        cmd.Parameters.AddWithValue("@aRoleId", nodeRole.RoleId);
        cmd.Parameters.AddWithValue("@aMinValue", nodeRole.MinValue);
        cmd.Parameters.AddWithValue("@aMaxValue", nodeRole.MaxValue);

        await connection.OpenAsync();
        var insertedRows = await cmd.ExecuteNonQueryAsync();

        return insertedRows != 0;
    }

    public async ValueTask<bool> AddSubNodeToNodeType(Guid nodeId, NodeChild nodeChild)
    {
        await using var connection = new SqlConnection(connectionString);
        SqlCommand cmd = new(addSubNodeToNodeTypeCommand, connection);

        cmd.Parameters.AddWithValue("@aNodeId", nodeId);
        cmd.Parameters.AddWithValue("@aSubNodeId", nodeChild.NodeTypeId);
        cmd.Parameters.AddWithValue("@aMinValue", nodeChild.MinValue);
        cmd.Parameters.AddWithValue("@aMaxValue", nodeChild.MaxValue);

        await connection.OpenAsync();
        var insertedRows = await cmd.ExecuteNonQueryAsync();

        return insertedRows != 0;
    }

    public async ValueTask<NodeType> SelectNodeTypeById(Guid id)
    {
        await using var connection = new SqlConnection(connectionString);
        SqlCommand cmd = new(selectNodeTypeByIdQuery, connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        cmd.Parameters.AddWithValue("@aNodeId", id);

        SqlDataAdapter da = new(cmd);
        connection.Open();
        DataSet ds = new();
        da.Fill(ds);

        if (ds.Tables[0].Rows.Count == 0)
            return null;

        return getFromDataSet(ds);
    }

    private NodeType getFromDataSet(DataSet dataset)
    {
        var result = new NodeType();
        var row = dataset.Tables[0].Rows[0];
        result = NodeType.Create(row["Id"].AsGuid(), row["Code"].AsString(), row["Name"].AsString());

        foreach (DataRow childRow in dataset.Tables[1].Rows)
            result.SubNodes.Add(new NodeChild
            {
                NodeTypeId = childRow["SubNodeId"].AsGuid(),
                MinValue = (int)childRow["MinValue"],
                MaxValue = (int)childRow["MaxValue"]
            });

        foreach (DataRow roleRow in dataset.Tables[2].Rows)
            result.Roles.Add(
                NodeRole.Create(roleRow["RoleId"].AsGuid(), (int)roleRow["MinValue"],
                    (int)roleRow["MaxValue"]));

        return result;
    }

    public async ValueTask<NodeType> SelectNodeTypeByCode(string code)
    {
        await using var connection = new SqlConnection(connectionString);
        SqlCommand cmd = new(selectNodeTypeByCodeQuery, connection);

        cmd.Parameters.AddWithValue("@aCode", code);

        SqlDataAdapter da = new(cmd);
        connection.Open();
        DataTable dt = new();
        da.Fill(dt);

        if (dt.Rows.Count == 0)
            return null;

        return getFromRow(dt.Rows[0]);
    }

    public async Task<List<NodeType>> SelectNodeTypes()
    {
        await using var connection = new SqlConnection(connectionString);
        SqlCommand cmd = new(selectNodeTypesQuery, connection);
        cmd.CommandType = CommandType.StoredProcedure;
        
        SqlDataAdapter da = new(cmd);
        connection.Open();
        DataSet ds = new();
        da.Fill(ds);


        if (ds.Tables.Count > 0)
        {
         return await getDataFromNodeTypeRow(ds);
        }

        return new List<NodeType>();
    }

    private async Task<List<NodeType>> getDataFromNodeTypeRow(DataSet dataSet)
    {
        List<NodeType> nodeTypes = new();
        
        foreach(DataRow row in dataSet.Tables[0].Rows)
        {
            nodeTypes.Add(new NodeType()
            {
                Id = row["Id"].AsGuid(),
                Name = row["Name"].ToString(),
                Code = row["Code"].ToString()
                
            });
            
        }

        foreach (DataRow row in dataSet.Tables[1].Rows)
        {
            Guid targetGuid = row["NodeId"].AsGuid();
            NodeType targetNode;
            
            if((targetNode=nodeTypes.FirstOrDefault(type => type.Id.CompareTo(targetGuid)==0))!=null)
            {
               var RoleId = row["RoleId"].AsGuid();
               var RoleName = row["RoleName"].ToString();
               var RoleCode = row["RoleCode"].ToString();
               var MinValue = row["MinValue"].AsInt();
               var MaxValue = row["MaxValue"].AsInt();
                
                targetNode.Roles.Add(NodeRole.Create(RoleId,RoleName,RoleCode,MinValue,MaxValue));
            }
        }

        foreach (DataRow row in dataSet.Tables[2].Rows)
        {
            Guid targetGuid = row["NodeId"].AsGuid();
            NodeType targetNode;
            
            if((targetNode=nodeTypes.FirstOrDefault(type => type.Id.CompareTo(targetGuid)==0))!=null)
            {
               targetNode.SubNodes.Add(new NodeChild()
               {
                   NodeTypeId = row["SubNodeId"].AsGuid(),
                   NodeTypeName = row["Name"].AsString(),
                   MinValue = row["MinValue"].AsInt(),
                   MaxValue = row["MaxValue"].AsInt()
               });
            }
        }
        
        
        {
            
        }
        return nodeTypes;
    }

    private NodeType getFromRow(DataRow row)
    {
        return NodeType.Create(row["Id"].AsGuid(), row["Code"].AsString(), row["Name"].AsString());
    }
}