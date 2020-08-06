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
    public class ctacteController : Controller
    {
        private CostosObraEntities db = new CostosObraEntities();

        // GET: ctacte
        public ActionResult Index()
        {
            if (!(bool)Session["login"])
                return RedirectToAction("Login", "Account");

            var ctacte = db.ctacte.Include(c => c.concepto).Include(c => c.proveedor).Include(c => c.proyecto);
            return View(ctacte.OrderByDescending(x=>x.id).ToList());
        }
        public ActionResult IndexProyecto(int id)
        {
            if (!(bool)Session["login"])
                return RedirectToAction("Login", "Account");

            var ctacte = db.ctacte.Where(x=>x.idproyecto==id);
            return View("Index",ctacte.OrderByDescending(x => x.id).ToList());
        }
        public ActionResult IndexCliente(int id, int idcliente)
        {
            if (!(bool)Session["login"])
                return RedirectToAction("Login", "Account");

            var ctacte = db.ctacte.Where(x => x.idproyecto == id && x.proyecto.idcliente == idcliente &&x.concepto.positivo);
            return View("Index", ctacte.OrderByDescending(x => x.id).ToList());
        }
        public ActionResult IndexProveedor(int idproveedor)
        {
            if (!(bool)Session["login"])
                return RedirectToAction("Login", "Account");

            var ctacte = db.ctacte.Where(x => x.idproveedor== idproveedor && !x.concepto.positivo && x.proyecto.fechafin==null);
            return View("Index", ctacte.OrderByDescending(x => x.id).ToList());
        }

        // GET: ctacte/Details/5
        public ActionResult Details(int? id)
        {
            if (!(bool)Session["login"])
                return RedirectToAction("Login", "Account");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ctacte ctacte = db.ctacte.Find(id);
            if (ctacte == null)
            {
                return HttpNotFound();
            }
            return View(ctacte);
        }

        // GET: ctacte/Create
        public ActionResult Create()
        {
            if (!(bool)Session["login"])
                return RedirectToAction("Login", "Account");

            ViewBag.idconcepto = new SelectList(db.concepto.OrderBy(x=>x.descripcion), "id", "descripcion");
            ViewBag.idproveedor = new SelectList(db.proveedor.OrderBy(x => x.descripcion), "id", "descripcion");
            ViewBag.idproyecto = new SelectList(db.proyecto.Where(x=>x.fechafin==null).OrderBy(x=>x.observacion), "id", "observacion");
            return View();
        }

        // POST: ctacte/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,idconcepto,monto,idproyecto,idproveedor,observacion,fecha")] ctacte ctacte, string proveedor, string proyecto)
        {
            if (!(bool)Session["login"])
                return RedirectToAction("Login", "Account");

            if (proveedor.IndexOf('|') > 0)
            {
                ctacte.idproveedor = int.Parse(proveedor.Split('|')[0]);
            }
            else if (proveedor == "---") {
                ctacte.idproveedor = null;
            }
            else
            {
                Models.proveedor p = new Models.proveedor();
                p.descripcion = proveedor;
                db.proveedor.Add(p);
                db.SaveChanges();
                ctacte.idproveedor = db.proveedor.Max(x=>x.id);
                ModelState.Remove("idproveedor");
            }
            if (proyecto.IndexOf('|') > 0)
            {
                ctacte.idproyecto= int.Parse(proyecto.Split('|')[0]);
            }
            else if (proyecto == "---")
            {
                ctacte.idproyecto = 0;
            }
            else
            {
                Models.cliente c = new Models.cliente();
                c.descripcion = proyecto + "(alta automatica)";
                db.cliente.Add(c);
                db.SaveChanges();
                int idcliente = db.cliente.Max(x => x.id);

                Models.proyecto p = new Models.proyecto();
                p.observacion = proyecto;
                p.idcliente = idcliente;
                db.proyecto.Add(p);
                db.SaveChanges();
                ctacte.idproyecto= db.proyecto.Max(x => x.id);
                ModelState.Remove("idproyecto");
            }

           
            
            if (ModelState.IsValid)
            {
                if (!db.concepto.Find(ctacte.idconcepto).positivo)
                    ctacte.monto = ctacte.monto * -1;

                db.ctacte.Add(ctacte);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idconcepto = new SelectList(db.concepto.OrderBy(x => x.descripcion), "id", "descripcion", ctacte.idconcepto);
            ViewBag.idproveedor = new SelectList(db.proveedor.OrderBy(x => x.descripcion), "id", "descripcion", ctacte.idproveedor);
            ViewBag.idproyecto = new SelectList(db.proyecto.Where(x => x.fechafin == null).OrderBy(x => x.observacion), "id", "observacion", ctacte.idproyecto);
            return View(ctacte);
        }

        // GET: ctacte/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!(bool)Session["login"])
                return RedirectToAction("Login", "Account");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ctacte ctacte = db.ctacte.Find(id);
            if (ctacte == null)
            {
                return HttpNotFound();
            }
            ViewBag.idconcepto = new SelectList(db.concepto.OrderBy(x => x.descripcion), "id", "descripcion", ctacte.idconcepto);
            ViewBag.idproveedor = new SelectList(db.proveedor.OrderBy(x => x.descripcion), "id", "descripcion", ctacte.idproveedor);
            ViewBag.idproyecto = new SelectList(db.proyecto.Where(x => x.fechafin == null).OrderBy(x => x.observacion), "id", "observacion", ctacte.idproyecto);
            return View(ctacte);
        }

        // POST: ctacte/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,idconcepto,monto,idproyecto,idproveedor,observacion,fecha")] ctacte ctacte, string proveedor, string proyecto)
        {
            if (!(bool)Session["login"])
                return RedirectToAction("Login", "Account");

            if (proveedor.IndexOf('|') > 0)
            {
                ctacte.idproveedor = int.Parse(proveedor.Split('|')[0]);
            }
            else if (proveedor == "---")
            {
                ctacte.idproveedor = null;
            }
            else
            {
                Models.proveedor p = new Models.proveedor();
                p.descripcion = proveedor;
                db.proveedor.Add(p);
                db.SaveChanges();
                ctacte.idproveedor = db.proveedor.Max(x => x.id);
                ModelState.Remove("idproveedor");
            }
            if (proyecto.IndexOf('|') > 0)
            {
                ctacte.idproyecto = int.Parse(proyecto.Split('|')[0]);
            }
            else if (proyecto == "---")
            {
                ctacte.idproyecto = 0;
            }
            else
            {
                Models.cliente c = new Models.cliente();
                c.descripcion = proyecto + "(alta automatica)";
                db.cliente.Add(c);
                db.SaveChanges();
                int idcliente = db.cliente.Max(x => x.id);

                Models.proyecto p = new Models.proyecto();
                p.observacion = proyecto;
                p.idcliente = idcliente;
                db.proyecto.Add(p);
                db.SaveChanges();
                ctacte.idproyecto = db.proyecto.Max(x => x.id);
                ModelState.Remove("idproyecto");
            }



            if (ModelState.IsValid)
            {
                if (!db.concepto.Find(ctacte.idconcepto).positivo)
                    ctacte.monto = ctacte.monto * -1;
                db.Entry(ctacte).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idconcepto = new SelectList(db.concepto.OrderBy(x => x.descripcion), "id", "descripcion", ctacte.idconcepto);
            ViewBag.idproveedor = new SelectList(db.proveedor.OrderBy(x => x.descripcion), "id", "descripcion", ctacte.idproveedor);
            ViewBag.idproyecto = new SelectList(db.proyecto.Where(x => x.fechafin == null).OrderBy(x => x.observacion), "id", "observacion", ctacte.idproyecto);
            return View(ctacte);
        }

        // GET: ctacte/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!(bool)Session["login"])
                return RedirectToAction("Login", "Account");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ctacte ctacte = db.ctacte.Find(id);
            if (ctacte == null)
            {
                return HttpNotFound();
            }
            return View(ctacte);
        }

        // POST: ctacte/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!(bool)Session["login"])
                return RedirectToAction("Login", "Account");

            ctacte ctacte = db.ctacte.Find(id);
            db.ctacte.Remove(ctacte);
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
