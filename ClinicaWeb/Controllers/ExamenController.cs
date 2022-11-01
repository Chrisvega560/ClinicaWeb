using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using ClinicaWeb.Models;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace ClinicaWeb.Controllers
{
    [Authorize(Roles = "Admin,Recepcionista,Paciente")]
    public class ExamenController : Controller
    {
        List<DetalleOrden> cart = new List<DetalleOrden>();
        private SantaFeModelContainer db = new SantaFeModelContainer();
        string Upattern = @"^[A-Z0-9]{2} [0-9]{2} [0-9]{2}[A-Z]{0,1}$";
        List<cat> Type = new List<cat>();
        List<CategoriaExamen> dato = new List<CategoriaExamen>();

        // GET: Examen
        public ActionResult Index()
        {
            if(User.IsInRole("Paciente"))
            {
                ViewBag.Id = db.Pacientes.Where(x => x.Correo == User.Identity.Name).FirstOrDefault().Id;
                var examenes = db.Examenes.Include(e => e.Categoria);
                return View(examenes.ToList());
            }
            else
            {
                ViewBag.Ide = db.Empleados.Where(x => x.User == User.Identity.Name).FirstOrDefault().Id;
                var examenes = db.Examenes.Include(e => e.Categoria);
                return View(examenes.ToList());
            }   
        }

        public ActionResult DescargarReportExamen()
        {
            try
            {
                var rptH = new ReportClass();
                rptH.FileName = Server.MapPath("/Reports/List_Examen.rpt");
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

        [HttpPost]
        public ActionResult ExamenCategoria(int? idcategoria)
        {
            if (idcategoria == null)
            {
                idcategoria = 0;
            }
            Categoria c = db.Categorias.Find(idcategoria);

            List<Examen> examen = db.Examenes.Where(e => e.CategoriaId == (idcategoria == 0 ? e.CategoriaId : idcategoria)).ToList();

            //List<Examen> examen = db.Examenes.ToList();

            foreach (Examen item in examen)
            {
                CategoriaExamen x = new CategoriaExamen();
                x.id = item.Id;
                x.Nombre = item.Nombre_exa;
                x.precio = item.Precio;
                x.categoria = item.Categoria.Tipo;
                dato.Add(x);
            }

            return Json(new { result = dato }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ExamenSearch(string valor)
        {
            if(User.IsInRole("Paciente"))
            {
                ViewBag.Id = db.Pacientes.Where(x => x.Correo == User.Identity.Name).FirstOrDefault().Id;
                List<Examen> result = new List<Examen>();

                valor = valor.Trim();
                ViewBag.titulo = " | Resultados para " + valor;
                if (Regex.IsMatch(valor, Upattern))
                {
                    result = db.Examenes.Where(x => x.Nombre_exa == valor).ToList();
                }
                result = db.Examenes.Where(x => x.Nombre_exa.Contains(valor)).ToList();
                return View(result);
            }
            else
            {
                ViewBag.Ide = db.Empleados.Where(x => x.User == User.Identity.Name).FirstOrDefault().Id;
                List<Examen> result = new List<Examen>();

                valor = valor.Trim();
                ViewBag.titulo = " | Resultados para " + valor;
                if (Regex.IsMatch(valor, Upattern))
                {
                    result = db.Examenes.Where(x => x.Nombre_exa == valor).ToList();
                }
                result = db.Examenes.Where(x => x.Nombre_exa.Contains(valor)).ToList();
                return View(result);
            }

        }

        [HttpGet]
        public ActionResult Categories()
        {
            List<Categoria> categories = db.Categorias.ToList();
            foreach (Categoria item in categories)
            {
                cat x = new cat();
                x.id = item.Id;
                x.tipo = item.Tipo;
                Type.Add(x);
            }
            return Json(new { data = Type }, JsonRequestBehavior.AllowGet);
        }

        // GET: Examen/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Examen examen = db.Examenes.Find(id);
            if (examen == null)
            {
                return HttpNotFound();
            }
            return View(examen);
        }

        // GET: Examen/Create
        public ActionResult Create()
        {
            ViewBag.Ide = db.Empleados.Where(x => x.User == User.Identity.Name).FirstOrDefault().Id;
            ViewBag.CategoriaId = new SelectList(db.Categorias, "Id", "Tipo");
            return View();
        }

        // POST: Examen/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre_exa,Precio,CategoriaId")] Examen examen)
        {
            if (ModelState.IsValid)
            {
                db.Examenes.Add(examen);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoriaId = new SelectList(db.Categorias, "Id", "Tipo", examen.CategoriaId);
            return View(examen);
        }

        // GET: Examen/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.Ide = db.Empleados.Where(x => x.User == User.Identity.Name).FirstOrDefault().Id;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Examen examen = db.Examenes.Find(id);
            if (examen == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoriaId = new SelectList(db.Categorias, "Id", "Tipo", examen.CategoriaId);
            return View(examen);
        }

        // POST: Examen/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre_exa,Precio,CategoriaId")] Examen examen)
        {
            if (ModelState.IsValid)
            {
                db.Entry(examen).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoriaId = new SelectList(db.Categorias, "Id", "Tipo", examen.CategoriaId);
            return View(examen);
        }

        // GET: Examen/Delete/5
        public ActionResult Delete(int? id)
        {
            ViewBag.Ide = db.Empleados.Where(x => x.User == User.Identity.Name).FirstOrDefault().Id;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Examen examen = db.Examenes.Find(id);
            if (examen == null)
            {
                return HttpNotFound();
            }
            return View(examen);
        }

        // POST: Examen/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Examen examen = db.Examenes.Find(id);
            db.Examenes.Remove(examen);
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
        public ActionResult agregar(int? idexamen, string cant)
        {

            if (idexamen == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Examen item = db.Examenes.Find(idexamen);

            if (item == null)
            {
                return HttpNotFound();
            }
            try
            {
                DetalleOrden valor = new DetalleOrden();
                valor.Cantidad = cant;
                valor.ExamenId = item.Id;
                valor.Examen = item;
                if (this.Session["ShoppingCart"] != null)
                {
                    cart = (List<DetalleOrden>)Session["ShoppingCart"];
                    cart.Add(valor);
                }
                else
                {
                    cart.Add(valor);
                }
                this.Session["ShoppingCart"] = cart;
                return Json("'Success':'true'");
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.Message);
            }

        }
        [HttpPost]
        public ActionResult eliminar(int? idexamen)
        {
            try
            {

                if (idexamen == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                cart = (List<DetalleOrden>)Session["ShoppingCart"];
                DetalleOrden item = cart.Where(x => x.ExamenId == idexamen).FirstOrDefault();
                if (item == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    cart.Remove(item);
                }


                this.Session["ShoppingCart"] = cart;
                return Json("'Success':'true'");
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        public ActionResult ListaExamenes()
        {
            if(User.IsInRole("Paciente"))
            {
                ViewBag.Id = db.Pacientes.Where(x => x.Correo == User.Identity.Name).FirstOrDefault().Id;
                if (this.Session["ShoppingCart"] != null)
                {

                    cart = (List<DetalleOrden>)Session["ShoppingCart"];
                }
                ViewBag.cart = cart.Count();
                return View(cart);
            }
            else
            {
                ViewBag.Ide = db.Empleados.Where(x => x.User == User.Identity.Name).FirstOrDefault().Id;
                if (this.Session["ShoppingCart"] != null)
                {

                    cart = (List<DetalleOrden>)Session["ShoppingCart"];
                }
                ViewBag.cart = cart.Count();
                return View(cart);
            }
          
         
        }

        struct cat
        {
            public int id { get; set; }
            public string tipo { get; set; }
        }
        struct CategoriaExamen
        {
            public int id { get; set; }
            public string Nombre { get; set; }
            public double precio { get; set; }
            public string categoria { get; set; }
        }
    }
}
