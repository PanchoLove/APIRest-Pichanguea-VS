using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PichangueaDataAccess;

namespace APIRestPichangueaVS.Controllers
{
    public class JugadorController : ApiController
    {
        public IEnumerable<Jugador> Get()
        {
            using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
            {
                return entities.Jugador.ToList();
            }
        }

        public HttpResponseMessage Get(int id)
        {
            using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
            {
                var entity = entities.Jugador.FirstOrDefault(e => e.idJugador == id);
                if(entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Jugador con ID: " + id.ToString() + " no existe");
                }
            }
        }

        public HttpResponseMessage Post([FromBody]Jugador jugador)
        {
            try
            {
                using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
                {
                    entities.Jugador.Add(jugador);
                    entities.SaveChanges();
                    var message = Request.CreateResponse(HttpStatusCode.Created, jugador);
                    message.Headers.Location = new Uri(Request.RequestUri + jugador.idJugador.ToString());
                    return message;
                }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }
    }
}
