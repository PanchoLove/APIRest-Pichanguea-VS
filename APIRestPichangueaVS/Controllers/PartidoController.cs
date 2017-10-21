using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PichangueaDataAccess;


namespace APIRestPichangueaVS.Controllers
{
    [RoutePrefix("api/Partido")]
    public class PartidoController : ApiController
    {

        public HttpResponseMessage Get()
        {
            try
            {
                //Se obtienen los modelos de la BD
                using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
                {
                    //Se crea una lista con todos los partidos
                    var partidos = entities.Partido.ToList();
                    if (partidos != null)
                    {
                        //Se retorna el estado OK y la lista de partidos
                        return Request.CreateResponse(HttpStatusCode.OK, partidos);
                    }
                    else
                    {
                        //Se retorna el estado NotFound y un string que indica el error
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No existen Partidos");
                    }
                }

            }
            catch (Exception ex)
            {
                //En caso de existir otro error, se envia estado de error y un mensaje
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

        // GET: api/Partido/5
        //Funcion que retorna un partido en base a su id
        public HttpResponseMessage Get(int id)
        {
            try
            {
                //Se obtienen los modelos de la BD
                using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
                {
                    //Se crea una variable con el partido correspondiente a la ID
                    var partido = entities.Partido.FirstOrDefault(p => p.idPartido == id);
                    if (partido != null)
                    {
                        //Se retorna el estado OK y el partido
                        return Request.CreateResponse(HttpStatusCode.OK, partido);
                    }
                    else
                    {
                        //Se retorna el estado NotFound y un string que indica el error
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Partido con ID: " + id.ToString() + " no existe");
                    }
                }
            }
            catch (Exception ex)
            {
                //En caso de existir otro error, se envia estado de error y un mensaje
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        //Funcion que agrega un partido
        public HttpResponseMessage Post([FromBody]Partido partido)
        {
            try
            {
                //Se obtienen los modelos de la BD
                using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
                {
                    //Se agrega el partido a las entidades
                    partido.idPartido = entities.Partido.ToList().Count + 1;
                    entities.Partido.Add(partido);
                    entities.SaveChanges();
                    //Se crea un un mensaje con el codigo Created y con el partido ingresado
                    var message = Request.CreateResponse(HttpStatusCode.Created, partido);
                    //Se concatena la ID al partido del mensaje
                    message.Headers.Location = new Uri(Request.RequestUri + partido.idPartido.ToString());
                    return message;
                }
            }
            catch (Exception ex)
            {
                //En caso de existir otro error, se envia estado de error y un mensaje
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

        //Funcion que modifica un partido
        public HttpResponseMessage Put(int id, [FromBody]Partido partido)
        {
            try
            {
                //Se obtienen los modelos de la BD
                using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
                {
                    //Se crea una variable con el partido correspondiente a la ID
                    var partidoEntity = entities.Partido.FirstOrDefault(p => p.idPartido == id);
                    if (partidoEntity == null)
                    {
                        //Se retorna el estado NotFound y un string que indica el error
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Partido con ID: " + id.ToString() + " no existe, no es posible actualizar");

                    }
                    else
                    {
                        //Se modifican los campos del partido
                        partidoEntity.idTipoPartido = partido.idTipoPartido;
                        partidoEntity.parCancha = partido.parCancha;
                        partidoEntity.parComplejo = partido.parComplejo;
                        partidoEntity.parCreacion = partido.parCreacion;
                        partidoEntity.parEstado = partido.parEstado;
                        partidoEntity.parFecha = partido.parFecha;
                        partidoEntity.parGeoReferencia = partido.parGeoReferencia;
                        partidoEntity.parHora = partido.parHora;
                        partidoEntity.parRival = partido.parRival;
                        partidoEntity.parUbicacion = partido.parUbicacion;
                        
                        //Se guardan los cambios
                        entities.SaveChanges();
                        //Se retorna el estado OK y el partido
                        return Request.CreateResponse(HttpStatusCode.OK, partidoEntity);
                    }
                }
            }
            catch (Exception ex)
            {
                //En caso de existir otro error, se envia estado de error y un mensaje
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        // DELETE: api/Partido/5
        public HttpResponseMessage Delete(int id)
        {

            try
            {
                //Se obtienen los modelos de la BD
                using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
                {
                    //Se crea una variable con el jugador correspondiente a la ID
                    var partidoEntity = entities.Partido.FirstOrDefault(p => p.idPartido == id);
                    if (partidoEntity == null)
                    {
                        //Se retorna el estado NotFound y un string que indica el error
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Partido con ID: " + id.ToString() + " no existe, no es posible actualizar");

                    }
                    else
                    {
                        //Se elimina de la BD el jugador
                        entities.Partido.Remove(partidoEntity);
                        entities.SaveChanges();
                        //Se retorna el estado OK
                        return Request.CreateResponse(HttpStatusCode.OK);
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
