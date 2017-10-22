using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIRestPichangueaVS.AdditionaModels
{
    public class JugadorSimple
    {

        public JugadorSimple() {

        }

        public JugadorSimple(decimal idJugador, string jugUsername, string jugFoto, string jugApodo)
        {
            this.idJugador = idJugador;
            this.jugUsername = jugUsername;
            this.jugFoto = jugFoto;
            this.jugApodo = jugApodo;
        }

        public decimal idJugador { get; set; }
        public string jugUsername { get; set; }
        public string jugFoto { get; set; }
        public string jugApodo { get; set; }
    }
}