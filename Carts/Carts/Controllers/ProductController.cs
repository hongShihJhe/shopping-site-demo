using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Carts.Models;

namespace Carts.Controllers
{
    public class ProductController : Controller
    {
        //商品列表頁面
        public ActionResult Index()
        {
            //宣告商品列表
            List<Product> result = new List<Product>();

            using (CartsEntities db = new CartsEntities())
            {
                result = (from s in db.Products select s).ToList();
            }

            //接收導向的訊息
            ViewBag.ResultMessage = TempData["ResultMessage"];

            return View(result);
        }

        //新增商品頁面
        public ActionResult Create()
        {
            return View();
        }

        //新增商品頁面 - 資料回傳處理
        [HttpPost]
        public ActionResult Create(Product postback)
        {
            if (ModelState.IsValid)  //如果資料驗證成功
            {
                using (CartsEntities db = new CartsEntities())
                {
                    db.Products.Add(postback);

                    db.SaveChanges();

                    //設定成功訊息
                    TempData["ResultMessage"] = String.Format("商品[{0}]建立成功", postback.Name);

                    //導向Index頁面
                    return RedirectToAction("Index");
                }
            }

            //失敗訊息
            TempData["ResultMessage"] = "資料有誤，請檢查";

            //停留在Create頁面
            return View(postback);
        }

        //編輯商品頁面
        public ActionResult Edit(int id)
        {
            //抓取輸入id的資料
            Product result = new Product();
            using (CartsEntities db = new CartsEntities())
            {
                result = (from s in db.Products where s.Id == id select s).FirstOrDefault();
            }

            //判斷此id是否有資料
            if (result != default(Product)) 
            {
                return View(result);
            }
            else  //如果沒有資料
            {
                TempData["ResultMessage"] = "資料有誤，請檢查";
                return RedirectToAction("Index");
            }          
        }

        //編輯商品頁面 - 資料回傳處理
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product postback)
        {
            if (ModelState.IsValid)  //如果資料驗證成功
            {
                using (CartsEntities db = new CartsEntities())
                {
                    //抓取對應id的資料
                    Product result = (from s in db.Products where s.Id == postback.Id select s).FirstOrDefault();

                    //資料更新
                    if (TryUpdateModel(result))
                    {
                        //儲存變更
                        db.SaveChanges();

                        //設定成功訊息
                        TempData["ResultMessage"] = String.Format("商品[{0}]編輯成功", postback.Name);

                        //導向Index頁面
                        return RedirectToAction("Index");
                    } 
                    else
                    {
                        //設定成功訊息
                        TempData["ResultMessage"] = String.Format("商品[{0}]編輯失敗", postback.Name);

                        //導向Index頁面
                        return RedirectToAction("Index");
                    }
                }
            }

            //失敗訊息
            TempData["ResultMessage"] = "資料有誤，請檢查";

            //停留在Edit頁面
            return View(postback);
        }

        //刪除商品
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            using(CartsEntities db = new CartsEntities())
            {
                //抓取對應id的資料
                Product result = (from s in db.Products where s.Id == id select s).FirstOrDefault();

                if (result != default(Product)) //判斷是否有資料
                {
                    db.Products.Remove(result);

                    //儲存變更
                    db.SaveChanges();

                    //設定成功訊息
                    TempData["ResultMessage"] = String.Format("商品[{0}]刪除成功", result.Name);

                    //導向Index頁面
                    return RedirectToAction("Index");
                }
                else
                {

                    TempData["ResultMessage"] = "指定資料不存在";

                    //導向Index頁面
                    return RedirectToAction("Index");
                }
            }

        }


        //end class
    }
}