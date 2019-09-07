

namespace PANDA.App.Models.User
{

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    public class UserRegisterInputModel
    {
        [Required]
        [StringLength(35, ErrorMessage ="Username must be between 5 and 35 symbols", MinimumLength =3 )]
        public string Username { get; set; }

        [Required]
        [StringLength(35, ErrorMessage = "Password must be between 5 and 35 symbols", MinimumLength = 3)]
        public string Password { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }
        [Required]
        public string Email { get; set; }

    }
}
