using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using PichangueaDataAccess;

namespace APIRestPichangueaVS.Controllers
{
    public class DeporteController : ApiController
    {
        private PichangueaUsachEntities db = new PichangueaUsachEntities();

        // GET: api/Deporte
        public IQueryable<Deporte> GetDeporte()
        {
            return db.Deporte;
        }

        // GET: api/Deporte/5
        [ResponseType(typeof(Deporte))]
        public IHttpActionResult GetDeporte(decimal id)
        {
            Deporte deporte = db.Deporte.Find(id);
            if (deporte == null)
            {
                return NotFound();
            }

            return Ok(deporte);
        }

        // PUT: api/Deporte/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDeporte(decimal id, Deporte deporte)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != deporte.idDeporte)
            {
                return BadRequest();
            }

            db.Entry(deporte).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeporteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Deporte
        [ResponseType(typeof(Deporte))]
        public IHttpActionResult PostDeporte(Deporte deporte)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Deporte.Add(deporte);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = deporte.idDeporte }, deporte);
        }

        // DELETE: api/Deporte/5
        [ResponseType(typeof(Deporte))]
        public IHttpActionResult DeleteDeporte(decimal id)
        {
            Deporte deporte = db.Deporte.Find(id);
            if (deporte == null)
            {
                return NotFound();
            }

            db.Deporte.Remove(deporte);
            db.SaveChanges();

            return Ok(deporte);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DeporteExists(decimal id)
        {
            return db.Deporte.Count(e => e.idDeporte == id) > 0;
        }
    }
}