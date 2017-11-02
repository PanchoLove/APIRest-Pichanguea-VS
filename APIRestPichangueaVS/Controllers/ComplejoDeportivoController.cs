using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PichangueaDataAccess;


namespace APIRestPichangueaVS.Controllers
{
    public class ComplejoDeportivoController : ApiController
    {
        //Funcion que retorna la lista de complejos deportivos
        public HttpResponseMessage Get()
        {
            try
            {
                //Se obtienen los modelos de la BD
                using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
                {
                    //Se crea una lista con todos los complejos deportivos
                    var entity = entities.Complejo_Deportivo.ToList();
                    if (entity != null && entity.Count() > 0)
                    {
                        //Se retorna el estado OK y la lista de complejos
                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                    else
                    {
                        //Se retorna el estado NotFound y un string que indica el error
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No existen complejos deportivos");
                    }
                }

            }
            catch (Exception ex)
            {
                //En caso de existir otro error, se envia estado de error y un mensaje
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

        //Funcion que retorna un complejo deportivo
        public HttpResponseMessage Get(int id)
        {
            try
            {
                //Se obtienen los modelos de la BD
                using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
                {
                    //Se crea una variable con el complejo deportivo correspondiente a la ID
                    var entity = entities.Complejo_Deportivo.FirstOrDefault(e => e.idComplejoDeportivo == id);
                    if (entity != null)
                    {
                        //Se retorna el estado OK y el complejo deportivo
                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                    else
                    {
                        //Se retorna el estado NotFound y un string que indica el error
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Complejo con ID: " + id.ToString() + " no existe");
                    }
                }
            }
            catch (Exception ex)
            {
                //En caso de existir otro error, se envia estado de error y un mensaje
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }


        //Funcion que retorna una lista de complejos deportivos en base a su nombre como entrada
        public HttpResponseMessage Get(String nombre)
        {

            try
            {

                //Se obtienen los modelos de la BD
                using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
                {
                    //Se crea una variable con el complejo deportivo correspondiente a su nombre
                    var entity = entities.Complejo_Deportivo.Where(e => e.cmdNombre == nombre).ToList();
                    if (entity != null && entity.Count() > 0)
                    {
                        //Se retorna el estado OK y el complejo deportivo
                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                    else
                    {
                        //Se retorna el estado NotFound y un string que indica el error
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Complejo con nombre: " + nombre + " no existe");
                    }
                }
            }
            catch (Exception ex)
            {
                //En caso de existir otro error, se envia estado de error y un mensaje
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        //Funcion que agrega un complejo deportivo
        public HttpResponseMessage Post([FromBody]Complejo_Deportivo complejo)
        {
            try
            {
                //Se obtienen los modelos de la BD
                using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
                {
                    //Se agrega el complejo a las entidades
                    entities.Complejo_Deportivo.Add(complejo);
                    entities.SaveChanges();
                    //Se crea un un mensaje con el codigo Created y con el complejo ingresado
                    var message = Request.CreateResponse(HttpStatusCode.Created, complejo);
                    //Se concatena la ID al complejo del mensaje
                    message.Headers.Location = new Uri(Request.RequestUri + complejo.idComplejoDeportivo.ToString());
                    return message;
                }
            }
            catch (Exception ex)
            {
                //En caso de existir otro error, se envia estado de error y un mensaje
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

        //Funcion que modifica un complejo deportivo
        public HttpResponseMessage Put(int id, [FromBody]Complejo_Deportivo complejo)
        {
            try
            {
                //Se obtienen los modelos de la BD
                using (PichangueaUsachEntities entities = new PichangueaUsachEntities())
                {
                    //Se crea una variable con el complejo correspondiente a la ID
                    var entity = entities.Complejo_Deportivo.FirstOrDefault(e => e.idComplejoDeportivo == id);
                    if (entity == null)
                    {
                        //Se retorna el estado NotFound y un string que indica el error
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Complejo con ID: " + id.ToString() + " no existe, no es posible actualizar");

                    }
                    else
                    {
                        //Se modifican los campos del complejo deportivo
                        entity.cmdCelular = complejo.cmdCelular;
                        entity.cmdCreacion = complejo.cmdCreacion;
                        entity.cmdDireccion = complejo.cmdDireccion;
                        entity.cmdEmail = complejo.cmdEmail;
                        entity.cmdFax = complejo.cmdFax;
                        entity.cmdFono = complejo.cmdFono;
                        entity.cmdGeoreferencia = complejo.cmdGeoreferencia;
                        entity.cmdNombre = complejo.cmdNombre;
                        entity.cmdUrl = complejo.cmdUrl;


                        //Se guardan los cambios
                        entities.SaveChanges();
                        //Se retorna el estado OK y el complejo
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
                    //Se crea una variable con el complejo correspondiente a la ID
                    var entity = entities.Complejo_Deportivo.FirstOrDefault(e => e.idComplejoDeportivo == id);
                    if (entity == null)
                    {
                        //Se retorna el estado NotFound y un string que indica el error
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Complejo con ID: " + id.ToString() + " no existe, no es posible actualizar");

                    }
                    else
                    {
                        //Se elimina de la BD el complejo deportivo
                        entities.Complejo_Deportivo.Remove(entity);
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
