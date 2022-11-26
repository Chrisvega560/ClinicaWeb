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
//using CrystalDecisions.CrystalReports.Engine;
//using CrystalDecisions.Shared;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Events;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace ClinicaWeb.Controllers
{
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
        public ActionResult DescargarReportEmpleados()
        {
            List<Empleado> model = db.Empleados.ToList();
            MemoryStream ms = new MemoryStream();

            PdfWriter pw = new PdfWriter(ms);

            PdfDocument pd = new PdfDocument(pw);
            Document doc = new Document(pd, PageSize.LETTER);
            doc.SetMargins(75, 35, 70, 35);

            string pathlogo = Server.MapPath("~/Content/header.jpg");
            Image img = new Image(ImageDataFactory.Create(pathlogo));


            PdfFont font = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN);


            pd.AddEventHandler(PdfDocumentEvent.START_PAGE, new HeaderEventHandler1(img));

            //table.AddCell(cell);

            //cell = new Cell().Add(new Paragraph("Calidad y Humanismo al servicio de la Salud…").SetFontSize(16)).SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER);
            //table.AddCell(cell);

            //cell = new Cell().Add(new Paragraph("Lic. Silvia Rodríguez López").SetFontSize(16)).SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER);
            //table.AddCell(cell);

            //cell = new Cell().Add(new Paragraph("Bioanalista Clínico UNAN-Managua").SetFontSize(14)).SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER);
            //table.AddCell(cell);

            //cell = new Cell().Add(new Paragraph("No.RUC: 4010301820007Y").SetFontSize(14)).SetTextAlignment(TextAlignment.CENTER).SetBorder(Border.NO_BORDER);
            //table.AddCell(cell);

            Table t = new Table(1).UseAllAvailableWidth();
            foreach (var item in model)
            {

                Cell c = new Cell().Add(new Paragraph("Reporte de Empleados").SetTextAlignment(TextAlignment.CENTER).SetFontSize(20F));
                t.AddCell(c);
                t.SetMarginBottom(50f);
                t.SetMarginTop(150f);
                break;
            }


            doc.Add(t);


            Table _table = new Table(3).UseAllAvailableWidth();
            Cell _Cell = new Cell().Add(new Paragraph("Nombre de los Empleados")).SetTextAlignment(TextAlignment.CENTER);
            _table.AddCell(_Cell);
            _Cell = new Cell().Add(new Paragraph("Usuario")).SetTextAlignment(TextAlignment.CENTER);
            _table.AddCell(_Cell);
            _Cell = new Cell().Add(new Paragraph("Cargo")).SetTextAlignment(TextAlignment.CENTER);
            _table.AddCell(_Cell);




            foreach (var item in model)
            {
                _Cell = new Cell().Add(new Paragraph(item.Nom_Empleado)).SetTextAlignment(TextAlignment.CENTER);
                _table.AddCell(_Cell);
                _Cell = new Cell().Add(new Paragraph(item.User)).SetTextAlignment(TextAlignment.CENTER);
                _table.AddCell(_Cell);

                _Cell = new Cell().Add(new Paragraph(item.Cargo.Descripcion)).SetTextAlignment(TextAlignment.CENTER);
                _table.AddCell(_Cell);
            }

            doc.Add(_table);

            doc.Close();

            byte[] bytesStream = ms.ToArray();
            ms = new MemoryStream();
            ms.Write(bytesStream, 0, bytesStream.Length);
            ms.Position = 0;

            return new FileStreamResult(ms, "application/pdf");

        }

        public class HeaderEventHandler1 : IEventHandler
        {
            Image Img;

            public HeaderEventHandler1(Image img)
            {
                Img = img;
            }
            public void HandleEvent(Event @event)
            {
                PdfDocumentEvent DocEvent = (PdfDocumentEvent)@event;
                PdfDocument doc = DocEvent.GetDocument();
                PdfPage page = DocEvent.GetPage();

                Rectangle RootArea = new Rectangle(35, page.GetPageSize().GetTop() - 70, page.GetPageSize().GetRight() - 70, 50);

                Canvas canvas = new Canvas(page, RootArea);
                canvas
                    .Add(getTable(DocEvent))
                    .ShowTextAligned("Dirección: Del parque central 2 cuadras al oeste y 2 cuadras al norte", 300, 50, TextAlignment.CENTER)
                     .ShowTextAligned("Clínica familiar Masatepe, Nicaragua Teléfono: 82463210(Movistar)", 300, 30, TextAlignment.CENTER)
                    .ShowTextAligned("Atenderle es un Privilegio", 300, 10, TextAlignment.CENTER)
                    .Close();
            }
            public Table getTable(PdfDocumentEvent DocEvent)
            {
                float[] cellWidth = { 40f, 60f };
                Table TableEvent = new Table(UnitValue.CreatePercentArray(cellWidth)).UseAllAvailableWidth();

                Style StyleCell = new Style().SetBorder(Border.NO_BORDER);

                Style StyleText = new Style().SetTextAlignment(TextAlignment.LEFT).SetFontSize(10f);

                Cell cell = new Cell().Add(Img.SetHeight(150).SetWidth(570));

                TableEvent.AddCell(cell.AddStyle(StyleCell).SetTextAlignment(TextAlignment.JUSTIFIED));

                return TableEvent;

            }
        }

        // GET: Empleadoes/Details/5
        [Authorize(Roles = "Recepcionista, Laboratorista")]
        public ActionResult Details(int? id)
        {
            ViewBag.Ide = db.Empleados.Where(x => x.User == User.Identity.Name).FirstOrDefault().Id;
            if(ViewBag.Ide == id)
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
            else
            {
                return HttpNotFound();
            }
            
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
        public ActionResult Editar(int? id)
        {
            ViewBag.Ide = db.Empleados.Where(x => x.User == User.Identity.Name).FirstOrDefault().Id;
            if(ViewBag.Ide == id)
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
            else
            {
                return HttpNotFound();
            }
            
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar([Bind(Include = "Id,Nom_Empleado,User,CargoId")] Empleado empleado)
        {
            if (ModelState.IsValid)
            {
                db.Entry(empleado).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new {id = empleado.Id });
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
            if (User.IsInRole("Admin"))
            {

                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Details");
            }
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
