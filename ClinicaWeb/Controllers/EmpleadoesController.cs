using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClinicaWeb.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.IO;

namespace ClinicaWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EmpleadoesController : Controller
    {
        private SantaFeModelContainer db = new SantaFeModelContainer();
        ApplicationDbContext context = new ApplicationDbContext();

        // GET: Empleadoes
        public ActionResult Index()
        {
            var empleados = db.Empleados.Include(e => e.Cargo);
            return View(empleados.ToList());
        }

        public ActionResult DescargarReporteEmpleado()
        {
            try
            {
                var rptH = new ReportClass();
                rptH.FileName = Server.MapPath("/Reports/Lista_Empleados.rpt");
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

        [AllowAnonymous]
        public ActionResult Print()
        {
            var empleados = db.Empleados.Include(e => e.Cargo);
            return new Rotativa.ActionAsPdf("Index", empleados) { FileName = "Lista_Empleados.pdf" };
        }

        // GET: Empleadoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Empleado empleado = db.Empleados.Find(id);
            if (empleado == null)
            {
                return HttpNotFound();
            }
            return View(empleado);
        }

        // GET: Empleadoes/Create
        public ActionResult Create()
        {
            ViewBag.CargoId = new SelectList(db.CargoSet, "Id", "Descripcion");
            return View();
        }

        // POST: Empleadoes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nom_Empleado,User,CargoId")] Empleado empleado)
        {
            if (ModelState.IsValid)
            {

                db.Empleados.Add(empleado);
                if (db.SaveChanges() > 0)
                {
                    if(empleado.CargoId == 1)
                    {
                        var ManejadorUsuario = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                        var user = new ApplicationUser();
                        user.UserName = empleado.User;
                        user.Email = empleado.User;
                        string pwd = "123456";
                        var verificar = ManejadorUsuario.Create(user, pwd);
                        if (verificar.Succeeded)
                        {
                            var result = ManejadorUsuario.AddToRole(user.Id, "Recepcionista");
                        }
                    }
                    else
                    if (empleado.CargoId == 2)
                    {
                        var ManejadorUsuario = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                        var user = new ApplicationUser();
                        user.UserName = empleado.User;
                        user.Email = empleado.User;
                        string pwd = "123456";
                        var verificar = ManejadorUsuario.Create(user, pwd);
                        if (verificar.Succeeded)
                        {
                            var result = ManejadorUsuario.AddToRole(user.Id, "Laboratorista");
                        }
                    }
                    
                }
            }

            ViewBag.CargoId = new SelectList(db.CargoSet, "Id", "Descripcion", empleado.CargoId);
            return RedirectToAction("Index");
        }

        // GET: Empleadoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Empleado empleado = db.Empleados.Find(id);
            if (empleado == null)
            {
                return HttpNotFound();
            }
            ViewBag.CargoId = new SelectList(db.CargoSet, "Id", "Descripcion", empleado.CargoId);
            return View(empleado);
        }

        // POST: Empleadoes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nom_Empleado,User,CargoId")] Empleado empleado)
        {
            if (ModelState.IsValid)
            {
                db.Entry(empleado).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CargoId = new SelectList(db.CargoSet, "Id", "Descripcion", empleado.CargoId);
            return View(empleado);
        }

        // GET: Empleadoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Empleado empleado = db.Empleados.Find(id);
            if (empleado == null)
            {
                return HttpNotFound();
            }
            return View(empleado);
        }

        // POST: Empleadoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Empleado empleado = db.Empleados.Find(id);
            db.Empleados.Remove(empleado);
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
