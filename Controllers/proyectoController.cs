using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CostosObras.Models;

namespace CostosObras.Controllers
{
    public class proyectoController : Controller
    {
        private CostosObraEntities db = new CostosObraEntities();

        // GET: proyecto
        public ActionResult Index()
        {
            if (!(bool)Session["login"])
                return RedirectToAction("Login", "Account");

            var proyecto = db.proyecto.Include(p => p.cliente);
            return View(proyecto.OrderBy(x => x.observacion).ToList());
        }

        // GET: proyecto/Details/5
        public ActionResult Details(int? id)
        {
            if (!(bool)Session["login"])
                return RedirectToAction("Login", "Account");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            proyecto proyecto = db.proyecto.Find(id);
            if (proyecto == null)
            {
                return HttpNotFound();
            }
            return View(proyecto);
        }

        // GET: proyecto/Create
        public ActionResult Create()
        {

            if (!(bool)Session["login"])
                return RedirectToAction("Login", "Account");

            ViewBag.idcliente = new SelectList(db.cliente.OrderBy(x => x.descripcion), "id", "descripcion");
            return View();
        }

        // POST: proyecto/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,observacion,idcliente,costototal,costoreal,archivo,nombrearchivo,fechafin")] proyecto proyecto, HttpPostedFileBase fileupload)
        {

            if (!(bool)Session["login"])
                return RedirectToAction("Login", "Account");

            if (fileupload != null && fileupload.ContentLength > 0 && fileupload.FileName.Split('.')[1].ToUpper() == "PDF")
            {
                proyecto.nombrearchivo = fileupload.FileName;
                proyecto.archivo = ReadToEnd(fileupload.InputStream);
            }
            else 
            {
                proyecto.archivo = null;
                proyecto.nombrearchivo = null;
            }
            if (ModelState.IsValid)
            {
                db.proyecto.Add(proyecto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idcliente = new SelectList(db.cliente.OrderBy(x => x.descripcion), "id", "descripcion", proyecto.idcliente);
            return View(proyecto);
        }

        // GET: proyecto/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!(bool)Session["login"])
                return RedirectToAction("Login", "Account");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            proyecto proyecto = db.proyecto.Find(id);
            if (proyecto == null)
            {
                return HttpNotFound();
            }
            ViewBag.idcliente = new SelectList(db.cliente.OrderBy(x => x.descripcion), "id", "descripcion", proyecto.idcliente);
            return View(proyecto);
        }

        // POST: proyecto/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,observacion,idcliente,costototal,costoreal,archivo,nombrearchivo,fechafin")] proyecto proyecto, HttpPostedFileBase fileupload)
        {
            if (!(bool)Session["login"])
                return RedirectToAction("Login", "Account");

            if (fileupload != null && fileupload.ContentLength > 0 && fileupload.FileName.Split('.')[1].ToUpper() =="PDF")
            {
                proyecto.nombrearchivo = fileupload.FileName;
                proyecto.archivo = ReadToEnd(fileupload.InputStream);
            }
            else
            {
                proyecto.archivo = null;
                proyecto.nombrearchivo = null;
            }
            if (ModelState.IsValid)
            {
                db.Entry(proyecto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idcliente = new SelectList(db.cliente.OrderBy(x => x.descripcion), "id", "descripcion", proyecto.idcliente);
            return View(proyecto);
        }

        // GET: proyecto/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!(bool)Session["login"])
                return RedirectToAction("Login", "Account");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            proyecto proyecto = db.proyecto.Find(id);
            if (proyecto == null)
            {
                return HttpNotFound();
            }
            int cantidad = db.ctacte.Count();
            ViewBag.Mensaje = "Se dara de baja un total de " + cantidad + " registros en la cuenta corriente asociados al proyecto.";
            if (cantidad == 0)
                ViewBag.Mensaje = "";
            return View(proyecto);
        }

        // POST: proyecto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!(bool)Session["login"])
                return RedirectToAction("Login", "Account");

            db.ctacte.RemoveRange(db.ctacte.Where(x => x.idproyecto == id));
            db.SaveChanges();

            db.adicionales.RemoveRange(db.adicionales.Where(x => x.idproyecto == id));
            db.SaveChanges();

            proyecto proyecto = db.proyecto.Find(id);
            db.proyecto.Remove(proyecto);
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

        public static byte[] ReadToEnd(System.IO.Stream stream)
        {
            long originalPosition = 0;

            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }
        }
        public ActionResult Descargar(int id)
        {
            proyecto proyecto = db.proyecto.Find(id);
            if (proyecto == null)
            {
                return HttpNotFound();
            }
            if (proyecto.archivo!=null) { 
                Response.Clear();
                MemoryStream ms = new MemoryStream(proyecto.archivo);
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename="+proyecto.nombrearchivo);
                Response.Buffer = true;
                ms.WriteTo(Response.OutputStream);
                Response.End();
                return new FileStreamResult(ms, "application/pdf");
            }
            return RedirectToAction("Index");

        }

    }
}
