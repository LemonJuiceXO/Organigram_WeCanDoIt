

using Org.Domains.Nodes;

namespace Org.Apps;

public interface INodeService
{
    Task CreateNode(Node node);

    Task AddPersonToNode(Guid nodeId, NodePerson person);

    Task AddSubNodeToNode(Guid nodeId, Node subNode);
}