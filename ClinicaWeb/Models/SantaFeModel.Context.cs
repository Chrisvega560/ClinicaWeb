//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClinicaWeb.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class SantaFeModelContainer : DbContext
    {
        public SantaFeModelContainer()
            : base("name=SantaFeModelContainer")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Departamento> Departamentos { get; set; }
        public virtual DbSet<Municipio> Municipios { get; set; }
        public virtual DbSet<Paciente> Pacientes { get; set; }
        public virtual DbSet<Sexo> Sexos { get; set; }
        public virtual DbSet<Categoria> Categorias { get; set; }
        public virtual DbSet<Examen> Examenes { get; set; }
        public virtual DbSet<ExamenParametro> ExamenesParametros { get; set; }
        public virtual DbSet<DetalleOrden> DetallesOrdenes { get; set; }
        public virtual DbSet<Orden> Ordenes { get; set; }
        public virtual DbSet<Medico> Medicos { get; set; }
        public virtual DbSet<DetalleFactura> DetallesFacturas { get; set; }
        public virtual DbSet<Empleado> Empleados { get; set; }
        public virtual DbSet<Factura> Facturas { get; set; }
        public virtual DbSet<Resultado> Resultados { get; set; }
        public virtual DbSet<DetalleResultado> DetallesResultados { get; set; }
        public virtual DbSet<Parametro> Parametros { get; set; }
        public virtual DbSet<Reactivo> Reactivos { get; set; }
        public virtual DbSet<Cargo> CargoSet { get; set; }
    }
}
