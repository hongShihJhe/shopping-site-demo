using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Carts.Models;

namespace Carts.Models
{
    public static class Operation
    {
        [WebMethod(EnableSession = true)] //啟用Session
        public static Cart GetCurrentCart() //取得目前Session中的Cart
        {
            if (HttpContext.Current != null)  //確認HttpContext.Current是否為空
            {
                //如果Session["Cart"]不存在，新增一個空的Cart
                if (HttpContext.Current.Session["Cart"] == null)
                {
                    Cart order = new Cart();
                    HttpContext.Current.Session["Cart"] = order;
                }
                
                return (Cart)HttpContext.Current.Session["Cart"];
            }
            else
            {
                throw new InvalidOperationException("System.Web.HttpContext.Current為空，請檢查");
            }
           
        }
    }
}