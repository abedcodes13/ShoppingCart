using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ShoppingCart.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [PersonalData]
        public string? Name { get; set; }

        [PersonalData]
        public string? ProfilePictureUrl { get; set; } // Store the profile picture URL or path
    }


}
