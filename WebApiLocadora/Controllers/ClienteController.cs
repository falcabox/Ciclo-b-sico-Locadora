using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using WebApiLocadora.Models;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace WebApiLocadora.Controllers
{
    public class ClienteController : ApiController
    {
        private MeuContext db = new MeuContext();

        // GET api/Cliente
        public IEnumerable<Cliente> GetClientes()
        {
            return db.Clientes.AsEnumerable();
        }

        // GET api/Cliente/5
        public Cliente GetCliente(int id)
        {
            Cliente cliente = db.Clientes.Find(id);
            if (cliente == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return cliente;
        }


        // PUT api/Cliente/5
        public HttpResponseMessage PutCliente(int id, Cliente cliente)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != cliente.id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(cliente).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
            
        }

        // POST api/Cliente
        public HttpResponseMessage PostCliente(int id, Cliente cliente)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Clientes.Add(cliente);
                    db.SaveChanges();

                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, cliente);
                    response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = cliente.id }));
                    return response;
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
