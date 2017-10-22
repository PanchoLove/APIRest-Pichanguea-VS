using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PichangueaDataAccess;

namespace APIRestPichangueaVS.AdditionaModels
{

    public class PartidoCompuesto 
    {
        public PartidoCompuesto()
        {

        }


        public Equipo equipo { get; set; }
        public Tipo_Partido Tipo_Partido { get; set; }

        public decimal idPartido { get; set; }

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