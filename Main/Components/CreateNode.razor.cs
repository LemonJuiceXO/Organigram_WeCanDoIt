using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;

namespace Main.Components
{
    public partial class CreateNode
    {
        private Model Model { get; set; } = new Model();

        private async Task OnSubmit(NavigationManager navigationManager)
        {
            var isValid = await ValidateForm();
            if (!isValid)
                return;
            navigationManager.NavigateTo("/SuccessPage");
        }

        private async Task<bool> ValidateForm()
        {
            var validationContext = new ValidationContext(Model);
            var validationResults = new List<ValidationResult>();

            if (!Validator.TryValidateObject(Model, validationContext, validationResults, true))
            {
                // Handle validation errors
                // You can display the errors or handle them in any other way
                return false;
            }

            return true;
        }
    }
}
