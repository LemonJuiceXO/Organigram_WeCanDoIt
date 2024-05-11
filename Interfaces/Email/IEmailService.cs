namespace Org.Apps.Email;

public interface IEmailService
{
    public  Task SendEmail(String from ,String to , String subject ,String body);
    public Task SendCompleteRegistrationEmail(string to, string userId);

}