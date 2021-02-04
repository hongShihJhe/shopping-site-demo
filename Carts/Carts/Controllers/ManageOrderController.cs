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
    public class ManageOrderController : Controller
    {
        // GET: ManageOrder
        public ActionResult Index()
        {
            //取得所有訂單資料
            List<Order> orderList;
            using (CartsEntities db = new CartsEntities())
            {
                orderList = (from s in db.Orders select s).ToList();
            }

            //傳入List<Order>
            return View(orderList);
        }

        //傳入訂單的id
        public ActionResult Details(int id)
        {
            //取得訂單id的所有商品列表
            List<OrderDetail> result = new List<OrderDetail>();
            using(CartsEntities db = new CartsEntities())
            {
                result = (from s in db.OrderDetails where s.OrderId == id select s).ToList();
            }

            //如果商品數目為0，代表該訂單異常，導回商品列表
            if (result.Count == 0)
            {
                return RedirectToAction("Index");
            }
            
            //傳入List<Order>
            return View(result);
        }

        public ActionResult MyOrder()
        {
            //取得目前登入使用者id
            string userId = User.Identity.GetUserId();

            //取得該使用者的訂單資料
            List<Order> orderList;
            using (CartsEntities db = new CartsEntities())
            {
                orderList = (from s in db.Orders where s.UserId == userId select s).ToList();
            }

            //訂單的使用者id清單
            List<string> userIdList = orderList.Select(s => s.UserId).ToList();

            //關聯的使用者名稱清單
            List<string> userNameList;
            using (UserEntities userEntities = new UserEntities())
            {
                userNameList = (from s in userEntities.AspNetUsers where userIdList.Contains(s.Id) select s.UserName).ToList();
            }

            ViewBag.userNameList = userNameList;

            return View(orderList);
        }

        //傳入訂單的id
        public ActionResult MyOrderDetail(int id)
        {
            //目前登入使用者id
            string userId = User.Identity.GetUserId();

            //使用者的訂單編號list
            List<int> userOrderIds;

            //取得訂單細節
            List<OrderDetail> orderDetails = new List<OrderDetail>();
            using (CartsEntities db = new CartsEntities())
            {
                //傳入的訂單id要在使用者的訂單id清單中
                userOrderIds = (from s in db.Orders where s.UserId == userId select s.Id).ToList();
                orderDetails = (from s in db.OrderDetails where s.OrderId == id && userOrderIds.Contains(id) select s).ToList();
            }

            //如果商品數目為0，代表該訂單異常，導回商品列表
            if (orderDetails.Count == 0)
            {
                return RedirectToAction("Index");
            }

            return View(orderDetails);
        }

        public ActionResult SearchByUserName(string UserName)
        {
            //如果UserName為空，顯示所有訂單
            if (string.IsNullOrEmpty(UserName))
            {
                //取得所有訂單資料
                List<Order> orderList;
                using (CartsEntities db = new CartsEntities())
                {
                    orderList = (from s in db.Orders select s).ToList();
                }

                return View("Index", orderList);
            }

            //透過傳入UserName取得UserId
            string userId;
            using(UserEntities db = new UserEntities())
            {
                userId = (from s in db.AspNetUsers where s.UserName == UserName select s.Id).FirstOrDefault();
            }

            if (!string.IsNullOrEmpty(userId)) //如果此userId存在
            {
                //透過UserId取得Order List
                List<Order> orders;
                using(CartsEntities db = new CartsEntities())
                {
                    orders = (from s in db.Orders where s.UserId == userId select s).ToList();
                }

                //導向訂單列表
                return View("Index", orders);
            }
            else //此userId是null or empty
            {
                //導向訂單列表，傳入空的List<Order>
                return View("Index", new List<Order>());
            }
        }

        //end class
    }
}