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
    public class ExamenParametroesController : Controller
    {
        private SantaFeModelContainer db = new SantaFeModelContainer();

        // GET: ExamenParametroes
        public ActionResult Index()
        {
            var examenesParametros = db.ExamenesParametros.Include(e => e.Examen).Include(e => e.Parametro);
            return View(examenesParametros.ToList());
        }

        // GET: ExamenParametroes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExamenParametro examenParametro = db.ExamenesParametros.Find(id);
            if (examenParametro == null)
            {
                return HttpNotFound();
            }
            return View(examenParametro);
        }

        // GET: ExamenParametroes/Create
        public ActionResult Create()
        {
            ViewBag.ExamenId = new SelectList(db.Examenes, "Id", "Nombre_exa");
            ViewBag.ParametroId = new SelectList(db.Parametros, "Id", "Nom_Parametro");
            return View();
        }

        // POST: ExamenParametroes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ExamenId,ParametroId")] ExamenParametro examenParametro)
        {
            if (ModelState.IsValid)
            {
                db.ExamenesParametros.Add(examenParametro);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ExamenId = new SelectList(db.Examenes, "Id", "Nombre_exa", examenParametro.ExamenId);
            ViewBag.ParametroId = new SelectList(db.Parametros, "Id", "Nom_Parametro", examenParametro.ParametroId);
            return View(examenParametro);
        }

        // GET: ExamenParametroes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExamenParametro examenParametro = db.ExamenesParametros.Find(id);
            if (examenParametro == null)
            {
                return HttpNotFound();
            }
            ViewBag.ExamenId = new SelectList(db.Examenes, "Id", "Nombre_exa", examenParametro.ExamenId);
            ViewBag.ParametroId = new SelectList(db.Parametros, "Id", "Nom_Parametro", examenParametro.ParametroId);
            return View(examenParametro);
        }

        // POST: ExamenParametroes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ExamenId,ParametroId")] ExamenParametro examenParametro)
        {
            if (ModelState.IsValid)
            {
                db.Entry(examenParametro).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ExamenId = new SelectList(db.Examenes, "Id", "Nombre_exa", examenParametro.ExamenId);
            ViewBag.ParametroId = new SelectList(db.Parametros, "Id", "Nom_Parametro", examenParametro.ParametroId);
            return View(examenParametro);
        }

        // GET: ExamenParametroes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExamenParametro examenParametro = db.ExamenesParametros.Find(id);
            if (examenParametro == null)
            {
                return HttpNotFound();
            }
            return View(examenParametro);
        }

        // POST: ExamenParametroes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ExamenParametro examenParametro = db.ExamenesParametros.Find(id);
            db.ExamenesParametros.Remove(examenParametro);
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
