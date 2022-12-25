using Prueba.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prueba.ViewModel
{
    public class IndexComercioViewModel
    {
        public FiltroDTO filtro { get; set; }
        public List<TransComercioDTO> transacciones { get; set; }
    }
}
