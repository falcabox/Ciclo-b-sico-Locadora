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
    public class LocacaoController : ApiController
    {
        private MeuContext db = new MeuContext();

        // GET api/Locacao
        public IEnumerable<Locacao> GetLocacoes()
        {
            return db.Locacoes.AsEnumerable();
        }



        // GET api/Locacao/5
        public Locacao GetLocacao(string titulo)
        {
            Filme filme = db.Filmes.Find(titulo);

            if (filme == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            int idFilme = filme.id;
            Locacao locacao = db.Locacoes.Find(idFilme);

            if (filme.Alugado == true)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.Unauthorized));
            }

            return locacao;
        }




        // PUT api/Locacao/5
        public HttpResponseMessage PutLocacao(string titulo, int idCliente, Locacao locacao)
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

            int idFilme = filmePesquisa.id;

            if (idFilme != locacao.id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            locacao.idFilme = idFilme;
            DateTime dtHoje = DateTime.Now;
            locacao.PrevisaoDevolucao = dtHoje.AddDays(1);

            db.Entry(locacao).State = EntityState.Modified;

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


        // POST api/Locacao
        public HttpResponseMessage PostLocacao(Locacao locacao)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Locacoes.Add(locacao);
                    db.SaveChanges();

                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, locacao);
                    response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = locacao.id }));
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


        // PUT api/Locacao/titulo/
        public HttpResponseMessage PutDevolucao(string titulo, Filme filme)
        {


            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }


            Filme filmePesquisa = db.Filmes.Find(titulo);
            if (filmePesquisa == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            int idFilme = filmePesquisa.id;
            Locacao locacao = db.Locacoes.Find(idFilme);

            if (locacao == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            DateTime dataPrevisao = locacao.PrevisaoDevolucao;

            filme.Alugado = false;
            locacao.PrevisaoDevolucao = DateTime.MinValue;

            db.Entry(filme).State = EntityState.Modified;
            db.Entry(locacao).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            if (dataPrevisao > DateTime.Now)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
