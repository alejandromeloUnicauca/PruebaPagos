using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Prueba.DTOs;
using Prueba.Facade;
using Prueba.Models;
using Prueba.Services;
using Prueba.Utils;
using Prueba.ViewModel;

namespace Prueba.Controllers
{
    public class ComercioController : Controller
    {
        private readonly UnitOfWorkRepositories _unitOfWorkRepositories;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private List<SelectListItem> listItems;
        public ComercioController(ZonapagosContext context, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWorkRepositories = new UnitOfWorkRepositories(context);
            _httpContextAccessor = httpContextAccessor;
            listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem
            {
                Text = "Todas",
                Value = "0",
                Selected = true
            });
            listItems.Add(new SelectListItem
            {
                Text = "Usuario",
                Value = "1",
            });
            listItems.Add(new SelectListItem
            {
                Text = "Código",
                Value = "2",
            });
            listItems.Add(new SelectListItem
            {
                Text = "Fecha",
                Value = "3",
            });
        }

        [PermissionRequired("COMERCIO")]
        // GET: Comercio
        public async Task<IActionResult> Index()
        {

            try
            {
                ViewBag.listItems = this.listItems;
                double total = 0;
                string idComercio = _httpContextAccessor.HttpContext.Session.GetString("userid");
                ComercioService comercioService = new ComercioService(this._unitOfWorkRepositories);
                IndexComercioViewModel viewModel = null;
                if (TempData.Keys.Contains("viewmodel"))
                {
                    viewModel = JsonConvert.DeserializeObject<IndexComercioViewModel>(TempData["viewmodel"].ToString());
                    if (viewModel != null)
                    {
                        if (viewModel.filtro != null)
                        {
                            List<TransComercioDTO> transacciones = null;
                            switch (viewModel.filtro.filtro)
                            {
                                case "0"://Todos
                                    transacciones = comercioService.getPagos(int.Parse(idComercio));
                                    break;
                                case "1"://Filtro por usuario
                                    transacciones = comercioService.GetTransByUsuarioId(int.Parse(idComercio), viewModel.filtro.idUsuario);
                                    break;
                                case "2"://Filtro por código de transacción
                                    transacciones = comercioService.GetTransByCodigo(int.Parse(idComercio), viewModel.filtro.codigo);
                                    break; 
                                case "3"://Filtro por fechas

                                    if (!string.IsNullOrEmpty(viewModel.filtro.fechaI) && !string.IsNullOrEmpty(viewModel.filtro.fechaF))
                                    {
                                        DateTime fechai = DateTime.ParseExact(viewModel.filtro.fechaI, "yyyy-MM-dd", null);
                                        DateTime fechaf = DateTime.ParseExact(viewModel.filtro.fechaF, "yyyy-MM-dd", null);
                                        if (fechai <= fechaf)
                                            transacciones = comercioService.getTransByFechas(int.Parse(idComercio), fechai, fechaf);
                                        else
                                        {
                                            viewModel.filtro.mensajeError = "La fecha inicial debe ser menor o igual a la fecha final";
                                            goto case "0";
                                        }
                                    }
                                    else
                                    {
                                        viewModel.filtro.mensajeError = "Debe seleccionar la fecha inicial y final";
                                        goto case "0";
                                    }
                                    break;
                                default:
                                    goto case "0";
                                    break;
                            }
                            transacciones.ForEach(p =>
                            {
                                total += p.transTotal;
                            });
                            viewModel.transacciones = transacciones;
                            ViewBag.total = total;
                            return View(viewModel);
                        }
                    }
                }

                var pagos = comercioService.getPagos(int.Parse(idComercio));
                total = 0;
                pagos.ForEach(p =>
                {
                    total += p.transTotal;
                });
                ViewBag.total = total;

                viewModel = new IndexComercioViewModel();
                viewModel.filtro = new FiltroDTO();
                viewModel.transacciones = pagos;

                return View(viewModel);
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        [PermissionRequired("COMERCIO")]
        // GET: Comercio/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            ComercioService comercioService = new ComercioService(_unitOfWorkRepositories);
            var trans = await comercioService.getTransByCodigoAsync(id);
            if (trans == null)
                return NotFound();

            //Se valida que la transacción corresponda al comercio loqueado
            string idComercio = _httpContextAccessor.HttpContext.Session.GetString("userid");
            if (trans.ComercioCodigo != int.Parse(idComercio))
                return NotFound();

            var estados = await _unitOfWorkRepositories.TransEstadoRepository.GetAll();
            var medios = await _unitOfWorkRepositories.TransMedioPagoRepository.GetAll();

            ViewData["TransEstadoId"] = new SelectList(estados, "TransEstadoId", "TransEstadoNombre", trans.TransEstadoId);
            ViewData["TransMediopId"] = new SelectList(medios, "TransMediopId", "TransMediopNombre", trans.TransMediopId);

            TransComercioDTO transComercioDTO = new TransComercioDTO()
            {
                transCodigo = trans.TransCodigo,
                transConcepto = trans.TransConcepto,
                transEstado = trans.TransEstadoId.ToString(),
                transFecha = trans.TransFecha,
                transIdentificacion = trans.UsuarioIdentificacion,
                transMedioP = trans.TransMediopId.ToString(),
                transTotal = (double)trans.TransTotal,
                transUsuario = trans.UsuarioIdentificacionNavigation.UsuarioNombre
            };

            return View(transComercioDTO);
        }

        // POST: Comercio/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, TransComercioDTO transComercioDTO)
        {
            if (id != transComercioDTO.transCodigo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    ComercioService comercioService = new ComercioService(_unitOfWorkRepositories);
                    await comercioService.updateTransaccionAsync(transComercioDTO);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await TransExistsAsync(transComercioDTO.transCodigo))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            var estados = await _unitOfWorkRepositories.TransEstadoRepository.GetAll();
            var medios = await _unitOfWorkRepositories.TransMedioPagoRepository.GetAll();

            ViewData["TransEstadoId"] = new SelectList(estados, "TransEstadoId", "TransEstadoNombre", transComercioDTO.transEstado);
            ViewData["TransMediopId"] = new SelectList(medios, "TransMediopId", "TransMediopNombre", transComercioDTO.transMedioP);

            return View(transComercioDTO);
        }

        public IActionResult search(IndexComercioViewModel viewModel)
        {
            TempData["viewmodel"] = JsonConvert.SerializeObject(viewModel);
            return Redirect(Url.Action("Index", "Comercio"));
        }

        private async Task<bool> TransExistsAsync(string id)
        {
            ComercioService comercioService = new ComercioService(_unitOfWorkRepositories);
            var trans = await comercioService.getTransByCodigoAsync(id);
            return trans != null;
        }
    }
}
