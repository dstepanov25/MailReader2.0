using System.Threading.Tasks;
using Email.EntityFramework.Models;

namespace Email.Svc.Services.Suppliers {

    public interface ISupplierService {
        Task<Supplier> GetSupplier(string address);

        Task<PriceList> GetPriceList(string fileName);

    }

 }