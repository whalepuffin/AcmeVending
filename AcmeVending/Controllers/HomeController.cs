using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using AcmeVending.Repositories;

namespace AcmeVending.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICardProcessingRepository _cardProcessingRepository;

        public HomeController(IProductRepository productRepository, ICardProcessingRepository cardProcessingRepository)
        {
            _productRepository = productRepository;
            _cardProcessingRepository = cardProcessingRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        public ActionResult VendingMachine()
        {
            var firstValue = _productRepository.GetProductValue();
            var model = new AcmeVending.Models.VendingMachineModel()
            {
                Value = firstValue
            };
            return View(model);
        }

        [HttpGet]
        public ActionResult CardProcessing()
        {
            bool isCardProcessed = _cardProcessingRepository.ProcessCard("1234567890123456");
            var model = new AcmeVending.Models.CardProcessingModel()
            {
                IsCardProcessed = isCardProcessed
            };

            return View(model);
        }
        
    }
}