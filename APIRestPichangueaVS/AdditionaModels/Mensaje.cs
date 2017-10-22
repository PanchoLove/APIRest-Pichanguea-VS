using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIRestPichangueaVS.AdditionaModels
{
    public class Mensaje
    {
        public Mensaje()
        {
        }

        public Mensaje(JugadorSimple jugador, string contenido, DateTime? creacion)
        {
            this.jugador = jugador;
            this.contenido = contenido;
            this.creacion = creacion;
        }

        public JugadorSimple jugador { get; set; }
        public String contenido { get; set; }
        public Nullable<System.DateTime> creacion { get; set; }


    }
}