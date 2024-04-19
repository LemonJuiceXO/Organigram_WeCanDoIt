using IAGE.Shared;
using Microsoft.AspNetCore.Components;
using Org.Apps;
using Org.Domains.Nodes;
using Org.Domains.NodeTypes;

namespace Components.CreateNode;

partial class CreateNode
{
   [Inject] private IOrgTypeService OrgTypeService { get; set; }
   private List<NodeType> NodeTypes;
   private NodeType selectedNodeType;
   private bool showAddPersonToNode=false;
   private List<NodePerson> NodePersons = new();
   private async Task selectNodeType(ChangeEventArgs selected)
   {
      Guid nodeTypeId = selected.Value.AsGuid();
      selectedNodeType = NodeTypes.FirstOrDefault(type => type.Id.CompareTo(nodeTypeId)==0 );
   }
   
   protected override void OnInitialized()
   {
      NodeTypes = OrgTypeService.GetNodeTypes().Result;
      selectedNodeType = NodeTypes[0];
   }
   
   private async Task addPersonToNode(NodePerson person)
   {
      NodePersons.Add(person);
      StateHasChanged();
   }
   
}