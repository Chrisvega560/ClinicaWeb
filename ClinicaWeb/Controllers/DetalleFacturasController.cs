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
    public class DetalleFacturasController : Controller
    {
        private SantaFeModelContainer db = new SantaFeModelContainer();

        // GET: DetalleFacturas
        public ActionResult Index()
        {
            var detallesFacturas = db.DetallesFacturas.Include(d => d.Orden).Include(d => d.Factura);
            return View(detallesFacturas.ToList());
        }

        // GET: DetalleFacturas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetalleFactura detalleFactura = db.DetallesFacturas.Find(id);
            if (detalleFactura == null)
            {
                return HttpNotFound();
            }
            return View(detalleFactura);
        }

        // GET: DetalleFacturas/Create
        public ActionResult Create()
        {
            ViewBag.OrdenId = new SelectList(db.Ordenes, "Id", "Cod_Orden");
            ViewBag.FacturaId = new SelectList(db.Facturas, "Id", "FechaFac");
            return View();
        }

        // POST: DetalleFacturas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Cantidad,Descripcion,PrecioUnitario,CodFactura,Subtotal,OrdenId,FacturaId")] DetalleFactura detalleFactura)
        {
            if (ModelState.IsValid)
            {
                db.DetallesFacturas.Add(detalleFactura);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrdenId = new SelectList(db.Ordenes, "Id", "Cod_Orden", detalleFactura.OrdenId);
            ViewBag.FacturaId = new SelectList(db.Facturas, "Id", "FechaFac", detalleFactura.FacturaId);
            return View(detalleFactura);
        }

        // GET: DetalleFacturas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetalleFactura detalleFactura = db.DetallesFacturas.Find(id);
            if (detalleFactura == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrdenId = new SelectList(db.Ordenes, "Id", "Cod_Orden", detalleFactura.OrdenId);
            ViewBag.FacturaId = new SelectList(db.Facturas, "Id", "FechaFac", detalleFactura.FacturaId);
            return View(detalleFactura);
        }

        // POST: DetalleFacturas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Cantidad,Descripcion,PrecioUnitario,CodFactura,Subtotal,OrdenId,FacturaId")] DetalleFactura detalleFactura)
        {
            if (ModelState.IsValid)
            {
                db.Entry(detalleFactura).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrdenId = new SelectList(db.Ordenes, "Id", "Cod_Orden", detalleFactura.OrdenId);
            ViewBag.FacturaId = new SelectList(db.Facturas, "Id", "FechaFac", detalleFactura.FacturaId);
            return View(detalleFactura);
        }

        // GET: DetalleFacturas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DetalleFactura detalleFactura = db.DetallesFacturas.Find(id);
            if (detalleFactura == null)
            {
                return HttpNotFound();
            }
            return View(detalleFactura);
        }

        // POST: DetalleFacturas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DetalleFactura detalleFactura = db.DetallesFacturas.Find(id);
            db.DetallesFacturas.Remove(detalleFactura);
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
