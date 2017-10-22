using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PichangueaDataAccess;

namespace APIRestPichangueaVS.AdditionaModels
{
    public class Sesion
    {
        public Sesion(Jugador jugador, string token)
        {
            this.jugador = jugador;
            this.token = token;
        }

        public Jugador jugador { get; set; }
     //  public List<Equipo> equipos{ get; set; }
     //  public List<PartidoInformacionExtra> eventos { get; set; }
        public String token { get; set; } 
    
    }
}