using FIXProtocolAdapter.Common.Models.Order.Income;
using FIXProtocolAdapter.Common.Enums;
using FIXProtocolAdapter.Providers.Classes;
using System.Web.Http;

namespace FIXProtocolAdapter.API.Controllers
{
    [Route("api/Order/{Action}")]
    public class OrderController : ApiController
    {
        private readonly ExchangeProviderFactory _factory;

        public OrderController()
        {
            _factory = new ExchangeProviderFactory();
        }

        [HttpPost]
        public IHttpActionResult Create([FromBody] CreateOrderIncomeModel model)
        {

            if (!ModelState.IsValid)
                BadRequest(ModelState);

            var provider = _factory.GetExchangeProvider(ExchangeProvider.GDAX);
            var responce = provider.CreateOrder(model);
            return Ok(responce);
        }

        [HttpPost]
        public IHttpActionResult Cancel([FromBody] CancelOrderIncomeModel model)
        {
            var provider = _factory.GetExchangeProvider(ExchangeProvider.GDAX);
            var responce = provider.CancleOrder(model);
            return Ok(responce);
        }

        [HttpGet]
        public IHttpActionResult OrderList(GetOrdersIncomeModel model)
        {
            var provider = _factory.GetExchangeProvider(ExchangeProvider.GDAX);
            var responce = provider.GetOrders(model);
            return Ok(responce);
        }

        [HttpGet]
        public IHttpActionResult FillList(GetFillsIncomeModel model)
        {
            var provider = _factory.GetExchangeProvider(ExchangeProvider.GDAX);
            var responce = provider.GetFills(model);
            return Ok(responce);
        }
    }
}
