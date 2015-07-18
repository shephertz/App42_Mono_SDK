using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.shephertz.app42.paas.sdk.csharp.shopping
{
    public class Cart : App42Response
    {

        public String userName;
        public String cartId;
        public DateTime creationTime;
        public DateTime checkOutTime;
        public String state;
        public Boolean isEmpty;
        public String cartSession;
        public Double totalAmount;
        public IList<Item> itemList = new List<Cart.Item>();
        public Payment payment;

        public String GetUserName()
        {
            return userName;
        }

        public void SetUserName(String userName)
        {
            this.userName = userName;
        }

        public String GetCartId()
        {
            return cartId;
        }

        public void SetCartId(String cartId)
        {
            this.cartId = cartId;
        }

        public DateTime GetCreationTime()
        {
            return creationTime;
        }

        public void SetCreationTime(DateTime creationTime)
        {
            this.creationTime = creationTime;
        }

        public DateTime GetCheckOutTime()
        {
            return checkOutTime;
        }

        public void SetCheckOutTime(DateTime checkOutTime)
        {
            this.checkOutTime = checkOutTime;
        }

        public String GetState()
        {
            return state;
        }

        public void SetState(String state)
        {
            this.state = state;
        }

        public Boolean GetIsEmpty()
        {
            return isEmpty;
        }

        public void SetIsEmpty(Boolean isEmpty)
        {
            this.isEmpty = isEmpty;
        }

        public String GetCartSession()
        {
            return cartSession;
        }

        public void SetCartSession(String cartSession)
        {
            this.cartSession = cartSession;
        }

        public Double GetTotalAmount()
        {
            return totalAmount;
        }

        public void SetTotalAmount(Double totalAmount)
        {
            this.totalAmount = totalAmount;
        }

        public IList<Item> GetItemList()
        {
            return itemList;
        }

        public void SetItemList(IList<Item> itemList)
        {
            this.itemList = itemList;
        }

        public Payment GetPayment()
        {
            return payment;
        }

        public void SetPayment(Payment payment)
        {
            this.payment = payment;
        }

        public class Item
        {

            public Item(Cart item)
            {
                item.itemList.Add(this);
            }

            public String itemId;
            public Int64 quantity;
            public String name;
            public String image;
            public Double price;
            public Double totalAmount;

            public String GetItemId()
            {
                return itemId;
            }

            public void SetItemId(String itemId)
            {
                this.itemId = itemId;
            }

            public Int64 GetQuantity()
            {
                return quantity;
            }

            public void SetQuantity(Int64 quantity)
            {
                this.quantity = quantity;
            }

            public String GetName()
            {
                return name;
            }

            public void SetName(String name)
            {
                this.name = name;
            }

            public String GetImage()
            {
                return image;
            }

            public void SetImage(String image)
            {
                this.image = image;
            }

            public Double GetPrice()
            {
                return price;
            }

            public void SetPrice(Double price)
            {
                this.price = price;
            }

            public Double GetTotalAmount()
            {
                return totalAmount;
            }

            public void SetTotalAmount(Double totalAmount)
            {
                this.totalAmount = totalAmount;
            }

            public override String ToString()
            {
                return " name : " + name + " : itemId : " + itemId
                        + " : price : " + price + " : quantity : " + quantity;

            }

        }

        public class Payment
        {

            public Payment(Cart cart)
            {
                cart.payment = this;
            }

            public String transactionId;
            public Double totalAmount;
            public String status;
            public DateTime date;

            public String GetTransactionId()
            {
                return transactionId;
            }

            public void SetTransactionId(String transactionId)
            {
                this.transactionId = transactionId;
            }

            public Double GetTotalAmount()
            {
                return totalAmount;
            }

            public void SetTotalAmount(Double totalAmount)
            {
                this.totalAmount = totalAmount;
            }

            public String GetStatus()
            {
                return status;
            }

            public void SetStatus(String status)
            {
                this.status = status;
            }

            public DateTime GetDate()
            {
                return date;
            }

            public void SetDate(DateTime date)
            {
                this.date = date;
            }
        }
    }
}
