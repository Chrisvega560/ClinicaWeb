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
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Events;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
//using CrystalDecisions.CrystalReports.Engine;
//using CrystalDecisions.Shared;

namespace ClinicaWeb.Controllers
{
    [Authorize(Roles = "Admin,Recepcionista")]
    public class FacturasController : Controller
    {
        private SantaFeModelContainer db = new SantaFeModelContainer();
        string Upattern = @"^[A-Z0-9]{2} [0-9]{2} [0-9]{2}[A-Z]{0,1}$";

        // GET: Facturas
        public ActionResult Index()
        {
            if(User.IsInRole("Admin"))
            {
                return View(db.Facturas.OrderByDescending(x => x.FechaFac).ToList());
            }
            else
            {
                ViewBag.Ide = db.Empleados.Where(x => x.User == User.Identity.Name).FirstOrDefault().Id;
                return View(db.Facturas.OrderByDescending(x => x.FechaFac).ToList());
            }
           
        }
        public ActionResult ListFact(string valor)
        {

            List<Factura> result = new List<Factura>();
            if  (User.IsInRole("Recepcionista"))
            {
                ViewBag.Ide = db.Empleados.Where(x => x.User == User.Identity.Name).FirstOrDefault().Id;
                valor = valor.Trim();
                ViewBag.titulo = " | Resultados para " + valor;
                if (Regex.IsMatch(valor, Upattern))
                {
                    result = db.Facturas.Where(x => x.DetalleFactura.FirstOrDefault().Orden.Paciente.Nombre_pac == valor).ToList();
                }
                result = db.Facturas.Where(x => x.DetalleFactura.FirstOrDefault().Orden.Paciente.Nombre_pac.Contains(valor)).ToList();

                @ViewBag.Total = result.Count();
            }
            else if (User.IsInRole("Admin"))
            {
                valor = valor.Trim();
                ViewBag.titulo = " | Resultados para " + valor;
                if (Regex.IsMatch(valor, Upattern))
                {
                    result = db.Facturas.Where(x => x.DetalleFactura.FirstOrDefault().Orden.Paciente.Nombre_pac == valor).ToList();
                }
                result = db.Facturas.Where(x => x.DetalleFactura.FirstOrDefault().Orden.Paciente.Nombre_pac.Contains(valor)).ToList();

                @ViewBag.Total = result.Count();
            }
            return View(result);

        }

        public ActionResult DescargarFact(int id)
        {
            List<Factura> model = db.Facturas.Where(x => x.Id == id).ToList();
            MemoryStream ms = new MemoryStream();

            PdfWriter pw = new PdfWriter(ms);

            PdfDocument pd = new PdfDocument(pw);
            Document doc = new Document(pd, PageSize.LETTER);
            doc.SetMargins(75, 35, 70, 35);

            string pathlogo = Server.MapPath("~/Content/header.jpg");
            Image img = new Image(ImageDataFactory.Create(pathlogo));


            PdfFont font = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN);


            pd.AddEventHandler(PdfDocumentEvent.START_PAGE, new HeaderEventHandler1(img));

            Table t = new Table(1).UseAllAvailableWidth();
            foreach (var item in model.FirstOrDefault().DetalleFactura)
            {

                Cell c = new Cell().Add(new Paragraph("Nombre del Cliente: " + item.Orden.Paciente.Nombre_pac).SetFontSize(14)).SetTextAlignment(TextAlignment.LEFT).SetBorder(Border.NO_BORDER);
                t.AddCell(c);
                c = new Cell().Add(new Paragraph("Dirección: " + item.Orden.Paciente.Direccion).SetFontSize(14)).SetTextAlignment(TextAlignment.LEFT).SetBorder(Border.NO_BORDER);
                t.AddCell(c);
                c = new Cell().Add(new Paragraph("Correo Electronico: " + item.Orden.Paciente.Correo).SetFontSize(14)).SetTextAlignment(TextAlignment.LEFT).SetBorder(Border.NO_BORDER);
                t.AddCell(c);
                c = new Cell().Add(new Paragraph("Contraseña Automatica: *123456").SetFontSize(14)).SetTextAlignment(TextAlignment.LEFT).SetBorder(Border.NO_BORDER);
                t.AddCell(c);
                t.SetMarginBottom(50f);
                t.SetMarginTop(100f);
                break;
            }


            doc.Add(t);


            Table _table = new Table(4).UseAllAvailableWidth();
            Cell _Cell = new Cell().Add(new Paragraph("Cantidad")).SetTextAlignment(TextAlignment.CENTER);
            _table.AddCell(_Cell);
            _Cell = new Cell().Add(new Paragraph("Codigo de Orden")).SetTextAlignment(TextAlignment.CENTER);
            _table.AddCell(_Cell);
            _Cell = new Cell().Add(new Paragraph("Examenes")).SetTextAlignment(TextAlignment.CENTER);
            _table.AddCell(_Cell);
            _Cell = new Cell().Add(new Paragraph("Precio Unitario")).SetTextAlignment(TextAlignment.CENTER);
            _table.AddCell(_Cell);




            foreach (var item in model.FirstOrDefault().DetalleFactura)
            {
                _Cell = new Cell().Add(new Paragraph(item.Cantidad.ToString())).SetTextAlignment(TextAlignment.CENTER);
                _table.AddCell(_Cell);
                _Cell = new Cell().Add(new Paragraph(item.Orden.Cod_Orden)).SetTextAlignment(TextAlignment.CENTER);
                _table.AddCell(_Cell);
                _Cell = new Cell().Add(new Paragraph(item.Orden.DetalleOrden.FirstOrDefault().Examen.Nombre_exa)).SetTextAlignment(TextAlignment.CENTER);
                _table.AddCell(_Cell);
                _Cell = new Cell().Add(new Paragraph("C$" + item.PrecioUnitario.ToString())).SetTextAlignment(TextAlignment.CENTER);
                _table.AddCell(_Cell);
            }

            Table tabla = new Table(1).UseAllAvailableWidth();
            Cell celda = new Cell().Add(new Paragraph(String.Format("Total: {0:C}", double.Parse(model.FirstOrDefault().DetalleFactura.Sum(x => x.Cantidad * x.PrecioUnitario).ToString()))).SetTextAlignment(TextAlignment.LEFT));
            tabla.AddCell(celda);


            doc.Add(_table);
            doc.Add(tabla);

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

                Rectangle RootArea = new Rectangle(10, page.GetPageSize().GetTop() - 55, page.GetPageSize().GetRight() - 70, 60);

                Canvas canvas = new Canvas(page, RootArea);
                canvas
                    .Add(getTable(DocEvent)).SetFontSize(10f)
                    .ShowTextAligned("Dirección: Del parque central 2 cuadras al oeste y 2 cuadras al norte", 300, 50, TextAlignment.CENTER).SetFontSize(10f)
                     .ShowTextAligned("Clínica familiar Masatepe, Nicaragua Teléfono: 82463210(Movistar)", 300, 30, TextAlignment.CENTER).SetFontSize(10f)
                    .ShowTextAligned("Atenderle es un Privilegio", 300, 10, TextAlignment.CENTER)
                    .Close();
            }
            public Table getTable(PdfDocumentEvent DocEvent)
            {
                float[] cellWidth = { 40f, 60f };
                Table TableEvent = new Table(UnitValue.CreatePercentArray(cellWidth)).UseAllAvailableWidth();

                Style StyleCell = new Style().SetBorder(Border.NO_BORDER);

                Style StyleText = new Style().SetTextAlignment(TextAlignment.LEFT).SetFontSize(10f);

                Cell cell = new Cell().Add(Img.SetHeight(120).SetWidth(570));

                TableEvent.AddCell(cell.AddStyle(StyleCell).SetTextAlignment(TextAlignment.JUSTIFIED));

                return TableEvent;

            }
        }

        // GET: Facturas/Details/5
        public ActionResult Details(int? id)
        {
            if(User.IsInRole("Admin"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Factura factura = db.Facturas.Find(id);
                if (factura == null)
                {
                    return HttpNotFound();
                }
                return View(factura);
            }
            else
            {
                ViewBag.Ide = db.Empleados.Where(x => x.User == User.Identity.Name).FirstOrDefault().Id;

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Factura factura = db.Facturas.Find(id);
                if (factura == null)
                {
                    return HttpNotFound();
                }
                return View(factura);
            }
           
        }

        // GET: Facturas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Facturas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FechaFac")] Factura factura)
        {
            if (ModelState.IsValid)
            {
                db.Facturas.Add(factura);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(factura);
        }

        // GET: Facturas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Factura factura = db.Facturas.Find(id);
            if (factura == null)
            {
                return HttpNotFound();
            }
            return View(factura);
        }

        // POST: Facturas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FechaFac,CodFactura")] Factura factura)
        {
            if (ModelState.IsValid)
            {
                db.Entry(factura).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(factura);
        }

        // GET: Facturas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Factura factura = db.Facturas.Find(id);
            if (factura == null)
            {
                return HttpNotFound();
            }
            return View(factura);
        }

        // POST: Facturas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Factura factura = db.Facturas.Find(id);
            db.Facturas.Remove(factura);
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
