using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Org.Domains.NodeTypes;

namespace Components.CreateNode;

partial class NodeTypeLayout
{
  [Parameter] public  NodeType nodeType { get; set; }
  [Parameter] public List<NodeType> availableNodes { get; set; }
  
  [Parameter] public int nodeNumber { get; set; }
  [Inject] public IJSRuntime JsRuntime { get; set; }

  private bool isAddPersonVisible=false;
  private bool isAddNodeTypeParent=false;


  

  

  private async Task showAddPerson()
  {
      isAddPersonVisible = true;
      
  }

  private async Task addNodeRoleToNode(NodeRole nodeRole)
  {
      nodeType.Roles.Add(nodeRole);
      Console.WriteLine(nodeType.Roles.Count);
  }
    
  private async Task showLinkToParent()
  {
      isAddNodeTypeParent = true;
  }

  private async Task linkNodeToParent(NodeChild nodeChild)
  {
      nodeType.SubNodes.Add(nodeChild);
      await JsRuntime.InvokeVoidAsync("linkNodes",nodeChild.NodeTypeId.ToString(),nodeType.Id.ToString());
  }
}