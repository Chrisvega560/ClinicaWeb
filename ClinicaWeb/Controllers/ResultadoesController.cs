using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClinicaWeb.Models;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace ClinicaWeb.Controllers
{
    [Authorize]
    public class ResultadoesController : Controller
    {
        private SantaFeModelContainer db = new SantaFeModelContainer();

        // GET: Resultadoes
        public ActionResult Index()
        {
            var resultados = db.Resultados.Include(r => r.DetalleOrden);
            return View(resultados.ToList());
        }
        public ActionResult DescargarResult(int id, int categoria)
        {
            try
            {
                var rptH = new ReportClass();
                if (categoria == 7)
                {
                    rptH.FileName = Server.MapPath("/Reports/SecondReport.rpt");
                }
                else if (categoria == 5)
                {
                    rptH.FileName = Server.MapPath("/Reports/ResultReport.rpt");
                }
                rptH.Load();

                rptH.SetParameterValue("Id", id);
                rptH.SetParameterValue("categoria", categoria);

                // Report connection
                var connInfo = CrystalReportsCnn.GetConnectionInfo();
                TableLogOnInfo logonInfo = new TableLogOnInfo();
                Tables tables;
                tables = rptH.Database.Tables;
                foreach (Table table in tables)
                {
                    logonInfo = table.LogOnInfo;
                    logonInfo.ConnectionInfo = connInfo;
                    table.ApplyLogOnInfo(logonInfo);
                }
                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();

                Stream stream = rptH.ExportToStream(ExportFormatType.PortableDocFormat);
                rptH.Dispose();
                rptH.Close();
                return new FileStreamResult(stream, "application/pdf");

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // GET: Resultadoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Resultado resultado = db.Resultados.Find(id);
            if (resultado == null)
            {
                return HttpNotFound();
            }
            return View(resultado);
        }

        // GET: Resultadoes/Create
        public ActionResult Create()
        {
            ViewBag.DetalleOrdenId = new SelectList(db.DetallesOrdenes, "Id", "Cantidad");
            return View();
        }

        // POST: Resultadoes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,DetalleOrdenId")] Resultado resultado)
        {
            if (ModelState.IsValid)
            {
                db.Resultados.Add(resultado);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DetalleOrdenId = new SelectList(db.DetallesOrdenes, "Id", "Cantidad", resultado.DetalleOrdenId);
            return View(resultado);
        }

        // GET: Resultadoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Resultado resultado = db.Resultados.Find(id);
            if (resultado == null)
            {
                return HttpNotFound();
            }
            ViewBag.DetalleOrdenId = new SelectList(db.DetallesOrdenes, "Id", "Cantidad", resultado.DetalleOrdenId);
            return View(resultado);
        }

        // POST: Resultadoes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,DetalleOrdenId")] Resultado resultado)
        {
            if (ModelState.IsValid)
            {
                db.Entry(resultado).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DetalleOrdenId = new SelectList(db.DetallesOrdenes, "Id", "Cantidad", resultado.DetalleOrdenId);
            return View(resultado);
        }

        // GET: Resultadoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Resultado resultado = db.Resultados.Find(id);
            if (resultado == null)
            {
                return HttpNotFound();
            }
            return View(resultado);
        }

        // POST: Resultadoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Resultado resultado = db.Resultados.Find(id);
            db.Resultados.Remove(resultado);
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
