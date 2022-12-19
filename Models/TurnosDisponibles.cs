using System;
using System.Collections.Generic;
using System.Text;

namespace HomilApp.Models
{
    public class Disponibilidad
    {
        public List<TurnosDisponibles> citafecha { get; set; }
    }
    public class TurnosDisponibles
    {
        public int oidTurno { get; set; }
        public int EspecialidadId { get; set; }
        public string Especialidad { get; set; }
        public DateTime fechaInicial { get; set; }
        public int MedicoId { get; set; }
        public string Medico { get; set; }
    }
}
