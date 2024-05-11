using System.Security.Claims;
using IAGE.Shared;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Org.Apps.Email;
using Org.Apps.Users;

namespace Implementations.Users;
using Microsoft.AspNetCore.Mvc;

[Microsoft.AspNetCore.Mvc.Route("/Login")]
[ApiController]

public class LoginController :ControllerBase
{
    private IEmailService EmailService;
    private IUserService UserService;
    private readonly IUserRolesService userRolesService;
    
    public LoginController(IEmailService service ,IUserService userService,IUserRolesService userRolesService)
    {
        EmailService = service;
        this.UserService = userService;
        this.userRolesService = userRolesService;
    }
        [HttpPost]
        [Microsoft.AspNetCore.Mvc.Route("Login")]
        public async Task<IActionResult> LoginUser([FromForm] string userName,[FromForm] string userPassword)
        {
            var userState=await  UserService.LoginUser(userName, userPassword);
            
            if (userState == Domains.User.User.UserState.CanLogin)
            {
                Guid userId = await UserService.getUserIdByEmail(userName);
                var userClaims = await userRolesService.getUserClaims(userId);
                
                var identity = new ClaimsIdentity(userClaims,CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                var props = new AuthenticationProperties();
                
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,principal, props);
                
                
                if (userClaims.Find(claim => claim.Value.Equals("Manager"))!=null)
                {
                    return Redirect("/CreateOrganigram");
                }
                else if (userClaims.Find(claim => claim.Value.Equals("Admin")) != null)
                {
                    return Redirect("/CreateOrganigra");  
                }
                
                
            }
           
            



                 return Redirect("/");
        }
        
}