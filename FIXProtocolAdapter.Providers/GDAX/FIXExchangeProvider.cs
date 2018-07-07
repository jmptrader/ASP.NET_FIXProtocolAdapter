using System;
using System.Text;
using System.Collections.Generic;
using System.Globalization;
using FIXForge.NET.FIX;
using FIXForge.NET.FIX.FIX42;
using FIXProtocolAdapter.Common.Classes;
using FIXProtocolAdapter.Common.Enums;
using FIXProtocolAdapter.Common.Interfaces;
using FIXProtocolAdapter.Common.Models.Order.Income;
using FIXProtocolAdapter.Common.Models.Order.View;
using System.Security.Cryptography;

namespace FIXProtocolAdapter.Providers.GDAX
{
    public class FIXExchangeProvider : IExchangeProvider
    {
        private readonly List<string> _logger;
        private Engine _engine;
        private Session _session;

        public FIXExchangeProvider()
        {
            _logger = new List<string>();
        }

        public CreateOrderViewModel CreateOrder(CreateOrderIncomeModel incomeModel)
        {
            try
            {
                InitEngine();

                CreateSession();

                var clOrdID = Guid.NewGuid().ToString();

                // TODO: See GDAX tag code https://docs.gdax.com/#new-order-single-d
                var msgOrder = new Message(MsgType.Order_Single, ProtocolVersion.FIX42);

                // 11	ClOrdID	UUID selected by client to identify the order
                msgOrder.Set(FIXForge.NET.FIX.FIX42.Tags.ClOrdID, clOrdID);

                // 54	Side	Must be 1 to buy or 2 to sell
                msgOrder.Set(FIXForge.NET.FIX.FIX42.Tags.Side, (int)incomeModel.Side);

                // 44	Price	Limit price (e.g. in USD) (Limit order only)
                msgOrder.Set(FIXForge.NET.FIX.FIX42.Tags.Price, incomeModel.Price);

                // 55	Symbol	E.g. BTC-USD
                msgOrder.Set(FIXForge.NET.FIX.FIX42.Tags.Symbol, "BTC-USD");

                // 38	OrderQty	Order size in base units (e.g. BTC)
                msgOrder.Set(FIXForge.NET.FIX.FIX42.Tags.OrderQty, incomeModel.Price);

                // 152	CashOrderQty	Order size in quote units (e.g. USD) (Market order only)
                //msgOrder.Set(FIXForge.NET.FIX.FIX42.Tags.CashOrderQty, "USD");

                // 99	StopPx	Stop price for order. (Stop Market order only)
                msgOrder.Set(FIXForge.NET.FIX.FIX42.Tags.StopPx, 10);

                // 21	HandlInst	Must be 1 (Automated)
                msgOrder.Set(FIXForge.NET.FIX.FIX42.Tags.HandlInst, 1);

                // 40	OrdType	Must be 1 for Market, 2 for Limit or 3 for Stop Market
                msgOrder.Set(FIXForge.NET.FIX.FIX42.Tags.OrdType, 1);

                // 59  TimeInForce Must be a valid TimeInForce value.See the table below(Limit order only)
                // 1   Good Till Cancel
                // 3   Immediate or Cancel
                // 4   Fill or Kill
                // P   Post - Only
                //msgOrder.Set(FIXForge.NET.FIX.FIX42.Tags.TimeInForce, "P");

                // 7928    SelfTradePrevention Optional, see the table below
                // D Decrement and cancel(the default)
                // O Cancel resting order
                // N Cancel incoming order
                // B Cancel both orders
                //msgOrder.Set(7928, "D");

                _session.Send(msgOrder);

                DestroySession();

                ShutdownEngine();

                return new CreateOrderViewModel()
                {
                    ClOrdID = clOrdID,
                    MessageLog = _logger
                };
            }
            catch (Exception e)
            {
                return new CreateOrderViewModel()
                {
                    ClOrdID = null,
                    MessageLog = _logger
                };
            }
            
        }

        public CancelOrderViewModel CancleOrder(CancelOrderIncomeModel incomeModel)
        {
            InitEngine();

            CreateSession();

            // todo: implement action

            DestroySession();

            ShutdownEngine();

            throw new NotImplementedException();
        }

        public FillListViewModel GetFills(GetFillsIncomeModel options)
        {
            InitEngine();

            CreateSession();

            // TODO: See GDAX tag code https://docs.gdax.com/#new-order-single-d
            var msgStatusRequest = new Message(MsgType.Order_Status_Request, ProtocolVersion.FIX42);
            msgStatusRequest.Set(FIXForge.NET.FIX.FIX42.Tags.OrderID, "3a87ad3c-ee15-4bf4-9166-facdaa390bdf");
            _session.Send(msgStatusRequest);

            //var msgExecutionReport = new Message(MsgType.Execution_Report, ProtocolVersion.FIX42);
            //msgExecutionReport.Set(FIXForge.NET.FIX.FIX42.Tags.ExecType, 1);
            //_session.Send(msgExecutionReport);


            //11  ClOrdID Only present on order acknowledgements, ExecType = New(150 = 0)
            //37  OrderID OrderID from the ExecutionReport with ExecType = New(39 = 0)
            //55  Symbol Symbol of the original order
            //54  Side Must be 1 to buy or 2 to sell

            //32  LastShares Amount filled(if ExecType = 1).Also called LastQty as of FIX 4.3
            //44  Price Price of the fill if ExecType indicates a fill, otherwise the order price
            //38  OrderQty OrderQty as accepted (may be less than requested upon self-trade prevention)
            //152 CashOrderQty Order size in quote units(e.g.USD) (Market order only)
            //60  TransactTime Time the event occurred

            //150	ExecType May be 1 (Partial fill) for fills, D for self-trade prevention, etc.
            //msgFillsReport.Set(FIXForge.NET.FIX.FIX42.Tags.ExecType, 1);

            //39	OrdStatus Order status as of the current message
            //103	OrdRejReason Insufficient funds=3, Post-only=8, Unknown error = 0
            //136	NoMiscFees	1 (Order Status Request response only)
            //137	MiscFeeAmt Fee(Order Status Request response only)
            //139	MiscFeeType	4 (Exchange fees) (Order Status Request response only)
            //1003	TradeID Product unique trade id
            //1057	AggressorIndicator Y for taker orders, N for maker orders



            //DestroySession();

            //ShutdownEngine();

            return new FillListViewModel()
            {
                MessageLog = _logger
            }; ;
        }

        public OrderListViewModel GetOrders(GetOrdersIncomeModel options)
        {
            InitEngine();

            CreateSession();

            // todo: implement action

            DestroySession();

            ShutdownEngine();

            throw new NotImplementedException();
        }

        private void InitEngine()
        {
            var settings = new EngineSettings
            {
                LicenseStore = SettingsManager.GetLicensePatch(),
                LogDirectory = SettingsManager.GetLogPatch(),
                ListenPort = Convert.ToInt16(SettingsManager.Get(ExchangeProvider.GDAX, "FIXServerAcceptPort")),
                SslCertificateFile = $"{SettingsManager.GetProvidersDataPatch()}\\{SettingsManager.Get(ExchangeProvider.GDAX, "SslCertificateFile")}",
            };

            _engine = Engine.Init(settings);
        }

        private void ShutdownEngine()
        {
            _engine?.Shutdown();
        }

        private void CreateSession()
        {
            try
            {
                _session = new Session(SettingsManager.Get(ExchangeProvider.GDAX, "APIKey"), "Coinbase",
                    ProtocolVersion.FIX42, false);

                // Session log hendlers
                _session.InboundApplicationMsgEvent += new InboundApplicationMsgEventHandler(OnInboundApplicationMsg);
                _session.OutboundApplicationMsgEvent += new OutboundApplicationMsgEventHandler(OnOutboundApplicationMsgEventHandler);

                _session.OutboundSessionMsgEvent += new OutboundSessionMsgEventHandler(OnOutboundSessionMsgEventHandler);

                _session.StateChangeEvent += new StateChangeEventHandler(OnStateChange);
                _session.ErrorEvent += new ErrorEventHandler(OnError);
                _session.WarningEvent += new WarningEventHandler(OnWarning);

                _session.Encryption = EncryptionMethod.SSL;
                _session.Ssl.CertificateFile = $"{SettingsManager.GetProvidersDataPatch()}\\{SettingsManager.Get(ExchangeProvider.GDAX, "SslCertificateFile")}";

                // TODO: See GDAX tag code https://docs.gdax.com/#connectivity
                var msgLogon = new Message(MsgType.Logon, ProtocolVersion.FIX42);

                // 98	EncryptMethod	Must be 0 (None)
                msgLogon.Set(FIXForge.NET.FIX.FIX42.Tags.EncryptMethod, 0);

                // TODO: FIX43!!! but in GDAX use version FIX42
                // 554 - Password	Client API passphrase
                msgLogon.Set(FIXForge.NET.FIX.FIX43.Tags.Password, SettingsManager.Get(ExchangeProvider.GDAX, "Passphrase"));

                // 8013 - CancelOrdersOnDisconnect	If set to Y, cancel all open orders for the current profile on disconnect
                msgLogon.Set(8013, "Y");

                // 9406 - DropCopyFlag	If set to Y, execution reports will be generated for all user orders (defaults to Y)
                msgLogon.Set(9406, "Y");

                _session.LogonAsInitiator(SettingsManager.Get(ExchangeProvider.GDAX, "FIXServerUrl"),
                    Convert.ToInt16(SettingsManager.Get(ExchangeProvider.GDAX, "FIXServerPort")),
                    Convert.ToInt16(SettingsManager.Get(ExchangeProvider.GDAX, "HeartBtInt")),
                    msgLogon);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        private void DestroySession()
        {
            _session?.Logout();
        }

        private string ComposeRawData(string sendingTime, string msgType, string msgSeqNum, string senderCompId, string targetCompId, string password)
        {
            var rawData = $"{sendingTime}\u0001{msgType}\u0001{msgSeqNum}\u0001{senderCompId}\u0001{targetCompId}\u0001{password}";
            return HashString(rawData, SettingsManager.Get(ExchangeProvider.GDAX, "APISecret"));
        }

        private string HashString(string rawData, string secret)
        {
            var bytes = Encoding.UTF8.GetBytes(rawData);
            var decodeSecret = Convert.FromBase64String(secret);
            using (var hmaccsha = new HMACSHA256(decodeSecret))
            {
                return Convert.ToBase64String(hmaccsha.ComputeHash(bytes));
            }
        }

        private void OnOutboundSessionMsgEventHandler(object sender, OutboundSessionMsgEventArgs e)
        {
            var outgoingMsg = e.Msg;
            if (outgoingMsg.Type == MsgType.Logon)
            {
                var sendingTime = outgoingMsg.Get(Tags.SendingTime);
                var msgSeqNum = outgoingMsg.Get(Tags.MsgSeqNum);

                var rawData = ComposeRawData(sendingTime, MsgType.Logon, msgSeqNum,
                    SettingsManager.Get(ExchangeProvider.GDAX, "APIKey"), "Coinbase",
                    SettingsManager.Get(ExchangeProvider.GDAX, "Passphrase"));

                // 96	RawData	Client message signature (see below)
                outgoingMsg.Set(Tags.RawData, rawData);
                e.ModifiedMsg = outgoingMsg;
            }

            _logger.Add("Outbound session message: " + e.Msg);
        }

        private void OnOutboundApplicationMsgEventHandler(object sender, OutboundApplicationMsgEventArgs args)
        {
            _logger.Add("Outbound application-level message: " + args.Msg);
        }

        private void OnInboundApplicationMsg(Object sender, InboundApplicationMsgEventArgs e)
        {
            _logger.Add("Incoming application-level message: " + e.Msg);
        }

        private void OnStateChange(Object sender, StateChangeEventArgs e)
        {
            _logger.Add("New session state: " + e.NewState);
        }

        private void OnError(Object sender, ErrorEventArgs e)
        {
            _logger.Add("Session Error: " + e.ToString());
        }

        private void OnWarning(Object sender, WarningEventArgs e)
        {
            _logger.Add("Session Warning: " + e.ToString());
        }

        private static string GetNextClientOrderID()
        {
            return DateTime.Now.ToString("yyyyMMdd-HHmmss-fffffff", CultureInfo.InvariantCulture);
        }
    }
}