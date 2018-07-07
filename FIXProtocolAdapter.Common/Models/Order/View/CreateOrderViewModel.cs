using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;

namespace FIXProtocolAdapter.Common.Models.Order.View
{
    public class CreateOrderViewModel
    {
        public string ClOrdID { get; set; }

        public List<string> MessageLog { get; set; }
    }
}