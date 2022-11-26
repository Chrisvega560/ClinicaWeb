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
    [Authorize(Roles = "Admin,Recepcionista")]
    public class MedicosController : Controller
    {
        private SantaFeModelContainer db = new SantaFeModelContainer();

        // GET: Medicos
        public ActionResult Index()
        {
            if(User.IsInRole("Admin"))
            {
                return View(db.Medicos.ToList());
            }
            else
            {
                ViewBag.Ide = db.Empleados.Where(x => x.User == User.Identity.Name).FirstOrDefault().Id;
                return View(db.Medicos.ToList());
            }
           
        }

        public ActionResult DescargarReportMedico()
        {
            List<Medico> model = db.Medicos.ToList();
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

                Cell c = new Cell().Add(new Paragraph("Reporte de Medicos").SetTextAlignment(TextAlignment.CENTER).SetFontSize(20F));
                t.AddCell(c);
                t.SetMarginBottom(50f);
                t.SetMarginTop(150f);
                break;
            }


            doc.Add(t);


            Table _table = new Table(2).UseAllAvailableWidth();
            Cell _Cell = new Cell().Add(new Paragraph("Nombre de los Medicos")).SetTextAlignment(TextAlignment.CENTER);
            _table.AddCell(_Cell);
            _Cell = new Cell().Add(new Paragraph("Numeros de telefono")).SetTextAlignment(TextAlignment.CENTER);
            _table.AddCell(_Cell);




            foreach (var item in model)
            {
                _Cell = new Cell().Add(new Paragraph(item.NombreMed)).SetTextAlignment(TextAlignment.CENTER);
                _table.AddCell(_Cell);
                _Cell = new Cell().Add(new Paragraph(item.Celular)).SetTextAlignment(TextAlignment.CENTER);
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

        // GET: Medicos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medico medico = db.Medicos.Find(id);
            if (medico == null)
            {
                return HttpNotFound();
            }
            return View(medico);
        }

        // GET: Medicos/Create
        public ActionResult Create()
        {
            if(User.IsInRole("Admin"))
            {
                return View();
            }
            else
            {
                ViewBag.Ide = db.Empleados.Where(x => x.User == User.Identity.Name).FirstOrDefault().Id;
                return View();
            }

        }

        // POST: Medicos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,NombreMed,Celular")] Medico medico)
        {
            if (ModelState.IsValid)
            {
                db.Medicos.Add(medico);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(medico);
        }

        // GET: Medicos/Edit/5
        public ActionResult Edit(int? id)
        {
            if(User.IsInRole("Admin"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Medico medico = db.Medicos.Find(id);
                if (medico == null)
                {
                    return HttpNotFound();
                }
                return View(medico);
            }
            else
            {
                ViewBag.Ide = db.Empleados.Where(x => x.User == User.Identity.Name).FirstOrDefault().Id;
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Medico medico = db.Medicos.Find(id);
                if (medico == null)
                {
                    return HttpNotFound();
                }
                return View(medico);
            }
           
        }

        // POST: Medicos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,NombreMed,Celular")] Medico medico)
        {
            if (ModelState.IsValid)
            {
                db.Entry(medico).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(medico);
        }

        // GET: Medicos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (User.IsInRole("Admin"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Medico medico = db.Medicos.Find(id);
                if (medico == null)
                {
                    return HttpNotFound();
                }
                return View(medico);
            }
            else
            {
                ViewBag.Ide = db.Empleados.Where(x => x.User == User.Identity.Name).FirstOrDefault().Id;
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Medico medico = db.Medicos.Find(id);
                if (medico == null)
                {
                    return HttpNotFound();
                }
                return View(medico);
            }
            
        }

        // POST: Medicos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Medico medico = db.Medicos.Find(id);
            db.Medicos.Remove(medico);
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
