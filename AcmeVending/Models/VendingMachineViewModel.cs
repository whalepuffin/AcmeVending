using System.Collections.Generic;

namespace AcmeVending.Models
{
    public class VendingMachineViewModel
    {
        public string CardNumber { get; set; }
        public string SelectedItemCode { get; set; }
        public List<VendableItemViewModel> VendableItems { get; set; }
        public bool IsProcessCardSuccessful { get; set; }
        public string ErrorMessage { get; set; }
        public decimal CashInserted { get; set; }
        public string ChangeDispensed { get; set; }
        public string AvailableChange { get; set; }
        public string AmountPaid { get; set; }
    } 
}