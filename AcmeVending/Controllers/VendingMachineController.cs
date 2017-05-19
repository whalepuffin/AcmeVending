using AcmeVending.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AcmeVending.Repositories;
using System.Text;
using AcmeVending.Repositories.DomainModels;

namespace AcmeVending.Controllers
{
    public class VendingMachineController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICardProcessingRepository _cardProcessingRepository;

        public VendingMachineController(IProductService productService, ICardProcessingRepository cardProcessingRepository)
        {
            _productService = productService;
            _cardProcessingRepository = cardProcessingRepository;
        }

        private List<VendableItemViewModel> _vendableItems;
        private List<VendableItemViewModel> VendableItems
        {
            get
            {
                _vendableItems = System.Web.HttpContext.Current.Session[SessionConstants.InventorySessionKey] as List<VendableItemViewModel>;
                if (_vendableItems == null || _vendableItems.Count == 0)
                {
                    _vendableItems = MapVendorItemToVendableItem(_productService.GetProducts());
                }
                return _vendableItems;
            }
            set
            {
                _vendableItems = value;
                System.Web.HttpContext.Current.Session[SessionConstants.InventorySessionKey] = value;
            }
        }

        [HttpGet]
        public ActionResult Initialize()
        {
            var availableCurrency = _productService.GetAvailableCurrency();

            var vendorItems = _productService.GetProducts();
            var vendableItems = MapVendorItemToVendableItem(vendorItems);

            System.Web.HttpContext.Current.Session[SessionConstants.InventorySessionKey] = vendableItems;

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Index()
        {
            var vendableItems = System.Web.HttpContext.Current.Session[SessionConstants.InventorySessionKey] as List<VendableItemViewModel>;
            if (vendableItems == null || vendableItems.Count == 0)
            {
                return RedirectToAction("Initialize");
            }

            var model = new VendingMachineViewModel();
            model.VendableItems = vendableItems;
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(VendingMachineViewModel model)
        {
            try
            {
                if (model.VendableItems == null || model.VendableItems.Count == 0)
                {
                    model.VendableItems = VendableItems;
                }

                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                if (!model.VendableItems.Any(m => m.ItemCode == model.SelectedItemCode && m.Quantity > 0))
                {
                    model.SelectedItemCode = string.Empty;
                    ModelState.Clear();

                    model.ErrorMessage = "Sorry, that item code does not exist or is out of stock, please select again.";
                    ModelState.AddModelError("InvalidItemCode", "Sorry, that item code does not exist or is out of stock, please select again.");
                    return View(model);
                }

                if (!string.IsNullOrWhiteSpace(model.CardNumber))
                {
                    if (_cardProcessingRepository.ProcessCard(model.CardNumber))
                    {
                        model.IsProcessCardSuccessful = true;
                    }
                    else
                    {
                        model.SelectedItemCode = string.Empty;
                        ModelState.Clear();

                        model.ErrorMessage = "Please insert a valid credit card.";
                        ModelState.AddModelError("ProcessCardError", "Please insert a valid credit card.");
                        return View(model);
                    }
                }

                var itemCost = model.VendableItems.Where(m => m.ItemCode == model.SelectedItemCode).Select(m => m.Price).FirstOrDefault();

                if ((model.CashInserted == 0 || itemCost > model.CashInserted) && !model.IsProcessCardSuccessful)
                {
                    model.SelectedItemCode = string.Empty;
                    ModelState.Clear();

                    model.ErrorMessage = "Please insert more cash and try again.";
                    ModelState.AddModelError("NoCashUsed", "Please insert more cash and try again.");
                    return View(model);
                }

                var changeToDispense = _productService.CalculateChange(model.CashInserted, itemCost);
                if (changeToDispense.Count == 0)
                {
                    model.SelectedItemCode = string.Empty;
                    ModelState.Clear();

                    model.ErrorMessage = "Sorry, we are unable to make change for that purchase, please use exact change.";
                    ModelState.AddModelError("ShortOnChange", "Sorry, we are unable to make change for that purchase, please use exact change.");
                    return View(model);
                }

                model.ChangeDispensed = CurrencyCounter(changeToDispense);

                var availableCurrency = System.Web.HttpContext.Current.Session[SessionConstants.AvailableCurrencySessionKey] as List<Currency>;
                model.AvailableChange = CurrencyCounter(availableCurrency);

                //Reduce the quantity of the inventory since it was dispensed and paid for properly.
                model.VendableItems.Where(m => m.ItemCode == model.SelectedItemCode).ToList()
                    .ForEach(m => m.Quantity = (m.Quantity - 1));

                model.AmountPaid = itemCost.ToString();
                if (model.IsProcessCardSuccessful)
                {
                    model.AmountPaid = (itemCost * 1.05m).ToString("0.##");
                }

                VendableItems = model.VendableItems;
            }
            catch(Exception)
            {
                model.SelectedItemCode = string.Empty;
                ModelState.Clear();

                model.ErrorMessage = "Something went wrong, please try again.";
                ModelState.AddModelError("GlobalError", "Something went wrong, please try again.");
                return View(model);
            }

            model.SelectedItemCode = string.Empty;
            ModelState.Clear();
            return View(model);
        }

        private List<VendableItemViewModel> MapVendorItemToVendableItem(List<VendableItem> vendorItems)
        {
            List<VendableItemViewModel> vendableItems = new List<VendableItemViewModel>();
            foreach (var item in vendorItems)
            {
                vendableItems.Add(new VendableItemViewModel { Name = item.Name, ItemCode = item.ItemCode, Price = item.Price, Quantity = item.Quantity });
            }
            return vendableItems;
        }

        private string CurrencyCounter(List<Currency> currency)
        {
            StringBuilder changeBuilder = new StringBuilder();
            foreach (var change in currency)
            {
                if (change.Quantity > 0)
                {
                    changeBuilder.AppendLine(" | " + change.Quantity + " | " + change.Name);
                }
            }
            return changeBuilder.ToString();
        }
    }
}