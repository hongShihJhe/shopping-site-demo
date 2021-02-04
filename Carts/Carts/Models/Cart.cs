using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Carts.Models
{
    [Serializable] //序列化
    public class Cart: IEnumerable<CartItem> //購物車類別
    {
        public Cart()
        {
            this.cartItems = new List<CartItem>();
        }

        //儲存所有商品
        private List<CartItem> cartItems;

        /// <summary>
        /// 購物車的商品數量
        /// </summary>
        public int Count
        {
            get
            {
                return cartItems.Count;
            }
        }

        //新增一筆Product，使用Product物件
        private bool AddProduct(Product product)
        {
            //將Product轉為CartItem
            CartItem cartItem = new CartItem 
            { 
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Quantity = 1,
                DefaultImageURL = product.DefaultImageURL
            };

            this.cartItems.Add(cartItem);

            return true;
        }

        //新增一筆Product，使用商品編號
        public bool AddProduct(int ProductId)
        {
            CartItem findItem = this.cartItems
                .Where(s => s.Id == ProductId)
                .Select(s => s)
                .FirstOrDefault();

            //判斷商品是否已在購物車
            if (findItem == default(CartItem)) //商品不存在，新增一筆
            {
                //取得商品
                Product product = new Product();
                using(CartsEntities db = new CartsEntities())
                {
                    product = (from s in db.Products where s.Id == ProductId select s).FirstOrDefault();
                }

                //如果有這個商品
                if (product != default(Product))
                {
                    this.AddProduct(product);
                }
            }
            else //商品已存在，數量+1
            {
                findItem.Quantity += 1;
            }

            return true;
        }

        //移除一筆商品
        public bool RemoveProduct(int ProductId)
        {
            CartItem findItem = this.cartItems
                .Where(s => s.Id == ProductId)
                .Select(s => s)
                .FirstOrDefault();

            if (findItem != default(CartItem))
            {
                this.cartItems.Remove(findItem);
            }

            return true;
        }

        public bool ClearCart()
        {
            this.cartItems.Clear();
            return true;
        }

        //取得商品總價
        public decimal TotalAmount
        {
            get
            {
                decimal totalAmount = 0.0m;
                foreach (CartItem cartItem in cartItems)
                {
                    totalAmount += cartItem.Quantity * cartItem.Price;
                }
                return totalAmount;
            }
        }

        //將 購物車商品項目 轉成 訂單細節清單，傳入訂單Id
        public List<OrderDetail> ToOrderDetailList(int orderId)
        {
            List<OrderDetail> result = new List<OrderDetail>();

            foreach (CartItem cartItem in this.cartItems)
            {
                result.Add(new OrderDetail { 
                    OrderId = orderId,
                    Name = cartItem.Name,
                    Price = cartItem.Price,
                    Quantity = cartItem.Quantity
                });
            }

            return result;
        }

        public IEnumerator<CartItem> GetEnumerator()
        {
            return ((IEnumerable<CartItem>)cartItems).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)cartItems).GetEnumerator();
        }

        

    }
}