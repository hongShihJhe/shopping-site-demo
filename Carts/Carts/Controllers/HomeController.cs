using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Carts.Models;
using Microsoft.AspNet.Identity;

namespace Carts.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //取得商品資料
            List<Product> products = new List<Product>();
            using(CartsEntities db = new CartsEntities())
            {
                products = (from s in db.Products select s).ToList();
            }

            return View(products);
        }

        //顯示某編號的商品資訊
        public ActionResult Details(int id)
        {
            //宣告商品
            Product product;

            //取得商品資料
            using(CartsEntities db = new CartsEntities())
            {
                product = (from s in db.Products where s.Id == id select s).FirstOrDefault();
            }

            //判斷是否有此商品
            if (product != default(Product))
            {
                return View(product);
            }
            else  //如果商品不存在
            {
                //導回首頁
                return RedirectToAction("Index");
            }
        }

        //新增商品留言
        [HttpPost]
        [Authorize] //登入會員才可留言
        public ActionResult AddComment(int productId, string commentContent)
        {
            //如果留言為空
            if (string.IsNullOrEmpty(commentContent))
            {
                return RedirectToAction("Details", new { id = productId });
            }

            string userId = User.Identity.GetUserId();

            DateTime datetime = DateTime.Now;

            ProductComment productComment = new ProductComment 
            { 
                ProductId = productId,
                Content = commentContent,
                UserId = userId,
                CreateDate = datetime
            };

            using (CartsEntities db = new CartsEntities())
            {
                db.ProductComments.Add(productComment);
                db.SaveChanges();
            }

            return RedirectToAction("Details", new { id = productId });
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}