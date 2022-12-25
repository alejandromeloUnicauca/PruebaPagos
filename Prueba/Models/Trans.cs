using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Prueba.Models
{
    public partial class Trans
    {
        public string TransCodigo { get; set; }
        public decimal TransTotal { get; set; }
        public string TransFecha { get; set; }
        public string TransConcepto { get; set; }
        public int TransEstadoId { get; set; }
        public int TransMediopId { get; set; }
        public int ComercioCodigo { get; set; }
        public string UsuarioIdentificacion { get; set; }

        public virtual Comercio ComercioCodigoNavigation { get; set; }
        public virtual TransEstado TransEstado { get; set; }
        public virtual TransMedioPago TransMediop { get; set; }
        public virtual Usuario UsuarioIdentificacionNavigation { get; set; }
    }
}
