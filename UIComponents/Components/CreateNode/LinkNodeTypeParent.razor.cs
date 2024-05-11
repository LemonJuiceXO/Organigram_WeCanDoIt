using Microsoft.AspNetCore.Components;
using Org.Domains.NodeTypes;

namespace Components.CreateNode;

partial class LinkNodeTypeParent
{
    [Parameter] public List<NodeType> availableNodes { get; set; }
    [Parameter] public EventCallback closeLinkNodeParent { get; set; }
    [Parameter] public EventCallback<NodeChild> linkNodeType { get; set; }
    [Parameter] public Guid nodeTypeId { get; set; }

    private NodeChild nodeChild=new ();
    
    
    private List<NodeType> targetNodeTypes;
    
    protected override async Task OnInitializedAsync()
    {
        await filterNodeTypes();
    }
    
    private async Task filterNodeTypes()
    {
        targetNodeTypes = availableNodes.FindAll(type => type.Id.CompareTo(nodeTypeId)!=0);
    }
    
    private NodeType selectedNodeType=new ();
    
    private async Task LinkNodeType()
    {
       await linkNodeType.InvokeAsync(nodeChild);
    }
    
}