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
    public class proveedorController : Controller
    {
        private CostosObraEntities db = new CostosObraEntities();

        // GET: proveedor
        public ActionResult Index()
        {
            if (!(bool)Session["login"])
                return RedirectToAction("Login", "Account");

            var proveedor = db.proveedor.Include(p => p.rubro);
            return View(proveedor.OrderBy(x => x.descripcion).ToList());
        }

        // GET: proveedor/Details/5
        public ActionResult Details(int? id)
        {
            if (!(bool)Session["login"])
                return RedirectToAction("Login", "Account");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            proveedor proveedor = db.proveedor.Find(id);
            if (proveedor == null)
            {
                return HttpNotFound();
            }
            return View(proveedor);
        }

        // GET: proveedor/Create
        public ActionResult Create()
        {
            if (!(bool)Session["login"])
                return RedirectToAction("Login", "Account");

            ViewBag.idrubro = new SelectList(db.rubro, "id", "descripcion");
            return View();
        }

        // POST: proveedor/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,descripcion,idrubro")] proveedor proveedor)
        {
            if (!(bool)Session["login"])
                return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                db.proveedor.Add(proveedor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idrubro = new SelectList(db.rubro, "id", "descripcion", proveedor.idrubro);
            return View(proveedor);
        }

        // GET: proveedor/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!(bool)Session["login"])
                return RedirectToAction("Login", "Account");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            proveedor proveedor = db.proveedor.Find(id);
            if (proveedor == null)
            {
                return HttpNotFound();
            }
            ViewBag.idrubro = new SelectList(db.rubro, "id", "descripcion", proveedor.idrubro);
            return View(proveedor);
        }

        // POST: proveedor/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,descripcion,idrubro")] proveedor proveedor)
        {
            if (!(bool)Session["login"])
                return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                db.Entry(proveedor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idrubro = new SelectList(db.rubro, "id", "descripcion", proveedor.idrubro);
            return View(proveedor);
        }

        // GET: proveedor/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!(bool)Session["login"])
                return RedirectToAction("Login", "Account");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            proveedor proveedor = db.proveedor.Find(id);
            if (proveedor == null)
            {
                return HttpNotFound();
            }
            int cantidad = db.proyecto.Count();
            int cantidadactivos = db.proyecto.Where(x => x.fechafin == null).Count();
            ViewBag.Mensaje = "Se dara de baja un total de " + cantidad +" registros en la cuenta corriente asociados a proyectos; de los cuales hay " + cantidadactivos + " registros de proyectos ACTIVOS.";
            if (cantidad == 0 && cantidadactivos == 0)
                ViewBag.Mensaje = "";
            return View(proveedor);
        }

        // POST: proveedor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!(bool)Session["login"])
                return RedirectToAction("Login", "Account");

            db.ctacte.RemoveRange(db.ctacte.Where(x => x.idproveedor == id));
            db.SaveChanges();
            
            proveedor proveedor = db.proveedor.Find(id);
            db.proveedor.Remove(proveedor);
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
