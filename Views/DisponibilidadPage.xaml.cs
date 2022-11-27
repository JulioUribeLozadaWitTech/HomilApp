using HomilApp.Models;
using HomilApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomilApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DisponibilidadPage : ContentPage
    {
        private ConsDisponibilidad consDisponibilidad;
        public List<Especialidad> listEspecialidades = new List<Especialidad>() {
                new Especialidad() { id= 1 , codigo="ESP1" , nombre = "Cardiologia" },
                new Especialidad() { id= 2 , codigo="ESP2" , nombre = "Cardiologia pediatria" },
                new Especialidad() { id= 3 , codigo="ESP3" , nombre = "Cirugia cardiovascular" },
                new Especialidad() { id= 4 , codigo="ESP4" , nombre = "Nefrologia" },
                new Especialidad() { id= 5 , codigo="ESP5" , nombre = "Oftalmologia" },
                new Especialidad() { id= 6 , codigo="ESP6" , nombre = "Neurologia" }
        };
        public List<Actividad> listActividades = new List<Actividad>() {
                new Actividad() { codigo = "20B" , id = 1 , nombre="telemedicina"},
                new Actividad() { codigo = "30B" , id = 1 , nombre="presencial"},
        };
        public DisponibilidadPage()
        {
            InitializeComponent();
            ActividadPiker.ItemsSource = listActividades;
            EspecialidadPiker.ItemsSource = listEspecialidades;
            consDisponibilidad = new ConsDisponibilidad();
            this.BindingContext = consDisponibilidad;
        }

        private void EspecialidadPiker_SelectedIndexChanged(object sender, EventArgs e)
        {
            consDisponibilidad.Especialidad = listEspecialidades.ToArray()[EspecialidadPiker.SelectedIndex].id;
        }

        private void ActividadPiker_SelectedIndexChanged(object sender, EventArgs e)
        {
            consDisponibilidad.Actividad = listActividades.ToArray()[ActividadPiker.SelectedIndex].id;
        }
    }
}