using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Prueba.DTOs
{
    public class TransComercioDTO
    {
        [DisplayName("Codigo")]
        public string transCodigo { get; set; }
        [DisplayName("Fecha")]
        public string transFecha { get; set; }
        [DisplayName("Concepto")]
        public string transConcepto { get; set; }
        [DisplayName("Estado")]
        public string transEstado { get; set; }
        [DisplayName("Medio de pago")]
        public string transMedioP { get; set; }
        [DisplayName("Usuario")]
        public string transUsuario { get; set; }
        [DisplayName("Identificación")]
        public string transIdentificacion { get; set; }
        [DisplayName("Total")]
        public double transTotal { get; set; }
    }
}
