using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Prueba.Models
{
    public partial class Comercio
    {
        public Comercio()
        {
            Trans = new HashSet<Trans>();
        }

        public int ComercioCodigo { get; set; }
        public string ComercioNombre { get; set; }
        public string ComercioNit { get; set; }
        public string ComercioDireccion { get; set; }
        public string ComercioPassword { get; set; }

        public virtual ICollection<Trans> Trans { get; set; }
    }
}
