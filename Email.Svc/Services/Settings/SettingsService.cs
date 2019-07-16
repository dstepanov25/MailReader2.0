using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Email.Svc.Services.Settings.Dto;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Email.Svc.Services.Settings {

    public class SettingsService : ISettingsService {
        private readonly IMongoDatabase _mongoDb;

        private const string MailSettingsCollection = "MailSettings";
        private const string ConnectSslFieldName = "ConnectSsl";
        private const string UserNameFieldName = "UserName";
        private const string PasswordFieldName = "Password";

        private const string PublicEmailHostsCollection = "PublicEmailHosts";
        private const string HostNameFieldName = "HostName";

        public SettingsService(IMongoDatabase mongoDb) {
            _mongoDb = mongoDb;
        }

        public async Task<IEnumerable<MailAccountDto>> GetEmailAccounts() {
            var response = await _mongoDb.GetCollection<BsonDocument>(MailSettingsCollection).FindAsync("{}");
            return response.ToList().Select(d => new MailAccountDto {
                ConnectSsl = d[ConnectSslFieldName].AsString,
                UserName = d[UserNameFieldName].AsString,
                Password = d[PasswordFieldName].AsString
            });
        }

        public async Task<IEnumerable<string>> GetPublicEmailHosts() {
            var response = await _mongoDb.GetCollection<BsonDocument>(PublicEmailHostsCollection).FindAsync("{}");
            return response.ToList().Select(d => d[HostNameFieldName].AsString);
        }

    }

}