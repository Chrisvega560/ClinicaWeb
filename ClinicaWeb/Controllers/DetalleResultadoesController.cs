using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClinicaWeb.Models;

namespace ClinicaWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DetalleResultadoesController : Controller
    {
        private SantaFeModelContainer db = new SantaFeModelContainer();

        // GET: DetalleResultadoes
        public ActionResult Index()
        {
            var detallesResultados = db.DetallesResultados.Include(d => d.Resultado).Include(d => d.Parametro);
            return View(detallesResultados.ToList());
        }

        // GET: DetalleResultadoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetalleResultado detalleResultado = db.DetallesResultados.Find(id);
            if (detalleResultado == null)
            {
                return HttpNotFound();
            }
            return View(detalleResultado);
        }

        // GET: DetalleResultadoes/Create
        public ActionResult Create()
        {
            ViewBag.ResultadoId = new SelectList(db.Resultados, "Id", "Descripcion");
            ViewBag.ParametroId = new SelectList(db.Parametros, "Id", "Nom_Parametro");
            return View();
        }

        // POST: DetalleResultadoes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ExamenResultado,Observacion,ResultadoId,ParametroId")] DetalleResultado detalleResultado)
        {
            if (ModelState.IsValid)
            {
                db.DetallesResultados.Add(detalleResultado);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ResultadoId = new SelectList(db.Resultados, "Id", "Descripcion", detalleResultado.ResultadoId);
            ViewBag.ParametroId = new SelectList(db.Parametros, "Id", "Nom_Parametro", detalleResultado.ParametroId);
            return View(detalleResultado);
        }

        // GET: DetalleResultadoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetalleResultado detalleResultado = db.DetallesResultados.Find(id);
            if (detalleResultado == null)
            {
                return HttpNotFound();
            }
            ViewBag.ResultadoId = new SelectList(db.Resultados, "Id", "Descripcion", detalleResultado.ResultadoId);
            ViewBag.ParametroId = new SelectList(db.Parametros, "Id", "Nom_Parametro", detalleResultado.ParametroId);
            return View(detalleResultado);
        }

        // POST: DetalleResultadoes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ExamenResultado,Observacion,ResultadoId,ParametroId")] DetalleResultado detalleResultado)
        {
            if (ModelState.IsValid)
            {
                db.Entry(detalleResultado).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ResultadoId = new SelectList(db.Resultados, "Id", "Descripcion", detalleResultado.ResultadoId);
            ViewBag.ParametroId = new SelectList(db.Parametros, "Id", "Nom_Parametro", detalleResultado.ParametroId);
            return View(detalleResultado);
        }

        // GET: DetalleResultadoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetalleResultado detalleResultado = db.DetallesResultados.Find(id);
            if (detalleResultado == null)
            {
                return HttpNotFound();
            }
            return View(detalleResultado);
        }

        // POST: DetalleResultadoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DetalleResultado detalleResultado = db.DetallesResultados.Find(id);
            db.DetallesResultados.Remove(detalleResultado);
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
