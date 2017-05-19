using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using AcmeVending.Repositories;
using AcmeVending.Models;

namespace AcmeVending.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICardProcessingRepository _cardProcessingRepository;

        public HomeController(IProductService productService, ICardProcessingRepository cardProcessingRepository)
        {
            _productService = productService;
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
        public ActionResult CardProcessing()
        {
            bool isCardProcessed = _cardProcessingRepository.ProcessCard("1234567890123456");
            var model = new CardProcessingModel()
            {
                IsCardProcessed = isCardProcessed
            };

            return View(model);
        }
        
    }
}