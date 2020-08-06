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
    public class adicionalesController : Controller
    {
        private CostosObraEntities db = new CostosObraEntities();

        // GET: adicionales
        public ActionResult Index(int idproyecto)
        {
            if (!(bool)Session["login"])
                return RedirectToAction("Login", "Account");

            var adicionales = db.adicionales.Where(x=>x.idproyecto== idproyecto).Include(a => a.proyecto);
            ViewBag.idproyecto = idproyecto;
            return View(adicionales.ToList());
        }

        // GET: adicionales/Details/5
        public ActionResult Details(int? id)
        {
            if (!(bool)Session["login"])
                return RedirectToAction("Login", "Account");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            adicionales adicionales = db.adicionales.Find(id);
            if (adicionales == null)
            {
                return HttpNotFound();
            }
            return View(adicionales);
        }

        // GET: adicionales/Create
        public ActionResult Create(int idproyecto)
        {
            if (!(bool)Session["login"])
                return RedirectToAction("Login", "Account");

            ViewBag.idproyecto = idproyecto;
            ViewBag.proyecto = db.proyecto.Find(idproyecto).observacion;
            return View();
        }

        // POST: adicionales/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,idproyecto,descripcion,monto")] adicionales adicionales)
        {
            if (!(bool)Session["login"])
                return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                db.adicionales.Add(adicionales);
                db.SaveChanges();
                return RedirectToAction("Index", new { idproyecto = adicionales.idproyecto });
            }

            ViewBag.idproyecto = adicionales.idproyecto;
            ViewBag.proyecto = db.proyecto.Find(adicionales.idproyecto).observacion;
            return View(adicionales);
        }

        // GET: adicionales/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            adicionales adicionales = db.adicionales.Find(id);
            if (adicionales == null)
            {
                return HttpNotFound();
            }
            ViewBag.idproyecto = adicionales.idproyecto;
            ViewBag.proyecto = db.proyecto.Find(adicionales.idproyecto).observacion;
            return View(adicionales);
        }

        // POST: adicionales/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,idproyecto,descripcion,monto")] adicionales adicionales)
        {
            if (!(bool)Session["login"])
                return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                db.Entry(adicionales).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { idproyecto = adicionales.idproyecto });
            }
            ViewBag.idproyecto = adicionales.idproyecto;
            ViewBag.proyecto = db.proyecto.Find(adicionales.idproyecto).observacion;
            return View(adicionales);
        }

        // GET: adicionales/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!(bool)Session["login"])
                return RedirectToAction("Login", "Account");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            adicionales adicionales = db.adicionales.Find(id);
            if (adicionales == null)
            {
                return HttpNotFound();
            }
            ViewBag.idproyecto = adicionales.idproyecto;
            ViewBag.proyecto = db.proyecto.Find(adicionales.idproyecto).observacion;

            return View(adicionales);
        }

        // POST: adicionales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!(bool)Session["login"])
                return RedirectToAction("Login", "Account");

            adicionales adicionales = db.adicionales.Find(id);
            int proyecto = adicionales.idproyecto;
            db.adicionales.Remove(adicionales);
            db.SaveChanges();
            return RedirectToAction("Index", new { idproyecto = adicionales.idproyecto });
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
