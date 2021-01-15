using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SUM.Controllers
{
    
    public class AccountController : Controller
    {
        public AccountController()
        {

        }

        public ActionResult Login()
        {
            Session["login"] = false;
            ViewBag.Resultado = "";
            ViewBag.cd_usuario = "";

            return View();
        }

        [HttpPost]
        // POST: Contacto/Delete/5
        public ActionResult Login(string cd_usuario, string tx_contrasena)
        {
            //TODO: cambiar usuario y psw
            if (tx_contrasena.ToLower() == "jerusalem123456" && cd_usuario.ToLower() == "fede")
            {
                Session["login"] = true;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Resultado = "Credenciales erroneas.";
                ViewBag.cd_usuario = cd_usuario;
                    
            }
            return View();

        }

        [HttpPost]
        // POST: Contacto/Delete/5
        public ActionResult LogOff()
        {
            Session["login"] = false;
            ViewBag.Resultado = "";
            ViewBag.cd_usuario = "";
            return RedirectToAction("Login", "Account");
        }

        

    }
}
