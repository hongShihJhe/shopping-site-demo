using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Carts.Models
{
    public class ManageUser
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [StringLength(maximumLength:256, ErrorMessage = "{0} 的長度至少必須為 {2} 字元", MinimumLength = 1)] //字元長度1~256
        [Display(Name ="暱稱")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name ="電子郵件")]
        public string Email { get; set; }
    }

    
   
}