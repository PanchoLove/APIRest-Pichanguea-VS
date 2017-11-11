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
                        //Se retorna el estado OK y la invitacion
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



        //Funcion para obtener todos los partidos del jugador
        [Route("{idJugador:int}/Partidos")]
        public HttpResponseMessage GetPartidos(int idJugador)
        {

            try
            {

                //Se obtienen los modelos de la BD
                using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
                {
                    var partidos = 
                        entities.Equipo_Jugador
                        .Where(ej => ej.idJugador == idJugador) //se filtran con Where los elementos de Equipo_Jugador con la id del jugador buscado
                        .Join(entities.Partido
                        .Where(p => p.parFecha.Value >= DateTime.Now), //el resultado del Where se cruza con la tabla de Partido 
                                ej => ej.idEquipo, //se toma como elemento de comparacion el idEquipo de la tabla Equipo_Jugador
                                p => p.idEquipo, //se toma como segundo elemento de comparacion el idEquipo de la tabla Partido
                                (EquiJug, partidoT)//el join devuelve los elementos de Equipo_Jugador y partido coincidentes
                                => new
                                {
                                    partido = new
                                    {
                                        idPartido = partidoT.idPartido,
                                        equipo = entities.Equipo.FirstOrDefault(e => e.idEquipo == partidoT.idEquipo), //para el equipo se aprovecha la idEquipo del partido y se agrega inmediatamente el equipo
                                        tipoPartido = entities.Tipo_Partido.FirstOrDefault(tp => tp.idTipoPartido == partidoT.idTipoPartido), //se aplica lo mismo para el tipo_partido
                                        parCancha = partidoT.parCancha,
                                        parComplejo = partidoT.parComplejo,
                                        parCreacion = partidoT.parCreacion,
                                        parEstado = partidoT.parEstado,
                                        parFecha = new
                                        {
                                            Año = partidoT.parFecha.Value.Year,
                                            Mes = partidoT.parFecha.Value.Month,
                                            Dia = partidoT.parFecha.Value.Day
                                        },
                                        parGeoReferencia = partidoT.parGeoReferencia,
                                        parHora = new
                                        {
                                            Hora = partidoT.parHora.Value.Hours,
                                            Minutos = partidoT.parHora.Value.Minutes
                                        },
                                        parRival = partidoT.parRival,
                                        parUbicacion = partidoT.parUbicacion
                                    }
                                }
                                ).ToList();

                    if (partidos != null && partidos.Count() > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, partidos);
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

        //Funcion para obtener todos los partidos del jugador
        [Route("{idJugador:int}/Partidos/NoConfirmados")]
        public HttpResponseMessage GetPartidosNC(int idJugador)
        {

            try
            {

                //Se obtienen los modelos de la BD
                using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
                {
                    var partidos = entities.Equipo_Jugador
                                                    .Where(ej => ej.idJugador == idJugador) //se filtran con Where los elementos de Equipo_Jugador con la id del jugador buscado
                                                    .Join(entities.Partido
                                                    .Where(p => p.parFecha.Value >= DateTime.Now), //el resultado del Where se cruza con la tabla de Partido 
                                                          ej => ej.idEquipo, //se toma como elemento de comparacion el idEquipo de la tabla Equipo_Jugador
                                                          p => p.idEquipo, //se toma como segundo elemento de comparacion el idEquipo de la tabla Partido
                                                          (EquiJug, partidoT)//el join devuelve los elementos de Equipo_Jugador y partido coincidentes
                                                          => partidoT).ToList();

                    var partidosConfirmados = entities.Partido_Jugador
                                                     .Where(pj => pj.idJugador == idJugador)
                                                     .ToList();

                    var partidosNOconfirmados = partidos.Where(p => !partidosConfirmados.
                                                                    Select(pj => pj.idPartido)
                                                                    .Contains(p.idPartido));

                    var PNC = 
                        partidosNOconfirmados
                        .Join(partidosNOconfirmados,
                        pnc1 => pnc1.idPartido,
                        pnc2 => pnc2.idPartido,
                        (p1,p2) => new
                        {
                            partido = new
                            {
                                idPartido = p2.idPartido,
                                equipo = entities.Equipo.FirstOrDefault(e => e.idEquipo == p2.idEquipo), //para el equipo se aprovecha la idEquipo del partido y se agrega inmediatamente el equipo
                                tipoPartido = entities.Tipo_Partido.FirstOrDefault(tp => tp.idTipoPartido == p2.idTipoPartido), //se aplica lo mismo para el tipo_partido
                                parCancha = p2.parCancha,
                                parComplejo = p2.parComplejo,
                                parCreacion = p2.parCreacion,
                                parEstado = p2.parEstado,
                                parFecha = new
                                {
                                    Año = p2.parFecha.Value.Year,
                                    Mes = p2.parFecha.Value.Month,
                                    Dia = p2.parFecha.Value.Day
                                },
                                parGeoReferencia = p2.parGeoReferencia,
                                parHora = new
                                {
                                    Hora = p2.parHora.Value.Hours,
                                    Minutos = p2.parHora.Value.Minutes
                                },
                                parRival = p2.parRival,
                                parUbicacion = p2.parUbicacion
                            }
                        }).ToList();

                    if (PNC != null && PNC.Count() > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, PNC);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Ocurrio un error al buscar los partidos o el jugador no tiene partidos no confirmados asociados");
                    }
                }
            }
            catch (Exception ex)
            {
                //En caso de existir otro error, se envia estado de error y un mensaje
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }


        //Funcion para obtener todos los partidos del jugador
        [Route("{idJugador:int}/Partidos/Confirmados")]
        public HttpResponseMessage GetPartidosC(int idJugador)
        {

            try
            {

                //Se obtienen los modelos de la BD
                using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
                {
                    
                    var partidosConfirmados = 
                        entities.Partido_Jugador
                        .Where(pj => pj.idJugador == idJugador)
                            .Join(entities.Partido.Where(p => p.parFecha.Value >= DateTime.Now),
                            pj => pj.idPartido,
                            p => p.idPartido,
                            (ParJug,Par) => new
                            {
                                partido = new
                                {
                                    idPartido = Par.idPartido,
                                    equipo = entities.Equipo.FirstOrDefault(e => e.idEquipo == Par.idEquipo), //para el equipo se aprovecha la idEquipo del partido y se agrega inmediatamente el equipo
                                    tipoPartido = entities.Tipo_Partido.FirstOrDefault(tp => tp.idTipoPartido == Par.idTipoPartido), //se aplica lo mismo para el tipo_partido
                                    parCancha = Par.parCancha,
                                    parComplejo = Par.parComplejo,
                                    parCreacion = Par.parCreacion,
                                    parEstado = Par.parEstado,
                                    parFecha = new
                                    {
                                        Año = Par.parFecha.Value.Year,
                                        Mes = Par.parFecha.Value.Month,
                                        Dia = Par.parFecha.Value.Day
                                    },
                                    parGeoReferencia = Par.parGeoReferencia,
                                    parHora = new
                                    {
                                        Hora = Par.parHora.Value.Hours,
                                        Minutos = Par.parHora.Value.Minutes
                                    },
                                    parRival = Par.parRival,
                                    parUbicacion = Par.parUbicacion
                                },
                                asistencia = ParJug.pjuEstado,
                                galletas = ParJug.pjuGalleta
                            })
                        .ToList();

                   

                    if (partidosConfirmados != null && partidosConfirmados.Count() > 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, partidosConfirmados);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Ocurrio un error al buscar los partidos o el jugador no tiene partidos confirmados asociados");
                    }
                }
            }
            catch (Exception ex)
            {
                //En caso de existir otro error, se envia estado de error y un mensaje
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
        



        //funcion para modificar la asistencia de un jugador a un partido
        /****  MODIFICAR O QUITAR (funciona bajo la idea antigua y solamente modifica el estado)****/
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
                        Nullable<decimal> cuposOcupados = asistentes.Count(); //contar cupos ocupados (puede seguir siendo valida)

                        //sumar las galletas de cada jugador al contador de cupos ocupados
                        foreach (Partido_Jugador pj in asistentes)
                        {
                            if (pj.pjuGalleta > 0)
                            {
                                cuposOcupados = cuposOcupados + pj.pjuGalleta;  
                            }
                        }

                        //obtener el tipoPartido del partido para determinar la cantidad de participantes maximos
                        var tipoPartido = entities.Partido.Where(p => p.idPartido == vinculo.idPartido)
                                                           .Join(entities.Tipo_Partido,
                                                                p => p.idTipoPartido,
                                                                tp => tp.idTipoPartido,
                                                                (par, tipoPar) => tipoPar).ToList()[0];

                        if (tipoPartido == null)
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No se encuentra toda la información necesaria del eequipo en la base de datos (tipo_partido)");
                        }

                        
                        Nullable<decimal> cuposMaximos = tipoPartido.tpaMaximoJugadores; //establecer cupos maximos
                        Nullable<decimal> cuposDisponibles = cuposMaximos - cuposOcupados; //determinar cupos disponibles

                        //modificar el estado si es que quedan cupos disponibles (idea antigua)
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


        //funcion para modificar la cantidad de galletas de un jugador (modifica el valor de "galleta")
        /*se inlcluyen 2 direcciones para que se pueda llamar como 
         * Galletas/{cantidad} o Galletas?cantidad={cantidad}
        */

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

                    //sumar las galletas de cada jugador al contador de cupos ocupados
                    foreach (Partido_Jugador pj in asistentes)
                        {
                            if (pj.pjuGalleta > 0)
                            {
                                cuposOcupados = cuposOcupados + pj.pjuGalleta;
                            }
                        }

                    //obtener el tipoPartido del partido para determinar la cantidad de participantes maximos
                    var tipoPartido = entities.Partido.Where(p => p.idPartido == vinculo.idPartido)
                                                           .Join(entities.Tipo_Partido,
                                                                p => p.idTipoPartido,
                                                                tp => tp.idTipoPartido,
                                                                (par, tipoPar) => tipoPar).ToList()[0];

                        if (tipoPartido == null)
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No se encuentra toda la información necesaria del eequipo en la base de datos (tipo_partido)");
                        }



                    Nullable<decimal> cuposMaximos = tipoPartido.tpaMaximoJugadores; //establecer cupos maximos
                    Nullable<decimal> cuposDisponibles = cuposMaximos - cuposOcupados; //determinar cupos disponibles

                    //modificar el estado si es que quedan cupos disponibles tanto para el jugador como las galletas 
                   // (idea antigua no se debe modificar el estado si no crear un nuevo patido_jugador)
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


        //funcion para agregar mas galletas al valor "galleta" ya existente
        //hace galleta = galleta + cantidad
        //MODIFICAR O QUITAR 
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

                    //sumar al contador de cupos ocupados las galletas de los asistentes
                    foreach (Partido_Jugador pj in asistentes)
                    {
                        if (pj.pjuGalleta > 0)
                        {
                            cuposOcupados = cuposOcupados + pj.pjuGalleta;
                        }
                    }
                    //obtener el tipoPartido del partido para obtener la cantidad maxima de cupos
                    var tipoPartido = entities.Partido.Where(p => p.idPartido == vinculo.idPartido)
                                                       .Join(entities.Tipo_Partido,
                                                            p => p.idTipoPartido,
                                                            tp => tp.idTipoPartido,
                                                            (par, tipoPar) => tipoPar).ToList()[0];

                    if (tipoPartido == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No se encuentra toda la información necesaria del eequipo en la base de datos (tipo_partido)");
                    }
                    
                    Nullable<decimal> cuposMaximos = tipoPartido.tpaMaximoJugadores; //determinando cupos maximos
                    Nullable<decimal> cuposDisponibles = cuposMaximos - cuposOcupados; //determinando cupos disponibles

                    //si alcanzan cupos para las nuevas galletas agregarlas
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


        //Funcion que confirma asistencia y agrega galletas al mismo tiempo
        /***MODIFICAR (el acto de confirmar asistencia ya no deberia cambiar el estado si no crear
         * un vinculo del tipo Partido_Jugador
         * )**/
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

                        //sumar al contador de cupos ocupados las galletas de los asistentes
                        foreach (Partido_Jugador pj in asistentes)
                        {
                            if (pj.pjuGalleta > 0)
                            {
                                cuposOcupados = cuposOcupados + pj.pjuGalleta;
                            }
                        }

                        //obtener el tipoPartido del partido para obtener la cantidad maxima de cupos
                        var tipoPartido = entities.Partido.Where(p => p.idPartido == vinculo.idPartido)
                                                           .Join(entities.Tipo_Partido,
                                                                p => p.idTipoPartido,
                                                                tp => tp.idTipoPartido,
                                                                (par, tipoPar) => tipoPar).ToList()[0];

                        if (tipoPartido == null)
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No se encuentra toda la información necesaria del eequipo en la base de datos (tipo_partido)");
                        }
                        Nullable<decimal> cuposMaximos = tipoPartido.tpaMaximoJugadores; //determinando cupos maximos
                        Nullable<decimal> cuposDisponibles = cuposMaximos - cuposOcupados; //determinando cupos disponibles

                        //si alcanzan cupos para el jugador + sus galletas confirmar asistencia
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

        //funcion para obtener los mensaje del chat de un partido
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

        //funcion para enviar un mensaje al chat de un partido
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

        //funcion para obtener los mensaje del chat de un equipo (creo que la base de datos no tiene)
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


        //funcion para enviar un mensaje chat de un equipo
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

        


    }
}
