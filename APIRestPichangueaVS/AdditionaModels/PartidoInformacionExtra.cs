using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIRestPichangueaVS.AdditionaModels
{
    public class PartidoInformacionExtra
    {
        public PartidoInformacionExtra() {
        }

        public PartidoInformacionExtra(PartidoCompuesto partido, decimal? asistencia, decimal? galletas)
        {
            this.partido = partido;
            this.asistencia = asistencia;
            this.galletas = galletas;
        }

        public PartidoCompuesto partido { get; set; }
        public Nullable<decimal> asistencia { get; set; }
        public Nullable<decimal> galletas { get; set; }


    }
}