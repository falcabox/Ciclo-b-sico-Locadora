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
    public class FilmeController : ApiController
    {
        private MeuContext db = new MeuContext();

        // GET api/Cliente
        public IEnumerable<Filme> GetFilmes()
        {
            return db.Filmes.AsEnumerable();
        }
        
        public Filme GetFilme(string titulo)
        {
            Filme filme = db.Filmes.Find(titulo);
            if (filme == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }
            return filme;

        }

        // PUT api/Filme/5/
        public HttpResponseMessage PutFilme(string titulo, Filme filme)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            Filme filmePesquisa = db.Filmes.Find(titulo);
            if (filmePesquisa != null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(filme).State = EntityState.Modified;

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

        // POST api/Filme
        public HttpResponseMessage PostFilme(string titulo, Filme filme)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Filmes.Add(filme);
                    db.SaveChanges();

                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, filme);
                    response.Headers.Location = new Uri(Url.Link("DefaoutApi", new { titulo = filme.Titulo }));
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
