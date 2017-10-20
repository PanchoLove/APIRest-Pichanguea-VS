using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PichangueaDataAccess;

namespace APIRestPichangueaVS.Controllers
{
    [RoutePrefix("api/Jugador")]
    public class JugadorController : ApiController
    {
        public String rutaInicial="api";

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


        //Funcion que retorna una lista de jugador en base a su nombre como entrada
        public HttpResponseMessage Get(String nombre)
        {
            
            try
            {

                //Se obtienen los modelos de la BD
                using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
                {
                    //Se crea una variable con los jugadores correspondiente al nombre
                    var entity = entities.Jugador.Where(e => e.jugNombre == nombre).ToList();
                    if (entity != null)
                    {
                        //Se retorna el estado OK y los jugadores
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
            }
        }

        //Se retorna una lista con los equipos afiliados a un jugador
        [Route("{idJugador:int}/Equipos")]
        public HttpResponseMessage GetEquipos(int idJugador)
        {
            try
            {
                
                //Se obtienen los modelos de la BD
                using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
                {
                    //Se crea la lista de equipos pertenecientes al jugador
                    var jugador = entities.Jugador.FirstOrDefault(j => j.idJugador == idJugador);
                    var intermedios = entities.Equipo_Jugador.Where(ej => ej.idJugador == jugador.idJugador).ToList();
                    List<Equipo> equiposFiltrados = new List<Equipo>();                  

                    foreach (Equipo_Jugador ej in intermedios ) {
                        equiposFiltrados.Add(entities.Equipo.FirstOrDefault(e=> e.idEquipo==ej.idEquipo));
                    }


                    if (equiposFiltrados != null)
                    {
                        if (equiposFiltrados.Count >= 0)
                        {
                            //Se retorna el estado OK y el jugador
                            return Request.CreateResponse(HttpStatusCode.OK, equiposFiltrados);
                        }
                        else
                        {
                            //Se retorna el estado NotFound y un string que indica el error
                            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Jugador con ID: " + idJugador + " no existe");
                        }
                    }
                    else
                    {
                        //Se retorna el estado NotFound y un string que indica el error
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Jugador con ID: " + idJugador + " no existe");
                    }
                }
            }
            catch (Exception ex)
            {
                //En caso de existir otro error, se envia estado de error y un mensaje
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }


        [Route("{idJugador:int}/Invitaciones")]
        public HttpResponseMessage GetInvitaciones(int idJugador)
        {


            try
            {

                //Se obtienen los modelos de la BD
                using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
                {

                    var invitaciones = entities.Equipo_Invitacion.ToList();


                    if (invitaciones != null )
                    {
                        //Se retorna el estado OK y el jugador
                        return Request.CreateResponse(HttpStatusCode.OK, invitaciones);
                    }
                    else
                    {
                        //Se retorna el estado NotFound y un string que indica el error
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Jugador con ID: " + idJugador + " no existe");
                    }
                }
            }
            catch (Exception ex)
            {
                //En caso de existir otro error, se envia estado de error y un mensaje
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [Route("{idJugador:int}/Invitaciones/{idInvitacion:int}")]
        public HttpResponseMessage GetInvitaciones(int idJugador, int idInvitacion)
        {
            try
            {
                //Se obtienen los modelos de la BD
                using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
                {
                    var invitacion = entities.Equipo_Invitacion.FirstOrDefault(i=> i.idEquipoInvitacion==idInvitacion); ;


                    if (invitacion != null)
                    {
                        //Se retorna el estado OK y el jugador
                        return Request.CreateResponse(HttpStatusCode.OK, invitacion);
                    }
                    else
                    {
                        //Se retorna el estado NotFound y un string que indica el error
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Invitacion con ID: " + idInvitacion + " no existe");
                    }
                }
            }
            catch (Exception ex)
            {
                //En caso de existir otro error, se envia estado de error y un mensaje
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
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
                        //Se retorna el estaado OK
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
