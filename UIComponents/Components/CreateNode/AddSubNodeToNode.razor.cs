using Microsoft.AspNetCore.Components;
using Org.Domains.Nodes;
using Org.Domains.NodeTypes;

namespace Components.CreateNode;

partial class AddSubNodeToNode
{
    [Parameter] public List<NodeChild> SubNodes { get; set; }
    [Parameter] public Func<Node, Task<int>> addSubToNode { get; set; }

    [Parameter] public EventCallback closeAddSubNode { get; set; }
    
    private Node subNode=new ();
    
    private async Task addSubNodeToNode()
    {
     int count= await addSubToNode.Invoke(subNode);
     NodeChild target = SubNodes.FirstOrDefault(child => child.NodeTypeId.CompareTo(subNode.TypeId) == 0);
     
     await verifySubNodeCount(count,target);
    }

    private async Task verifySubNodeCount(int occurances, NodeChild targetNode)
    {
        if (occurances >= targetNode.MaxValue || targetNode.MaxValue == 1)
        {
            SubNodes.Remove(targetNode);
        }
    }
    
    private async Task close()
    {
        await closeAddSubNode.InvokeAsync();
    }
}