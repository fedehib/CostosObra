using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CostosObras.Models;

namespace CostosObras.Controllers
{
    public class HomeController : Controller
    {
        private CostosObraEntities db = new CostosObraEntities();
        private bool login()
        {
            try
            {
                return ((bool)Session["login"]);
            }
            catch (Exception)
            {
                return false;
            } 
        }
        public ActionResult Index()
        {
            if (login() == false)
                return RedirectToAction("Login", "Account");
            
            var proyecto = db.proyecto.Where(x=>x.fechafin == null).OrderBy(x=>x.observacion);
            List<ProyectosActivos> lista = new List<ProyectosActivos>();
            decimal totalingreso = 0;
            decimal totalegreso = 0;
            foreach (var item in proyecto)
            {
                ProyectosActivos pr = new ProyectosActivos();
                pr.proyecto = item;
                pr.ingresos = item.ctacte.Where(x => x.concepto.positivo).ToList();
                pr.egresos = item.ctacte.Where(x => !x.concepto.positivo).ToList();
                totalingreso += item.ctacte.Where(x => x.concepto.positivo).Sum(x => x.monto).Value;
                totalegreso += item.ctacte.Where(x => !x.concepto.positivo).Sum(x => x.monto).Value;
                lista.Add(pr);
            }
            ViewBag.totalingreso = totalingreso;
            ViewBag.totalegreso = totalegreso;

            return View(lista);
        }

       
    }
}