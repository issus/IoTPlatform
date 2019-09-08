using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace IoTPlatform.Data.Identity
{
    public class WebProfileUser : IdentityUser
    {
        [Required]
        [Display(Name = "Date Registered")]
        public DateTime DateRegistered { get; set; }

        [Display(Name = "Date Email Confirmed")]
        public DateTime? DateEmailConfirmed { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [DisplayFormat(ConvertEmptyStringToNull = true)]
        public string LastName { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = true)]
        public string Company { get; set; }

        [Display(Name = "Avatar Path")] public string AvatarPath { get; set; }
    }
}