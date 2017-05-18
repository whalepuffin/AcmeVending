using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AcmeVending.Models
{
    public class VendingMachineViewModel
    {
        public string CardNumber { get; set; }
        public string SelectedItemCode { get; set; }
        public List<VendableItemViewModel> VendableItems { get; set; }
        public bool IsProcessCardSuccessful { get; set; }
        public string ErrorMessage { get; set; }

    } 
}