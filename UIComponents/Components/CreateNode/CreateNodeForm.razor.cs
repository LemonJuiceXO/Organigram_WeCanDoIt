using Microsoft.AspNetCore.Components;
using Org.Apps;
using Org.Domains.Nodes;
using Org.Domains.NodeTypes;

namespace Components.CreateNode;

partial class CreateNodeForm
{

    [Parameter] public List<NodeType> NodeTypes { get; set; }
    [Parameter] public EventCallback close { get; set; }
    
    private Node nodeToCreate=Node.Create(Guid.NewGuid());
    [Parameter] public EventCallback<Node> createNode { get; set; }
   
    
    private async Task SubmitNewNode()
    {
       await createNode.InvokeAsync(nodeToCreate);
       nodeToCreate = new();
    }

    private async Task closeForm()
    {
       await close.InvokeAsync();
    }
    
    
}