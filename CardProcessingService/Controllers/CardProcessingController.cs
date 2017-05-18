using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;

namespace CardProcessingService.Controllers
{
    public class CardProcessingController : ApiController
    {
        // POST api/values
        [HttpGet]
        [Route("cardprocessing/process")]
        public HttpResponseMessage Process(string cardNumber)
        {

            if (String.IsNullOrWhiteSpace(cardNumber) || cardNumber.Length != 16 || !Regex.IsMatch(cardNumber, @"^\d+$"))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
