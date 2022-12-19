using System;
using System.Collections.Generic;
using System.Text;

namespace HomilApp.Models
{
    public class Especialidad
    {
        public List<EspecialidadItem> especialidad { get; set; }
    }
    public class EspecialidadItem
    {
        public string geeDescri { get; set; }
        public int oid { get; set; }
    }
}
