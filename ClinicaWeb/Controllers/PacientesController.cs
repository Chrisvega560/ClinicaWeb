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
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ClinicaWeb.Controllers
{
    [Authorize(Roles = "Admin,Recepcionista")]
    public class PacientesController : Controller
    {
        private SantaFeModelContainer db = new SantaFeModelContainer();
        ApplicationDbContext context = new ApplicationDbContext();

        // GET: Pacientes
        public ActionResult Index()
        {
            var pacientes = db.Pacientes.Include(p => p.Municipio).Include(p => p.Sexo);
            return View(pacientes.ToList());
        }
        public ActionResult DescargarReportPaciente()
        {
            try
            {
                var rptH = new ReportClass();
                rptH.FileName = Server.MapPath("/Reports/Lista_Pacientes.rpt");
                rptH.Load();

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

        // GET: Pacientes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Paciente paciente = db.Pacientes.Find(id);
            if (paciente == null)
            {
                return HttpNotFound();
            }
            return View(paciente);
        }

        // GET: Pacientes/Create
        public ActionResult Create()
        {
            ViewBag.MunicipioId = new SelectList(db.Municipios, "Id", "Nombre_mun");
            ViewBag.SexoId = new SelectList(db.Sexos, "Id", "TipoSexo");
            return View();
        }

        // POST: Pacientes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre_pac,Edad,Correo,Celular,Direccion,MunicipioId,SexoId")] Paciente paciente)
        {
            if (ModelState.IsValid)
            {
                db.Pacientes.Add(paciente);
                if (db.SaveChanges() > 0)
                {
                    var ManejadorUsuario = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                    var user = new ApplicationUser();
                    user.UserName = paciente.Correo;
                    user.Email = paciente.Correo;
                    string pwd = "123456";
                    var verificar = ManejadorUsuario.Create(user, pwd);
                    if (verificar.Succeeded)
                    {
                        var result = ManejadorUsuario.AddToRole(user.Id, "Paciente");
                        return RedirectToAction("Index");
                    }
                }
                    
            }

            ViewBag.MunicipioId = new SelectList(db.Municipios, "Id", "Nombre_mun", paciente.MunicipioId);
            ViewBag.SexoId = new SelectList(db.Sexos, "Id", "TipoSexo", paciente.SexoId);
            return View(paciente);
        }

        // GET: Pacientes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Paciente paciente = db.Pacientes.Find(id);
            if (paciente == null)
            {
                return HttpNotFound();
            }
            ViewBag.MunicipioId = new SelectList(db.Municipios, "Id", "Nombre_mun", paciente.MunicipioId);
            ViewBag.SexoId = new SelectList(db.Sexos, "Id", "TipoSexo", paciente.SexoId);
            return View(paciente);
        }

        // POST: Pacientes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre_pac,Edad,Correo,Celular,Direccion,MunicipioId,SexoId")] Paciente paciente)
        {
            if (ModelState.IsValid)
            {
                db.Entry(paciente).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MunicipioId = new SelectList(db.Municipios, "Id", "Nombre_mun", paciente.MunicipioId);
            ViewBag.SexoId = new SelectList(db.Sexos, "Id", "TipoSexo", paciente.SexoId);
            return View(paciente);
        }

        // GET: Pacientes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Paciente paciente = db.Pacientes.Find(id);
            if (paciente == null)
            {
                return HttpNotFound();
            }
            return View(paciente);
        }

        // POST: Pacientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Paciente paciente = db.Pacientes.Find(id);
            db.Pacientes.Remove(paciente);
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
