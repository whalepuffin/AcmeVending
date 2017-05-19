using AcmeVending.Repositories.DomainModels;
using System.Collections.Generic;

namespace AcmeVending.Repositories
{
    public interface IProductService
    {
        List<Currency> GetAvailableCurrency();
        List<VendableItem> GetProducts();
        List<Currency> CalculateChange(decimal changeRequired, decimal itemCost);
    }
}