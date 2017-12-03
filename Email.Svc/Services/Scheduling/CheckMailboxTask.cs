using System.Threading;
using System.Threading.Tasks;
using Email.Svc.Services.Mailbox;
using Email.Svc.Services.Settings;

namespace Email.Svc.Services.Scheduling {

    public class CheckMailboxTask : IScheduledTask {
        private readonly IMailService _mailService;
        private readonly ISettingsService _settingsService;

        public CheckMailboxTask(IMailService mailService, ISettingsService settingsService) {
            _mailService = mailService;
            _settingsService = settingsService;
        }

        public string Schedule => "0 20 * * * *";

        public async Task ExecuteAsync(CancellationToken cancellationToken) {
            var emailAccounts = await _settingsService.GetEmailAccounts();
            foreach (var emailAccount in emailAccounts) {
                await _mailService.GetMessagesAsync(emailAccount);
            }
        }
    }

}