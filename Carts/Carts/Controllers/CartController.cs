using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Carts.Models;

namespace Carts.Controllers
{
    public class CartController : Controller
    {
        //取得目前購物車頁面
        public ActionResult GetCart()
        {
            return PartialView("_CartPartial");
        }

        //以id將Product加入至購物車，回傳購物車頁面
        public ActionResult AddToCart(int id)
        {
            Cart currentCart = Operation.GetCurrentCart();
            currentCart.AddProduct(id);
            return PartialView("_CartPartial");
        }

        public ActionResult RemoveFromCart(int id)
        {
            Cart currentCart = Operation.GetCurrentCart();
            currentCart.RemoveProduct(id);
            return PartialView("_CartPartial");
        }

        public ActionResult ClearCart()
        {
            Cart currentCart = Operation.GetCurrentCart();
            currentCart.ClearCart();
            return PartialView("_CartPartial");
        }

    }
}