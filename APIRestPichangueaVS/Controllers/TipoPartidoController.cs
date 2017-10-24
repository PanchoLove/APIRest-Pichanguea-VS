using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PichangueaDataAccess;

namespace APIRestPichangueaVS.Controllers
{
    public class TipoPartidoController : ApiController
    {
        //Funcion que retorna la lista de tipos de partidos
        public HttpResponseMessage Get()
        {
            try
            {
                //Se obtienen los modelos de la BD
                using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
                {
                    //Se crea una lista con todos los tipos de partidos
                    var entity = entities.Tipo_Partido.ToList();
                    if (entity != null && entity.Count() > 0)
                    {
                        //Se retorna el estado OK y la lista de tipos de partidos
                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                    else
                    {
                        //Se retorna el estado NotFound y un string que indica el error
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No existen tipos de partidos");
                    }
                }

            }
            catch (Exception ex)
            {
                //En caso de existir otro error, se envia estado de error y un mensaje
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

        //Funcion que retorna un tipo de partido segun su id
        public HttpResponseMessage Get(int id)
        {
            try
            {
                //Se obtienen los modelos de la BD
                using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
                {
                    //Se crea una variable con el tipo de partido correspondiente a la ID
                    var entity = entities.Tipo_Partido.FirstOrDefault(e => e.idTipoPartido == id);
                    if (entity != null)
                    {
                        //Se retorna el estado OK y el tipo de partido
                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                    else
                    {
                        //Se retorna el estado NotFound y un string que indica el error
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Tipo de partido con ID: " + id.ToString() + " no existe");
                    }
                }
            }
            catch (Exception ex)
            {
                //En caso de existir otro error, se envia estado de error y un mensaje
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }


        //Funcion que retorna una lista de tipos de partido en base a su nombre como entrada
        public HttpResponseMessage Get(String nombre)
        {

            try
            {

                //Se obtienen los modelos de la BD
                using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
                {
                    //Se crea una variable con el tipo de partido correspondiente a su nombre
                    var entity = entities.Tipo_Partido.Where(e => e.tpaNombre == nombre).ToList();
                    if (entity != null)
                    {
                        //Se retorna el estado OK y los tipos de partido
                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                    else
                    {
                        //Se retorna el estado NotFound y un string que indica el error
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Tipo de partido con nombre: " + nombre + " no existe");
                    }
                }
            }
            catch (Exception ex)
            {
                //En caso de existir otro error, se envia estado de error y un mensaje
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        //Funcion que agrega un tipo de partido
        public HttpResponseMessage Post([FromBody]Tipo_Partido tpa)
        {
            try
            {
                //Se obtienen los modelos de la BD
                using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
                {
                    //Se agrega el tipo de partido a las entidades
                    entities.Tipo_Partido.Add(tpa);
                    entities.SaveChanges();
                    //Se crea un un mensaje con el codigo Created y con el tipo de partido ingresado
                    var message = Request.CreateResponse(HttpStatusCode.Created, tpa);
                    //Se concatena la ID al tipo de partido del mensaje
                    message.Headers.Location = new Uri(Request.RequestUri + tpa.idTipoPartido.ToString());
                    return message;
                }
            }
            catch (Exception ex)
            {
                //En caso de existir otro error, se envia estado de error y un mensaje
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

        //Funcion que modifica un tipo de partido
        public HttpResponseMessage Put(int id, [FromBody]Tipo_Partido tpa)
        {
            try
            {
                //Se obtienen los modelos de la BD
                using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
                {
                    //Se crea una variable con el tipo de partido correspondiente a la ID
                    var entity = entities.Tipo_Partido.FirstOrDefault(e => e.idTipoPartido == id);
                    if (entity == null)
                    {
                        //Se retorna el estado NotFound y un string que indica el error
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Tipo de partido con ID: " + id.ToString() + " no existe, no es posible actualizar");

                    }
                    else
                    {
                        //Se modifican los campos del tipo de cancha
                        entity.idDeporte = tpa.idDeporte;
                        entity.tpaMaximoJugadores = tpa.tpaMaximoJugadores;
                        entity.tpaNombre = tpa.tpaNombre;


                        //Se guardan los cambios
                        entities.SaveChanges();
                        //Se retorna el estado OK y el tipo de partido
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

        //Funcion que elimina un tipo de partido
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                //Se obtienen los modelos de la BD
                using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
                {
                    //Se crea una variable con el tipo de partido correspondiente a la ID
                    var entity = entities.Tipo_Partido.FirstOrDefault(e => e.idTipoPartido == id);
                    if (entity == null)
                    {
                        //Se retorna el estado NotFound y un string que indica el error
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Tipo de partido con ID: " + id.ToString() + " no existe, no es posible actualizar");

                    }
                    else
                    {
                        //Se elimina de la BD el complejo deportivo
                        entities.Tipo_Partido.Remove(entity);
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
