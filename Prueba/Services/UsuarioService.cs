using Microsoft.EntityFrameworkCore;
using Prueba.DTOs;
using Prueba.Facade;
using Prueba.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prueba.Services
{
    public class UsuarioService
    {
        private readonly UnitOfWorkRepositories _unitOfWorkRepositories;
        public UsuarioService(UnitOfWorkRepositories unitOfWorkRepositories)
        {
            _unitOfWorkRepositories = unitOfWorkRepositories;
        }

        /// <summary>
        /// Obtiene una lista de pagos de un usuario especifico
        /// </summary>
        /// <param name="identificacion">Identificación del usuario</param>
        /// <returns>Retorna una lista con los pagos filtrados por la identificación del usuario</returns>
        public async Task<List<TransUsuarioDTO>> getPagos(string identificacion)
        {
            try
            {
                var pagos = _unitOfWorkRepositories.TransRepository.dbContext.Trans
                    .Include(t => t.ComercioCodigoNavigation)
                    .Include(t => t.TransEstado)
                    .Include(t => t.TransMediop)
                    .Where(t => t.UsuarioIdentificacion == identificacion).ToList();
                List<TransUsuarioDTO> pagosdto = new List<TransUsuarioDTO>();
                foreach (var pago in pagos)
                {
                    TransUsuarioDTO pagoTemp = new TransUsuarioDTO
                    {
                        transCodigo = pago.TransCodigo,
                        transComercio = pago.ComercioCodigoNavigation.ComercioNombre,
                        transConcepto = pago.TransConcepto,
                        transEstado = pago.TransEstado.TransEstadoNombre,
                        transFecha = pago.TransFecha,
                        transMedioP = pago.TransMediop.TransMediopNombre,
                        transTotal = (double)pago.TransTotal
                    };
                    pagosdto.Add(pagoTemp);
                }
                return pagosdto;
            }
            catch(Exception ex)
            {
                throw ex;
            }
           
        }

        /// <summary>
        /// Guarda en base de datos una transacción para de un usuario en especifico
        /// </summary>
        /// <param name="transaccion">Objeto con los datos de la transacción</param>
        /// <param name="identificacion">Identificación del usuario que se le asignara la transacción</param>
        /// <returns></returns>
        public async Task guardarPagoAsync(TransUsuarioDTO transaccion, string identificacion)
        {
            try
            {
                long ultimoCodigo = long.Parse((await _unitOfWorkRepositories.TransRepository.GetAll()).ToList().OrderBy(t => long.Parse(t.TransCodigo)).LastOrDefault().TransCodigo);
                Trans trans = new Trans
                {
                    TransCodigo = (++ultimoCodigo).ToString(),
                    TransEstadoId = 999,
                    TransFecha = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"),
                    TransConcepto = transaccion.transConcepto,
                    TransMediopId = int.Parse(transaccion.transMedioP),
                    TransTotal = (decimal)transaccion.transTotal,
                    UsuarioIdentificacion = identificacion,
                    ComercioCodigo = int.Parse(transaccion.transComercio)
                };
                _unitOfWorkRepositories.TransRepository.Add(trans);
                await _unitOfWorkRepositories.Save();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
