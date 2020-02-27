using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiLocadora.Models
{
    public class Filme
    {
        public int id { get; set; }
        public string Titulo { get; set; }
        public bool Alugado { get; set; }
        //public DateTime PrevisaoDevolucao { get; set; }
    }
}