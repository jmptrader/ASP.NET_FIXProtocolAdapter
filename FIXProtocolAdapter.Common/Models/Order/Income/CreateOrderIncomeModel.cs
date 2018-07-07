using FIXProtocolAdapter.Common.Enums;

namespace FIXProtocolAdapter.Common.Models.Order.Income
{
    public class CreateOrderIncomeModel
    {
        public OrderSide Side { get; set; }
        public int Price { get; set; }
    }
}