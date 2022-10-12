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
    [Authorize]
    public class DetalleOrdensController : Controller
    {
        private SantaFeModelContainer db = new SantaFeModelContainer();

        // GET: DetalleOrdens
        public ActionResult Index()
        {
            var detallesOrdenes = db.DetallesOrdenes.Include(d => d.Orden).Include(d => d.Examen);
            return View(detallesOrdenes.ToList());
        }

        // GET: DetalleOrdens/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetalleOrden detalleOrden = db.DetallesOrdenes.Find(id);
            if (detalleOrden == null)
            {
                return HttpNotFound();
            }
            List<ExamenParametro> parametro = db.ExamenesParametros.Where(x => x.ExamenId == detalleOrden.ExamenId).ToList();
            ViewBag.lista = parametro;
            return View(detalleOrden);
        }

        // GET: DetalleOrdens/Create
        public ActionResult Create()
        {
            ViewBag.OrdenId = new SelectList(db.Ordenes, "Id", "Cod_Orden");
            ViewBag.ExamenId = new SelectList(db.Examenes, "Id", "Nombre_exa");
            return View();
        }

        // POST: DetalleOrdens/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Cantidad,FechaFinal,Precio,Estado,OrdenId,ExamenId")] DetalleOrden detalleOrden)
        {
            if (ModelState.IsValid)
            {
                db.DetallesOrdenes.Add(detalleOrden);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrdenId = new SelectList(db.Ordenes, "Id", "Cod_Orden", detalleOrden.OrdenId);
            ViewBag.ExamenId = new SelectList(db.Examenes, "Id", "Nombre_exa", detalleOrden.ExamenId);
            return View(detalleOrden);
        }

        // GET: DetalleOrdens/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetalleOrden detalleOrden = db.DetallesOrdenes.Find(id);
            if (detalleOrden == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrdenId = new SelectList(db.Ordenes, "Id", "Cod_Orden", detalleOrden.OrdenId);
            ViewBag.ExamenId = new SelectList(db.Examenes, "Id", "Nombre_exa", detalleOrden.ExamenId);
            return View(detalleOrden);
        }

        // POST: DetalleOrdens/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Cantidad,FechaFinal,Precio,Estado,OrdenId,ExamenId")] DetalleOrden detalleOrden)
        {
            if (ModelState.IsValid)
            {
                db.Entry(detalleOrden).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrdenId = new SelectList(db.Ordenes, "Id", "Cod_Orden", detalleOrden.OrdenId);
            ViewBag.ExamenId = new SelectList(db.Examenes, "Id", "Nombre_exa", detalleOrden.ExamenId);
            return View(detalleOrden);
        }

        // GET: DetalleOrdens/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetalleOrden detalleOrden = db.DetallesOrdenes.Find(id);
            if (detalleOrden == null)
            {
                return HttpNotFound();
            }
            return View(detalleOrden);
        }

        // POST: DetalleOrdens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DetalleOrden detalleOrden = db.DetallesOrdenes.Find(id);
            db.DetallesOrdenes.Remove(detalleOrden);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult save(string[] data, string[] obs, int id, int[] idparametro)
        {

            if (data == null)
            {
                return HttpNotFound();
            }

            Resultado resp = new Resultado();
            resp.Descripcion = "Sin detalles";
            resp.DetalleOrdenId = id;

            for (int i = 0; i < data.Length; i++)
            {
                Parametro p = new Parametro();
                DetalleResultado result = new DetalleResultado();
                result.ExamenResultado = data[i];
                result.Observacion = obs[i];
                result.ParametroId = idparametro[i];
                result.ResultadoId = resp.Id;
                db.DetallesResultados.Add(result);

                DetalleOrden d = db.DetallesOrdenes.Find(id);
                d.Estado = "Terminado";

            }
            db.Resultados.Add(resp);
            db.SaveChanges();
            return Json("'Success':'true'");
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
