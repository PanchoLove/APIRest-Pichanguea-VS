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


                    //preparando lista de retornos
                    List<PartidoCompuesto> partidosRespuesta = new List<PartidoCompuesto>();

                    if (partidos != null && partidos.Count() > 0)
                    {

                        foreach (Partido partido in partidos) {

                            PartidoCompuesto pc = new PartidoCompuesto();

                            pc.idPartido = partido.idPartido;
                            pc.equipo = entities.Equipo.FirstOrDefault(e => e.idEquipo == partido.idEquipo);
                            pc.Tipo_Partido = entities.Tipo_Partido.FirstOrDefault(tp => tp.idTipoPartido == partido.idTipoPartido);
                            pc.parCancha = partido.parCancha;
                            pc.parComplejo = partido.parComplejo;
                            pc.parCreacion = partido.parCreacion;
                            pc.parEstado = partido.parEstado;
                            pc.parFecha = partido.parFecha;
                            pc.parGeoReferencia = partido.parGeoReferencia;
                            pc.parHora = partido.parHora;
                            pc.parRival = partido.parRival;
                            pc.parUbicacion = partido.parUbicacion;

                            partidosRespuesta.Add(pc);
                        }

                        //Se retorna el estado OK y la lista de partidos
                        return Request.CreateResponse(HttpStatusCode.OK, partidosRespuesta);
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

/* opcion sin procesamiento
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
*/


        
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

                        PartidoCompuesto pc = new PartidoCompuesto();

                        pc.idPartido = partido.idPartido;
                        pc.equipo = entities.Equipo.FirstOrDefault(e => e.idEquipo == partido.idEquipo);
                        pc.Tipo_Partido = entities.Tipo_Partido.FirstOrDefault(tp => tp.idTipoPartido == partido.idTipoPartido);
                        pc.parCancha = partido.parCancha;
                        pc.parComplejo = partido.parComplejo;
                        pc.parCreacion = partido.parCreacion;
                        pc.parEstado = partido.parEstado;
                        pc.parFecha = partido.parFecha;
                        pc.parGeoReferencia = partido.parGeoReferencia;
                        pc.parHora = partido.parHora;
                        pc.parRival = partido.parRival;
                        pc.parUbicacion = partido.parUbicacion;

                        return Request.CreateResponse(HttpStatusCode.OK, pc);
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

/* version ligera

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
  */


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

        [Route("{idPartido:int}/Chat")]
        public HttpResponseMessage GetChat(int idPartido)
        {

            try
            {
                //Se obtienen los modelos de la BD
                using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
                {
                    //se obtienen todos los registros de chat para el idPartido
                    var chat = entities.Partido_Chat.Where(pch => pch.idPartido == idPartido).ToList();

                    if (chat != null)
                    {

                            //se entregan los mensajes del chat en un formato mas manejable por la parte del front-end
                            List<Mensaje> mensajes = new List<Mensaje>();

                            foreach (Partido_Chat c  in chat) {

                                Jugador jugador = entities.Jugador.FirstOrDefault(j => j.idJugador == c.idJugador);
                                JugadorSimple autor = new JugadorSimple( jugador.idJugador,
                                                                         jugador.jugUsername,
                                                                         jugador.jugFoto,
                                                                          jugador.jugApodo);

                                Mensaje mensaje = new Mensaje(autor, c.pchMensaje, c.pchCreacion);
                                mensajes.Add(mensaje);
                            }

                            return Request.CreateResponse(HttpStatusCode.OK, mensajes);
                    }
                    else {

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

        [Route("{idPartido:int}/Chat")]
        public HttpResponseMessage PostChat([FromBody]MensajeEntrada mensaje , int idPartido)
        {

            try
            {



                //Se obtienen los modelos de la BD
                using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
                {

                    //comprobar si el partido esta vinculado al jugador
                    var comprobacion = entities.Partido_Jugador.FirstOrDefault(pj => pj.idJugador == mensaje.idJugador &&
                                                                               pj.idPartido == idPartido);


                    if (comprobacion == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "El jugador no se encuentra en el partido especificado");
                    }


                    Partido_Chat pch = new Partido_Chat();
                    pch.idJugador = mensaje.idJugador;
                    pch.idPartido = idPartido;
                    pch.pchCreacion = DateTime.Now;
                    pch.pchMensaje = mensaje.contenido;
                    

                    entities.Partido_Chat.Add(pch);
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


    }
}
