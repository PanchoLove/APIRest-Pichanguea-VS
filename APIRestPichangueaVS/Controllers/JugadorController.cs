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
        //Funcion que retorna la lista de jugadores
        public HttpResponseMessage Get()
        {
            try
            {
                //Se obtienen los modelos de la BD
                using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
                {
                    //Se crea una lista con todos los jugadores
                    var entity = entities.Jugador.ToList();
                    if (entity != null)
                    {
                        //Se retorna el estado OK y la lista de jugadores
                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                    else
                    {
                        //Se retorna el estado NotFound y un string que indica el error
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No existen jugadores");
                    }
                }

            }
            catch (Exception ex)
            {
                //En caso de existir otro error, se envia estado de error y un mensaje
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

        //Funcion que retorna un jugador
        public HttpResponseMessage Get(int id)
        {
            try
            {
                //Se obtienen los modelos de la BD
                using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
                {
                    //Se crea una variable con el jugador correspondiente a la ID
                    var entity = entities.Jugador.FirstOrDefault(e => e.idJugador == id);
                    if (entity != null)
                    {
                        //Se retorna el estado OK y el jugador
                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                    else
                    {
                        //Se retorna el estado NotFound y un string que indica el error
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Jugador con ID: " + id.ToString() + " no existe");
                    }
                }
            }
            catch (Exception ex)
            {
                //En caso de existir otro error, se envia estado de error y un mensaje
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }


        //Funcion que retorna un jugador en base a su nombre como entrada
        public HttpResponseMessage Get(String nombre)
        {
            
            try
            {

                //Se obtienen los modelos de la BD
                using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
                {
                    //Se crea una variable con el jugador correspondiente a la ID
                    var entity = entities.Jugador.FirstOrDefault(e => e.jugNombre == nombre);
                    if (entity != null)
                    {
                        //Se retorna el estado OK y el jugador
                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                    else
                    {
                        //Se retorna el estado NotFound y un string que indica el error
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Jugador con nombre: " + nombre + " no existe");
                    }
                }
            }
            catch (Exception ex)
            {
                //En caso de existir otro error, se envia estado de error y un mensaje
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [Route("Equipos")]
        public HttpResponseMessage GetEquipos(String idJugador)
        {
            return Request.CreateResponse(HttpStatusCode.OK, "ooooooooooos") ;
            /*
            try
            {
                
                //Se obtienen los modelos de la BD
                using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
                {
                    //Se crea una variable con el jugador correspondiente a la ID
                    var entity = entities.Jugador.FirstOrDefault(e => e.jugEmail = idJugador);
                    if (entity != null)
                    {
                        //Se retorna el estado OK y el jugador
                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                    else
                    {
                        //Se retorna el estado NotFound y un string que indica el error
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Jugador con ID: " + nombre + " no existe");
                    }
                }
            }
            catch (Exception ex)
            {
                //En caso de existir otro error, se envia estado de error y un mensaje
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }*/
        }

        //Funcion que agrega un jugador
        public HttpResponseMessage Post([FromBody]Jugador jugador)
        {
            try
            {
                //Se obtienen los modelos de la BD
                using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
                {
                    //Se agrega el jugador a las entidades
                    entities.Jugador.Add(jugador);
                    entities.SaveChanges();
                    //Se crea un un mensaje con el codigo Created y con el jugador ingresado
                    var message = Request.CreateResponse(HttpStatusCode.Created, jugador);
                    //Se concatena la ID al jugador del mensaje
                    message.Headers.Location = new Uri(Request.RequestUri + jugador.idJugador.ToString());
                    return message;
                }
            }
            catch(Exception ex)
            {
                //En caso de existir otro error, se envia estado de error y un mensaje
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

        //Funcion que modifica un jugador
        public HttpResponseMessage Put(int id, [FromBody]Jugador jugador)
        {
            try
            {
                //Se obtienen los modelos de la BD
                using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
                {
                    //Se crea una variable con el jugador correspondiente a la ID
                    var entity = entities.Jugador.FirstOrDefault(e => e.idJugador == id);
                    if (entity == null)
                    {
                        //Se retorna el estado NotFound y un string que indica el error
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Jugador con ID: " + id.ToString() + " no existe, no es posible actualizar");
                         
                    }
                    else
                    {
                        //Se modifican los campos del jugador
                        entity.jugApodo = jugador.jugApodo;
                        entity.jugCelular = jugador.jugCelular;
                        entity.jugCreacion = jugador.jugCreacion;
                        entity.jugEmail = jugador.jugEmail;
                        entity.jugFono = jugador.jugFono;
                        entity.jugFoto = jugador.jugFoto;
                        entity.jugMaterno = jugador.jugMaterno;
                        entity.jugNombre = jugador.jugNombre;
                        entity.jugPassword = jugador.jugPassword;
                        entity.jugPaterno = jugador.jugPaterno;
                        entity.jugRut = jugador.jugRut;
                        entity.jugRutDv = jugador.jugRutDv;
                        entity.jugUsername = jugador.jugUsername;

                        //Se guardan los cambios
                        entities.SaveChanges();
                        //Se retorna el estado OK y el jugador
                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                }
            }
            catch (Exception ex)
            {
                //En caso de existir otro error, se envia estado de error y un mensaje
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        //Funcion que elimina un jugador
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                //Se obtienen los modelos de la BD
                using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
                {
                    //Se crea una variable con el jugador correspondiente a la ID
                    var entity = entities.Jugador.FirstOrDefault(e => e.idJugador == id);
                    if (entity == null)
                    {
                        //Se retorna el estado NotFound y un string que indica el error
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Jugador con ID: " + id.ToString() + " no existe, no es posible actualizar");

                    }
                    else
                    {
                        //Se elimina de la BD el jugador
                        entities.Jugador.Remove(entity);
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
