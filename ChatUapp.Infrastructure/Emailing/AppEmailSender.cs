using ChatUapp.Core.Interfaces.Emailing;
using System.Net.Mail;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Emailing;
using Volo.Abp.MultiTenancy;

namespace ChatUapp.Infrastructure.Emailing
{
    public class AppEmailSender : EmailSenderBase, IAppEmailSender
    {
        public AppEmailSender(
            ICurrentTenant currentTenant,
            IEmailSenderConfiguration configuration,
            IBackgroundJobManager backgroundJobManager
            ) : base(
                currentTenant,
                configuration,
                backgroundJobManager)
        {
        }

        protected override Task SendEmailAsync(MailMessage mail)
        {
            throw new NotImplementedException();
        }
    }
}
