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
    
    public partial class Reactivo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Reactivo()
        {
            this.Parametro = new HashSet<Parametro>();
        }
    
        public int Id { get; set; }
        public string Maximo { get; set; }
        public string Minimo { get; set; }
        public string Densidad { get; set; }
        public string ValoresNormales { get; set; }
        public string Periodo { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Parametro> Parametro { get; set; }
    }
}