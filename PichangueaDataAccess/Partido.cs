//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PichangueaDataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class Partido
    {
        public decimal idPartido { get; set; }
        public Nullable<decimal> idTipoPartido { get; set; }
        public Nullable<decimal> idEquipo { get; set; }
        public Nullable<System.DateTime> parFecha { get; set; }
        public Nullable<System.TimeSpan> parHora { get; set; }
        public string parRival { get; set; }
        public string parComplejo { get; set; }
        public string parCancha { get; set; }
        public string parUbicacion { get; set; }
        public string parGeoReferencia { get; set; }
        public Nullable<decimal> parEstado { get; set; }
        public Nullable<System.DateTime> parCreacion { get; set; }
    }
}