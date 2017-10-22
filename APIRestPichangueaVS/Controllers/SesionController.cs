using APIRestPichangueaVS.AdditionaModels;
using PichangueaDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace APIRestPichangueaVS.Controllers
{
    [RoutePrefix("api/Sesion")]
    public class SesionController : ApiController
    {

        public HttpResponseMessage Get(String usuario, String pass) {

            try
            {
                //Se obtienen los modelos de la BD
                using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
                {
                    //Se crea una lista con todos los jugadores
                    var jugador = entities.Jugador.FirstOrDefault(j => (j.jugUsername == usuario && j.jugPassword == pass) ||
                                                                       (j.jugEmail == usuario && j.jugPassword == pass)  
                                                                       );
                    if (jugador != null)
                    {
                        //Se retorna el estado OK y el usuario
                        //aca va la logica de obtencion del token y toda la cosa
                        String token = "tokendepruebaxd";

                        return Request.CreateResponse(HttpStatusCode.OK, new Sesion(jugador,token));
                    }
                    else
                    {
                        //Se retorna el estado NotFound y un string que indica el error
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No se encuentro un jugador que cumpla con el usuario y contraseña especificados");
                    }
                }

            }
            catch (Exception ex)
            {
                //En caso de existir otro error, se envia estado de error y un mensaje
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }


        }

    }
}
