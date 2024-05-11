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
    
    
    [Parameter] public Func<NodePerson,Task<int>> addPerson { set; get; }
    
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
        NodeRole targetRole = roles.FirstOrDefault(role => role.RoleId.CompareTo(nodePerson.RoleId) == 0);
        nodePerson.RoleName = targetRole.RoleName;
        
        int occurances= await addPerson.Invoke(nodePerson);
      
        verifyRoleOccurances(targetRole,occurances);
       
        nodePerson = new();
        
    }

    private void verifyRoleOccurances(NodeRole role,int occurances)
    {
        if (occurances >= role.MaxValue || role.MaxValue == 1)
        {
            roles.Remove(role);
        }
            
        
    }
    private async Task close()
    {
       await closeForm.InvokeAsync();
    }
}