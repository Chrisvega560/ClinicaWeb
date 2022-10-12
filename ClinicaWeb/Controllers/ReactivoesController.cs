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
    public class ReactivoesController : Controller
    {
        private SantaFeModelContainer db = new SantaFeModelContainer();

        // GET: Reactivoes
        public ActionResult Index()
        {
            var reactivos = db.Reactivos.Include(r => r.Parametro);
            return View(reactivos.ToList());
        }

        // GET: Reactivoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reactivo reactivo = db.Reactivos.Find(id);
            if (reactivo == null)
            {
                return HttpNotFound();
            }
            return View(reactivo);
        }

        // GET: Reactivoes/Create
        public ActionResult Create()
        {
            ViewBag.ParametroId = new SelectList(db.Parametros, "Id", "Nom_Parametro");
            return View();
        }

        // POST: Reactivoes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Maximo,Minimo,Densidad,ValoresNormales,Periodo,ParametroId")] Reactivo reactivo)
        {
            if (ModelState.IsValid)
            {
                db.Reactivos.Add(reactivo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(reactivo);
        }

        // GET: Reactivoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reactivo reactivo = db.Reactivos.Find(id);
            if (reactivo == null)
            {
                return HttpNotFound();
            }
            return View(reactivo);
        }

        // POST: Reactivoes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Maximo,Minimo,Densidad,ValoresNormales,Periodo,ParametroId")] Reactivo reactivo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reactivo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(reactivo);
        }

        // GET: Reactivoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reactivo reactivo = db.Reactivos.Find(id);
            if (reactivo == null)
            {
                return HttpNotFound();
            }
            return View(reactivo);
        }

        // POST: Reactivoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Reactivo reactivo = db.Reactivos.Find(id);
            db.Reactivos.Remove(reactivo);
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
