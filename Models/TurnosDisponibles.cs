using System;
using System.Collections.Generic;
using System.Text;

namespace HomilApp.Models
{
    internal class TurnosDisponibles
    {
        public int Row { get; set; }
        public int TurnoId { get; set; }
        public int CentroAtencionId { get; set; }
        public string CentroAtencion { get; set; }
        public int EspecialidadId { get; set; }
        public string Especialidad { get; set; }
        public DateTime Fecha { get; set; }
        public int MedicoId { get; set; }
        public string Medico { get; set; }
    }
}
