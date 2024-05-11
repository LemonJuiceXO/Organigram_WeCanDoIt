using System.Drawing;
using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Org.Domains.Nodes;

namespace Components.CreateNode;

partial class SubNodeLayout
{
    [Parameter] public Node subNode { get; set; }
    [Parameter] public List<Node> nodes { get; set; }
    
    [Parameter] public EventCallback<List<Node>> nodesChanged { get; set; }
    
    [Inject] private IJSRuntime js { get; set; }
    private bool Menu = false;
    private Node parent=Node.Create(Guid.Empty);
    
    private List<Node> potentialParent = new ();
    
    private async Task showMenu()
    {
        await getPotentialNodes();
        Claim c = new Claim("aze","aze");
        Menu = !Menu;
        
    }
    

    protected override void OnInitialized()
    {
        getPotentialNodes();
    }

   async Task getPotentialNodes()
    {
        potentialParent.Clear();
        
        if (nodes != null)
        {
            foreach (var node in nodes)
            {
                if (node != subNode)
                {
                    potentialParent.Add(node);  
                }
             
            }
        }
    }
    
}