using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Prueba.Models
{
    public partial class TransEstado
    {
        public TransEstado()
        {
            Trans = new HashSet<Trans>();
        }

        public int TransEstadoId { get; set; }
        public string TransEstadoNombre { get; set; }

        public virtual ICollection<Trans> Trans { get; set; }
    }
}
