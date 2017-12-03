using System.Threading.Tasks;
using Email.Svc.Services.Mailbox;
using Email.Svc.Services.Settings;
using Email.Svc.Services.Suppliers;
using Quartz;

namespace Email.Svc.Services.Scheduling {

    public class ReadEmailsJob : IJob {
        private readonly IMailService _mailService;
        private readonly ISettingsService _settingsService;
        private readonly ISupplierService _supplierService;

        public ReadEmailsJob(IMailService mailService, ISettingsService settingsService, ISupplierService supplierService) {
            _mailService = mailService;
            _settingsService = settingsService;
            _supplierService = supplierService;
        }

        public async Task Execute(IJobExecutionContext context) {
            var emailAccounts = await _settingsService.GetEmailAccounts();
            foreach (var emailAccount in emailAccounts) {
                var messages = _mailService.ReadMessages(emailAccount);

                foreach (var message in messages) {
                    var supplier = await _supplierService.GetSupplier(message.From.ToLower());

                    // todo save mail attachments into special folder
                }
            }
        }

        
    }

}