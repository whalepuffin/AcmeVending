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
        public double Price { get; set; }
        public bool IsAvailable { get; set; }
    }
}