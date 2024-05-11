using Microsoft.AspNetCore.Components;
using Org.Domains.Nodes;
using Org.Domains.NodeTypes;

namespace Components.CreateNode;

partial class CreateNodeTypeForm
{
    
    [Parameter] public EventCallback close { get; set; }
    [Parameter] public EventCallback<NodeType> createNode { get; set; }
    [Parameter] public int nodeTypesCount { get; set; }
    private NodeType createdNodeType = NodeType.Create(Guid.NewGuid());
    
    
    private async Task SubmitNewNodeType()
    {
        if (nodeTypesCount == 0)
        {
            createdNodeType.isRoot = true;
        }
        
        await createNode.InvokeAsync(createdNodeType);
        
        createdNodeType = NodeType.Create(Guid.NewGuid());
    }

    private async Task closeForm()
    {
        await close.InvokeAsync();
    }

    

}