using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PichangueaDataAccess;


namespace APIRestPichangueaVS.Controllers
{
    [RoutePrefix("api/Cancha")]
    public class CanchaController : ApiController
    {
        public HttpResponseMessage Get()
        {
            try
            {
                //Se obtienen los modelos de la BD
                using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
                {
                    //Se crea una lista con todos los canchas
                    var canchas = entities.Cancha.ToList();
                    if (canchas != null)
                    {
                        //Se retorna el estado OK y la lista de canchas
                        return Request.CreateResponse(HttpStatusCode.OK, canchas);
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
        //Funcion que retorna un cancha en base a su id
        public HttpResponseMessage Get(int id)
        {
            try
            {
                //Se obtienen los modelos de la BD
                using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
                {
                    //Se crea una variable con el cancha correspondiente a la ID
                    var partido = entities.Partido.FirstOrDefault(p => p.idPartido == id);
                    if (partido != null)
                    {
                        //Se retorna el estado OK y el cancha
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

        //Funcion que agrega un cancha
        public HttpResponseMessage Post([FromBody]Cancha cancha)
        {
            try
            {
                //Se obtienen los modelos de la BD
                using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
                {

                    //Se agrega el cancha a las entidades
                    entities.Cancha.Add(cancha);
                    entities.SaveChanges();
                    //Se crea un un mensaje con el codigo Created y con el cancha ingresado
                    var message = Request.CreateResponse(HttpStatusCode.Created, cancha);
                    //Se concatena la ID al cancha del mensaje
                    message.Headers.Location = new Uri(Request.RequestUri + cancha.idCancha.ToString());
                    return message;
                }
            }
            catch (Exception ex)
            {
                //En caso de existir otro error, se envia estado de error y un mensaje
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

        //Funcion que modifica un cancha
        public HttpResponseMessage Put(int idCancha, [FromBody]Cancha cancha)
        {
            try
            {
                //Se obtienen los modelos de la BD
                using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
                {
                    //Se crea una variable con el cancha correspondiente a la ID
                    var canchaEntity = entities.Cancha.FirstOrDefault(c => c.idCancha == idCancha);
                    if (canchaEntity == null)
                    {
                        //Se retorna el estado NotFound y un string que indica el error
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Cancha con ID: " + idCancha.ToString() + " no existe, no es posible actualizar");

                    }
                    else
                    {
                        //Se modifican los campos del cancha
                        canchaEntity.canCreacion = cancha.canCreacion;
                        canchaEntity.canNombre = cancha.canNombre;
                        canchaEntity.idComplejoDeportivo = cancha.idComplejoDeportivo;
                        canchaEntity.idTipoCancha = cancha.idTipoCancha;

                        //Se guardan los cambios
                        entities.SaveChanges();
                        //Se retorna el estado OK y el cancha
                        return Request.CreateResponse(HttpStatusCode.OK, canchaEntity);
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
                    //Se crea una variable con la cancha correspondiente a la ID
                    var cancha = entities.Cancha.FirstOrDefault(c => c.idCancha == id);
                    if (cancha == null)
                    {
                        //Se retorna el estado NotFound y un string que indica el error
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Cancha con ID: " + id.ToString() + " no existe, no es posible actualizar");

                    }
                    else
                    {
                        //Se elimina de la BD el jugador
                        entities.Cancha.Remove(cancha);
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
