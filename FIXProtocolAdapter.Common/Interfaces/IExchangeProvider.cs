using FIXProtocolAdapter.Common.Models.Order.Income;
using FIXProtocolAdapter.Common.Models.Order.View;

namespace FIXProtocolAdapter.Common.Interfaces
{
    public interface IExchangeProvider
    {
        CreateOrderViewModel CreateOrder(CreateOrderIncomeModel model);

        CancelOrderViewModel CancleOrder(CancelOrderIncomeModel model);

        FillListViewModel GetFills(GetFillsIncomeModel model);

        OrderListViewModel GetOrders(GetOrdersIncomeModel model);
    }
}