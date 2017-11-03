using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PichangueaDataAccess;
using APIRestPichangueaVS.AdditionaModels;
using System.Data.Entity.Core.Objects;

namespace APIRestPichangueaVS.Controllers
{
    [RoutePrefix("api/Jugador")]
    public class JugadorController : ApiController
    {
        public decimal estadoConfirmado = 1;
        public decimal estadoCancelado = 0;

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
                    if (entity != null && entity.Count() > 0)
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


        //Funcion que retorna una lista de jugadores en base a su nombre como entrada
        public HttpResponseMessage Get(String nombre)
        {
            
            try
            {

                //Se obtienen los modelos de la BD
                using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
                {
                    //Se crea una variable con el jugador correspondiente a su nombre
                    var entity = entities.Jugador.Where(e => e.jugNombre == nombre).ToList();
                    if (entity != null && entity.Count() > 0)
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

        [Route("{idJugador:int}/Equipos")]
        public HttpResponseMessage GetEquipos(int idJugador)
        {

            try
            {

                //Se obtienen los modelos de la BD
                using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
                {
                    var consulta = entities.Equipo_Jugador.Where(ej => ej.idEquipoJugador == idJugador)
                                                          .Join(entities.Equipo,
                                                                ej => ej.idEquipo,
                                                                e => e.idEquipo,
                                                                (equipoJugador, equipo) => equipo)
                                                                .ToList();


                    if (consulta != null )
                    {
                        if (consulta.Count > 0)
                        {
                            //Se retorna el estado OK y el jugador
                            return Request.CreateResponse(HttpStatusCode.OK, consulta);
                        }
                        else {
                            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "El jugador no posee equipos asociados");
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

        [Route("{idJugador:int}/Invitaciones")]
        public HttpResponseMessage GetInvitaciones(int idJugador)
        {


            try
            {

                //Se obtienen los modelos de la BD
                using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
                {

                    var invitaciones = entities.Equipo_Invitacion.Where(ei => ei.idJugador == idJugador).ToList();

                    if (invitaciones != null && invitaciones.Count()>0)
                    {
                        //Se retorna el estado OK y el jugador
                        return Request.CreateResponse(HttpStatusCode.OK, invitaciones);
                    }
                    else
                    {
                        //Se retorna el estado NotFound y un string que indica el error
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Jugador con ID: " + idJugador + " no tiene invitaciones");
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
                    var invitacion = entities.Equipo_Invitacion.FirstOrDefault(i => i.idEquipoInvitacion == idInvitacion); ;


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


        [Route("{idJugador:int}/Partidos")]
        public HttpResponseMessage GetPartidos(int idJugador)
        {

            try
            {

                //Se obtienen los modelos de la BD
                using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
                {

                    //metodo 2 obtener el cruce de las tablas jugador, partido y partidoJugador
                    /*   var consulta = (from pj in entities.Partido_Jugador
                                       join p in entities.Partido on pj.idJugador == p. )*/

                    var consulta = entities.Partido_Jugador
                                                    .Where(pj => pj.idJugador == idJugador)
                                                    .Join(entities.Partido,
                                                          pj => pj.idPartido,
                                                          p => p.idPartido,
                                                          (partido_JugadorT, partidoT)
                                                          => new
                                                          {
                                                              partido = new {
                                                                  idPartido = partidoT.idPartido,
                                                                  equipo = entities.Equipo.FirstOrDefault(e => e.idEquipo == partidoT.idEquipo),
                                                                  tipoPartido = entities.Tipo_Partido.FirstOrDefault(tp => tp.idTipoPartido == partidoT.idTipoPartido),
                                                                  parCancha = partidoT.parCancha,
                                                                  parComplejo = partidoT.parComplejo,
                                                                  parCreacion = partidoT.parCreacion,
                                                                  parEstado = partidoT.parEstado,
                                                                  parFecha = partidoT.parFecha,
                                                                  parGeoReferencia = partidoT.parGeoReferencia,
                                                                  parHora = partidoT.parHora,
                                                                  parRival = partidoT.parRival,
                                                                  parUbicacion = partidoT.parUbicacion
                                                              }
                                                              ,
                                                              asistencia = partido_JugadorT.pjuEstado,
                                                              galletas= partido_JugadorT.pjuGalleta
                                                          }
                                                          ).ToList();
                    if(consulta != null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, consulta);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Ocurrio un error al buscar los partidos o el jugador no tiene partidos asociados");
                    }
                }
            }
            catch (Exception ex)
            {
                //En caso de existir otro error, se envia estado de error y un mensaje
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [Route("{idJugador:int}/Partidos/{idPartido:int}")]
        public HttpResponseMessage GetPartidos(int idJugador, int idPartido)
        {

            try
            {
                //Se obtienen los modelos de la BD
                using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
                {
                    //obtener la lista de partidos pertenecientes al jugador

                    //obtener de la tabla intermedia entre jugador y partido la fila donde se encuentra la id del jugador y la id del partido al mismo tiempos
                    var intermedio = entities.Partido_Jugador.Where(pj => pj.idJugador == idJugador && pj.idPartido == idPartido);

                    if (intermedio == null)
                    {
                            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Jugador con ID: " + idJugador + " no existe o no posee partidos");          
                    }
                    else
                    {

                        //si efectivamente el partido esta vinculado al jugador retornarlo
                        var partido = entities.Partido.FirstOrDefault(p=> p.idPartido == idPartido);
                        var pj = intermedio.ToList();

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

                        //Se retorna el estado NotFound y un string que indica el error
                        return Request.CreateResponse(HttpStatusCode.OK, new PartidoInformacionExtra(pc, pj[0].pjuEstado, pj[0].pjuGalleta));
                    }

                }
            }
            catch (Exception ex)
            {
                //En caso de existir otro error, se envia estado de error y un mensaje
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [Route("{idJugador:int}/Partidos/{idPartido:int}/Jugadores/Confirmados")]
        public HttpResponseMessage GetJugadoresPartido(int idJugador, int idPartido)
        {

            try
            {
                //Se obtienen los modelos de la BD
                using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
                {
                    //obtener la lista de partidos pertenecientes al jugador

                    //obtener de la tabla intermedia entre jugador y partido la fila donde se encuentra la id del jugador y la id del partido al mismo tiempos
                    var intermedio = entities.Partido_Jugador.Where(pj => pj.idJugador == idJugador && pj.idPartido == idPartido);

                    if (intermedio == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Jugador con ID: " + idJugador + " no existe o no posee partidos");
                    }
                    else
                    {
                        /* var jugadoresConfirmados = entities.Jugador.Join(entities.Partido_Jugador.Where(pj => pj.idPartido == idPartido),
                                                                          j => idJugador,
                                                                          pj => idJugador,
                                                                          (jug, parJug) => jug
                                                                           );*/

                        var confirmados = entities.Partido_Jugador.Where(pj => pj.idPartido == idPartido)
                                                                  .Join(entities.Jugador,
                                                                       pj => pj.idJugador,
                                                                        j => j.idJugador,
                                                                        (parJug, jug) => jug
                                                                        );

                      /*  var equipo = entities.Equipo.FirstOrDefault(e=> e.idEquipo == 
                                                entities.Partido.FirstOrDefault(p=> p.idPartido == idPartido).idEquipo);

                        var otrosJugadores = entities.Equipo_Jugador.Where(e => equipo.idEquipo == e.idEquipo)
                                                                            .Join(entities.Jugador,
                                                                                  ej => ej.idJugador,
                                                                                  j => j.idJugador,
                                                                                  (ejug,jug)=>jug);


                        var jugadores = new {
                                                jugadoresConfirmados = jugadoresConfirmados,
                            //otrosJugadores = otrosJugadores
                                            };*/


                        return Request.CreateResponse(HttpStatusCode.OK, confirmados.ToList());
                    }

                }
            }
            catch (Exception ex)
            {
                //En caso de existir otro error, se envia estado de error y un mensaje
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }



        [Route("{idJugador:int}/Partidos/{idPartido:int}/Asistencia/{estado:int}")]
        public HttpResponseMessage PutAsistencia(int idJugador, int idPartido, int estado)
        {
            try
            {
                //Se obtienen los modelos de la BD
                using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
                {
                    //obtener los asistentes a un partido
                    var asistencias = entities.Partido_Jugador.Where(pj => pj.idPartido == idPartido);

                    //verificar si el jugador esta vinculado al partido
                    var vinculo = asistencias.FirstOrDefault(pj=>pj.idJugador==idJugador);
                    if (vinculo == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.OK, "El jugador no se encuentra vinculado al partido");
                    }

                    //caso 1: confirmar asistencia
                    if (estado == estadoConfirmado)
                    {
                        //obtener la cantidad de asistentes y cupos disponibles
                        var asistentes = asistencias.Where(pj => pj.pjuEstado == 1).ToList();
                        Nullable<decimal> cuposOcupados = asistentes.Count();

                        foreach (Partido_Jugador pj in asistentes)
                        {
                            if (pj.pjuGalleta > 0)
                            {
                                cuposOcupados = cuposOcupados + pj.pjuGalleta;
                            }
                        }

                        var tipoPartido = entities.Partido.Where(p => p.idPartido == vinculo.idPartido)
                                                           .Join(entities.Tipo_Partido,
                                                                p => p.idTipoPartido,
                                                                tp => tp.idTipoPartido,
                                                                (par, tipoPar) => tipoPar).ToList()[0];

                        if (tipoPartido == null)
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No se encuentra toda la información necesaria del eequipo en la base de datos (tipo_partido)");
                        }
                        Nullable<decimal> cuposMaximos = tipoPartido.tpaMaximoJugadores;
                        Nullable<decimal> cuposDisponibles = cuposMaximos - cuposOcupados;

                        if (cuposDisponibles > 0)
                        {
                            vinculo.pjuEstado = estado;
                            entities.SaveChanges();

                            return Request.CreateResponse(HttpStatusCode.OK, "Asistencia confirmada");
                        }
                        else
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, "No hay suficientes cupos disponibles");
                        }
                    }
                    //caso 2: asistencia cancelada
                    if(estado==estadoCancelado) {
                        if (vinculo.pjuEstado==estado) {
                            return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, "La asistencia del jugador ya se encontraba cancelada o nunca fue creada");
                        }
                        else{
                            return Request.CreateResponse(HttpStatusCode.OK, "Asistencia cancelada");
                        }
                    }
                    //caso 3: se ingresa in valor de estado de asistencia incorrecto (ni 0 ni 1)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "El estado ingresado no es valido");
                }
            }
            catch (Exception ex)
            {
                //En caso de existir otro error, se envia estado de error y un mensaje
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }


        [Route("{idJugador:int}/Partidos/{idPartido:int}/Asistencia/Galletas/")]
        [Route("{idJugador:int}/Partidos/{idPartido:int}/Asistencia/Galletas/{cantidad:int}")]
        public HttpResponseMessage PutGalletas(int idJugador, int idPartido, int cantidad)
        {

            try
            {
                //Se obtienen los modelos de la BD
                using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
                {
                    //obtener los asistentes a un partido
                    var asistencias = entities.Partido_Jugador.Where(pj => pj.idPartido == idPartido);

                    //verificar si el jugador esta vinculado al partido
                    var vinculo = asistencias.FirstOrDefault(pj => pj.idJugador == idJugador && pj.pjuEstado == estadoConfirmado);
                    if (vinculo == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.OK, "El jugador no ha confirma asistencia o no se encuentra vinculado a este");
                    }

                        //obtener la cantidad de asistentes y cupos disponibles
                        var asistentes = asistencias.Where(pj => pj.pjuEstado == 1).ToList();
                        Nullable<decimal> cuposOcupados = asistentes.Count();

                        foreach (Partido_Jugador pj in asistentes)
                        {
                            if (pj.pjuGalleta > 0)
                            {
                                cuposOcupados = cuposOcupados + pj.pjuGalleta;
                            }
                        }

                        var tipoPartido = entities.Partido.Where(p => p.idPartido == vinculo.idPartido)
                                                           .Join(entities.Tipo_Partido,
                                                                p => p.idTipoPartido,
                                                                tp => tp.idTipoPartido,
                                                                (par, tipoPar) => tipoPar).ToList()[0];

                        if (tipoPartido == null)
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No se encuentra toda la información necesaria del eequipo en la base de datos (tipo_partido)");
                        }
                        Nullable<decimal> cuposMaximos = tipoPartido.tpaMaximoJugadores;
                        Nullable<decimal> cuposDisponibles = cuposMaximos - cuposOcupados;

                        if (cuposDisponibles > cantidad)
                        {
                            vinculo.pjuGalleta = cantidad;
                            entities.SaveChanges();

                            return Request.CreateResponse(HttpStatusCode.OK, cantidad.ToString() + " galletas");
                        }
                        else
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, "No hay suficientes cupos disponibles");
                        }
                    }

                
            }
            
            catch (Exception ex)
            {
                //En caso de existir otro error, se envia estado de error y un mensaje
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [Route("{idJugador:int}/Partidos/{idPartido:int}/Asistencia/Galletas/")]
        [Route("{idJugador:int}/Partidos/{idPartido:int}/Asistencia/Galletas/{cantidad:int}")]
        public HttpResponseMessage PostGalletas(int idJugador, int idPartido, int cantidad)
        {

            try
            {
                //Se obtienen los modelos de la BD
                using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
                {

                    //obtener los asistentes a un partido
                    var asistencias = entities.Partido_Jugador.Where(pj => pj.idPartido == idPartido);

                    //verificar si el jugador esta vinculado al partido
                    var vinculo = asistencias.FirstOrDefault(pj => pj.idJugador == idJugador && pj.pjuEstado == estadoConfirmado);
                    if (vinculo == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.OK, "El jugador no ha confirma asistencia o no se encuentra vinculado a este");
                    }

                    //obtener la cantidad de asistentes y cupos disponibles
                    var asistentes = asistencias.Where(pj => pj.pjuEstado == 1).ToList();
                    Nullable<decimal> cuposOcupados = asistentes.Count();

                    foreach (Partido_Jugador pj in asistentes)
                    {
                        if (pj.pjuGalleta > 0)
                        {
                            cuposOcupados = cuposOcupados + pj.pjuGalleta;
                        }
                    }

                    var tipoPartido = entities.Partido.Where(p => p.idPartido == vinculo.idPartido)
                                                       .Join(entities.Tipo_Partido,
                                                            p => p.idTipoPartido,
                                                            tp => tp.idTipoPartido,
                                                            (par, tipoPar) => tipoPar).ToList()[0];

                    if (tipoPartido == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No se encuentra toda la información necesaria del eequipo en la base de datos (tipo_partido)");
                    }
                    Nullable<decimal> cuposMaximos = tipoPartido.tpaMaximoJugadores;
                    Nullable<decimal> cuposDisponibles = cuposMaximos - cuposOcupados;

                    if (cuposDisponibles > cantidad)
                    {
                        vinculo.pjuGalleta = vinculo.pjuGalleta + cantidad;
                        entities.SaveChanges();

                        return Request.CreateResponse(HttpStatusCode.OK, cantidad.ToString() + " galletas");
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, "No hay suficientes cupos disponibles");
                    }

                }
            }

            catch (Exception ex)
            {
                //En caso de existir otro error, se envia estado de error y un mensaje
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }


        //Funcion que confirma asistencia y galletas al mismo tiempo
        [Route("{idJugador:int}/Partidos/{idPartido:int}/Asistencia/")]
        public HttpResponseMessage PutAsistencia(int idJugador, int idPartido, int estado, int galletas)
        {

            try
            {
                //Se obtienen los modelos de la BD
                using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
                {
                    //obtener los asistentes a un partido
                    var asistencias = entities.Partido_Jugador.Where(pj => pj.idPartido == idPartido);

                    //verificar si el jugador esta vinculado al partido
                    var vinculo = asistencias.FirstOrDefault(pj => pj.idJugador == idJugador);
                    if (vinculo == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.OK, "El jugador no se encuentra vinculado al partido");
                    }

                    //caso 1: confirmar asistencia
                    if (vinculo.pjuEstado == estadoConfirmado)
                    {
                        //obtener la cantidad de asistentes y cupos disponibles
                        var asistentes = asistencias.Where(pj => pj.pjuEstado == 1).ToList();
                        Nullable<decimal> cuposOcupados = asistentes.Count();

                        foreach (Partido_Jugador pj in asistentes)
                        {
                            if (pj.pjuGalleta > 0)
                            {
                                cuposOcupados = cuposOcupados + pj.pjuGalleta;
                            }
                        }

                        var tipoPartido = entities.Partido.Where(p => p.idPartido == vinculo.idPartido)
                                                           .Join(entities.Tipo_Partido,
                                                                p => p.idTipoPartido,
                                                                tp => tp.idTipoPartido,
                                                                (par, tipoPar) => tipoPar).ToList()[0];

                        if (tipoPartido == null)
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No se encuentra toda la información necesaria del eequipo en la base de datos (tipo_partido)");
                        }
                        Nullable<decimal> cuposMaximos = tipoPartido.tpaMaximoJugadores;
                        Nullable<decimal> cuposDisponibles = cuposMaximos - cuposOcupados;

                        if (cuposDisponibles > galletas+1)
                        {
                            vinculo.pjuEstado = estado;
                            vinculo.pjuGalleta = galletas;
                            entities.SaveChanges();

                            return Request.CreateResponse(HttpStatusCode.OK, "Asistencia confirmada");
                        }
                        else
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, "No hay suficientes cupos disponibles");
                        }
                    }
                    //caso 2: asistencia cancelada
                    if (estado == estadoCancelado)
                    {
                        if (vinculo.pjuEstado == estado)
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, "La asistencia del jugador ya se encontraba cancelada o nunca fue creada");
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, "Asistencia cancelada");
                        }
                    }
                    //caso 3: se ingresa in valor de estado de asistencia incorrecto (ni 0 ni 1)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "La asistencia del jugador ya se encontraba cancelada o nunca fue creada");
                }
            }
            catch (Exception ex)
            {
                //En caso de existir otro error, se envia estado de error y un mensaje
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [Route("{idJugador:int}/Partidos/{idPartido:int}/Chat/")]
        public HttpResponseMessage GetChat(int idPartido, int idJugador)
        {
            try
            {
                //Se obtienen los modelos de la BD
                using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
                {
                    //comprobar si el partido esta vinculado al jugador
                    var comprobacion = entities.Partido_Jugador.FirstOrDefault(pj => pj.idJugador==idJugador &&
                                                                                     pj.idPartido==idPartido );


                    if (comprobacion==null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "El jugador no se encuentra en el partido especificado");
                    }

                    //se obtienen todos los registros de chat para el idPartido
                    var chat = entities.Partido_Chat.Where(pch => pch.idPartido == idPartido)
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

                                                                contenidoMensaje = mensaje.pchMensaje,
                                                                creacion = mensaje.pchCreacion
                                                                }
                                                            ).ToList();

                    if (chat != null && chat.Count()>0)
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

       [Route("{idJugador:int}/Partidos/{idPartido:int}/Chat/")]
        public HttpResponseMessage PostChat([FromBody]mensajeSimple mensaje, int idPartido, int idJugador)
        {

            try
            {
                //Se obtienen los modelos de la BD
                using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
                {

                    var comprobacion = entities.Partido_Jugador.FirstOrDefault(pj => pj.idJugador == idJugador &&
                                                                                     pj.idPartido == idPartido);


                    if (comprobacion == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "El jugador no se encuentra en el partido especificado");
                    }

                    Partido_Chat pch = new Partido_Chat();
                    pch.idJugador = idJugador;
                    pch.idPartido = idPartido;
                    pch.pchCreacion = DateTime.Now;
                    pch.pchMensaje = mensaje.contenidoMensaje;


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


        [Route("{idJugador:int}/Equipos/{idEquipo:int}/Chat/")]
        public HttpResponseMessage GetEquipoChat(int idEquipo, int idJugador)
        {
            try
            {
                //Se obtienen los modelos de la BD
                using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
                {
                    //comprobar si el partido esta vinculado al jugador
                    var comprobacion = entities.Equipo_Jugador.FirstOrDefault(ej => ej.idJugador == idJugador &&
                                                                                     ej.idEquipo == idEquipo);


                    if (comprobacion == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "El jugador no se encuentra en el equipo especificado");
                    }

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

                    if (chat != null && chat.Count()>0)
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

        [Route("{idJugador:int}/Equipos/{idEquipo:int}/Chat/")]
        public HttpResponseMessage PostEquipoChat([FromBody]mensajeSimple mensaje, int idEquipo, int idJugador)
        {

            try
            {
                //Se obtienen los modelos de la BD
                using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
                {

                    var comprobacion = entities.Equipo_Jugador.FirstOrDefault(ej => ej.idJugador == idJugador &&
                                                                                    ej.idEquipo == idEquipo);


                    if (comprobacion == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "El jugador no pertenece al equipo especificado");
                    }

                    Equipo_Chat ech = new Equipo_Chat();
                    ech.idJugador = idJugador;
                    ech.idEquipo = idEquipo;
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

        [Route("{idJugador:int}/Partidos/Recientes")]
        public HttpResponseMessage GetRecientes(int idJugador)
        {

            try
            {

                //Se obtienen los modelos de la BD
                using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
                {

                    //metodo 2 obtener el cruce de las tablas jugador, partido y partidoJugador
                    /*   var consulta = (from pj in entities.Partido_Jugador
                                       join p in entities.Partido on pj.idJugador == p. )*/

                    var consulta = entities.Partido_Jugador
                                                    .Where(pj => pj.idJugador == idJugador)
                                                    .Join(entities.Partido.Where(p => p.parFecha.Value.Year >= DateTime.Now.Year),
                                                          pj => pj.idPartido,
                                                          p => p.idPartido,
                                                          (partido_JugadorT, partidoT)
                                                          => new
                                                          {
                                                              partido = new
                                                              {
                                                                  idPartido = partidoT.idPartido,
                                                                  equipo = entities.Equipo.FirstOrDefault(e => e.idEquipo == partidoT.idEquipo),
                                                                  tipoPartido = entities.Tipo_Partido.FirstOrDefault(tp => tp.idTipoPartido == partidoT.idTipoPartido),
                                                                  parCancha = partidoT.parCancha,
                                                                  parComplejo = partidoT.parComplejo,
                                                                  parCreacion = partidoT.parCreacion,
                                                                  parEstado = partidoT.parEstado,
                                                                  parFecha = partidoT.parFecha,
                                                                  parGeoReferencia = partidoT.parGeoReferencia,
                                                                  parHora = partidoT.parHora,
                                                                  parRival = partidoT.parRival,
                                                                  parUbicacion = partidoT.parUbicacion
                                                              }
                                                              ,
                                                              asistencia = partido_JugadorT.pjuEstado,
                                                              galletas = partido_JugadorT.pjuGalleta
                                                          }
                                                          ).ToList();

                    return Request.CreateResponse(HttpStatusCode.OK, consulta);
                }
            }
            catch (Exception ex)
            {
                //En caso de existir otro error, se envia estado de error y un mensaje
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [Route("{idJugador:int}/Partidos/Proximos")]
        public HttpResponseMessage GetProximos(int idJugador)
        {

            try
            {

                //Se obtienen los modelos de la BD
                using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
                {

                    //metodo 2 obtener el cruce de las tablas jugador, partido y partidoJugador
                    /*   var consulta = (from pj in entities.Partido_Jugador
                                       join p in entities.Partido on pj.idJugador == p. )*/

                    var consulta = entities.Partido_Jugador
                                                    .Where(pj => pj.idJugador == idJugador)
                                                    .Join(entities.Partido.Where(p => p.parFecha.Value.Year >= DateTime.Now.Year &&
                                                                                      p.parFecha.Value.Month >= DateTime.Now.Month &&
                                                                                      p.parFecha.Value.Day >= DateTime.Now.Day &&
                                                                                      p.parHora.Value.Hours >= DateTime.Now.Hour &&
                                                                                      p.parHora.Value.Minutes >= DateTime.Now.Minute),
                                                                                      pj => pj.idPartido,
                                                                                      p => p.idPartido,
                                                                                      (partido_JugadorT, partidoT)
                                                                                      => new
                                                                                      {
                                                                                          partido = new
                                                                                          {
                                                                                              idPartido = partidoT.idPartido,
                                                                                              equipo = entities.Equipo.FirstOrDefault(e => e.idEquipo == partidoT.idEquipo),
                                                                                              tipoPartido = entities.Tipo_Partido.FirstOrDefault(tp => tp.idTipoPartido == partidoT.idTipoPartido),
                                                                                              parCancha = partidoT.parCancha,
                                                                                              parComplejo = partidoT.parComplejo,
                                                                                              parCreacion = partidoT.parCreacion,
                                                                                              parEstado = partidoT.parEstado,
                                                                                              parFecha = partidoT.parFecha,
                                                                                              parGeoReferencia = partidoT.parGeoReferencia,
                                                                                              parHora = partidoT.parHora,
                                                                                              parRival = partidoT.parRival,
                                                                                              parUbicacion = partidoT.parUbicacion
                                                                                          }
                                                                                          ,
                                                                                          asistencia = partido_JugadorT.pjuEstado,
                                                                                          galletas = partido_JugadorT.pjuGalleta
                                                                                      }
                                                                                      ).ToList();

                    return Request.CreateResponse(HttpStatusCode.OK, consulta);
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
