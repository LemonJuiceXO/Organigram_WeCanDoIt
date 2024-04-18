using Microsoft.AspNetCore.Components;

namespace Main.Pages
{

    public partial class Login
    {
        private string Username { get; set; }
        private string Password { get; set; }
        private string ErrorMessage { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        private void OnSubmit()
        {
            if (Username == "user" && Password == "password")
            {
                NavigationManager.NavigateTo("/CreateNode");
            }
            else
            {
                ErrorMessage = "Invalid username or password.";
            }
        }
    }
}
