using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using AcmeVending.Repositories.DomainModels;

namespace AcmeVending.Repositories
{
    public class ProductService : IProductService
    {

        public void InitializeVendingMachine()
        {
            HttpContext.Current.Session["VendingMachineState_Products"] = "";

            var availableCurrency = new List<Currency>()
            {
                new Currency{ Name = "five-dollar", Denomination=5.00m, Quantity=5 },
                new Currency{ Name = "one-dollar", Denomination=1.00m, Quantity=5 },
                new Currency{ Name = "quarter", Denomination=0.25m, Quantity=5 },
                new Currency{ Name = "dime", Denomination=0.10m, Quantity=5 },
                new Currency{ Name = "nickel", Denomination=0.05m, Quantity=5 }
            };
            HttpContext.Current.Session["VendingMachineState_AvailableCurrency"] = availableCurrency;
        }

        public string GetProductValue()
        {

            return string.Empty;
        }

        public List<Currency> CalculateChange(decimal changeRequired)
        {
            var changeToDispense = new List<Currency>();
            try
            {
                var availableCurrency = HttpContext.Current.Session["VendingMachineState_AvailableCurrency"] as List<Currency>;

                var currencyChange = new List<Currency>()
                {
                    new Currency{ Name = "five-dollar", Denomination=5.00m, Quantity=0 },
                    new Currency{ Name = "one-dollar", Denomination=1.00m, Quantity=0 },
                    new Currency{ Name = "quarter", Denomination=0.25m, Quantity=0 },
                    new Currency{ Name = "dime", Denomination=0.10m, Quantity=0 },
                    new Currency{ Name = "nickel", Denomination=0.05m, Quantity=0 }
                };

                foreach (var currency in currencyChange)
                {
                    int count = (int)(changeRequired / currency.Denomination);
                    changeRequired -= count * currency.Denomination;
                    currency.Quantity = count;
                }

                //If no negative change, then remove chagne amounts from the vending machine state.
                foreach (var currency in availableCurrency)
                {
                    var newQuantity = currency.Quantity - currencyChange.Where(c => c.Name == currency.Name).Select(q => q.Quantity).FirstOrDefault();
                    if (newQuantity < 0)
                    {
                        return changeToDispense;
                    }
                    currency.Quantity = newQuantity;
                }

                HttpContext.Current.Session["VendingMachineState_AvailableCurrency"] = availableCurrency;
                changeToDispense = currencyChange;
            }
            catch (Exception)
            {
                throw;
            }

            return changeToDispense;
        }

    }
}