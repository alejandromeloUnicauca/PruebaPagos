using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prueba.DTOs
{
    public class UserRegisterDTO
    {
        public string identificacion { get; set; }
        public string tipo { get; set; }
        public string password { get; set; }
        public string confirmpassword { get; set; }
    }
}
