using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Carts.Models;
using Carts.Models.OrderModel;
using Microsoft.AspNet.Identity;

namespace Carts.Controllers
{
    public class OrderController : Controller
    {
        // GET: Order
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Ship postback)
        {
            if (ModelState.IsValid)
            {
                //取得目前購物車
                Cart currentCart = Operation.GetCurrentCart();

                //取得目前登入使用者id
                string userId = User.Identity.GetUserId();

                //
                using(CartsEntities db = new CartsEntities())
                {
                    //建立order物件
                    Order order = new Order 
                    { 
                        UserId = userId,
                        ReceiverName = postback.ReceiverName,
                        ReceiverPhone = postback.ReceiverPhone,
                        ReceiverAddress = postback.ReceiverAddress
                    };

                    //加入Orders資料表，儲存變更
                    db.Orders.Add(order);
                    db.SaveChanges();

                    //取得購物車中OrderDetail物件
                    List<OrderDetail> orderDetails = currentCart.ToOrderDetailList(order.Id);

                    //加入OrderDetail資料表，儲存變更
                    db.OrderDetails.AddRange(orderDetails);
                    db.SaveChanges();

                }

                return Content("訂購成功");
            }

            //資料驗證沒過
            return View(postback);
        }

     
        //end class
    }
}