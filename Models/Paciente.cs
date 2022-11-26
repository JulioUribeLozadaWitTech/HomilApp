using System;
using System.Collections.Generic;
using System.Text;

namespace HomilApp.Models
{
    internal class Paciente
    {
        public string PacienteId { get; set; }
        public string Identificacion { get; set; }
        public string TipoIdentificacion { get; set; }
        public string PrimerNombre { get; set; }
        public string PrimerApellido { get; set; }
        public string Password { get; set; }

    }
}
