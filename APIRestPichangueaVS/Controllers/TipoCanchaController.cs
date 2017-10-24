using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PichangueaDataAccess;

namespace APIRestPichangueaVS.Controllers
{
    public class TipoCanchaController : ApiController
    {
        //Funcion que retorna la lista de tipos de canchas
        public HttpResponseMessage Get()
        {
            try
            {
                //Se obtienen los modelos de la BD
                using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
                {
                    //Se crea una lista con todos los tipos de canchas
                    var entity = entities.Tipo_Cancha.ToList();
                    if (entity != null)
                    {
                        //Se retorna el estado OK y la lista de tipos de canchas
                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                    else
                    {
                        //Se retorna el estado NotFound y un string que indica el error
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No existen tipos de canchas");
                    }
                }

            }
            catch (Exception ex)
            {
                //En caso de existir otro error, se envia estado de error y un mensaje
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

        //Funcion que retorna un tipo de cancha segun su id
        public HttpResponseMessage Get(int id)
        {
            try
            {
                //Se obtienen los modelos de la BD
                using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
                {
                    //Se crea una variable con el tipo de cancha correspondiente a la ID
                    var entity = entities.Tipo_Cancha.FirstOrDefault(e => e.idTipoCancha == id);
                    if (entity != null)
                    {
                        //Se retorna el estado OK y el tipo de cancha
                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                    else
                    {
                        //Se retorna el estado NotFound y un string que indica el error
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Tipo de cancha con ID: " + id.ToString() + " no existe");
                    }
                }
            }
            catch (Exception ex)
            {
                //En caso de existir otro error, se envia estado de error y un mensaje
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }


        //Funcion que retorna una lista de tipos de cancha en base a su nombre como entrada
        public HttpResponseMessage Get(String nombre)
        {

            try
            {

                //Se obtienen los modelos de la BD
                using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
                {
                    //Se crea una variable con el tipo de cancha correspondiente a su nombre
                    var entity = entities.Tipo_Cancha.Where(e => e.tcaNombre == nombre).ToList();
                    if (entity != null)
                    {
                        //Se retorna el estado OK y los tipos de cancha
                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                    else
                    {
                        //Se retorna el estado NotFound y un string que indica el error
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Tipo de cancha con nombre: " + nombre + " no existe");
                    }
                }
            }
            catch (Exception ex)
            {
                //En caso de existir otro error, se envia estado de error y un mensaje
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        //Funcion que agrega un tipo de cancha
        public HttpResponseMessage Post([FromBody]Tipo_Cancha tca)
        {
            try
            {
                //Se obtienen los modelos de la BD
                using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
                {
                    //Se agrega el complejo a las entidades
                    entities.Tipo_Cancha.Add(tca);
                    entities.SaveChanges();
                    //Se crea un un mensaje con el codigo Created y con el tipo de cancha ingresado
                    var message = Request.CreateResponse(HttpStatusCode.Created, tca);
                    //Se concatena la ID al tipo de cancha del mensaje
                    message.Headers.Location = new Uri(Request.RequestUri + tca.idTipoCancha.ToString());
                    return message;
                }
            }
            catch (Exception ex)
            {
                //En caso de existir otro error, se envia estado de error y un mensaje
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

        //Funcion que modifica un tipo de cancha
        public HttpResponseMessage Put(int id, [FromBody]Tipo_Cancha tca)
        {
            try
            {
                //Se obtienen los modelos de la BD
                using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
                {
                    //Se crea una variable con el tipo de cancha correspondiente a la ID
                    var entity = entities.Tipo_Cancha.FirstOrDefault(e => e.idTipoCancha == id);
                    if (entity == null)
                    {
                        //Se retorna el estado NotFound y un string que indica el error
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Tipo de cancha con ID: " + id.ToString() + " no existe, no es posible actualizar");

                    }
                    else
                    {
                        //Se modifican los campos del tipo de cancha
                        entity.idDeporte = tca.idDeporte;
                        entity.tcaCreacion = tca.tcaCreacion;
                        entity.tcaNombre = tca.tcaNombre;


                        //Se guardan los cambios
                        entities.SaveChanges();
                        //Se retorna el estado OK y el tipo de cancha
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

        //Funcion que elimina un complejo deportivo
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                //Se obtienen los modelos de la BD
                using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
                {
                    //Se crea una variable con el tipo de cancha correspondiente a la ID
                    var entity = entities.Tipo_Cancha.FirstOrDefault(e => e.idTipoCancha == id);
                    if (entity == null)
                    {
                        //Se retorna el estado NotFound y un string que indica el error
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Tipo de cancha con ID: " + id.ToString() + " no existe, no es posible actualizar");

                    }
                    else
                    {
                        //Se elimina de la BD el complejo deportivo
                        entities.Tipo_Cancha.Remove(entity);
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
