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
        //StringContent content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");
        public bool ProcessCard(string cardNumber)
        {
            bool isCardProcessed = false;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

                //var response = client.GetAsync("cardprocessing/process", new StringContent(cardNumber, System.Text.Encoding.UTF8, "application/x-www-form-urlencoded")).Result;
                // I would never send a card number across using HTTP and a GET with the value in the query string. This is obviously insecure.
                var uriWithQueryStrign = string.Format("cardprocessing/process?cardNumber={0}", cardNumber);
                var response = client.GetAsync(uriWithQueryStrign).Result;
                if (response.IsSuccessStatusCode)
                {
                    isCardProcessed = true;
                }
            }

            return isCardProcessed;
        }

    }
}