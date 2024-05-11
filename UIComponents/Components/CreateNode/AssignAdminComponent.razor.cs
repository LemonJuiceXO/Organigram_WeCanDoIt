using Domains.User;
using Microsoft.AspNetCore.Components;
using Org.Apps.Email;
using Org.Apps.Users;

namespace Components.CreateNode;

partial class AssignAdminComponent
{
    [Inject] private IEmailService emailService { get; set; }
    [Inject] private IUserService userService { get; set; }
    private bool isLoading = false,isDialogShown=false;
    private List<Exception> exceptionList = new();

    private string firstEmail, confirmedEmail;
    
    private async Task submitSendEmail()
    {
       isLoading = true;
       isDialogShown = true;
       
      await Task.Run(() =>
       { 
           verifyEmail();
          bool isSuccefful= sendEmail().Result;
          
          if (!isSuccefful)
          {
              isDialogShown = false;
          }
       });
       isLoading = false;
       
    }

    private async Task verifyEmail()
    {
        if (!firstEmail.Equals(confirmedEmail))
        {
           exceptionList.Add( new Exception("الرجاء التأكد من الايمايل"));
           isDialogShown = false;
           throw exceptionList[0];
        }
        
    }

    private async Task<bool> sendEmail()
    {
      var user=await userService.createNewAdmin(firstEmail);
      if (user != null)
      {
          await emailService.SendCompleteRegistrationEmail(firstEmail, user.UserId.ToString()); 
      }

      return true;
    }

   
}