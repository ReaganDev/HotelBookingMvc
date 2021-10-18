using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_model.AuthModels
{
    public class LoginModel
    {

        [Required(ErrorMessage ="Enter Email To Continue")]
        public string Email { get; set; }

        [Required(ErrorMessage ="Enter Password To Continue")]
        public string Password { get; set; }
    }
}