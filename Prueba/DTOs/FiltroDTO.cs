using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prueba.DTOs
{
    public class FiltroDTO
    {
        public string filtro { get; set; }
        public string codigo { get; set; }
        public string idUsuario { get; set; }
        public string fechaI { get; set; }
        public string fechaF { get; set; }
        public string mensajeError { get; set; }
    }
}
