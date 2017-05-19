using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AcmeVending.Repositories.DomainModels
{
    public class VendableItem
    {
        public string Name { get; set; }
        public string ItemCode { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}