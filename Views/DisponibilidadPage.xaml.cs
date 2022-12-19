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
        public DisponibilidadPage()
        {
            InitializeComponent();
            
            consDisponibilidad = new ConsDisponibilidad();
            this.BindingContext = consDisponibilidad;
        }

        private void EspecialidadPiker_SelectedIndexChanged(object sender, EventArgs e)
        {
            consDisponibilidad.Especialidad = consDisponibilidad.ListEspecialidades.ToArray()[EspecialidadPiker.SelectedIndex].oid;
            var selectedOption = (sender as Picker).SelectedItem;
            consDisponibilidad.ConsultarProfecionales.Execute(selectedOption);
        }

        private void ProfecionalPiker_SelectedIndexChanged(object sender, EventArgs e)
        {
            consDisponibilidad.Profecional = consDisponibilidad.ListProfecionales.ToArray()[ProfecionalPiker.SelectedIndex].oid;
        }
    }
}