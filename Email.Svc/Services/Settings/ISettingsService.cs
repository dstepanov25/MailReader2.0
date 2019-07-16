using System.Collections.Generic;
using System.Threading.Tasks;
using Email.Svc.Services.Settings.Dto;

namespace Email.Svc.Services.Settings {

    public interface ISettingsService {
        Task<IEnumerable<MailAccountDto>> GetEmailAccounts();

        Task<IEnumerable<string>> GetPublicEmailHosts();

    }

}