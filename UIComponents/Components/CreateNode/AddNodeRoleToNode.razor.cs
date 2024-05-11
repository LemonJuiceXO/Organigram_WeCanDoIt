using Microsoft.AspNetCore.Components;
using Org.Domains.NodeTypes;

namespace Components.CreateNode;

partial class AddNodeRoleToNode
{
    private bool isSuccess=false;
    private NodeRole createdRole = NodeRole.Create();
    [Parameter] public EventCallback<NodeRole> submitNewRole { get; set; }
    [Parameter] public EventCallback closeAddNodeRoleToNode { get; set; }

    private async Task createNewRole()
    {
        createdRole.RoleId = Guid.NewGuid();
        await  submitNewRole.InvokeAsync(createdRole);
        createdRole = NodeRole.Create();
        isSuccess = true;

        await Task.Delay(1000);
        isSuccess = false;
    }
}