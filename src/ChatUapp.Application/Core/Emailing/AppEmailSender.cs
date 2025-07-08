using ChatUapp.Emailing.Interfaces;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Emailing.Smtp;
using Volo.Abp.MultiTenancy;

namespace ChatUapp.Core.Emailing
{
    public class AppEmailSender : SmtpEmailSender, IAppEmailSender
    {
        public AppEmailSender(ICurrentTenant currentTenant,
            ISmtpEmailSenderConfiguration smtpConfiguration,
            IBackgroundJobManager backgroundJobManager)
            : base(currentTenant, smtpConfiguration, backgroundJobManager)
        {


        }
    }
}
