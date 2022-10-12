using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClinicaWeb.Models;

namespace ClinicaWeb
{
    [Authorize(Roles = "Admin")]
    public class ParametroesController : Controller
    {
        private SantaFeModelContainer db = new SantaFeModelContainer();

        // GET: Parametroes
        public ActionResult Index()
        {
            var parametros = db.Parametros.Include(p => p.Reactivo);
            return View(parametros.ToList());
        }

        // GET: Parametroes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Parametro parametro = db.Parametros.Find(id);
            if (parametro == null)
            {
                return HttpNotFound();
            }
            return View(parametro);
        }

        // GET: Parametroes/Create
        public ActionResult Create()
        {
            ViewBag.ReactivoId = new SelectList(db.Reactivos, "Id", "Maximo");
            return View();
        }

        // POST: Parametroes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nom_Parametro,ReactivoId")] Parametro parametro)
        {
            if (ModelState.IsValid)
            {
                db.Parametros.Add(parametro);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ReactivoId = new SelectList(db.Reactivos, "Id", "Maximo", parametro.ReactivoId);
            return View(parametro);
        }

        // GET: Parametroes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Parametro parametro = db.Parametros.Find(id);
            if (parametro == null)
            {
                return HttpNotFound();
            }
            ViewBag.ReactivoId = new SelectList(db.Reactivos, "Id", "Maximo", parametro.ReactivoId);
            return View(parametro);
        }

        // POST: Parametroes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nom_Parametro,ReactivoId")] Parametro parametro)
        {
            if (ModelState.IsValid)
            {
                db.Entry(parametro).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ReactivoId = new SelectList(db.Reactivos, "Id", "Maximo", parametro.ReactivoId);
            return View(parametro);
        }

        // GET: Parametroes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Parametro parametro = db.Parametros.Find(id);
            if (parametro == null)
            {
                return HttpNotFound();
            }
            return View(parametro);
        }

        // POST: Parametroes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Parametro parametro = db.Parametros.Find(id);
            db.Parametros.Remove(parametro);
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
