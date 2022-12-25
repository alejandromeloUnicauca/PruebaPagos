using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Prueba.DTOs;
using Prueba.Facade;
using Prueba.Models;
using Prueba.Services;

namespace Prueba.Controllers
{
    public class AuthController : Controller
    {
        private readonly UnitOfWorkRepositories _unitOfWorkRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private List<SelectListItem> listItems;

        public AuthController(ZonapagosContext context, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWorkRepository = new UnitOfWorkRepositories(context);
            _httpContextAccessor = httpContextAccessor;

            listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem
            {
                Text = "Persona",
                Value = "PERSONA",
                Selected = true
            }); ;
            listItems.Add(new SelectListItem
            {
                Text = "Comercio",
                Value = "COMERCIO",
            });
        }

        // GET: Users/Login
        public IActionResult Login()
        {
            ViewBag.listItems = this.listItems;
            return View();
        }

        // POST: Users/Login
        [HttpPost]
        public async Task<IActionResult> Login(UserDTO user)
        {
            ViewBag.listItems = this.listItems;
            if (ModelState.IsValid)
            {
                try
                {
                    UserService userService = new UserService(_unitOfWorkRepository);
                    try
                    {
                        await userService.cargarDatosAsync();
                    }catch(Exception ex) { }
                    var res = await userService.validarLoginAsync(user);
                    if(res.Item1)
                    {
                        _httpContextAccessor.HttpContext.Session.SetString("userid", user.identificacion);
                        _httpContextAccessor.HttpContext.Session.SetString("tipo", user.tipo);
                        if(user.tipo.Equals("PERSONA"))
                            return Redirect("~/Usuario/Index");
                        else if(user.tipo.Equals("COMERCIO"))
                            return Redirect("~/Comercio/Index");
                        else
                            return View();
                    }    
                    else
                    {
                        ViewData["MensajeErrorLogin"] = res.Item2;
                        return View();  
                    }
                    
                }
                catch (Exception ex)
                {
                    return View();
                }
            }
            return View(user);
        }

        public IActionResult Registrarse() 
        {
            ViewBag.listItems = this.listItems;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registrarse(UserRegisterDTO user)
        {
            ViewBag.listItems = this.listItems;
            if (ModelState.IsValid)
            {
                try
                {
                    if (user.confirmpassword.Equals(user.password))
                    {
                        UserService userService = new UserService(_unitOfWorkRepository);
                        var res = await userService.registrarPasswordAsync(user);
                        if (!res.Item1) {
                            ViewData["MensajeErrorRegister"] = res.Item2;
                            return View();
                        }
                        else
                        {
                            return RedirectToAction(nameof(Login));
                        }
                    }
                    else
                    {
                        ViewData["MensajeErrorRegister"] = "Las contraseñas no coinciden";
                        return View();
                    }
                }
                catch(Exception ex)
                {
                    return View();
                }
            }
            return View();
        }

        public IActionResult CerrarSesion()
        {
            _httpContextAccessor.HttpContext.Session.Clear();
            return RedirectToAction(nameof(Login));
        }
    }
}
