using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;

namespace Lab7.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            //dummy email sender for testing
            return Task.CompletedTask;
        }
    }
}

