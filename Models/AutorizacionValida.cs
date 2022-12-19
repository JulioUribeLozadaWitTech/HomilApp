using System;
using System.Collections.Generic;
using System.Text;

namespace HomilApp.Models
{
    public class AutorizacionValida
    {
        public string Cie10 { get; set; }
        public DateTime Fecha { get; set; }
        public int IdEspecializacion { get; set; }
        public string NombreEspecialidacion { get; set; }
        public string NumeroAutorizacion { get; set; }
    }
}
