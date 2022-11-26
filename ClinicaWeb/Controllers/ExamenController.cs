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
                if(User.IsInRole("Admin"))
                {
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
        }

        public ActionResult DescargarReportExamen()
        {

            List<Examen> model = db.Examenes.ToList();
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

                Cell c = new Cell().Add(new Paragraph("Reporte de Examenes").SetTextAlignment(TextAlignment.CENTER).SetFontSize(20F));
                t.AddCell(c);
                t.SetMarginBottom(50f);
                t.SetMarginTop(150f);
                break;
            }


            doc.Add(t);


            Table _table = new Table(3).UseAllAvailableWidth();
            Cell _Cell = new Cell().Add(new Paragraph("Nombre del examen")).SetTextAlignment(TextAlignment.CENTER);
            _table.AddCell(_Cell);
            _Cell = new Cell().Add(new Paragraph("Categoria")).SetTextAlignment(TextAlignment.CENTER);
            _table.AddCell(_Cell);
            _Cell = new Cell().Add(new Paragraph("Precio")).SetTextAlignment(TextAlignment.CENTER);
            _table.AddCell(_Cell); 





            foreach (var item in model)
            {
                _Cell = new Cell().Add(new Paragraph(item.Nombre_exa)).SetTextAlignment(TextAlignment.CENTER);
                _table.AddCell(_Cell);
                _Cell = new Cell().Add(new Paragraph(item.Categoria.Tipo)).SetTextAlignment(TextAlignment.CENTER);
                _table.AddCell(_Cell);
                _Cell = new Cell().Add(new Paragraph("C$ " + item.Precio.ToString())).SetTextAlignment(TextAlignment.CENTER);
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
                    .ShowTextAligned("Dirección: Del parque central 2 cuadras al oeste y 2 cuadras al norte", 300,50, TextAlignment.CENTER)
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
                if(User.IsInRole("Admin"))
                {
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
                if(User.IsInRole("Admin"))
                {
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
