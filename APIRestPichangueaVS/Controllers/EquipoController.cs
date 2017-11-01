using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PichangueaDataAccess;
using APIRestPichangueaVS.AdditionaModels;

namespace APIRestPichangueaVS.Controllers
{
    [RoutePrefix("api/Equipo")]
    public class EquipoController : ApiController
    {

    
        //Funcion que retorna la lista de Equipos
        public HttpResponseMessage Get()
        {
            try
            {
                //Se obtienen los modelos de la BD
                using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
                {
                    //Se crea una lista con todos los Equipos
                    var entity = entities.Equipo.ToList();
                    if (entity != null && entity.Count() > 0)
                    {
                        //Se retorna el estado OK y la lista de Equipos
                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                    else
                    {
                        //Se retorna el estado NotFound y un string que indica el error
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No existen equipos");
                    }
                }

            }
            catch (Exception ex)
            {
                //En caso de existir otro error, se envia estado de error y un mensaje
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }





        //Funcion que retorna un equipo dada su id
        public HttpResponseMessage Get(int id)
        {
            try
            {
                //Se obtienen los modelos de la BD
                using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
                {
                    //Se crea una variable con el equipo correspondiente a la ID
                    var entity = entities.Equipo.FirstOrDefault(e => e.idEquipo == id);
                    if (entity != null)
                    {
                        //Se retorna el estado OK y el jugador
                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                    else
                    {
                        //Se retorna el estado NotFound y un string que indica el error
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Equipo con ID: " + id.ToString() + " no existe");
                    }
                }
            }
            catch (Exception ex)
            {
                //En caso de existir otro error, se envia estado de error y un mensaje
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        //Funcion que retorna una lista de equipos dado un nombre
        public HttpResponseMessage Get(String nombreEquipo)
        {
            try
            {
                //Se obtienen los modelos de la BD
                using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
                {
                    //Se crea una lista con todos los Equipos
                    var entity = entities.Equipo.Where(e => e.equNombre == nombreEquipo).ToList();

                    if (entity != null)
                    {
                        //Se retorna el estado OK y la lista de Equipos
                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                    else
                    {
                        //Se retorna el estado NotFound y un string que indica el error
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No existen equipos");
                    }
                }

            }
            catch (Exception ex)
            {
                //En caso de existir otro error, se envia estado de error y un mensaje
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }



        //Funcion que agrega un equipo
        public HttpResponseMessage Post([FromBody]Equipo equipo)
        {
            try
            {
                //Se obtienen los modelos de la BD
                using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
                {
                    //Se agrega el jugador a las entidades
                    entities.Equipo.Add(equipo);
                    entities.SaveChanges();
                    //Se crea un un mensaje con el codigo Created y con el jugador ingresado
                    var message = Request.CreateResponse(HttpStatusCode.Created, equipo);
                    //Se concatena la ID al jugador del mensaje
                    message.Headers.Location = new Uri(Request.RequestUri + equipo.idEquipo.ToString());
                    return message;
                }
            }
            catch (Exception ex)
            {
                //En caso de existir otro error, se envia estado de error y un mensaje
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        //Funcion que crea una invitacion de un jugador a un equipo
        [Route("{idEquipo:int}/InvitarJugador/{idJugador:int}")]
        public HttpResponseMessage PostInvitacion(int idEquipo, int idJugador)
        {
            try
            {
                //Se obtienen los modelos de la BD
                using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
                {

                    Equipo_Invitacion nuevaInvitacion = new Equipo_Invitacion();
                    nuevaInvitacion.einCreacion = DateTime.Now;
                    nuevaInvitacion.idEquipo = idEquipo;
                    nuevaInvitacion.idJugador = idJugador;

                    entities.Equipo_Invitacion.Add(nuevaInvitacion);
                    entities.SaveChanges();

                    //Se crea un un mensaje con el codigo Created y con la invitacion ingresado
                    var message = Request.CreateResponse(HttpStatusCode.Created, nuevaInvitacion);
                    //Se concatena la ID a la invitacion del mensaje
                    message.Headers.Location = new Uri(Request.RequestUri + nuevaInvitacion.idEquipoInvitacion.ToString());
                    return message;
                }
            }
            catch (Exception ex)
            {
                //En caso de existir otro error, se envia estado de error y un mensaje
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }




        //Funcion que modifica un equipo
        public HttpResponseMessage Put(int id, [FromBody]Equipo equipo)
        {

            try
            {
                //Se obtienen los modelos de la BD
                using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
                {
                    //Se crea una variable con el jugador correspondiente a la ID
                    var entity = entities.Equipo.FirstOrDefault(e => e.idEquipo == id);
                    if (entity == null)
                    {
                        //Se retorna el estado NotFound y un string que indica el error
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Equipo con ID: " + id.ToString() + " no existe, no es posible actualizar");

                    }
                    else
                    {
                        //Se modifican los campos del jugador
                        entity.equContacto = equipo.equContacto;
                        entity.equContactoCelular = equipo.equContactoCelular;
                        entity.equContactoEmail = equipo.equContactoEmail;
                        entity.equContactoFono = equipo.equContactoFono;
                        entity.equDescripcion = equipo.equDescripcion;
                        entity.equEmail = equipo.equEmail;
                        entity.equNombre = equipo.equNombre;
                        entity.equUrl = equipo.equUrl;

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

        //Funcion que elimina un Equipo
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                //Se obtienen los modelos de la BD
                using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
                {
                    //Se crea una variable con el equipo correspondiente a la ID
                    var entity = entities.Equipo.FirstOrDefault(e => e.idEquipo == id);
                    if (entity == null)
                    {
                        //Se retorna el estado NotFound y un string que indica el error
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Equipo con ID: " + id.ToString() + " no existe, no es posible actualizar");

                    }
                    else
                    {
                        //Se elimina de la BD el jugador
                        entities.Equipo.Remove(entity);
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


        [Route("{idEquipo:int}/Chat")]
        public HttpResponseMessage GetChat(int idEquipo)
        {
            try
            {
                //Se obtienen los modelos de la BD
                using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
                {

                    //se obtienen todos los registros de chat para el idPartido
                    var chat = entities.Equipo_Chat.Where(ech => ech.idEquipo == idEquipo)
                                                      .Join(entities.Jugador,
                                                            pch => pch.idJugador,
                                                            j => j.idJugador,
                                                            (mensaje, jugador) => new
                                                            {
                                                                autor = new
                                                                {
                                                                    idJugador = jugador.idJugador,
                                                                    jugUsername = jugador.jugUsername,
                                                                    jugFoto = jugador.jugFoto,
                                                                    jugApodo = jugador.jugApodo
                                                                },

                                                                contenidoMensaje = mensaje.echMensaje,
                                                                creacion = mensaje.echaCreacion
                                                            }
                                                            ).ToList();

                    if (chat != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, chat);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Ha ocurrido un error al intentar obtener los mensajes asociados al partido");
                    }
                }
            }
            catch (Exception ex)
            {
                //En caso de existir otro error, se envia estado de error y un mensaje
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [Route("{idEquipo:int}/Chat")]
        public HttpResponseMessage PostChat([FromBody]MensajeEntrada mensaje, int idEquipo)
        {

            try
            {
                //Se obtienen los modelos de la BD
                using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
                {
                    if (mensaje.idJugador==null || mensaje.idJugador==null) {
                        return Request.CreateErrorResponse(HttpStatusCode.PartialContent,"Faltan campos por rellenar");
                    }

                    var comprobacion = entities.Equipo_Jugador.FirstOrDefault(ej => ej.idJugador == mensaje.idJugador &&
                                                                                    ej.idEquipo == idEquipo);


                    if (comprobacion == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "El jugador no pertenece al equipo especificado");
                    }


                    Equipo_Chat ech = new Equipo_Chat();
                    ech.idJugador = mensaje.idJugador;
                    ech.idEquipo= idEquipo;
                    ech.echaCreacion = DateTime.Now;
                    ech.echMensaje = mensaje.contenidoMensaje;


                    entities.Equipo_Chat.Add(ech);
                    entities.SaveChanges();

                    return Request.CreateResponse(HttpStatusCode.OK, "Mensaje creado");
                }
            }
            catch (Exception ex)
            {
                //En caso de existir otro error, se envia estado de error y un mensaje
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

        [Route("{idEquipo:int}/SolicitarIngreso")]
        [Route("{idEquipo:int}/SolicitarIngreso/{idJugador:int}")]
        public HttpResponseMessage PostSolicitarIngreso(int idEquipo,int idJugador)
        {

            try
            {
                //Se obtienen los modelos de la BD
                using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
                {
                    //verificar si existe una solicitud de ingreso
                    var comprobacion0 = entities.Equipo_Solicita_Ingreso.FirstOrDefault(esi => esi.idJugador == idJugador &&
                                                                                               esi.idEquipo==idEquipo &&
                                                                                               esi.esiEstado == 0
                                                                                               );

                    if (comprobacion0 != null) { return Request.CreateErrorResponse(HttpStatusCode.NotModified, "Ya existe una solicitud pendiente"); }

                    //verificar si existe el equipo
                    var comprobacion1 = entities.Equipo.FirstOrDefault(e => e.idEquipo==e.idEquipo);
                    if (comprobacion1==null) { return Request.CreateErrorResponse(HttpStatusCode.NotFound,"El equipo de id: "+ idEquipo.ToString()+ "no existe"); }

                    //verificar si eljugador existe
                    var comprobacion2 = entities.Jugador.FirstOrDefault(j => j.idJugador == j.idJugador);
                    if (comprobacion2 == null) { return Request.CreateErrorResponse(HttpStatusCode.NotFound, "El jugador de id: " + idEquipo.ToString() + "no existe"); }

                    var solicitud = new Equipo_Solicita_Ingreso();
                    solicitud.esiCreacion = DateTime.Now;
                    solicitud.esiEstado = 0;
                    solicitud.idEquipo = idEquipo;
                    solicitud.idJugador = idJugador;

                    entities.Equipo_Solicita_Ingreso.Add(solicitud);
                    entities.SaveChanges();

                    return Request.CreateResponse(HttpStatusCode.Created, "Solicitud enviado al equipo: " + solicitud.idEquipo.ToString());
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
