using AcmeVending.Repositories.DomainModels;
using System.Collections.Generic;

namespace AcmeVending.Repositories
{
    public interface IProductService
    {
        void InitializeVendingMachine();
        string GetProductValue();
        List<Currency> CalculateChange(decimal changeRequired);
    }
}