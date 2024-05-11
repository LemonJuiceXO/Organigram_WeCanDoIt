using Microsoft.AspNetCore.Components;

namespace Components.SharedComponents;

partial class LoadingComponent
{
    [Parameter] public bool isLoading { get; set; }
    [Parameter] public bool navigateButton { get; set; }
    [Parameter] public EventCallback navigateTo { get; set; }

    private async Task navigateToNextPage()
    {
      await  navigateTo.InvokeAsync();
    }
}