using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace com.shephertz.app42.paas.sdk.csharp.shopping
{
    public class CartResponseBuilder : App42ResponseBuilder
    {

        public Cart BuildResponse(String json)
        {
            JObject cartsJSONObj = GetServiceJSONObject("carts", json);
            JObject cartJSONObj = (JObject)cartsJSONObj["cart"];
            Cart cart = BuildCartObject(cartJSONObj);
            cart.SetStrResponse(json);
            cart.SetResponseSuccess(IsResponseSuccess(json));
            return cart;
        }
        /// <summary>
        /// @throws Exception
        /// </summary>
        /// <param name="json">cartJSONObj</param>

        private Cart BuildCartObject(JObject cartJSONObj)
        {

            Cart cart = new Cart();
            BuildObjectFromJSONTree(cart, cartJSONObj);
            if (cartJSONObj["items"] != null && cartJSONObj["items"]["item"] != null)
            {

                // Fetch Items
                if (cartJSONObj["items"]["item"] != null && cartJSONObj["items"]["item"] is JObject)
                {
                    // Single Item

                    JObject itemJSONObj = (JObject)cartJSONObj["items"]["item"];
                    Cart.Item item = new Cart.Item(cart);
                    BuildObjectFromJSONTree(item, itemJSONObj);
                }
                else
                {
                    // Multiple Items
                    JArray itemJSONArray = (JArray)cartJSONObj["items"]["item"];
                    for (int i = 0; i < itemJSONArray.Count; i++)
                    {
                        JObject itemJSONObj = (JObject)itemJSONArray[i];
                        Cart.Item item = new Cart.Item(cart);
                        BuildObjectFromJSONTree(item, itemJSONObj);
                    }

                }
            }
            if (cartJSONObj["payments"] != null && cartJSONObj["payments"]["payment"] != null)
            {

                // Fetch Payment
                JObject paymentJSONObj = (JObject)cartJSONObj["payments"]["payment"];
                Cart.Payment payment = new Cart.Payment(cart);
                BuildObjectFromJSONTree(payment, paymentJSONObj);
            }
            return cart;

        }
        /// <summary>
        /// @throws Exception
        /// </summary>
        /// <param name="json">json</param>


        public IList<Cart> BuildArrayResponse(String json)
        {
            JObject cartsJSONObj = GetServiceJSONObject("carts", json);
            IList<Cart> cartList = new List<Cart>();
            if (cartsJSONObj["cart"] != null && cartsJSONObj["cart"] is JArray)
            {
                JArray cartJSONArray = (JArray)cartsJSONObj["cart"];

                for (int i = 0; i < cartJSONArray.Count; i++)
                {
                    JObject cartJSONObj = (JObject)cartJSONArray[i];
                    Cart cart = BuildCartObject(cartJSONObj);
                    cart.SetStrResponse(json);
                    cart.SetResponseSuccess(IsResponseSuccess(json));
                    cartList.Add(cart);
                }
            }
            else
            {
                JObject cartJSONObj = (JObject)cartsJSONObj["cart"];
                Cart cart = BuildCartObject(cartJSONObj);
                cart.SetStrResponse(json);
                cart.SetResponseSuccess(IsResponseSuccess(json));
                cartList.Add(cart);
            }
            return cartList;
        }
    }
}