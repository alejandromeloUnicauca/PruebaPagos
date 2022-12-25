using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Prueba.DTOs;
using Prueba.Facade;
using Prueba.Models;
using Prueba.Services;
using Prueba.Utils;

namespace Prueba.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly UnitOfWorkRepositories _unitOfWorkRepositories;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsuarioController(ZonapagosContext context, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWorkRepositories = new UnitOfWorkRepositories(context);
            _httpContextAccessor = httpContextAccessor;
        }

        [PermissionRequired("PERSONA")]
        // GET: Usuario
        public async Task<IActionResult> Index()
        {
            try
            {
                UsuarioService usuarioService = new UsuarioService(this._unitOfWorkRepositories);
                string identificacion = _httpContextAccessor.HttpContext.Session.GetString("userid");
                var pagos = await usuarioService.getPagos(identificacion);
                return View(pagos);
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        [PermissionRequired("PERSONA")]
        // GET: Usuario/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                var comercios = await _unitOfWorkRepositories.ComercioRepository.GetAll();
                var mediosp = await _unitOfWorkRepositories.TransMedioPagoRepository.GetAll();
                ViewData["ComercioCodigo"] = new SelectList(comercios, "ComercioCodigo", "ComercioNombre");
                ViewData["TransMediopId"] = new SelectList(mediosp, "TransMediopId", "TransMediopNombre");
                return View();
            }
            catch
            {
                return View();
            }
        }

        [PermissionRequired("PERSONA")]
        // POST: Usuario/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TransUsuarioDTO transaccion)
        {
            if (ModelState.IsValid)
            {
                string identificacion = _httpContextAccessor.HttpContext.Session.GetString("userid");
                if (!string.IsNullOrEmpty(identificacion))
                {
                    UsuarioService usuarioService = new UsuarioService(_unitOfWorkRepositories);
                    await usuarioService.guardarPagoAsync(transaccion, identificacion);
                }
                return RedirectToAction(nameof(Index));
            }
            var comercios = await _unitOfWorkRepositories.ComercioRepository.GetAll();
            var mediosp = await _unitOfWorkRepositories.TransMedioPagoRepository.GetAll();
            ViewData["ComercioCodigo"] = new SelectList(comercios, "ComercioCodigo", "ComercioNombre");
            ViewData["TransMediopId"] = new SelectList(mediosp, "TransMediopId", "TransMediopNombre");
            return View(transaccion);
        }


    }
}
