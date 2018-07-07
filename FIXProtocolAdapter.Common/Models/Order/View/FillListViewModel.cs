using System.Collections.Generic;

namespace FIXProtocolAdapter.Common.Models.Order.View
{
    public class FillListViewModel
    {
        public int CountPages { get; set; }
        public int CountItems { get; set; }
        public List<FillItemViewModel> Items { get; set; }

        public List<string> MessageLog { get; set; }
    }
}