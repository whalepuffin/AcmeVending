using AcmeVending.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AcmeVending.Repositories;

namespace AcmeVending.Controllers
{
    public class VendingMachineController : Controller
    {
        private readonly IProductService _productService;

        public VendingMachineController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public ActionResult Initialize()
        {
            _productService.InitializeVendingMachine();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Index()
        {
            var model = new VendingMachineViewModel();
            model.VendableItems = new List<VendableItemViewModel>();
            model.VendableItems.Add(new VendableItemViewModel
            {
                IsAvailable = true,
                Name = "Pop Tarts | Strawbery",
                ItemCode = "123",
                Price = Double.Parse("2.25"),
                Quantity = 5
            });
            model.VendableItems.Add(new VendableItemViewModel
            {
                IsAvailable = true,
                Name = "Snickers",
                ItemCode = "433",
                Price = Double.Parse("1.25"),
                Quantity = 5
            });
            model.VendableItems.Add(new VendableItemViewModel
            {
                IsAvailable = true,
                Name = "Potato Chips",
                ItemCode = "623",
                Price = Double.Parse(".95"),
                Quantity = 5
            });
            model.VendableItems.Add(new VendableItemViewModel
            {
                IsAvailable = true,
                Name = "Trident",
                ItemCode = "789",
                Price = Double.Parse(".50"),
                Quantity = 5
            });

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VendSubmit(VendingMachineViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var changeToDispense = _productService.CalculateChange(7.90m);
            if (changeToDispense.Count == 0)
            {
                ModelState.AddModelError("shortOnChange", "Sorry, we are unable to make change for that purchase, please use exact change.");
            }

            return RedirectToAction("Index");
        }
    }
}