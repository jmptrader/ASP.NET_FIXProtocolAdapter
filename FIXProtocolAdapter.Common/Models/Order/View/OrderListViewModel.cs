using System.Collections.Generic;

namespace FIXProtocolAdapter.Common.Models.Order.View
{
    public class OrderListViewModel
    {
        public int CountPages { get; set; }
        public int CountItems { get; set; }
        public List<OrderItemViewModel> Items { get; set; }
    }
}