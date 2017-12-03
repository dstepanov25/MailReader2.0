using System.Collections.Generic;
using System.Threading.Tasks;
using Email.Svc.Models;

namespace Email.Svc.Services.Settings {

    public interface ISettingsService {
        Task<IEnumerable<MailAccount>> GetEmailAccounts();

        Task<IEnumerable<string>> GetPublicEmailHosts();

    }

}