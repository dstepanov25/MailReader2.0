using System.Linq;
using System.Threading.Tasks;
using Email.Svc.Models;
using Email.Svc.Services.Settings;

namespace Email.Svc.Services.Suppliers {

    public class SupplierService : ISupplierService {
        //private readonly EmailContext _emailContext;
        private readonly ISettingsService _settingsService;

        public SupplierService(/*EmailContext emailContext, */ISettingsService settingsService) {
            //_emailContext = emailContext;
            _settingsService = settingsService;
        }

        public async Task<Supplier> GetSupplier(string address) {
            throw new System.NotImplementedException();
            //var mailHost = address.Split('@')[1];
            //return _emailContext.Suppliers.FirstOrDefault(t => t.Emails.ToLower().Contains($"|{address}|")
            //                                                   || t.Emails.ToLower().Contains($"|*@{mailHost}|"))
            //       ?? await NewSupplier(address, mailHost);
        }


        public async Task<Supplier> NewSupplier(string address, string mailHost) {
            throw new System.NotImplementedException();
            //var publicEmailHosts = await _settingsService.GetPublicEmailHosts();

            //var emails = $"|{address}|";
            //var supplierName = address;
            //var supplierEmailHost = _emailContext.Suppliers.FirstOrDefault(t => t.Emails.Contains($"@{mailHost}|"));

            //if (!publicEmailHosts.Contains(mailHost) && supplierEmailHost == null) {
            //    emails += $"*@{mailHost}|";
            //    supplierName = mailHost;
            //}

            //var newSupplier = new Supplier {
            //    Contacts = address,
            //    Name = supplierName,
            //    Emails = emails,
            //    IsNew = true
            //};

            //_emailContext.Suppliers.Add(newSupplier);
            //_emailContext.SaveChanges();

            //return _emailContext.Suppliers.Where(t => t.Name == supplierName && t.IsNew).FirstOrDefault();
        }
    }

}