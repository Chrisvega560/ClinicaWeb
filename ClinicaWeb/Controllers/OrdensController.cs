﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using ClinicaWeb.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ClinicaWeb.Controllers
{
    public class OrdensController : Controller
    {
        private SantaFeModelContainer db = new SantaFeModelContainer();
        string Upattern = @"^[A-Z0-9]{2} [0-9]{2} [0-9]{2}[A-Z]{0,1}$";
        ApplicationDbContext context = new ApplicationDbContext();

        public static string cod()
        {
            string charset = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            Random random = new Random();
            string cad = "";
            for (int i = 1; i < 3; i++)
            {
                cad += charset.Substring(random.Next(0, 36), 1);
            }
            return cad;
        }
        [Authorize(Roles = "Admin,Recepcionista,Laboratorista")]
        // GET: Ordens
        public ActionResult Index()
        {
            var ordenes = db.Ordenes.Include(o => o.Medico).Include(o => o.Paciente).Include(o => o.Empleado).Where(x=> x.Estado== "Aprobado");
            return View(ordenes.ToList());
        }

        [Authorize(Roles = "Admin,Recepcionista,Laboratorista")]
        public ActionResult OrdenesFinalizadas()
        {
            var ordenes = db.Ordenes.Where(x => x.Estado == "Finalizado");
            return View(ordenes.ToList());
        }
        [Authorize(Roles = "Paciente")]
        public ActionResult OrdenesPacientes()
        {

            var ordenes = db.Ordenes.Where(x => x.Paciente.Correo == User.Identity.Name.ToString());
            ViewBag.cant = (from o in db.Ordenes where o.Paciente.Correo == User.Identity.Name.ToString() select o).Count();
            return View(ordenes.ToList());
        }

        public ActionResult Solicitudes()
        {

            var Do = db.Ordenes.Where(x => x.Estado == "Solicitud");
            ViewBag.cant = Do.ToList().Count();
            return View(Do.ToList());
        }

        //Busqueda Implacable
        public ActionResult ListOrder(string valor)
        {
            List<Orden> result = new List<Orden>();
            if (User.IsInRole("Recepcionista") || User.IsInRole("Laboratorista"))
            {
                
                valor = valor.Trim();
                ViewBag.titulo = " | Resultados para " + valor;
                if (Regex.IsMatch(valor, Upattern))
                {
                    result = db.Ordenes.Where(x => x.Paciente.Nombre_pac == valor).ToList();
                }
                result = db.Ordenes.Where(x => x.Paciente.Nombre_pac.Contains(valor)).ToList();

                @ViewBag.Total = result.Count();
            }
            else if (User.IsInRole("Paciente"))
            {
                //Busqueda Implacable por codigo
                valor = valor.Trim();
                ViewBag.titulo = " | Resultados para " + valor;
                if (Regex.IsMatch(valor, Upattern))
                {
                    result = db.Ordenes.Where(x => x.Cod_Orden == valor).ToList();
                }
                result = db.Ordenes.Where(x => x.Cod_Orden.Contains(valor)).ToList();

                @ViewBag.Total = result.Count();
            }
            return View(result);

        }

        // GET: Ordens/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orden orden = db.Ordenes.Find(id);
            if (orden == null)
            {
                return HttpNotFound();
            }
            return View(orden);
        }

        // GET: Ordens/Create
        public ActionResult Create()
        {

            ViewBag.MunicipioId = new SelectList(db.Municipios, "Id", "Nombre_mun");
            ViewBag.SexoId = new SelectList(db.Sexos, "Id", "TipoSexo");
            ViewBag.MedicoId = new SelectList(db.Medicos, "Id", "NombreMed");
            ViewBag.PacienteId = new SelectList(db.Pacientes, "Id", "Nombre_pac");
            ViewBag.EmpleadoId = new SelectList(db.Empleados, "Id", "Nom_Empleado");
            return View();
        }

        // POST: Ordens/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Cod_Orden,Fecha,Lugar,MedicoId,PacienteId,EmpleadoId")] Orden orden)
        {

            if(User.IsInRole("Paciente"))
            {
                orden.EmpleadoId = (from Empleado in db.Empleados where Empleado.User == "Movil@gmail.com" select Empleado.Id).FirstOrDefault();
                orden.PacienteId = (from paciente in db.Pacientes where paciente.Correo == User.Identity.Name.ToString() select paciente.Id).FirstOrDefault();
                orden.Estado = "Solicitud";

                List<DetalleOrden> cart = (List<DetalleOrden>)Session["ShoppingCart"];
                orden.DetalleOrden = cart.Select(x => new DetalleOrden()
                {
                    OrdenId = x.OrdenId,
                    Cantidad = x.Cantidad,
                    FechaFinal = DateTime.Today.ToString(),
                    Precio = x.Examen.Precio,
                    Estado = "Pendiente",
                    ExamenId = x.ExamenId,
                }).ToList();

                Factura fact = new Factura();
                fact.CodFactura = cod();
                fact.FechaFac = DateTime.Today.ToString();
                fact.DetalleFactura = cart.Select(y => new DetalleFactura()
                {
                    Cantidad = double.Parse(y.Cantidad),
                    Descripcion = y.Examen.Nombre_exa,
                    OrdenId = orden.Id,
                    PrecioUnitario = y.Examen.Precio,
                    FacturaId = fact.Id,
                    Subtotal = (y.Examen.Precio * double.Parse(y.Cantidad)),
                }).ToList();

                if (ModelState.IsValid)
                {
                    db.Ordenes.Add(orden);
                    db.Facturas.Add(fact);
                    db.SaveChanges();
                    Session.Clear();

                    return RedirectToAction("OrdenesPacientes", "Ordens");
                }


            }
            else
            {
                orden.EmpleadoId = (from Empleado in db.Empleados where Empleado.User == User.Identity.Name.ToString() select Empleado.Id).FirstOrDefault();
                orden.Estado = "Aprobado";

                List<DetalleOrden> cart = (List<DetalleOrden>)Session["ShoppingCart"];
                orden.DetalleOrden = cart.Select(x => new DetalleOrden()
                {
                    OrdenId = x.OrdenId,
                    Cantidad = x.Cantidad,
                    FechaFinal = DateTime.Today.ToString(),
                    Precio = x.Examen.Precio,
                    Estado = "Pendiente",
                    ExamenId = x.ExamenId,
                }).ToList();

                Factura fact = new Factura();
                fact.CodFactura = cod();
                fact.FechaFac = DateTime.Today.ToString();
                fact.DetalleFactura = cart.Select(y => new DetalleFactura()
                {
                    Cantidad = double.Parse(y.Cantidad),
                    Descripcion = y.Examen.Nombre_exa,
                    OrdenId = orden.Id,
                    PrecioUnitario = y.Examen.Precio,
                    FacturaId = fact.Id,
                    Subtotal = (y.Examen.Precio * double.Parse(y.Cantidad)),
                }).ToList();
                
                if (ModelState.IsValid)
                {
                    db.Ordenes.Add(orden);
                    db.Facturas.Add(fact);
                    db.SaveChanges();
                    Session.Clear();
                    return RedirectToAction("Details", "Facturas", new { id = db.Facturas.ToList().Last().Id });

                }
            }

           

            

            ViewBag.MedicoId = new SelectList(db.Medicos, "Id", "NombreMed", orden.MedicoId);
            ViewBag.PacienteId = new SelectList(db.Pacientes, "Id", "Nombre_pac", orden.PacienteId);
            ViewBag.EmpleadoId = new SelectList(db.Empleados, "Id", "Nom_Empleado", orden.EmpleadoId);
            return View(orden);
        }

        // GET: Ordens/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orden orden = db.Ordenes.Find(id);
            if (orden == null)
            {
                return HttpNotFound();
            }
            ViewBag.MedicoId = new SelectList(db.Medicos, "Id", "NombreMed", orden.MedicoId);
            ViewBag.PacienteId = new SelectList(db.Pacientes, "Id", "Nombre_pac", orden.PacienteId);
            ViewBag.EmpleadoId = new SelectList(db.Empleados, "Id", "Nom_Empleado", orden.EmpleadoId);
            return View(orden);
        }

        // POST: Ordens/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Cod_Orden,Fecha,Lugar,MedicoId,PacienteId,EmpleadoId")] Orden orden)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orden).State = EntityState.Modified;
                orden.Estado = "Aprobado";
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MedicoId = new SelectList(db.Medicos, "Id", "NombreMed", orden.MedicoId);
            ViewBag.PacienteId = new SelectList(db.Pacientes, "Id", "Nombre_pac", orden.PacienteId);
            ViewBag.EmpleadoId = new SelectList(db.Empleados, "Id", "Nom_Empleado", orden.EmpleadoId);
            return View(orden);
        }

        public ActionResult OrdenFinal(int id)
        {
            Orden orden = db.Ordenes.Find(id);

            if (ModelState.IsValid)
            {
                db.Entry(orden).State = EntityState.Modified;
                orden.Estado = "Finalizado";
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MedicoId = new SelectList(db.Medicos, "Id", "NombreMed", orden.MedicoId);
            ViewBag.PacienteId = new SelectList(db.Pacientes, "Id", "Nombre_pac", orden.PacienteId);
            ViewBag.EmpleadoId = new SelectList(db.Empleados, "Id", "Nom_Empleado", orden.EmpleadoId);
            return View(orden);
        }

        // GET: Ordens/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orden orden = db.Ordenes.Find(id);
            if (orden == null)
            {
                return HttpNotFound();
            }
            return View(orden);
        }

        // POST: Ordens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Orden orden = db.Ordenes.Find(id);
            db.Ordenes.Remove(orden);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult addpaciente(string nombre, int edad, string correo, string celular, string direccion, int Municipio, int sexo)
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
                    string pwd = "123456";
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
        [HttpPost]
        public ActionResult addMedico(string nombre,string celular)
        {
            try
            {
                Medico med = new Medico();

                med.NombreMed = nombre;
                med.Celular = celular;

                db.Medicos.Add(med);
                db.SaveChanges();
                return Json("'Success':'true'");

            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.Message);
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