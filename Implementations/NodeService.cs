using Microsoft.Extensions.Configuration;
using Org.Apps;
using Org.Domains.Nodes;
using Org.Storages;

namespace Org.Impl;

public class NodeService : INodeService
{
    private readonly NodeStorage nodeStorage;

    public NodeService(
        IConfiguration configuration
    )
    {
        var connectionString = configuration.GetConnectionString("ORGDB");
        nodeStorage = new NodeStorage(connectionString);
    }

    public async Task CreateNode(Node node)
    {
        await nodeStorage.InsertNode(node);
    }

    public async Task AddPersonToNode(Guid nodeId, NodePerson person)
    {
        await nodeStorage.AddPersonToNode(nodeId, person);
    }
}