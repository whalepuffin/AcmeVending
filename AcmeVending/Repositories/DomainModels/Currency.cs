using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AcmeVending.Repositories.DomainModels
{
    public class Currency
    {
        public string Name { get; set; }
        public decimal Denomination { get; set; }
        public int Quantity { get; set; }
    }
}