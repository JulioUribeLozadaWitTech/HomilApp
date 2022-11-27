using Acr.UserDialogs;
using HomilApp.Models;
using HomilApp.Service;
using HomilApp.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace HomilApp.ViewModels
{
    internal class ConsDisponibilidad : INotifyPropertyChanged
    {
        private DateTime fechaInicial;
        private int especialidad;
        private int actividad;
        private bool isVisible;
        public ICommand ConsultarDisponibilidad { get; set; }

        HomilClient homilClient = new HomilClient();

        private ObservableCollection<TurnosDisponibles> listTurnosDisponibles { get; set; }



        public List<TurnosDisponibles> listDisponibilidad = new List<TurnosDisponibles>() {
                new TurnosDisponibles() {  TurnoId = 1111111 , Fecha = DateTime.Now.AddDays(10) , EspecialidadId = 1 , CentroAtencionId = 1 , CentroAtencion = "Hospital general militar" , MedicoId = 1 , Medico = "Alicia fernandez"},
                new TurnosDisponibles() {  TurnoId = 11112311 , Fecha = DateTime.Now.AddDays(5) , EspecialidadId = 1 ,  CentroAtencionId = 1 , CentroAtencion = "Hospital general militar" , MedicoId = 1 , Medico = "Alicia fernandez"},
                new TurnosDisponibles() {  TurnoId = 11145511 , Fecha = DateTime.Now.AddDays(20) , EspecialidadId = 2 , CentroAtencionId = 1 , CentroAtencion = "Hospital general militar" , MedicoId = 1 , Medico = "Alicia fernandez"},
                new TurnosDisponibles() {  TurnoId = 1166111 , Fecha = DateTime.Now.AddDays(12) , EspecialidadId = 2 ,  CentroAtencionId = 1 , CentroAtencion = "Hospital general militar" , MedicoId = 1 , Medico = "Alicia fernandez"},
                new TurnosDisponibles() {  TurnoId = 7811111 , Fecha = DateTime.Now.AddDays(13) , EspecialidadId = 3 , CentroAtencionId = 1 , CentroAtencion = "Hospital general militar" , MedicoId = 1 , Medico = "Alicia fernandez"},
                new TurnosDisponibles() {  TurnoId = 7811111 , Fecha = DateTime.Now.AddDays(7) , EspecialidadId = 3 , CentroAtencionId = 1 , CentroAtencion = "Hospital general militar" , MedicoId = 1 , Medico = "Alicia fernandez"},
                new TurnosDisponibles() {  TurnoId = 7811111 , Fecha = DateTime.Now.AddDays(5) , EspecialidadId = 4 , CentroAtencionId = 1 , CentroAtencion = "Hospital general militar" , MedicoId = 1 , Medico = "Alicia fernandez"},
                new TurnosDisponibles() {  TurnoId = 7811111 , Fecha = DateTime.Now.AddDays(4) , EspecialidadId = 5 , CentroAtencionId = 1 , CentroAtencion = "Hospital general militar" , MedicoId = 1 , Medico = "Alicia fernandez"},
                new TurnosDisponibles() {  TurnoId = 7811111 , Fecha = DateTime.Now.AddDays(15) , EspecialidadId = 6 , CentroAtencionId = 1 , CentroAtencion = "Hospital general militar" , MedicoId = 1 , Medico = "Alicia fernandez"},
                new TurnosDisponibles() {  TurnoId = 7811111 , Fecha = DateTime.Now.AddDays(12) , EspecialidadId = 4 , CentroAtencionId = 1 , CentroAtencion = "Hospital general militar" , MedicoId = 1 , Medico = "Alicia fernandez"},
                new TurnosDisponibles() {  TurnoId = 7811111 , Fecha = DateTime.Now.AddDays(3) , EspecialidadId = 6 , CentroAtencionId = 1 , CentroAtencion = "Hospital general militar" , MedicoId = 1 , Medico = "Alicia fernandez"},
                new TurnosDisponibles() {  TurnoId = 7811111 , Fecha = DateTime.Now.AddDays(2) , EspecialidadId = 5 , CentroAtencionId = 1 , CentroAtencion = "Hospital general militar" , MedicoId = 1 , Medico = "Alicia fernandez"},
        };

        public List<Especialidad> listEspecialidades = new List<Especialidad>() {
                new Especialidad() { id= 1 , codigo="ESP1" , nombre = "Cardiologia" },
                new Especialidad() { id= 2 , codigo="ESP2" , nombre = "Cardiologia pediatria" },
                new Especialidad() { id= 3 , codigo="ESP3" , nombre = "Cirugia cardiovascular" },
                new Especialidad() { id= 4 , codigo="ESP4" , nombre = "Nefrologia" },
                new Especialidad() { id= 5 , codigo="ESP5" , nombre = "Oftalmologia" },
                new Especialidad() { id= 6 , codigo="ESP6" , nombre = "Neurologia" }
        };

        public ConsDisponibilidad()
        {
            IsVisible = false;
            ListTurnosDisponibles = new ObservableCollection<TurnosDisponibles>();
            ConsultarDisponibilidad = new Command(async () => await consultarDisponibilidad());
        }

        private async Task consultarDisponibilidad()
        {
            UserDialogs.Instance.ShowLoading("Consultando...");
            if (FechaInicial == null || Especialidad == 0 || Actividad == 0)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Campos no validos por favor diligenciar valores correctos.", "Acceptar");
                IsVisible = false;
            }
            else
            {
                var json = JsonConvert.SerializeObject(new { FechaInicial, DiasAdicionales = 30, OidEspecialidad = Especialidad, OidActividad = Actividad, OidMedico = 0, OidCentroAtencion = 1, CantidadRegistros = 3 });
                List<TurnosDisponibles> responseListTurnosDisponibles = await homilClient.executeRequestPost<List<TurnosDisponibles>>("/HospitalMilitar/CitasMedicas/consultarDisponibilidad", json);
                if (responseListTurnosDisponibles.Count() > 0)
                {
                    ListTurnosDisponibles = new ObservableCollection<TurnosDisponibles>(responseListTurnosDisponibles);
                }
                else
                {
                    var listDisponibilidadO = listDisponibilidad.Where(item => item.EspecialidadId == Especialidad && DateTime.Compare(FechaInicial, item.Fecha) < 0).Select(item =>
                    {
                        item.Especialidad = listEspecialidades.Find(e => e.id == item.EspecialidadId).nombre;
                        return item;
                    }).ToList();
                    ListTurnosDisponibles = new ObservableCollection<TurnosDisponibles>(listDisponibilidadO);
                }
                IsVisible = true;
            }
            UserDialogs.Instance.HideLoading();
        }

        public ObservableCollection<TurnosDisponibles> ListTurnosDisponibles { get => listTurnosDisponibles; set { listTurnosDisponibles = value; OnPropertyChanged("ListTurnosDisponibles"); } }
        public DateTime FechaInicial { get => fechaInicial; set { fechaInicial = value; OnPropertyChanged("FechaInicial"); } }
        public int Especialidad { get => especialidad; set { especialidad = value; OnPropertyChanged("Especialidad"); } }

        public int Actividad { get => actividad; set { actividad = value; OnPropertyChanged("Actividad"); } }

        public DateTime MinDate { get; set; } = DateTime.Now;

        public bool IsVisible { get => isVisible; set { isVisible = value; OnPropertyChanged("IsVisible"); } }
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
