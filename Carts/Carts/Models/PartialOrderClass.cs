using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Carts.Models
{
    public class PartialOrderClass
    {
    }

    //擴充Order類別
    public partial class Order
    {
        public string GetUserName(string userId)
        {
            using (UserEntities db = new UserEntities())
            {
                string result = (from s in db.AspNetUsers where s.Id == userId select s.UserName).FirstOrDefault();

                return result;
            }
        }

    }

}