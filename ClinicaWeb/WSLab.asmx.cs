using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using ClinicaWeb.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace ClinicaWeb
{
    /// <summary>
    /// Descripción breve de WSLab
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    public class WSLab : System.Web.Services.WebService
    {

        ApplicationDbContext context = new ApplicationDbContext();
        SantaFeModelContainer db = new SantaFeModelContainer();

        [WebMethod]
        public bool Logueo(string UserName, string password)
        {
            return Validar(UserName, password);
        }

        bool Validar(string UserName, string password)
        {
            var result = HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>().PasswordSignIn(UserName, password, false, false);

            //Verificamos si fue exitoso
            if (result == SignInStatus.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        [WebMethod]
        public bool AddPaciente(string nombrePac, int Edad, string correo, string Celular, int idSexo, int idMunicipio, string direccion, string pass)
        {
            Paciente p = new Paciente();
            p.Nombre_pac = nombrePac;
            p.Edad = Edad;
            p.Correo = correo;
            p.Celular = Celular;
            p.SexoId = idSexo;
            p.MunicipioId = idMunicipio;
            p.Direccion = direccion;

            db.Pacientes.Add(p);

            if (db.SaveChanges() > 0)
            {
                //Si se guardo la informacion del usuario
                //Agregamos un usuario para ese usuario
                var ManejadorUsuario = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var user = new ApplicationUser();

                //Asignamos los valores
                user.UserName = correo;
                user.Email = correo;
                string pwd = pass;

                //Procedemos a agregar el usuario
                var verificar = ManejadorUsuario.Create(user, pwd);

                //Asignamos el usuario ocn su respectivo rol
                if (verificar.Succeeded)
                {
                    var result = ManejadorUsuario.AddToRole(user.Id, "Paciente");
                    return true;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        [WebMethod]
        public bool AddOrden(string codigo, string fechaInicio, string correo, List<DetalleOrdenWS> Cart_Mobile)
        {
            Orden item = new Orden();
            item.Cod_Orden = codigo;
            item.Fecha = fechaInicio;
            item.Lugar = "Domicilio";
            item.PacienteId = PacientebyId(correo);
            item.MedicoId = 1;
            item.EmpleadoId = 2;
            item.Estado = "Solicitud";
            item.DetalleOrden = Cart_Mobile.Select(x => new DetalleOrden()
            {
                OrdenId = x.Id,
                Cantidad = "1",
                Estado = "Pendiente",
                FechaFinal = DateTime.Today.ToString(),
                Precio = x.Precio,
                ExamenId = x.ExamenId
            }).ToList();

            db.Ordenes.Add(item);
            if (db.SaveChanges() > 0)
            {
                return true;
            }
            else
                return false;
        }

        [WebMethod]
        public List<SexoWS> ListaSexo()
        {
            return db.Sexos.Select(x => new SexoWS()
            {
                Id = x.Id,
                TipoSexo = x.TipoSexo
            }).ToList();
        }

        [WebMethod]
        public List<MedicoWS> ListaMedico()
        {
            return db.Medicos.Select(x => new MedicoWS()
            {
                Id = x.Id,
                Nombre_Med = x.NombreMed,
                celular = x.Celular
            }).ToList();
        }

        [WebMethod]
        public List<EmpleadoWS> ListaEmpleado()
        {
            return db.Empleados.Select(x => new EmpleadoWS()
            {
                Id = x.Id,
                Nombre_emp = x.Nom_Empleado,
                user = x.User
            }).ToList();
        }

        [WebMethod]
        public List<CategoriaWS> ListaCategoria()
        {
            return db.Categorias.Select(x => new CategoriaWS()
            {
                Id = x.Id,
                Tipo = x.Tipo
            }).ToList();
        }

        [WebMethod]
        public List<ExamenWS> ListaExamen()
        {
            return db.Examenes.Select(x => new ExamenWS()
            {
                Id = x.Id,
                Nombre_exa = x.Nombre_exa,
                Precio = x.Precio,
                CategoriaId = x.CategoriaId
            }).ToList();
        }

        [WebMethod]
        public List<OrdenWS> ListaOrden(string correo)
        {
            int idpac = PacientebyId(correo);
            return db.Ordenes.Where(x => x.PacienteId == idpac).OrderByDescending(x=> x.Fecha).Select(x => new OrdenWS()
            {
                Id = x.Id,
                EmpleadoId = x.EmpleadoId,
                PacienteId = x.PacienteId,
                Cod_orden = x.Cod_Orden,
                Fecha = x.Fecha,
                MedicoId = x.MedicoId,
                tipo = x.Lugar,
                Estado = x.Estado

            }).ToList();
        }

        [WebMethod]
        public List<DepartamentoWS> ListaDepartamento()
        {
            return db.Departamentos.Select(x => new DepartamentoWS()
            {
                Id = x.Id,
                Nombre_dpto = x.Nombre_dpto
            }).ToList();
        }

        [WebMethod]
        public List<MunicipioWS> ListaMunicipio()
        {
            return db.Municipios.Select(x => new MunicipioWS()
            {
                Id = x.Id,
                Nombre_mun = x.Nombre_mun,
                DepartamentoId = x.DepartamentoId
            }).ToList();
        }

        [WebMethod]
        public List<DetalleOrdenWS> ListaDetalleOrden(int ordenId)
        {
            return db.DetallesOrdenes.Where(x => x.OrdenId == ordenId).Select(x => new DetalleOrdenWS()
            {
                Id = x.Id,
                Cantidad = x.Cantidad,
                ExamenId = x.ExamenId,
                idCategoria = x.Examen.CategoriaId,
                Precio = x.Precio,
                Estado = x.Estado,
            }).ToList();
        }

        public int PacientebyId(string user)
        {
            PacienteWS p = db.Pacientes.Where(x => x.Correo == user).Select(x => new PacienteWS()
            {
                Id = x.Id
            }).FirstOrDefault();
            return p.Id;
        }

        [WebMethod]
        public string PacientebyName(string user)
        {
            PacienteWS p = db.Pacientes.Where(x => x.Correo == user).Select(x => new PacienteWS()
            {
                Nombre_pac = x.Nombre_pac
            }).FirstOrDefault();
            return p.Nombre_pac;
        }
        public class DepartamentoWS
        {
            public int Id { get; set; }
            public string Nombre_dpto { get; set; }
        }

        public class MunicipioWS
        {
            public int Id { get; set; }
            public int DepartamentoId { get; set; }
            public string Nombre_mun { get; set; }

        }
        public class SexoWS
        {
            public int Id { get; set; }
            public string TipoSexo { get; set; }
        }
        public class PacienteWS
        {
            public int Id { get; set; }
            public string Nombre_pac { get; set; }
            public int edad { get; set; }
            public string Correo { get; set; }
            public string celular { get; set; }
            public int SexoId { get; set; }
            public int MunicipioId { get; set; }
            public string Direccion { get; set; }
        }

        public class CategoriaWS
        {
            public int Id { get; set; }
            public string Tipo { get; set; }
        }

        public class ExamenWS
        {
            public int Id { get; set; }
            public string Nombre_exa { get; set; }
            public double Precio { get; set; }
            public int CategoriaId { get; set; }
        }

        public class MedicoWS
        {
            public int Id { get; set; }
            public string Nombre_Med { get; set; }
            public string celular { get; set; }
        }

        public class EmpleadoWS
        {
            public int Id { get; set; }
            public string Nombre_emp { get; set; }
            public string user { get; set; }
            public int CagoId { get; set; }
        }

        public class OrdenWS
        {
            public int Id { get; set; }
            public int EmpleadoId { get; set; }
            public int PacienteId { get; set; }
            public string Cod_orden { get; set; }
            public string Fecha { get; set; }
            public int MedicoId { get; set; }
            public string tipo { get; set; }
            public string Estado { get; set; }
        }

        public class DetalleOrdenWS
        {
            public int Id { get; set; }
            public string Cantidad { get; set; }
            public int ExamenId { get; set; }
            public Nullable<double> Precio { get; set; }
            public string Estado { get; set; }
            public int OrdenId { get; set; }
            public int idCategoria { get; set; }
        }
    }
}
