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
        public List<Currency> GetAvailableCurrency()
        {
            var availableCurrency = new List<Currency>
            {
                new Currency { Name = "five-dollar", Denomination=5.00m, Quantity=5 },
                new Currency { Name = "one-dollar", Denomination=1.00m, Quantity=5 },
                new Currency { Name = "quarter", Denomination=0.25m, Quantity=5 },
                new Currency { Name = "dime", Denomination=0.10m, Quantity=5 },
                new Currency { Name = "nickel", Denomination=0.05m, Quantity=5 }
            };               

            HttpContext.Current.Session[SessionConstants.AvailableCurrencySessionKey] = availableCurrency;
            return availableCurrency;
        }

        public List<VendableItem> GetProducts()
        {
            List<VendableItem> vendableItems = new List<VendableItem>
            {
                new VendableItem { Name = "Snickers", ItemCode = "123", Price = Decimal.Parse("2.25"), Quantity = 5 },
                new VendableItem { Name = "Skittles", ItemCode = "411", Price = Decimal.Parse("0.65"), Quantity = 5 },
                new VendableItem { Name = "Pop Tarts", ItemCode = "766", Price = Decimal.Parse("0.90"), Quantity = 5 },
                new VendableItem { Name = "Gum | Mint", ItemCode = "098", Price = Decimal.Parse("0.85"), Quantity = 5 },
                new VendableItem { Name = "Potato Chips", ItemCode = "232", Price = Decimal.Parse("1.50"), Quantity = 5 },
                new VendableItem { Name = "Cookies", ItemCode = "433", Price = Decimal.Parse("1.25"), Quantity = 5 },
                new VendableItem { Name = "Trail mix", ItemCode = "655", Price = Decimal.Parse("4.25"), Quantity = 5 },
                new VendableItem { Name = "Doritos", ItemCode = "133", Price = Decimal.Parse("0.35"), Quantity = 5 },
                new VendableItem { Name = "Trail mix", ItemCode = "712", Price = Decimal.Parse("2.65"), Quantity = 5 },
                new VendableItem { Name = "Trail mix", ItemCode = "944", Price = Decimal.Parse("1.25"), Quantity = 5 }
            };
            return vendableItems;
        }

        public List<Currency> CalculateChange(decimal cashInserted, decimal itemCost)
        {
            var changeRequired = cashInserted - itemCost;

            var changeToDispense = new List<Currency>();
            try
            {
                var availableCurrency = HttpContext.Current.Session[SessionConstants.AvailableCurrencySessionKey] as List<Currency>;
                if (availableCurrency == null || availableCurrency.Count == 0)
                {
                    availableCurrency = GetAvailableCurrency();
                }

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

                HttpContext.Current.Session[SessionConstants.AvailableCurrencySessionKey] = availableCurrency;
                changeToDispense = currencyChange;
            }
            catch 
            {
                // I would rather not just catch and throw, it would be better to have this entire layer in a Web API in my opinion.
                throw;
            }

            return changeToDispense;
        }

    }
}