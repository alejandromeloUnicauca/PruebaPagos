using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Prueba.Models
{
    public partial class Usuario
    {
        public Usuario()
        {
            Trans = new HashSet<Trans>();
        }

        public string UsuarioIdentificacion { get; set; }
        public string UsuarioNombre { get; set; }
        public string UsuarioEmail { get; set; }
        public string UsuarioPassword { get; set; }

        public virtual ICollection<Trans> Trans { get; set; }
    }
}
