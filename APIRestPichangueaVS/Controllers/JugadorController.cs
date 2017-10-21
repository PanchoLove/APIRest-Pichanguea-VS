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


        //Funcion que retorna una lista de jugadores en base a su nombre como entrada
        public HttpResponseMessage Get(String nombre)
        {
            
            try
            {

                //Se obtienen los modelos de la BD
                using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
                {
                    //Se crea una variable con el jugador correspondiente a la ID
                    var entity = entities.Jugador.Where(e => e.jugNombre == nombre).ToList();
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

        [Route("{idJugador:int}/Equipos")]
        public HttpResponseMessage GetEquipos(int idJugador)
        {

            try
            {

                //Se obtienen los modelos de la BD
                using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
                {
                    //Se con la lista de equipos pertenecientes al jugador
                    var jugador = entities.Jugador.FirstOrDefault(j => j.idJugador == idJugador);
                    /*List<Equipo_Jugador>*/
                    var intermedios = entities.Equipo_Jugador.Where(ej => ej.idJugador == jugador.idJugador).ToList();
                    var equipos = entities.Equipo.ToList();
                    List<Equipo> equiposFiltrados = new List<Equipo>();

                    foreach (Equipo_Jugador ej in intermedios)
                    {
                        equiposFiltrados.Add(entities.Equipo.FirstOrDefault(e => e.idEquipo == ej.idEquipo));
                    }


                    if (equiposFiltrados != null || equiposFiltrados.Count <= 0)
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

                    var invitaciones = entities.Equipo_Invitacion.ToList();

                    if (invitaciones != null)
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
                    //obtener la lista de partidos pertenecientes al jugador

                    //obtener de la tabla intermedia entre jugador y partido las filas donde se encuentra la id del jugador
                    var intermedios = entities.Partido_Jugador.Where(pj => pj.idJugador == idJugador).ToList();

                    if(intermedios != null){
                        if (intermedios.Count <=0) {
                            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Jugador con ID: " + idJugador + " no existe o no posee partidos");
                        }
                    }

                    //preparando la lista de retorno
                    var equipos = entities.Equipo.ToList();
                    List<PartidoCompuesto> partidosRespuesta = new List<PartidoCompuesto>();


                    //recorrer la lista de filas que relacionan jugador con partido y obtener el partido correspondiente
                    foreach (Partido_Jugador pj in intermedios)
                    {
                        var partido = entities.Partido.FirstOrDefault(p => p.idPartido == pj.idPartido);

                        PartidoCompuesto pc = new PartidoCompuesto();
                        pc.idPartido = partido.idPartido;
                        pc.equipo = entities.Equipo.FirstOrDefault(e=> e.idEquipo == partido.idEquipo);
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
                        pc.asistencia = pj.pjuEstado;
                        pc.galletas = pj.pjuGalleta;  

                        partidosRespuesta.Add(pc);
                        

                    }

                    if (partidosRespuesta.Count>0)
                    {
                            //Se retorna el estado OK y el jugador
                            return Request.CreateResponse(HttpStatusCode.OK, partidosRespuesta);
                        
                    }

                    else
                    {
                        //Se retorna el estado NotFound y un string que indica el error
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Existe un error, se encuentran partidos asociados al jugador pero no la informacion del partido en la base de datos");
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
                        pc.asistencia = pj[0].pjuEstado ;
                        pc.galletas = pj[0].pjuGalleta;

                        //Se retorna el estado NotFound y un string que indica el error
                        return Request.CreateResponse(HttpStatusCode.OK, pc);
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
                    //obtener la lista de partidos pertenecientes al jugador

                    //obtener de la tabla intermedia entre jugador y partido la fila donde se encuentra la id del jugador y la id del partido al mismo tiempos
                    var intermedio = entities.Partido_Jugador.Where(pj => pj.idJugador == idJugador && pj.idPartido == idPartido).ToList();
                    var partido = entities.Partido.FirstOrDefault(p=> p.idPartido == idPartido);

                    if (intermedio == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Jugador con ID: " + idJugador + " no existe o no posee partidos");
                    }
                    else
                    {
                        if (estado == estadoConfirmado)
                        {
                            //caso 1: confirmaAsistencia

                            //pasar la consulta "intermedio" a un formato manejable (una lista xDDD)
                            var partidoJugador = intermedio.ToList()[0];

                            //si efectivamente el partido esta vinculado al usuario realizar modificacion de estado

                            //comprobar si la cantidad de jugadores asistiendo al partido no excede el maximo del partido
                            //obtener todos los jugadores vinculados al partido
                            var asistentes = entities.Partido_Jugador.Where(pj => pj.idPartido == idPartido && pj.pjuEstado == estado).ToList();

                            //obtener los cupos ocupados
                            Nullable<decimal> cuposOcupados = asistentes.Count;

                            foreach (Partido_Jugador pj in asistentes)
                            {
                                if (pj.pjuGalleta > 0)
                                {
                                    cuposOcupados++;
                                }
                            }

                            //obtener los cupos maximos
                            var tipoPartido = entities.Tipo_Partido.FirstOrDefault(tp => tp.idTipoPartido == partido.idTipoPartido);
                            if (tipoPartido == null){
                                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No se encuentra toda la información necesaria del eequipo en la base de datos (tipo_partido)");
                            }
                            Nullable<decimal> cuposMaximos = tipoPartido.tpaMaximoJugadores;

                            //obtener los cupos disponibles;
                            Nullable<decimal> cuposDisponibles = cuposMaximos - cuposOcupados;

                            if (cuposDisponibles > 0)
                            {
                                partidoJugador.pjuEstado = estado;
                                entities.SaveChanges();

                                return Request.CreateResponse(HttpStatusCode.OK, "Asistencia confirmada");
                            }
                            else
                            {
                                return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, "No hay suficientes cupos disponibles");
                            }

                        }
                        else {

                            if (estado == estadoCancelado)
                            {
                                //pasar la consulta "intermedio" a un formato manejable (una lista xDDD)
                                var partidoJugador = intermedio.ToList()[0];
                                partidoJugador.pjuEstado = estado;
                                entities.SaveChanges();
                                return Request.CreateErrorResponse(HttpStatusCode.OK, "Asitencia cancelada");

                            }
                            else {
                                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Codigo de estado desconocido");

                            }

                        }
                        return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, "Codigo de estado desconocido");

                    }
                }
            }
            catch (Exception ex)
            {
                //En caso de existir otro error, se envia estado de error y un mensaje
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }


        [Route("{idJugador:int}/Partidos/{idPartido:int}/Asistencia/Galletas{cantidad:int}")]
        public HttpResponseMessage PostGalletas(int idJugador, int idPartido, int cantidad)
        {

            try
            {
                //Se obtienen los modelos de la BD
                using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
                {


                    //obtener la lista de partidos pertenecientes al jugador
            

                    //obtener de la tabla intermedia entre jugador y partido la fila donde se encuentra la id del jugador y la id del partido al mismo tiempos
                    var intermedio = entities.Partido_Jugador.Where(pj => pj.idJugador == idJugador && pj.idPartido == idPartido).ToList();

                    if (intermedio == null){
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Jugador con ID: " + idJugador + " no existe o no posee partidos");
                    }

                    //pasar la consulta "intermedio" a un formato manejable (una lista xDDD) (aca creo que debe haber un if)
                    var partidoJugador = intermedio.ToList()[0];

                    //revisar si e jugador confirmo su asistencia
                    if (partidoJugador.pjuEstado == estadoCancelado) {
                        return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "El jugador no se encuentra dentro de la lista de asistentes");
                    }

                    var partido = entities.Partido.FirstOrDefault(p => p.idPartido == idPartido);


                    //si efectivamente el partido esta vinculado al usuario realizar modificacion de estado

                    //comprobar si la cantidad de jugadores asistiendo al partido no excede el maximo del partido
                    //obtener todos los jugadores vinculados al partido
                    var asistentes = entities.Partido_Jugador.Where(pj => pj.idPartido == idPartido && pj.pjuEstado == estadoConfirmado).ToList();

                            //obtener los cupos ocupados
                            Nullable<decimal> cuposOcupados = asistentes.Count;

                            foreach (Partido_Jugador pj in asistentes)
                            {
                                if (pj.pjuGalleta > 0)
                                {
                                    cuposOcupados++;
                                }
                            }

                            //obtener los cupos maximos
                            var tipoPartido = entities.Tipo_Partido.FirstOrDefault(tp => tp.idTipoPartido == partido.idTipoPartido);
                            if (tipoPartido == null)
                            {
                                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No se encuentra toda la información necesaria del eequipo en la base de datos (tipo_partido)");
                            }
                            Nullable<decimal> cuposMaximos = tipoPartido.tpaMaximoJugadores;

                            //obtener los cupos disponibles;
                            Nullable<decimal> cuposDisponibles = cuposMaximos - cuposOcupados;

                            if (cuposDisponibles > cantidad)
                            {
                                partidoJugador.pjuEstado = estadoConfirmado;
                                entities.SaveChanges();

                                return Request.CreateResponse(HttpStatusCode.OK, "galletas agregadas: " + cantidad);
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

    }
}
