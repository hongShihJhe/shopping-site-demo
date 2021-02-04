using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Carts.Controllers
{
    public class TestController : Controller
    {
        public ActionResult GetCart()
        {

            //取得目前購物車
            var cart = Models.Operation.GetCurrentCart();

            ////如果購物車沒有商品，增加一筆
            cart.AddProduct(1);

            //回傳目前購物車的總價
            return Content(string.Format("目前購物車總共: {0}元", cart.TotalAmount));
        }
    }
}