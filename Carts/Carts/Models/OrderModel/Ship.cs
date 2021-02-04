using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Carts.Models.OrderModel
{
    public class Ship
    {
        [Required]
        [Display(Name = "收貨人姓名")]
        [StringLength(30, ErrorMessage = "{0} 的長度至少必須為 {2}", MinimumLength = 2)]  //字元長度2~30
        public string ReceiverName { get; set; }

        [Required]
        [Display(Name = "收貨人電話")]
        [StringLength(15, ErrorMessage = "{0} 的長度至少必須為 {2}", MinimumLength = 8)]  //字元長度8~15
        public string ReceiverPhone { get; set; }

        [Required]
        [Display(Name = "收貨人地址")]
        [StringLength(256, ErrorMessage = "{0} 的長度至少必須為 {2}", MinimumLength = 8)]  //字元長度8~256
        public string ReceiverAddress { get; set; }
    }
}