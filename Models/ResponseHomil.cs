using System;
using System.Collections.Generic;
using System.Text;

namespace HomilApp.Models
{
    public class ResponseHomil<T>
    {
        public bool sucess { get; set; }
        public string message { get; set; }
        public T result { get; set; }
    }
}
