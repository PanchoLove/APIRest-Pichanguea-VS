using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIRestPichangueaVS.AdditionaModels
{
    public class MensajeEntrada
    {
        public MensajeEntrada() {

        }

        public MensajeEntrada(decimal? idPartido, decimal? idJugador, string contenido)
        {
            this.idJugador = idJugador;
            this.contenido = contenido;
        }

        public Nullable<decimal> idPartido { get; set; }
        public Nullable<decimal> idJugador { get; set; }
        public String contenido { get; set; }
    }
}