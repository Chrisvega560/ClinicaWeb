using ClinicaWeb.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ClinicaWeb.Controllers
{
    public partial class ProductoMes
    {
        int id;
        Examen examen;
        int cant;
        double total;

        public ProductoMes()
        { }
        public ProductoMes(int id, Examen ex, int cant, double total)
        {
            this.Id = id;
            this.examen = ex;
            this.Cant = cant;
            this.Total = total;
        }

        public int Id { get => id; set => id = value; }
        public Examen exa { get => examen; set => examen = value; }
        public int Cant { get => cant; set => cant = value; }
        public double Total { get => total; set => total = value; }
    }

    public partial class Ventas
    {
        double monto;
        DateTime dia;
        int cant;
        int totalitem;
        public Ventas()
        { }
        public Ventas(double monto, DateTime dia, int cant)
        {
            this.Monto = monto;
            this.Dia = dia;
            this.Cant = cant;
        }

        public double Monto { get => monto; set => monto = value; }
        public int Cant { get => cant; set => cant = value; }
        public DateTime Dia { get => dia; set => dia = value; }
        public int Totalitem { get => totalitem; set => totalitem = value; }
    }

    public partial class Dashboard
    {
        List<ProductoMes> mes = new List<ProductoMes>();
        List<Ventas> annio = new List<Ventas>();
        public List<ProductoMes> Mes { get => mes; set => mes = value; }
        public List<Ventas> Annio { get => annio; set => annio = value; }
    }
    [Authorize]
    public class HomeController : Controller
    {
        private SantaFeModelContainer db = new SantaFeModelContainer();
        ApplicationDbContext context = new ApplicationDbContext();
        public ActionResult Index()
        {
            if (User.IsInRole("Paciente"))
            {
                return RedirectToAction("OrdenesPacientes", "Ordens");
            }
            else
            {
                ViewBag.examen = db.Examenes.Count();
                ViewBag.pacientes = db.Pacientes.Count();
                ViewBag.ave = db.DetallesFacturas.Sum(x => x.Cantidad);
                ViewBag.price = db.DetallesFacturas.Sum(x => x.PrecioUnitario);
                ViewBag.facturas = db.Facturas.ToList();
                ViewBag.Orders = db.Ordenes.Where(x => x.Estado == "Aprobado").Count();

                Dashboard d = new Dashboard();

                d.Mes = db.DetallesFacturas.ToList().Where(x => DateTime.Parse(x.Factura.FechaFac) > DateTime.Today.AddDays(-30)).GroupBy(x => x.Orden.DetalleOrden.FirstOrDefault().ExamenId).Select(x => new ProductoMes()
                {
                    Id = x.FirstOrDefault().Id,
                    Cant = x.Count(),
                    exa = x.FirstOrDefault().Orden.DetalleOrden.FirstOrDefault().Examen,
                    Total = x.Sum(y => y.Cantidad * y.PrecioUnitario),

                }).Take(10).ToList();


                DateTime inicio = new DateTime(DateTime.Today.Year - 1, DateTime.Today.Month, 1);

                if (d.Mes == null)
                    d.Mes = new List<ProductoMes>();

                d.Annio = db.Facturas.ToList().Where(x => DateTime.Parse(x.FechaFac) >= inicio && DateTime.Parse(x.FechaFac) <= DateTime.Today).GroupBy(x => DateTime.Parse(x.FechaFac).Month).Select(x => new Ventas()
                {
                    Monto = x.Sum(y => y.DetalleFactura.Sum(z=> z.Cantidad * z.PrecioUnitario)),
                    Cant = x.Count(),
                    Dia = DateTime.Parse(x.FirstOrDefault().FechaFac),
                    Totalitem = (int)x.Sum(y => y.DetalleFactura.Sum(z=> z.Cantidad))
                }).ToList();

                ViewBag.Ide = db.Empleados.Where(x => x.User == User.Identity.Name).FirstOrDefault().Id;

                return View(d);
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [AllowAnonymous]
        public ActionResult PrincipalPage()
        {
            return View();
        }
        public ActionResult Registro()
        {
            ViewBag.MunicipioId = new SelectList(db.Municipios, "Id", "Nombre_mun");
            ViewBag.SexoId = new SelectList(db.Sexos, "Id", "TipoSexo");
            ViewBag.DepartamentoId = new SelectList(db.Departamentos, "Id", "Nombre_dpto");
            return View();
        }
        [HttpPost]
        public ActionResult addpaciente(string nombre, int edad, string correo, string celular, string direccion, int Municipio, int sexo, string pass)
        {
            try
            {
                Paciente paciente = new Paciente();
                paciente.Nombre_pac = nombre;
                paciente.Edad = edad;
                paciente.Correo = correo;
                paciente.Celular = celular;
                paciente.Direccion = direccion;
                paciente.MunicipioId = Municipio;
                paciente.SexoId = sexo;

                db.Pacientes.Add(paciente);
                if (db.SaveChanges() > 0)
                {
                    var ManejadorUsuario = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                    var user = new ApplicationUser();
                    user.UserName = paciente.Correo;
                    user.Email = paciente.Correo;
                    string pwd = pass;
                    var verificar = ManejadorUsuario.Create(user, pwd);
                    if (verificar.Succeeded)
                    {
                        var result = ManejadorUsuario.AddToRole(user.Id, "Paciente");
                        return Json("'Success':'true'");
                    }
                }
                return Json("'Success':'true'");

            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        public JsonResult Select(int id)
        {
            return Json(new SelectList(db.Municipios.Where(empt => empt.DepartamentoId == id), "Id", "Nombre_mun"), JsonRequestBehavior.AllowGet);
        }


    }
}