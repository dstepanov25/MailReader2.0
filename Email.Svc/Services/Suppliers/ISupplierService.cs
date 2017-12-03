using System.Threading.Tasks;
using Email.Svc.Models;

namespace Email.Svc.Services.Suppliers {

    public interface ISupplierService {
        Task<Supplier> GetSupplier(string address);

        Task<Supplier> NewSupplier(string address, string mailHost);
    }

}