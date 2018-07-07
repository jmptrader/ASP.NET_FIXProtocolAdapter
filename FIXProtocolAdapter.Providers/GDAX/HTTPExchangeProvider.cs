using System;
using FIXProtocolAdapter.Common.Interfaces;
using FIXProtocolAdapter.Common.Models.Order.Income;
using FIXProtocolAdapter.Common.Models.Order.View;

namespace FIXProtocolAdapter.Providers.GDAX
{
    public class ExchangeProviderGDAX : IExchangeProvider
    {
        public CancelOrderViewModel CancleOrder(CancelOrderIncomeModel incomeModel)
        {
            throw new NotImplementedException();
        }

        public CreateOrderViewModel CreateOrder(CreateOrderIncomeModel incomeModel)
        {
            throw new NotImplementedException();
        }

        public FillListViewModel GetFills(GetFillsIncomeModel options)
        {
            throw new NotImplementedException();
        }

        public OrderListViewModel GetOrders(GetOrdersIncomeModel options)
        {
            throw new NotImplementedException();
        }
    }
}