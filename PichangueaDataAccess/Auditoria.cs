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
    
    public partial class Auditoria
    {
        public decimal idAuditoria { get; set; }
        public string audTipo { get; set; }
        public string audPagina { get; set; }
        public string audEvento { get; set; }
        public string audId { get; set; }
        public string audTexto { get; set; }
        public string audError { get; set; }
        public string audIp { get; set; }
        public string audUsuario { get; set; }
        public Nullable<System.DateTime> audCreacion { get; set; }
    }
}