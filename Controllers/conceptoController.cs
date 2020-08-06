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
    public class conceptoController : Controller
    {
        private CostosObraEntities db = new CostosObraEntities();

        // GET: concepto
        public ActionResult Index()
        {
            if (!(bool)Session["login"])
                return RedirectToAction("Login", "Account");

            return View(db.concepto.OrderBy(x => x.descripcion).ToList());
        }

        // GET: concepto/Details/5
        public ActionResult Details(int? id)
        {
            if (!(bool)Session["login"])
                return RedirectToAction("Login", "Account");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            concepto concepto = db.concepto.Find(id);
            if (concepto == null)
            {
                return HttpNotFound();
            }
            return View(concepto);
        }

        // GET: concepto/Create
        public ActionResult Create()
        {
            if (!(bool)Session["login"])
                return RedirectToAction("Login", "Account");

            return View();
        }

        // POST: concepto/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,descripcion,positivo")] concepto concepto)
        {
            if (!(bool)Session["login"])
                return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                db.concepto.Add(concepto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(concepto);
        }

        // GET: concepto/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!(bool)Session["login"])
                return RedirectToAction("Login", "Account");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            concepto concepto = db.concepto.Find(id);
            if (concepto == null)
            {
                return HttpNotFound();
            }
            return View(concepto);
        }

        // POST: concepto/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,descripcion,positivo")] concepto concepto)
        {
            if (!(bool)Session["login"])
                return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                db.Entry(concepto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(concepto);
        }

        // GET: concepto/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!(bool)Session["login"])
                return RedirectToAction("Login", "Account");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            concepto concepto = db.concepto.Find(id);
            if (concepto == null)
            {
                return HttpNotFound();
            }
            int cantidad = db.proyecto.Count();
            int cantidadactivos = db.proyecto.Where(x => x.fechafin == null).Count();
            ViewBag.Mensaje = "Se dara de baja un total de " + cantidad + " registros en la cuenta corriente asociados a proyectos; de los cuales hay " + cantidadactivos + " registros de proyectos ACTIVOS.";
            if (cantidad == 0 && cantidadactivos == 0)
                ViewBag.Mensaje = "";
            return View(concepto);
        }

        // POST: concepto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!(bool)Session["login"])
                return RedirectToAction("Login", "Account");

            db.ctacte.RemoveRange(db.ctacte.Where(x => x.idconcepto == id));
            db.SaveChanges();

            
            concepto concepto = db.concepto.Find(id);
            db.concepto.Remove(concepto);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
