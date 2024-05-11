using Microsoft.AspNetCore.Components;
using Org.Apps;
using Org.Domains.NodeTypes;

namespace Components.CreateNode;

partial class CreateNodeTypes
{
    [Inject] private IOrgTypeService orgTypeService { get; set; }
    [Inject] private NavigationManager navigationManager { get; set; }
    
    private bool showCreateNodeType=false,isSuccessDialogShown=false,isLoading=false;
    
    private List<NodeType> nodeTypes=new ();
    private int nodeNumber=0;
    
    protected override async Task OnInitializedAsync()
    {
        nodeNumber = 0;
    }
    
    private async Task closeAddNodeType()
    {
        showCreateNodeType = false;
    }

    private async Task addNodeTypeToList(NodeType nodeType)
    {
        nodeTypes.Add(nodeType);
    }

    private async Task saveNodeTypes()
    {
        isLoading = true;
        isSuccessDialogShown = true;

        await Task.Run(() =>
        {
            foreach (var nodeType in nodeTypes)
            {
                orgTypeService.CreateNodeType(nodeType);
            }

        });

        showSuccessDialog();




    }

    private async Task showSuccessDialog()
    {
        
        isLoading = false;
        
    }
    private async Task navigateToNextPage()
    {
        navigationManager.NavigateTo("/AssignAdmin");
    }
}
