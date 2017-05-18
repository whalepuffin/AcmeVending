using AcmeVending.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AcmeVending.Repositories;
using System.Text;

namespace AcmeVending.Controllers
{
    public class VendingMachineController : Controller
    {
        private readonly IProductService _productService;
        const string InventorySessionKey = "VendingMachineState_Inventory";

        public VendingMachineController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public ActionResult Initialize()
        {
            _productService.InitializeVendingMachine();

            List<VendableItemViewModel> vendableItems = new List<VendableItemViewModel>();
            vendableItems.Add(new VendableItemViewModel { IsAvailable = true, Name = "Snickers", ItemCode = "123", Price = Double.Parse("2.25"), Quantity = 5 });
            vendableItems.Add(new VendableItemViewModel { IsAvailable = true, Name = "Skittles", ItemCode = "411", Price = Double.Parse("0.65"), Quantity = 5 });
            vendableItems.Add(new VendableItemViewModel { IsAvailable = true, Name = "Pop Tarts", ItemCode = "766", Price = Double.Parse("0.90"), Quantity = 5 });
            vendableItems.Add(new VendableItemViewModel { IsAvailable = true, Name = "Gum | Mint", ItemCode = "098", Price = Double.Parse("0.85"), Quantity = 5 });
            vendableItems.Add(new VendableItemViewModel { IsAvailable = true, Name = "Potato Chips", ItemCode = "232", Price = Double.Parse("1.50"), Quantity = 5 });
            vendableItems.Add(new VendableItemViewModel { IsAvailable = true, Name = "Cookies", ItemCode = "433", Price = Double.Parse("1.25"), Quantity = 5 });
            vendableItems.Add(new VendableItemViewModel { IsAvailable = true, Name = "Trail mix", ItemCode = "655", Price = Double.Parse("4.25"), Quantity = 5 });

            System.Web.HttpContext.Current.Session[InventorySessionKey] = vendableItems;

            return RedirectToAction("Index");

        }

        [HttpGet]
        public ActionResult Index()
        {
            var vendableItems = System.Web.HttpContext.Current.Session[InventorySessionKey] as List<VendableItemViewModel>;
            if (vendableItems == null || vendableItems.Count == 0)
            {
                return RedirectToAction("Initialize", "VendingMachine");
            }

            var model = new VendingMachineViewModel();
            model.VendableItems = vendableItems;
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(VendingMachineViewModel model)
        {
            if (model.VendableItems == null || model.VendableItems.Count == 0)
            {
                model.VendableItems = System.Web.HttpContext.Current.Session[InventorySessionKey] as List<VendableItemViewModel>;
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (!model.VendableItems.Any(m=>m.ItemCode == model.SelectedItemCode && m.Quantity > 0))
            {
                model.ErrorMessage = "Sorry, that item code does not exist or is out of stock, please select again.";
                ModelState.AddModelError("shortOnChange", "Sorry, we are unable to make change for that purchase, please use exact change.");
                return View(model);
            }

            //Validate if Card Number is entered
            //Continue if YES and Processed or card number is null/empty
            //if()


            var changeToDispense = _productService.CalculateChange(model.CashInserted);
            if (changeToDispense.Count == 0)
            {
                model.ErrorMessage = "Sorry, we are unable to make change for that purchase, please use exact change.";
                ModelState.AddModelError("shortOnChange", "Sorry, we are unable to make change for that purchase, please use exact change.");
                return View(model);
            }

            StringBuilder changeBuilder = new StringBuilder();
            foreach (var change in changeToDispense)
            {
                if (change.Quantity > 0 )
                {
                    changeBuilder.AppendLine(change.Name + " " + change.Quantity);
                }
            }
            model.ChangeDispensed = changeBuilder.ToString();
            //items.Where(it => it.Name == "name2").ToList().ForEach(it => it.Value = "value2");
            model.VendableItems.Where(m => m.ItemCode == model.SelectedItemCode).ToList()
                .ForEach(m => m.Quantity = (m.Quantity - 1));

            model.SelectedItemCode = string.Empty;
            ModelState.Clear();
            return View(model);
        }
    }
}