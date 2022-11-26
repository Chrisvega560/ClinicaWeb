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
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;

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


        public ActionResult pdf(int id)
        {
            List<DetalleOrden> model = db.DetallesOrdenes.Where(x => x.Id == id).ToList();
            MemoryStream ms = new MemoryStream();

            PdfWriter pw = new PdfWriter(ms);

            PdfDocument pd = new PdfDocument(pw);
            Document doc = new Document(pd, PageSize.LETTER);
            doc.SetMargins(75, 35, 70, 35);

            PdfFont font = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN);

            Table table = new Table(1).UseAllAvailableWidth();
            foreach (var item in model)
            {
                Cell cell = new Cell().Add(new Paragraph("Nombre del Paciente: " + model.FirstOrDefault().Orden.Paciente.Nombre_pac).SetFontSize(14)).SetTextAlignment(TextAlignment.LEFT).SetBorder(Border.NO_BORDER);
                table.AddCell(cell);
                cell = new Cell().Add(new Paragraph("Edad: " + model.FirstOrDefault().Orden.Paciente.Edad).SetFontSize(14)).SetTextAlignment(TextAlignment.LEFT).SetBorder(Border.NO_BORDER);
                table.AddCell(cell);
                cell = new Cell().Add(new Paragraph("Codigo de Orden: " + model.FirstOrDefault().Orden.Cod_Orden).SetFontSize(14)).SetTextAlignment(TextAlignment.LEFT).SetBorder(Border.NO_BORDER);
                table.AddCell(cell);
            }
            doc.Add(table);

            Table t = new Table(1).UseAllAvailableWidth();
            foreach (var item in model)
            {

                Cell c = new Cell().Add(new Paragraph(item.Examen.Nombre_exa).SetTextAlignment(TextAlignment.CENTER).SetFontSize(20F));
                t.AddCell(c);
                t.SetMarginBottom(50f);
            }


            doc.Add(t);
            Table _table = new Table(2).UseAllAvailableWidth();
            Cell _Cell = new Cell().Add(new Paragraph("Nombre del parametro")).SetTextAlignment(TextAlignment.LEFT).SetBorder(Border.NO_BORDER).SetBold();
            _table.AddHeaderCell(_Cell);
            _Cell = new Cell().Add(new Paragraph("Resultado")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetBold();
            _table.AddHeaderCell(_Cell);




            foreach (var item in model.FirstOrDefault().Resultado.FirstOrDefault().DetalleResultado)
            {
                _Cell = new Cell().Add(new Paragraph(item.Parametro.Nom_Parametro)).SetTextAlignment(TextAlignment.LEFT).SetBorder(Border.NO_BORDER);
                _table.AddCell(_Cell);
                _Cell = new Cell().Add(new Paragraph(item.Parametro.DetalleResultado.FirstOrDefault().ExamenResultado)).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER);
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

        public ActionResult Resultado(int id, int cat)
        {
            List<DetalleOrden> model = db.DetallesOrdenes.Where(x => x.Id == id).ToList();
            MemoryStream ms = new MemoryStream();

            PdfWriter pw = new PdfWriter(ms);

            PdfDocument pd = new PdfDocument(pw);
            Document doc = new Document(pd, PageSize.LETTER);
            doc.SetMargins(75, 35, 70, 35);

            string pathlogo = Server.MapPath("~/Content/header.jpg");
            Image img = new Image(ImageDataFactory.Create(pathlogo));
            pd.AddEventHandler(PdfDocumentEvent.START_PAGE, new HeaderEventHandler1(img));
            

            PdfFont font = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN);


            


            Table table = new Table(1).UseAllAvailableWidth();
            foreach (var item in model)
            {
                Cell cell = new Cell().Add(new Paragraph("Nombre del Paciente: " + model.FirstOrDefault().Orden.Paciente.Nombre_pac).SetFontSize(14)).SetTextAlignment(TextAlignment.LEFT).SetBorder(Border.NO_BORDER);
                table.AddCell(cell);
                cell = new Cell().Add(new Paragraph("Edad: " + model.FirstOrDefault().Orden.Paciente.Edad).SetFontSize(14)).SetTextAlignment(TextAlignment.LEFT).SetBorder(Border.NO_BORDER);
                table.AddCell(cell);
                cell = new Cell().Add(new Paragraph("Codigo de Orden: " + model.FirstOrDefault().Orden.Cod_Orden).SetFontSize(14)).SetTextAlignment(TextAlignment.LEFT).SetBorder(Border.NO_BORDER);
                table.AddCell(cell);
            }

          
            table.SetMarginBottom(10f);
            table.SetMarginTop(20f);

            doc.Add(table);


            Table t = new Table(1).UseAllAvailableWidth();
            foreach (var item in model)
            {

                Cell c = new Cell().Add(new Paragraph(item.Examen.Nombre_exa).SetTextAlignment(TextAlignment.CENTER).SetFontSize(20F));
                t.AddCell(c);
                t.SetMarginBottom(30f);
            }


            doc.Add(t);

            if (cat == 7 || cat == 3 || cat == 4 || cat == 8)
            {
                Table _table = new Table(4).UseAllAvailableWidth();
                Cell _Cell = new Cell().Add(new Paragraph("Nombre del parametro ")).SetTextAlignment(TextAlignment.LEFT).SetBorder(Border.NO_BORDER);
                _table.AddCell(_Cell);
                _Cell = new Cell().Add(new Paragraph("Resultado")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER);
                _table.AddCell(_Cell);
                _Cell = new Cell().Add(new Paragraph("Maximo")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER);
                _table.AddCell(_Cell);
                _Cell = new Cell().Add(new Paragraph("Minimo")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER);
                _table.AddCell(_Cell);




                foreach (var item in model.FirstOrDefault().Resultado.FirstOrDefault().DetalleResultado)
                {
                    _Cell = new Cell().Add(new Paragraph(item.Parametro.Nom_Parametro)).SetTextAlignment(TextAlignment.LEFT).SetBorder(Border.NO_BORDER);
                    _table.AddCell(_Cell);
                    _Cell = new Cell().Add(new Paragraph(item.ExamenResultado)).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER);
                    _table.AddCell(_Cell);
                    _Cell = new Cell().Add(new Paragraph(item.Parametro.Reactivo.Maximo)).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER);
                    _table.AddCell(_Cell);
                    _Cell = new Cell().Add(new Paragraph(item.Parametro.Reactivo.Minimo))
                        .SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER);
                    _table.AddCell(_Cell);
                }

                doc.Add(_table);
            }
            else if (cat == 5)
            {
                Table _table = new Table(2).UseAllAvailableWidth();
                Cell _Cell = new Cell().Add(new Paragraph("Nombre del parametro")).SetTextAlignment(TextAlignment.LEFT).SetBorder(Border.NO_BORDER);
                _table.AddCell(_Cell);
                _Cell = new Cell().Add(new Paragraph("Antibiograma")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER);
                _table.AddCell(_Cell);




                foreach (var item in model.FirstOrDefault().Resultado.FirstOrDefault().DetalleResultado)
                {
                    _Cell = new Cell().Add(new Paragraph(item.Parametro.Nom_Parametro)).SetTextAlignment(TextAlignment.LEFT).SetBorder(Border.NO_BORDER);
                    _table.AddCell(_Cell);
                    _Cell = new Cell().Add(new Paragraph(item.ExamenResultado)).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER);
                    _table.AddCell(_Cell);
                }

                doc.Add(_table);
            }
            else if (cat == 6)
            {
                Table _table = new Table(5).UseAllAvailableWidth();
                Cell _Cell = new Cell().Add(new Paragraph("Nombre del parametro ")).SetTextAlignment(TextAlignment.LEFT).SetBorder(Border.NO_BORDER);
                _table.AddCell(_Cell);
                _Cell = new Cell().Add(new Paragraph("Resultado")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER);
                _table.AddCell(_Cell);
                _Cell = new Cell().Add(new Paragraph("Maximo")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER);
                _table.AddCell(_Cell);
                _Cell = new Cell().Add(new Paragraph("Minimo")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER);
                _table.AddCell(_Cell);
                _Cell = new Cell().Add(new Paragraph("Dimensional")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER);
                _table.AddCell(_Cell);




                foreach (var item in model.FirstOrDefault().Resultado.FirstOrDefault().DetalleResultado)
                {
                    _Cell = new Cell().Add(new Paragraph(item.Parametro.Nom_Parametro)).SetTextAlignment(TextAlignment.LEFT).SetBorder(Border.NO_BORDER);
                    _table.AddCell(_Cell);
                    _Cell = new Cell().Add(new Paragraph(item.Parametro.DetalleResultado.FirstOrDefault().ExamenResultado)).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER);
                    _table.AddCell(_Cell);
                    _Cell = new Cell().Add(new Paragraph(item.Parametro.Reactivo.Maximo)).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER);
                    _table.AddCell(_Cell);
                    _Cell = new Cell().Add(new Paragraph(item.Parametro.Reactivo.Minimo))
                        .SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER);
                    _table.AddCell(_Cell);
                    _Cell = new Cell().Add(new Paragraph(item.Parametro.Reactivo.Densidad))
                        .SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER);
                    _table.AddCell(_Cell);
                }

                doc.Add(_table);
            }
            else if (cat == 1)
            {
                
                Table _table = new Table(2).UseAllAvailableWidth();
                Cell _Cell = new Cell().Add(new Paragraph("Nombre del parametro")).SetTextAlignment(TextAlignment.LEFT).SetBorder(Border.NO_BORDER).SetBold();
                _table.AddHeaderCell(_Cell);
                _Cell = new Cell().Add(new Paragraph("Resultado")).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER).SetBold();
                _table.AddHeaderCell(_Cell);




                foreach (var item in model.FirstOrDefault().Resultado.FirstOrDefault().DetalleResultado)
                {
                    _Cell = new Cell().Add(new Paragraph(item.Parametro.Nom_Parametro)).SetTextAlignment(TextAlignment.LEFT).SetBorder(Border.NO_BORDER);
                    _table.AddCell(_Cell);
                    _Cell = new Cell().Add(new Paragraph(item.ExamenResultado)).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER);
                    _table.AddCell(_Cell);

                }

                doc.Add(_table);

            }


           

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
                    .Add(getTable(DocEvent))
                    .ShowTextAligned("Dirección: Del parque central 2 cuadras al oeste y 2 cuadras al norte", 300, 50, TextAlignment.CENTER)
                     .ShowTextAligned("Clínica familiar Masatepe, Nicaragua Teléfono: 82463210(Movistar)", 300, 30, TextAlignment.CENTER)
                    .ShowTextAligned("Atenderle es un Privilegio", 300, 10, TextAlignment.CENTER)
                    .Close();
            }
            public Table getTable(PdfDocumentEvent DocEvent)
            {
                float[] cellWidth = {20f, 80f };
                Table TableEvent = new Table(UnitValue.CreatePercentArray(cellWidth)).UseAllAvailableWidth();

                Style StyleCell = new Style().SetBorder(Border.NO_BORDER);

                Style StyleText = new Style().SetTextAlignment(TextAlignment.LEFT).SetFontSize(10f);

                Cell cell = new Cell().Add(Img.SetHeight(100).SetWidth(570));

                TableEvent.AddCell(cell.AddStyle(StyleCell).SetTextAlignment(TextAlignment.JUSTIFIED));

                return TableEvent;

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
