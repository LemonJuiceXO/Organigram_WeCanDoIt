using IAGE.Shared;
using Microsoft.AspNetCore.Components;
using Org.Domains.Nodes;
using Org.Domains.NodeTypes;
using Org.Domains.Persons;

namespace Components.CreateNode;

partial class AddPersonToNode
{
    [Parameter] public List<NodeRole> roles { get; set; }
    [Parameter] public EventCallback closeForm { get; set; }
    [Parameter] public EventCallback<NodePerson> addPerson { get; set; }
    
    private NodeRole selectedRole;
    
    private NodePerson nodePerson;
    
    protected override void OnInitialized()
    {
        selectedRole = roles[0];
        nodePerson=new();
    }
    
    
    private async Task addPersonToNode()
    {
        nodePerson.PersonId=Guid.NewGuid();
        nodePerson.RoleName = roles.FirstOrDefault(role => role.RoleId.CompareTo(nodePerson.RoleId) == 0).RoleName;
        addPerson.InvokeAsync(nodePerson);
        nodePerson = new();
    }
    
    private async Task close()
    {
       await closeForm.InvokeAsync();
    }
}