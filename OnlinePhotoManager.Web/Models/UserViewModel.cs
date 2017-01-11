using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlinePhotoManager.Web.Models
{
    public class UserViewModel
    {
        public bool IsPremium { get; set; }
        [Required]
        [StringLength(32, ErrorMessage = "UserName must be at least 3 symbols and max 32 symbols.", MinimumLength = 3)]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z0-9]{1,32}$")]
        [Display(Name = "User Name")]
        public string UserTag { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [StringLength(16, ErrorMessage = "Password must be at least 6 symbols and max 16 symbols.", MinimumLength = 6)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string Password2 { get; set; }
        [RegularExpression(@"^[a-zA-Z]{2,32}$")]
        public string FirstName { get; set; }
        [RegularExpression(@"^[a-zA-Z]{2,32}$")]
        public string LastName { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9]{1,32}[@][a-zA-Z]{1,10}[.][a-zA-Z0-9]{1,5}$")]
        public string Email { get; set; }
        [Required]
        [Compare("Email")]
        [RegularExpression(@"^[a-zA-Z0-9]{1,32}[@][a-zA-Z]{1,10}[.][a-zA-Z0-9]{1,5}$")]
        public string Email2 { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        public byte[] IconData { get; set; }
        public string IconMimeType { get; set; }
        [DataType(DataType.MultilineText)]
        [StringLength(500)]
        public string Description { get; set; }
    }
}