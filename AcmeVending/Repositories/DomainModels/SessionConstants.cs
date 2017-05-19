using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AcmeVending.Repositories.DomainModels
{
    public static class SessionConstants
    {
        public const string AvailableCurrencySessionKey = "VendingMachineState_AvailableCurrency";
        public const string InventorySessionKey = "VendingMachineState_Inventory";
    }
}