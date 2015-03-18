using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.shephertz.app42.paas.sdk.csharp.util;
using Newtonsoft.Json;
using System.IO;
using System.IO.MemoryMappedFiles;
using com.shephertz.app42.paas.sdk.csharp.connection;
using com.shephertz.app42.paas.sdk.csharp;



namespace com.shephertz.app42.paas.sdk.csharp.shopping
{
    public sealed class PaymentStatus
    {
        public static readonly string DECLINED = "DECLINED";
        public static readonly string AUTHORIZED = "AUTHORIZED";
        public static readonly string PENDING = "PENDING";
        private String value;

        private PaymentStatus(String value)
        {
            this.value = value;
        }

        public String GetValue()
        {
            return value;
        }
    }

    /// <summary>
    /// This is Cloud Persistent Shopping Cart Service. App Developers can use this to 
    /// create a Shopping Cart. Add Items and Check Out items. It also maintains the transactions
    /// and the corresponding Payment Status.
    /// The Payment Gateway interface is not provided by the Platform. It is left to the App developer
    /// how he wants to do the Payment Integration. This can be used along with Catalogue or used independently.
    /// <see cref="CartService">Catalgoue</see>
    /// <see cref="CartService">Cart</see>
    /// <see cref="CartService">App42Response</see>
    /// <see cref="CartService">ItemData</see>
    /// <see cref="CartService">PaymentStatus</see>
    /// </summary>

    public class CartService
    {
        Config config;
        private String version = "1.0";
        private String resource = "cart";
        private String apiKey;
        private String secretKey;
        String baseURL;
        /// <summary>
        /// Constructor that takes
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="secretKey"></param>
        /// <param name="baseURL"></param>
        public CartService(String apiKey, String secretKey, String baseURL)
        {
            this.apiKey = apiKey;
            this.secretKey = secretKey;
            this.baseURL = baseURL;
        }


        public void status(String paymentStatus)
        {
            if (!PaymentStatus.AUTHORIZED.ToString().Equals(paymentStatus) && !PaymentStatus.DECLINED.ToString().Equals(paymentStatus) && !PaymentStatus.PENDING.ToString().Equals(paymentStatus))
            {
                throw new App42Exception("Invalid Status. Could be either AUTHORIZED or UNAUTHORIZED or DECLINED");
            }

        }
        /// <summary>
        /// Creates a Cart Session for the specified User
        /// </summary>
        /// <param name="user"> User for whom Cart Session has to be created</param>
        /// <returns>
        /// The Cart Id with Creation Time. The id has to be used in subsequent calls for 
        /// adding and checking out
        /// </returns>
        /// <exception>App42Exception</exception>

        public Cart CreateCart(String user)
        {

            String response = null;
            Cart cart = null;
            Util.ThrowExceptionIfNullOrBlank(user, "User");

            Dictionary<String, String> queryParams = new Dictionary<String, String>();
            queryParams.Add("apiKey", this.apiKey);
            queryParams.Add("version", this.version);
            queryParams.Add("timeStamp", Util.GetUTCFormattedTimestamp());

            Dictionary<String, String> paramsDics = new Dictionary<String, String>(queryParams);
            StringBuilder sbJson = new StringBuilder();
            StringWriter sw = new StringWriter(sbJson);
            JsonWriter jsonWriter = new JsonTextWriter(sw);
            jsonWriter.WriteStartObject();
            jsonWriter.WritePropertyName("userName");
            jsonWriter.WriteValue(user);
            jsonWriter.WriteEndObject();
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"app42\":{\"cart\":").Append(sbJson.ToString())
                    .Append("}}");

            paramsDics.Add("body", sb.ToString());
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource;
            response = RESTConnector.getInstance().ExecutePost(signature, resourceURL, queryParams, sb.ToString());
            cart = new CartResponseBuilder().BuildResponse(response);
            return cart;
        }

        /// <summary>
        /// Fetch Cart details. Can be used by the App developer to display Cart Details i.e. Items in a Cart.
        /// </summary>
        /// <param name="cartId"> The Cart Id that has to be fetched.</param>
        /// <returns>Cart object containing cart details with all the items which are in it. It also tells the state of
        ///  the Cart.</returns>
        /// <exception>App42Exception</exception>

        public Cart GetCartDetails(String cartId)
        {
            String response = null;
            Cart cart = null;
            Util.ThrowExceptionIfNullOrBlank(cartId, "CartId");


            Dictionary<String, String> queryParams = new Dictionary<String, String>();
            queryParams.Add("apiKey", this.apiKey);
            queryParams.Add("version", this.version);
            queryParams.Add("timeStamp", Util.GetUTCFormattedTimestamp());

            Dictionary<String, String> paramsDics = new Dictionary<String, String>(queryParams);
            paramsDics.Add("cartId", cartId);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/"
                    + cartId + "/details";
            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            cart = new CartResponseBuilder().BuildResponse(response);
            return cart;
        }
        /// <summary>
        /// Adds an Item in the Cart with quantity and price. This method does not take currency. Its the
        /// bonus of the App developer to maitain the currency. It takes only the price.
        /// </summary>
        /// <param name="cartID">The Cart Id into which item has to be added</param>
        /// <param name="itemID">
        /// The Item id which has to be added in the cart. If the Catalogue Service is used along
        /// with the Cart Service then the Item ids should be same.
        /// </param>
        /// <param name="itemQuantity"> Quantity of the Item to be purchased.</param>
        /// <param name="price"> Price of the item.</param>
        ///<returns>Cart object containing added item.</returns> 
        ///<exception>App42Exception</exception>


        public Cart AddItem(String cartID, String itemID, Int64 itemQuantity,
                Double price)
        {
            String response = null;
            Cart cart = null;

            Util.ThrowExceptionIfNullOrBlank(cartID, "CartID");
            Util.ThrowExceptionIfNullOrBlank(itemID, "ItemID");
            Dictionary<String, String> queryParams = new Dictionary<String, String>();
            queryParams.Add("apiKey", this.apiKey);
            queryParams.Add("version", this.version);
            queryParams.Add("timeStamp", Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> paramsDics = new Dictionary<String, String>(queryParams);

            StringBuilder sbJson1 = new StringBuilder();
            StringWriter sw = new StringWriter(sbJson1);
            JsonWriter itemObj = new JsonTextWriter(sw);
            itemObj.WriteStartObject();
            itemObj.WritePropertyName("quantity");
            itemObj.WriteValue(itemQuantity);
            itemObj.WritePropertyName("amount");
            itemObj.WriteValue(price);
            itemObj.WriteEndObject();

            StringBuilder sbJson = new StringBuilder();
            StringWriter swComplete = new StringWriter(sbJson);
            JsonWriter jsonWriter1 = new JsonTextWriter(swComplete);
            jsonWriter1.WriteStartObject();
            jsonWriter1.WritePropertyName("cartId");
            jsonWriter1.WriteValue(cartID);
            jsonWriter1.WritePropertyName("item");
            jsonWriter1.WriteValue(sbJson1.ToString());
            jsonWriter1.WriteEndObject();

            paramsDics.Add("itemId", itemID);
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"app42\":{\"cart\":").Append(sbJson)
                    .Append("}}");

            App42Log.Debug("Created JSon for Profile : " + sb);
            paramsDics.Add("body", sb.ToString());
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/" + "item" + "/" + itemID;
            response = RESTConnector.getInstance().ExecutePost(signature,
                    resourceURL, queryParams, sb.ToString());
            cart = new CartResponseBuilder().BuildResponse(response);
            return cart;
        }



        /// <summary>
        /// Fetches the Items from the specified Cart
        /// </summary>
        /// <param name="cartId">The cart id from which items have to be fetched</param>
        /// <returns>Cart object which contains all items in the cart.</returns>
        ///<exception>App42Exception</exception>

        public Cart GetItems(String cartId)
        {
            String response = null;
            Cart cart = null;
            Util.ThrowExceptionIfNullOrBlank(cartId, "CartId");
            Dictionary<String, String> queryParams = new Dictionary<String, String>();
            queryParams.Add("apiKey", this.apiKey);
            queryParams.Add("version", this.version);
            queryParams.Add("timeStamp", Util.GetUTCFormattedTimestamp());

            Dictionary<String, String> paramsDics = new Dictionary<String, String>(queryParams);
            paramsDics.Add("cartId", cartId);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/"
                    + cartId;
            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            cart = new CartResponseBuilder().BuildResponse(response);
            return cart;
        }


        /// <summary>
        /// Fetches the specified Item from the specified Cart
        /// </summary>
        /// <param name="cartId"> The cart id from which item has to be fetched.</param>
        /// <param name="itemId"> The item for which the information has to be fetched.</param>
        /// <returns>Cart Object</returns>
        /// <exception>App42Exception</exception>
        public Cart GetItem(String cartId, String itemId)
        {

            String response = null;
            Cart cart = null;
            Util.ThrowExceptionIfNullOrBlank(cartId, "CartId");
            Util.ThrowExceptionIfNullOrBlank(itemId, "ItemId");
            Dictionary<String, String> queryParams = new Dictionary<String, String>();
            queryParams.Add("apiKey", this.apiKey);
            queryParams.Add("version", this.version);
            queryParams.Add("timeStamp", Util.GetUTCFormattedTimestamp());

            Dictionary<String, String> paramsDics = new Dictionary<String, String>(queryParams);
            paramsDics.Add("cartId", cartId);
            paramsDics.Add("itemId", itemId);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/"
                        + cartId + "/" + itemId;
            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            cart = new CartResponseBuilder().BuildResponse(response);
            return cart;

        }

        /// <summary>
        /// Removes the specified item from the specified Cart.
        /// </summary>
        /// <param name="cartId"> The cart id from which the item has to be removed.</param>
        /// <param name="itemId"> Id of the Item which has to be removed.</param>
        /// <returns>App42Response if removed successfully.</returns>
        ///<exception>App42Exception</exception>


        public App42Response RemoveItem(String cartId, String itemId)
        {
            String response = null;
            App42Response responseObj = new App42Response();

            Util.ThrowExceptionIfNullOrBlank(cartId, "CartId");
            Util.ThrowExceptionIfNullOrBlank(itemId, "ItemId");

            Dictionary<String, String> queryParams = new Dictionary<String, String>();
            queryParams.Add("apiKey", this.apiKey);
            queryParams.Add("version", this.version);
            queryParams.Add("timeStamp", Util.GetUTCFormattedTimestamp());

            Dictionary<String, String> paramsDics = new Dictionary<String, String>(queryParams);
            paramsDics.Add("cartId", cartId);
            paramsDics.Add("itemId", itemId);
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/"
                        + cartId + "/" + itemId;
            response = RESTConnector.getInstance().ExecuteDelete(signature,
                    resourceURL, queryParams);
            responseObj.SetStrResponse(response);
            responseObj.SetResponseSuccess(true);
            return responseObj;

        }

        /// <summary>
        /// Removes all Items from the specified Cart
        /// </summary>
        /// <param name="cartId"> The cart id from which items have to be removed.</param>
        /// <returns>App42Response if removed successfully.</returns>
        ///<exception>App42Exception</exception>

        public App42Response RemoveAllItems(String cartId)
        {

            String response = null;
            App42Response responseObj = new App42Response();
            Util.ThrowExceptionIfNullOrBlank(cartId, "CartId");

            Dictionary<String, String> queryParams = new Dictionary<String, String>();
            queryParams.Add("apiKey", this.apiKey);
            queryParams.Add("version", this.version);
            queryParams.Add("timeStamp", Util.GetUTCFormattedTimestamp());

            Dictionary<String, String> paramsDics = new Dictionary<String, String>(queryParams);
            paramsDics.Add("cartId", cartId);

            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/"
                        + cartId;
            response = RESTConnector.getInstance().ExecuteDelete(signature,
                    resourceURL, queryParams);
            responseObj.SetStrResponse(response);
            responseObj.SetResponseSuccess(true);
            return responseObj;


        }

        /// <summary>
        /// Checks whether the Cart is Empty or not.
        /// </summary>
        /// <param name="cartId"> The cart id to check for empty.</param>
        /// <returns>Cart Object</returns>
        ///<exception>App42Exception</exception>
        public Cart IsEmpty(String cartId)
        {
            String response = null;
            Cart cart = null;
            Util.ThrowExceptionIfNullOrBlank(cartId, "CartId");
            Dictionary<String, String> queryParams = new Dictionary<String, String>();
            queryParams.Add("apiKey", this.apiKey);
            queryParams.Add("version", this.version);
            queryParams.Add("timeStamp", Util.GetUTCFormattedTimestamp());

            Dictionary<String, String> paramsDics = new Dictionary<String, String>(queryParams);
            paramsDics.Add("cartId", cartId);

            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/"
                         + cartId + "/isEmpty";
            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);

            cart = new CartResponseBuilder().BuildResponse(response);
            return cart;
        }


        /// <summary>
        /// Checks out the Cart and put it in CheckOut Stage and returns the Transaction Id
        /// The transaction id has to be used in future to update the Payment Status.
        /// </summary>
        /// <param name="cartId">The cart id that has to be checkedout.</param>
        /// <returns>Cart object containing Checked Out Cart Information with the Transaction Id.</returns>
        ///<exception>App42Exception</exception>
        public Cart CheckOut(String cartId)
        {
            String response = null;
            Cart cart = null;

            Util.ThrowExceptionIfNullOrBlank(cartId, "CartId");


            Dictionary<String, String> queryParams = new Dictionary<String, String>();
            queryParams.Add("apiKey", this.apiKey);
            queryParams.Add("version", this.version);
            queryParams.Add("timeStamp", Util.GetUTCFormattedTimestamp());

            Dictionary<String, String> paramsDics = new Dictionary<String, String>(queryParams);
            StringBuilder sbJson = new StringBuilder();
            StringWriter sw = new StringWriter(sbJson);
            JsonWriter jsonWriter = new JsonTextWriter(sw);
            jsonWriter.WriteStartObject();
            jsonWriter.WritePropertyName("cartId");
            jsonWriter.WriteValue(cartId);
            jsonWriter.WriteEndObject();
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"app42\":{\"cart\":").Append(sbJson.ToString())
                    .Append("}}");

            paramsDics.Add("body", sb.ToString());
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/checkOut";
            response = RESTConnector.getInstance().ExecutePut(signature,
                    resourceURL, queryParams, sb.ToString());
            cart = new CartResponseBuilder().BuildResponse(response);
            return cart;

        }

        /// <summary>
        /// Update Payment Status of the Cart. When a Cart is checkout, it is in Checkout state. The payment
        /// status has to be updated based on the Payment Gateway interaction
        /// </summary>
        /// <param name="cartID"> The cart id for which the payment status has to be updated.</param>
        /// <param name="transactionID"> Transaction id for which the payment status has to be updated.</param>
        /// <param name="paymentStatus"> Payment Status to be updated. The probable values are PaymentStatus.DECLINED, PaymentStatus.AUTHORIZED, PaymentStatus.PENDING.</param>
        /// <returns> Cart object which contains Payment Status.</returns>
        ///<exception>App42Exception</exception>
        public Cart Payment(String cartID, String transactionID, String paymentStatus)
        {
            String response = null;
            Cart cart = null;


            Util.ThrowExceptionIfNullOrBlank(transactionID, "TransactionId");
            Util.ThrowExceptionIfNullOrBlank(paymentStatus, "paymentStatus");
            Util.ThrowExceptionIfNullOrBlank(cartID, "CartID");


            if (!PaymentStatus.AUTHORIZED.ToString().Equals(paymentStatus) && !PaymentStatus.DECLINED.ToString().Equals(paymentStatus) && !PaymentStatus.DECLINED.ToString().Equals(paymentStatus))
            {
                throw new App42Exception("Invalid Status. Could be either AUTHORIZED or UNAUTHORIZED or DECLINED");
            }



            Dictionary<String, String> queryParams = new Dictionary<String, String>();
            queryParams.Add("apiKey", this.apiKey);
            queryParams.Add("version", this.version);
            queryParams.Add("timeStamp", Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> paramsDics = new Dictionary<String, String>(queryParams);

            StringBuilder sbJson = new StringBuilder();
            StringWriter sw = new StringWriter(sbJson);
            JsonWriter jsonWriter = new JsonTextWriter(sw);
            jsonWriter.WriteStartObject();
            jsonWriter.WritePropertyName("cartId");
            jsonWriter.WriteValue(cartID);
            jsonWriter.WritePropertyName("transactionId");
            jsonWriter.WriteValue(transactionID);
            jsonWriter.WritePropertyName("status");
            jsonWriter.WriteValue(paymentStatus);
            jsonWriter.WriteEndObject();

            StringBuilder sb = new StringBuilder();
            sb.Append("{\"app42\":{\"cart\":").Append(sbJson.ToString())
                    .Append("}}");

            paramsDics.Add("body", sb.ToString());
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/payment";

            response = RESTConnector.getInstance().ExecutePut(signature,
                    resourceURL, queryParams, sb.ToString());
            cart = new CartResponseBuilder().BuildResponse(response);
            return cart;
        }

        /// <summary>
        /// Fetches Payment information for a User. This can be used to display Order and Payment History.
        /// </summary>
        /// <param name="userId"> User Id for whom payment information has to be fetched.</param>
        /// <returns>IList containing Cart objects. Payment history can be retrieved from individual Cart object.</returns>
        ///<exception>App42Exception</exception>
        public IList<Cart> GetPaymentsByUser(String userId)
        {
            String response = null;
            IList<Cart> cartList = null;
            Util.ThrowExceptionIfNullOrBlank(userId, "UserId");

            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp", Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("userId", userId);
            // Construct a json body for Receive Message
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource
                    + "/payments" + "/user" + "/" + userId;
            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            cartList = new CartResponseBuilder().BuildArrayResponse(response);
            return cartList;
        }

        /// <summary>
        /// Fetches Payment information for the specified Cart Id
        /// </summary>
        /// <param name="cartId"> Cart Id for which the payment information has to be fetched.</param>
        /// <returns>Cart object which contains Payment History for the specified Cart.</returns>
        ///<exception>App42Exception</exception>
        public Cart GetPaymentByCart(String cartId)
        {
            String response = null;
            Cart cart = null;

            Util.ThrowExceptionIfNullOrBlank(cartId, "CartId");

            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp", Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("cartId", cartId);

            // Construct a json body for Receive Message
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource
                    + "/payments/cart/" + cartId;
            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            cart = new CartResponseBuilder().BuildResponse(response);
            return cart;
        }

        /// <summary>
        /// Fetches Payment information based on User Id and Status
        /// </summary>
        /// <param name="userId"> User Id for whom payment information has to be fetched</param>
        /// <param name="paymentStatus"> Status of type which payment information has to be fetched</param>
        /// <returns>IList containing Cart objects. Payment history can be retrieved from individual Cart object.</returns>
        ///<exception>App42Exception</exception>
        public IList<Cart> GetPaymentsByUserAndStatus(String userId, String paymentStatus)
        {
            String response = null;
            IList<Cart> cartList = null;

            Util.ThrowExceptionIfNullOrBlank(userId, "userId");
            Util.ThrowExceptionIfNullOrBlank(paymentStatus, "paymentStatus");

            if (!PaymentStatus.AUTHORIZED.ToString().Equals(paymentStatus) && !PaymentStatus.DECLINED.ToString().Equals(paymentStatus) && !PaymentStatus.PENDING.ToString().Equals(paymentStatus))
            {
                throw new App42Exception("Invalid Status. Could be either AUTHORIZED or UNAUTHORIZED or DECLINED");
            }

            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp", Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("status", paymentStatus);
            paramsDics.Add("userId", userId);
            // Construct a json body for Receive Message
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/payments" + "/user" + "/" + userId + "/" + paymentStatus;
            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            cartList = new CartResponseBuilder().BuildArrayResponse(response);
            return cartList;
        }


        /// <summary>
        /// Fetches Payment information based on Status
        /// </summary>
        /// <param name="paymentStatus"> Status of type which payment information has to be fetched</param>
        /// <returns>IList containing Cart objects. Payment history can be retrieved from individual Cart object.</returns>
        /// <exception>App42Exception</exception>
        public IList<Cart> GetPaymentsByStatus(String paymentStatus)
        {
            String response = null;
            IList<Cart> cartList = null;


            Util.ThrowExceptionIfNullOrBlank(paymentStatus, "PaymentStatus");

            if (!PaymentStatus.AUTHORIZED.ToString().Equals(paymentStatus) && !PaymentStatus.DECLINED.ToString().Equals(paymentStatus) && !PaymentStatus.PENDING.ToString().Equals(paymentStatus))
            {
                throw new App42Exception("Invalid Status. Could be either AUTHORIZED or UNAUTHORIZED or DECLINED");
            }
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp", Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("status", paymentStatus);

            // Construct a json body for Receive Message
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource
                    + "/payments" + "/status" + "/" + paymentStatus;
            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            cartList = new CartResponseBuilder().BuildArrayResponse(response);
            return cartList;
        }

        /// <summary>
        /// History of Carts and Payments for a User. It gives all the carts which are in AUTHORIZED, DECLINED, PENDING state.
        /// </summary>
        /// <param name="userId">User Id for whom payment history has to be fetched</param>
        /// <returns>IList containing Cart objects. Payment history can be retrieved from individual Cart object.</returns>
        ///<exception>App42Exception</exception>

        public IList<Cart> GetPaymentHistoryByUser(String userId)
        {
            String response = null;
            IList<Cart> cartList = null;
            Util.ThrowExceptionIfNullOrBlank(userId, "UserId");
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp", Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            paramsDics.Add("userId", userId);

            // Construct a json body for Receive Message
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource
                    + "/payment" + "/history" + "/" + userId;
            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            cartList = new CartResponseBuilder().BuildArrayResponse(response);
            return cartList;
        }

        /// <summary>
        /// History of all carts. It gives all the carts which are in AUTHORIZED, DECLINED, PENDING state.
        /// </summary>
        /// <returns>IList containing Cart objects. Payment history can be retrieved from individual Cart object.</returns>
        ///<exception>App42Exception</exception>
        public IList<Cart> GetPaymentHistoryAll()
        {
            String response = null;
            IList<Cart> cartList = null;
            Dictionary<String, String> paramsDics = new Dictionary<String, String>();

            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp", Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);

            // Construct a json body for Receive Message
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource
                    + "/payment" + "/history";
            response = RESTConnector.getInstance().ExecuteGet(signature,
                    resourceURL, queryParams);
            cartList = new CartResponseBuilder().BuildArrayResponse(response);
            return cartList;
        }


        /// <summary>
        /// Increase the quantity of specified Item to the specified Cart
        /// </summary>
        /// <param name="cartID">The cart id for which item has to be increased.</param>
        /// <param name="itemID">The item id that has to be increased.</param>
        /// <param name="itemQuantity">The quantity of the item that has to be increased.</param>
        /// <returns>Cart object containing updated item.</returns>
        ///<exception>App42Exception</exception>
        public Cart IncreaseQuantity(String cartID, String itemID, int itemQuantity)
        {
            String response = null;
            Cart cart = null;

            Util.ThrowExceptionIfNullOrBlank(cartID, "CartId");
            Util.ThrowExceptionIfNullOrBlank(itemID, "ItemId");

            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp", Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);
            StringBuilder sbJson = new StringBuilder();
            StringWriter sw = new StringWriter(sbJson);
            JsonWriter jsonWriter = new JsonTextWriter(sw);
            jsonWriter.WriteStartObject();
            jsonWriter.WritePropertyName("cartId");
            jsonWriter.WriteValue(cartID);
            jsonWriter.WritePropertyName("itemId");
            jsonWriter.WriteValue(itemID);
            jsonWriter.WritePropertyName("quantity");
            jsonWriter.WriteValue(itemQuantity);
            jsonWriter.WriteEndObject();
            StringBuilder sb = new StringBuilder();

            sb.Append("{\"app42\":{\"cart\":").Append(sbJson.ToString())
                    .Append("}}");
            App42Log.Debug("Created JSOn for Profile : " + sb);

            paramsDics.Add("body", sb.ToString());
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/increaseQuantity";
            response = RESTConnector.getInstance().ExecutePut(signature,
                    resourceURL, queryParams, sb.ToString());

            cart = new CartResponseBuilder().BuildResponse(response);
            return cart;
        }


        /// <summary>
        /// Decrease the quantity of specified Item from the specified Cart
        /// </summary>
        /// <param name="cartID">The cart id for which item has to be decreased.</param>
        /// <param name="itemID">The item id that has to be decreased.</param>
        /// <param name="itemQuantity">The quantity of the item that has to be decreased.</param>
        /// <returns> Cart object containing updated item.</returns>
        ///<exception>App42Exception</exception>

        public Cart DecreaseQuantity(String cartID, String itemID, int itemQuantity)
        {

            String response = null;
            Cart cart = null;
            Util.ThrowExceptionIfNullOrBlank(cartID, "CartId");
            Util.ThrowExceptionIfNullOrBlank(itemID, "ItemId");

            Dictionary<String, String> paramsDics = new Dictionary<String, String>();
            paramsDics.Add("apiKey", this.apiKey);
            paramsDics.Add("version", this.version);
            paramsDics.Add("timeStamp", Util.GetUTCFormattedTimestamp());
            Dictionary<String, String> queryParams = new Dictionary<String, String>(paramsDics);

            StringBuilder sbJson = new StringBuilder();
            StringWriter sw = new StringWriter(sbJson);
            JsonWriter jsonWriter = new JsonTextWriter(sw);
            jsonWriter.WriteStartObject();
            jsonWriter.WritePropertyName("cartId");
            jsonWriter.WriteValue(cartID);
            jsonWriter.WritePropertyName("itemId");
            jsonWriter.WriteValue(itemID);
            jsonWriter.WritePropertyName("quantity");
            jsonWriter.WriteValue(itemQuantity);
            jsonWriter.WriteEndObject();
            StringBuilder sb = new StringBuilder();

            sb.Append("{\"app42\":{\"cart\":").Append(sbJson.ToString())
                    .Append("}}");

            paramsDics.Add("body", sb.ToString());
            String signature = Util.Sign(this.secretKey, paramsDics);
            String resourceURL = this.version + "/" + this.resource + "/decreaseQuantity";
            response = RESTConnector.getInstance().ExecutePut(signature,
                    resourceURL, queryParams, sb.ToString());

            cart = new CartResponseBuilder().BuildResponse(response);
            return cart;
        }
    }
}