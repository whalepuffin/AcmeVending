using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AcmeVending.Models
{
    public class VendableItemViewModel
    {
        public string Name { get; set; }
        public string ItemCode { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public bool IsAvailable { get; set; }
    }
}