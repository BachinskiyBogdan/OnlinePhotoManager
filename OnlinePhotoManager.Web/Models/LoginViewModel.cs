using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlinePhotoManager.Web.Models
{
    public class LoginViewModel
    {
        [Required]
        [StringLength(32, ErrorMessage = "UserName must be at least 3 symbols and max 32 symbols.", MinimumLength = 3)]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z0-9]{1,32}$")]
        [Display(Name = "User Name")]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}