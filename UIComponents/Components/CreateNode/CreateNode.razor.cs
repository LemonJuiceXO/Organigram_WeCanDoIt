using IAGE.Shared;
using Microsoft.AspNetCore.Components;
using Org.Apps;
using Org.Domains.Nodes;
using Org.Domains.NodeTypes;

namespace Components.CreateNode;

partial class CreateNode
{
   [Inject] private IOrgTypeService OrgTypeService { get; set; }
   [Inject] private INodeService NodeService { get; set; }
   private List<NodeType> NodeTypes;
   private NodeType selectedNodeType;
   private bool showAddPersonToNode=false;
   private bool showAddSubNode=false;
   private Node nodeToCreate=new();
   
   private async Task selectNodeType(ChangeEventArgs selected)
   {
      Guid nodeTypeId = selected.Value.AsGuid();
      nodeToCreate.TypeId = nodeTypeId;
      selectedNodeType = NodeTypes.FirstOrDefault(type => type.Id.CompareTo(nodeTypeId)==0 );
   }
   
   protected override void OnInitialized()
   {
      NodeTypes = OrgTypeService.GetNodeTypes().Result;
      selectedNodeType = NodeTypes[0];
   }

 
   private async Task<int> addPersonToNode(NodePerson person)
   {
      nodeToCreate.Persons.Add(person);
      StateHasChanged();
      return nodeToCreate.Persons.Count(per => per.RoleId.CompareTo(person.RoleId) == 0);
   }

   private async Task<int> addSubNodeToNode(Node subNode)
   {
      nodeToCreate.SubNodes.Add(subNode);
      StateHasChanged();
      return nodeToCreate.SubNodes.Count(node => node.NodeId == subNode.NodeId);
   }

   private async Task createNode()
   {
      await NodeService.CreateNode(nodeToCreate);
   }
}