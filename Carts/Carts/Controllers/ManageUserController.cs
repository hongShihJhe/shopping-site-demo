using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Carts.Models;

namespace Carts.Controllers
{
    public class ManageUserController : Controller
    {
        // GET: UserManage
        public ActionResult Index()
        {
            //接收導向的訊息
            ViewBag.ResultMessage = TempData["ResultMessage"];

            List<ManageUser> result = new List<ManageUser>();
            using (UserEntities db = new UserEntities()) 
            {
                result = (from s in db.AspNetUsers select new ManageUser 
                { 
                    Id = s.Id,
                    Email = s.Email,
                    UserName = s.UserName
                }).ToList();
            }

            return View(result);
        }

        //編輯會員頁面
        public ActionResult Edit(string id)
        {
            //抓取輸入id的資料
            ManageUser result = new ManageUser();
            using (UserEntities db = new UserEntities())
            {
                result = (from s in db.AspNetUsers where s.Id == id select new ManageUser 
                {
                    Id = s.Id,
                    UserName = s.UserName,
                    Email = s.Email
                }).FirstOrDefault();
            }

            //判斷此id是否有資料
            if (result != default(ManageUser))
            {
                return View(result);
            }
            else  //如果沒有資料
            {
                TempData["ResultMessage"] = "資料有誤，請檢查";
                return RedirectToAction("Index");
            }
        }

        //編輯會員頁面 - 資料回傳處理
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ManageUser postback)
        {
            if (ModelState.IsValid)  //如果資料驗證成功
            {
                using (UserEntities db = new UserEntities())
                {
                    //抓取對應id的資料
                    AspNetUsers result = (from s in db.AspNetUsers where s.Id == postback.Id select s).FirstOrDefault();

                    //資料更新
                    if (TryUpdateModel(result, new string[]{ "UserName", "Email" }))
                    {
                        //儲存變更
                        db.SaveChanges();

                        //設定成功訊息
                        TempData["ResultMessage"] = String.Format("會員[{0}]編輯成功", postback.UserName);

                        //導向Index頁面
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        //設定成功訊息
                        TempData["ResultMessage"] = String.Format("會員[{0}]編輯失敗", postback.UserName);

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

        //刪除會員
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id)
        {
            using (UserEntities db = new UserEntities())
            {
                //抓取對應id的資料
                AspNetUsers result = (from s in db.AspNetUsers where s.Id == id select s).FirstOrDefault();

                if (result != default(AspNetUsers)) //判斷是否有資料
                {
                    db.AspNetUsers.Remove(result);

                    //儲存變更
                    db.SaveChanges();

                    //設定成功訊息
                    TempData["ResultMessage"] = String.Format("會員[{0}]刪除成功", result.UserName);

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