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
    public class ComercioService
    {
        private readonly UnitOfWorkRepositories _unitOfWorkRepositories;

        public ComercioService(UnitOfWorkRepositories unitOfWorkRepositories)
        {
            _unitOfWorkRepositories = unitOfWorkRepositories;
        }
        /// <summary>
        /// Obtiene una lista de pagos de un usuario especifico
        /// </summary>
        /// <param name="identificacion">Identificación del usuario</param>
        /// <returns>Retorna una lista con los pagos filtrados por la identificación del usuario</returns>
        public List<TransComercioDTO> getPagos(int codigoComercio)
        {
            try
            {
                var pagos = _unitOfWorkRepositories.TransRepository.dbContext.Trans
                    .Include(t => t.UsuarioIdentificacionNavigation)
                    .Include(t => t.TransEstado)
                    .Include(t => t.TransMediop)
                    .Where(t => t.ComercioCodigo == codigoComercio).ToList();
                List<TransComercioDTO> pagosdto = new List<TransComercioDTO>();
                foreach (var pago in pagos)
                {
                    TransComercioDTO pagoTemp = new TransComercioDTO
                    {
                        transCodigo = pago.TransCodigo,
                        transIdentificacion = pago.UsuarioIdentificacionNavigation.UsuarioIdentificacion,
                        transUsuario = pago.UsuarioIdentificacionNavigation.UsuarioNombre,
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
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Obtiene una transacción filtrada por su código
        /// </summary>
        /// <param name="codigo">Bodigo de la transacción</param>
        /// <returns>Retorna un objeto con la transacción si el código existe en la base de datos de lo contrario retorna null</returns>
        public async Task<Trans> getTransByCodigoAsync(string codigo)
        {
            try
            {
                return _unitOfWorkRepositories.TransRepository.dbContext.Trans
                    .Include(t => t.UsuarioIdentificacionNavigation)
                    .Include(t => t.TransEstado)
                    .Include(t => t.TransMediop)
                    .Where(t => t.TransCodigo.Equals(codigo)).FirstOrDefault();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Actualiza la información de una transacción
        /// </summary>
        /// <param name="transaccionDTO">Recibe un objeto con la información de la transacción</param>
        /// <returns></returns>
        public async Task updateTransaccionAsync(TransComercioDTO transaccionDTO)
        {
            try
            {
                var transaccion = await getTransByCodigoAsync(transaccionDTO.transCodigo);
                if(transaccion != null)
                {
                    transaccion.TransEstadoId = int.Parse(transaccionDTO.transEstado);
                    transaccion.TransMediopId = int.Parse(transaccionDTO.transMedioP);
                    transaccion.TransTotal = (decimal)transaccionDTO.transTotal;
                    _unitOfWorkRepositories.TransRepository.Update(transaccion);
                    await _unitOfWorkRepositories.Save();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene las transacciones en un rango de fechas determinado para un comercio especifico
        /// </summary>
        /// <param name="idComercio">código del comercio </param>
        /// <param name="fechaI">Fecha inicial del rango</param>
        /// <param name="fechaF">Fecha final del rango</param>
        /// <returns>Retorna una lista con las transacciones en el rango de fechas determinado</returns>
        public List<TransComercioDTO> getTransByFechas(int idComercio, DateTime fechaI, DateTime fechaF)
        {
            try
            {
                var pagos = _unitOfWorkRepositories.TransRepository.dbContext.Trans
                   .Include(t => t.UsuarioIdentificacionNavigation)
                   .Include(t => t.TransEstado)
                   .Include(t => t.TransMediop)
                   .Where(t => t.ComercioCodigo.Equals(idComercio))
                   .ToList();
                List<TransComercioDTO> pagosdto = new List<TransComercioDTO>();

                foreach (var pago in pagos)
                {
                    try
                    {
                        var splitf = pago.TransFecha.Split(" ");

                        DateTime fecha = DateTime.ParseExact(splitf[0], "d/MM/yyyy", null);
                        if (fecha >= fechaI && fecha <= fechaF)
                        {
                            TransComercioDTO pagoTemp = new TransComercioDTO
                            {
                                transCodigo = pago.TransCodigo,
                                transIdentificacion = pago.UsuarioIdentificacionNavigation.UsuarioIdentificacion,
                                transUsuario = pago.UsuarioIdentificacionNavigation.UsuarioNombre,
                                transConcepto = pago.TransConcepto,
                                transEstado = pago.TransEstado.TransEstadoNombre,
                                transFecha = pago.TransFecha,
                                transMedioP = pago.TransMediop.TransMediopNombre,
                                transTotal = (double)pago.TransTotal
                            };
                            pagosdto.Add(pagoTemp);
                        }
                    }
                    catch { }
                }
                return pagosdto;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene las transacciones de un comercio filtradas por la identificación de un usuario
        /// </summary>
        /// <param name="idComercio">código del comercio</param>
        /// <param name="usuarioid">identificación del usuario</param>
        /// <returns>Retorna una lista de transacciones filtradas por el código del comercio y la identificación de un usuario</returns>
        public List<TransComercioDTO> GetTransByUsuarioId(int idComercio, string usuarioid)
        {
            try
            {
                var pagos = _unitOfWorkRepositories.TransRepository.dbContext.Trans
                   .Include(t => t.UsuarioIdentificacionNavigation)
                   .Include(t => t.TransEstado)
                   .Include(t => t.TransMediop)
                   .Where(t => t.UsuarioIdentificacion.Equals(usuarioid))
                   .Where(t => t.ComercioCodigo.Equals(idComercio))
                   .ToList();
                List<TransComercioDTO> pagosdto = new List<TransComercioDTO>();

                foreach (var pago in pagos)
                {
                    TransComercioDTO pagoTemp = new TransComercioDTO
                    {
                        transCodigo = pago.TransCodigo,
                        transIdentificacion = pago.UsuarioIdentificacionNavigation.UsuarioIdentificacion,
                        transUsuario = pago.UsuarioIdentificacionNavigation.UsuarioNombre,
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
        /// Obtiene las transacciones filtradas por código de transacción
        /// </summary>
        /// <param name="idComercio">código del comercio</param>
        /// <param name="codigo">código de la transacción</param>
        /// <returns></returns>
        public List<TransComercioDTO> GetTransByCodigo(int idComercio, string codigo)
        {
            try
            {
                var pagos = _unitOfWorkRepositories.TransRepository.dbContext.Trans
                   .Include(t => t.UsuarioIdentificacionNavigation)
                   .Include(t => t.TransEstado)
                   .Include(t => t.TransMediop)
                   .Where(t => t.TransCodigo.Equals(codigo))
                   .Where(t => t.ComercioCodigo.Equals(idComercio))
                   .ToList();
                List<TransComercioDTO> pagosdto = new List<TransComercioDTO>();

                foreach (var pago in pagos)
                {
                    TransComercioDTO pagoTemp = new TransComercioDTO
                    {
                        transCodigo = pago.TransCodigo,
                        transIdentificacion = pago.UsuarioIdentificacionNavigation.UsuarioIdentificacion,
                        transUsuario = pago.UsuarioIdentificacionNavigation.UsuarioNombre,
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
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
