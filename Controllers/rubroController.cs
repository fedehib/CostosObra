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
    public class rubroController : Controller
    {
        private CostosObraEntities db = new CostosObraEntities();

        // GET: rubro
        public ActionResult Index()
        {
            if (!(bool)Session["login"])
                return RedirectToAction("Login", "Account");

            return View(db.rubro.OrderBy(x => x.descripcion).ToList());
        }

        // GET: rubro/Details/5
        public ActionResult Details(int? id)
        {
            if (!(bool)Session["login"])
                return RedirectToAction("Login", "Account");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            rubro rubro = db.rubro.Find(id);
            if (rubro == null)
            {
                return HttpNotFound();
            }
            return View(rubro);
        }

        // GET: rubro/Create
        public ActionResult Create()
        {
            if (!(bool)Session["login"])
                return RedirectToAction("Login", "Account");

            return View();
        }

        // POST: rubro/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,descripcion")] rubro rubro)
        {
            if (!(bool)Session["login"])
                return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                db.rubro.Add(rubro);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(rubro);
        }

        // GET: rubro/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!(bool)Session["login"])
                return RedirectToAction("Login", "Account");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            rubro rubro = db.rubro.Find(id);
            if (rubro == null)
            {
                return HttpNotFound();
            }
            return View(rubro);
        }

        // POST: rubro/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,descripcion")] rubro rubro)
        {
            if (!(bool)Session["login"])
                return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                db.Entry(rubro).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(rubro);
        }

        // GET: rubro/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!(bool)Session["login"])
                return RedirectToAction("Login", "Account");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            rubro rubro = db.rubro.Find(id);
            if (rubro == null)
            {
                return HttpNotFound();
            }
            return View(rubro);
        }

        // POST: rubro/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!(bool)Session["login"])
                return RedirectToAction("Login", "Account");

            db.Database.ExecuteSqlCommand("UPDATE proveedor SET idrubro = NULL WHERE idrubro = {0}", id);
            
            rubro rubro = db.rubro.Find(id);
            db.rubro.Remove(rubro);
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
