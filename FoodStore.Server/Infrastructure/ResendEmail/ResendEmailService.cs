using FoodStore.Server.Application.Common.Interfaces;
using Resend;

namespace FoodStore.Server.Infrastructure.ResendEmail;


public class ResendEmailService : IEmailService
{

    private readonly IResend _resend;

    public ResendEmailService(IResend resend)
    {
        _resend = resend;
    }

    public async Task SendEmailAsync(string to, string subject, string htmlContent)
    {

        var message = new EmailMessage();
        message.From = "Food Store <onboarding@resend.dev>";
        message.To.Add(to);
        message.Subject = subject;
        message.HtmlBody = htmlContent;

        await _resend.EmailSendAsync(message);

    }

}