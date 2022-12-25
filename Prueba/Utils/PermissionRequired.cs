using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prueba.Utils
{
    public class PermissionRequired:ActionFilterAttribute
    {
        private readonly string rol;
        public PermissionRequired(string rol)
        {
            this.rol = rol;
        }

        /// <summary>
        /// Se encarga de validar si el usuario esta logueado y el rol que tiene
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string tipo = filterContext.HttpContext.Session.GetString("tipo");
            //Valida si el usuario esta logueado
            if (string.IsNullOrEmpty(tipo))
                filterContext.Result = new RedirectResult("~/Auth/Login");
            else if(!tipo.Equals(rol))
                filterContext.Result = new RedirectResult("~/Auth/Login");

            base.OnActionExecuting(filterContext);
        }
    }
}
