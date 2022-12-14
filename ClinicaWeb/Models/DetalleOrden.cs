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
    using System.Collections.Generic;
    
    public partial class DetalleOrden
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DetalleOrden()
        {
            this.Resultado = new HashSet<Resultado>();
        }
    
        public int Id { get; set; }
        public string Cantidad { get; set; }
        public string FechaFinal { get; set; }
        public Nullable<double> Precio { get; set; }
        public string Estado { get; set; }
        public int OrdenId { get; set; }
        public int ExamenId { get; set; }
    
        public virtual Orden Orden { get; set; }
        public virtual Examen Examen { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Resultado> Resultado { get; set; }
    }
}
