using Domains.User;
using IAGE.Shared;
using Microsoft.AspNetCore.Components;
using Org.Apps.Users;

namespace Components.Login;

partial class CompleteAdminRegistrationComponent
{
    [Inject] private IUserService userService { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    
    [Parameter] public string adminID { get; set; }
    private bool isLoading = false,isDialogShown=false;
    private string error;
    private string firstPassword,secondPassword;
    private async Task confirmRegistration()
    {
        isLoading = true;
        isDialogShown = true;
        
     bool taskResult=  await Task.Run(() =>
     
        {
           bool isSame= verifyPassword(firstPassword,secondPassword).Result;
           if (isSame)
           {
             bool isSuccessful=  userService.confirmUserRegistration(Guid.Parse(adminID),firstPassword).Result;
             
             if (isSuccessful)
             {
                 return true;
             }
             
           }
            
          
               return false;
        });
     
        isLoading = false;
        if (!taskResult)
        {
            isDialogShown = false;
        }
    }

    private async Task<bool> verifyPassword(string pass1 ,string pass2)
    {
        if (!pass1.Equals(pass2))
        {
            error = "الرجاء التأكد من صحة كلمة المرور";
    
            return false;
        }

        return true;
    }

    private async Task navigateToLogin()
    {
        NavigationManager.NavigateTo("/");
    }
}