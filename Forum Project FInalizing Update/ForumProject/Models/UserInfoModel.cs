using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ForumProject.Models
{
    public class UserInfoModel
    {
        [Display(Name = "FullName")]
        public string FullName { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }
    }
}