using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace AcmeVending.Repositories
{
    public class CardProcessingRepository : ICardProcessingRepository
    {
        const string _baseAddress = "http://localhost:50355/";
        
        public bool ProcessCard(string cardNumber)
        {
            bool isCardProcessed = false;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_baseAddress);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

                    // I would never send a card number across using HTTP and a GET with the value in the query string. This is obviously insecure, should be sent via POST or PUT in the body and encrypted.
                    var uriWithQueryStrign = string.Format("cardprocessing/process?cardNumber={0}", cardNumber);
                    var response = client.GetAsync(uriWithQueryStrign).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        isCardProcessed = true;
                    }
                }
            }
            catch
            {
                // I would log something here for troubleshooting.
                // I would also generally not just throw an error back.  I would prefer to send an error code and handle that rather than use a catch for business logic.
                throw;
            }

            return isCardProcessed;
        }

    }
}