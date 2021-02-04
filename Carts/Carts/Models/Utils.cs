using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Carts.Models
{
    public class Utils
    {
        public string GetUserName(string userId)
        {
            using (UserEntities db = new UserEntities())
            {
                string result = (from s in db.AspNetUsers where s.Id == userId select s.UserName).FirstOrDefault();

                return result;
            }
        }

        /// <summary>
        /// 傳入user id清單，回傳user name清單
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        public static List<string> GetUserNames(List<string> userIds)
        {
            List<string> userNames;
            using (UserEntities db = new UserEntities())
            {
                //UserId List映射UserName List 
                userNames = userIds.Select(userId => (from s in db.AspNetUsers where s.Id == userId select s.UserName).FirstOrDefault()).ToList();
            }

            return userNames;
        }

        //end class
    }
}