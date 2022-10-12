using ClinicaWeb.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ClinicaWeb.Startup))]
namespace ClinicaWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            Roles();
        }
        private void Roles()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var manejadorRol = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            var manejadorUsuario = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if (!manejadorRol.RoleExists("Admin"))
            {
                var rol = new IdentityRole();

                rol.Name = "Admin";
                manejadorRol.Create(rol);


                var user = new ApplicationUser();
                user.UserName = "admin@gmail.com";
                user.Email = "admin@gmail.com";

                string pwd = "123456";

                var verificar = manejadorUsuario.Create(user, pwd);

                if (verificar.Succeeded)
                {
                    var result = manejadorUsuario.AddToRole(user.Id, "Admin");
                }
            }

            //Rol Empleado - Laboratorista

            if (!manejadorRol.RoleExists("Laboratorista"))
            {
                var rol = new IdentityRole();

                rol.Name = "Laboratorista";
                manejadorRol.Create(rol);


                var user = new ApplicationUser();
                user.UserName = "laboratorista@gmail.com";
                user.Email = "laboratorista@gmail.com";

                string pwd = "123456";

                var verificar = manejadorUsuario.Create(user, pwd);

                if (verificar.Succeeded)
                {
                    var result = manejadorUsuario.AddToRole(user.Id, "Laboratorista");
                }
            }

            //Rol Empleado - Recepcionista

            if (!manejadorRol.RoleExists("Recepcionista"))
            {
                var rol = new IdentityRole();

                rol.Name = "Recepcionista";
                manejadorRol.Create(rol);


                var user = new ApplicationUser();
                user.UserName = "recepcionista@gmail.com";
                user.Email = "recepcionista@gmail.com";

                string pwd = "123456";

                var verificar = manejadorUsuario.Create(user, pwd);

                if (verificar.Succeeded)
                {
                    var result = manejadorUsuario.AddToRole(user.Id, "Recepcionista");
                }
            }
            //Rol Paciente

            if (!manejadorRol.RoleExists("Paciente"))
            {
                var rol = new IdentityRole();

                rol.Name = "Paciente";
                manejadorRol.Create(rol);


                var user = new ApplicationUser();
                user.UserName = "paciente@gmail.com";
                user.Email = "paciente@gmail.com";

                string pwd = "123456";

                var verificar = manejadorUsuario.Create(user, pwd);

                if (verificar.Succeeded)
                {
                    var result = manejadorUsuario.AddToRole(user.Id, "Paciente");
                }
            }
        }
    }
}
