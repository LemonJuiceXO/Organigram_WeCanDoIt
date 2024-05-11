using System.Net;
using System.Net.Mail;
using FluentEmail.Core.Defaults;
using FluentEmail.Smtp;
using Org.Apps.Email;

namespace Org.Impl.Email;

public class EmailService :IEmailService
{
    private SmtpClient getSmtpClient()
    {
        SmtpClient smtpClient = new SmtpClient()
        {
            Host = "smtp.gmail.com",
            Port = 587,
            EnableSsl = true,
            UseDefaultCredentials = false,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            Credentials = new NetworkCredential("haithemkahoul1@gmail.com","eeoo riep xhci sdmi")
        };
        FluentEmail.Core.Email.DefaultSender = new SmtpSender(smtpClient);
        
        return smtpClient;
    }
    
    
    public async Task SendEmail(string from, string to, string subject, string body)
    {

         getSmtpClient();
        
      
     await   FluentEmail.Core.Email.From("haithemkahoul1@gmail.com",from).To(to).Subject(subject)
            .Body(body,true)
            .SendAsync();

    }

    public async Task SendCompleteRegistrationEmail(string to, string userId)
    {
        getSmtpClient();
        
      
        
        string URL = "http://localhost:5014/FinishAdminRegistration/"+userId+"/"+Guid.NewGuid();

        string body =
            "<!DOCTYPE html>\n<html lang=\"ar\">\n<head>\n    <meta charset=\"UTF-8\">\n    <link rel=\"preconnect\" href=\"https://fonts.googleapis.com\">\n    <link rel=\"preconnect\" href=\"https://fonts.gstatic.com\" crossorigin>\n    <link href=\"https://fonts.googleapis.com/css2?family=Open+Sans:ital,wght@0,300..800;1,300..800&family=Platypi:ital,wght@0,300..800;1,300..800&family=Roboto+Condensed:ital,wght@0,100..900;1,100..900&family=Roboto:ital,wght@0,100;0,300;0,400;0,500;0,700;0,900;1,100;1,300;1,400;1,500;1,700;1,900&family=Rubik:ital,wght@0,300..900;1,300..900&family=Sedgwick+Ave+Display&display=swap\" rel=\"stylesheet\">\n<style>\n*{\n    font-family: \"Roboto\", sans-serif;\n    font-weight: 100;\n    font-style: normal;\n    box-sizing: border-box;\n}\n\nbody{\n    position: relative;\n}\n.TemplateContainer{\n    text-align: center;\n    padding: 2rem;\n    background-color: rgb(86, 20, 157);\n    border-radius: 1rem;\n    width: fit-content;\n    height: fit-content;\n    margin: auto;\n}\n.templateTitle{\n    color: #11bd75;\n    font-weight: bold;\n    font-size: 2rem;\n    display: block;\n    position: relative;\n}\n.templateSpeach{\n    font-size: 1rem;\n    display: block;\n    position: relative;\n    color: white;\n}\n.templateButton{\n    display: block;\n    background-color: #11bd75;\n    border-radius: 5px;\n    padding: 1rem;\n    font-size: 1.5rem;\n    width: fit-content;\n    margin: auto;\n    border: none;\n    margin-top: 2rem;\n    color: white;\n    text-decoration: none;\n}\n</style>\n</head>\n<body>\n<div class=\"TemplateContainer\">\n   <h1 class=\"templateTitle\">مرحبا بك في اورغانيغرام</h1>\n   <span class=\"templateSpeach\">يسعدنا انضمامك معنا لاكمال تسجيلك يرجى الضغط على الرابط ادناه</span>\n    <a class=\"templateButton\" href="+URL+">تأكيد</a>\n</div>\n</body>\n</html>";
        
        await   FluentEmail.Core.Email.From("haithemkahoul1@gmail.com","Organigram@univ-biskra.dz").To(to).Subject("اكمال التسجيل")
            .Body(body,true)
            .SendAsync();
    }
}