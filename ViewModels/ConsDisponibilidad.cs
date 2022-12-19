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
        private DateTime fechaInicial = DateTime.Now;
        private int especialidad;
        private int profecional;
        private bool isVisible;
        private bool isVisibleProfecional;
        public ICommand ConsultarDisponibilidad { get; set; }
        public ICommand ConsultarProfecionales { get; set; }

        HomilClient homilClient = new HomilClient();

        private ObservableCollection<TurnosDisponibles> listTurnosDisponibles { get; set; }

        private ObservableCollection<EspecialidadItem> listEspecialidades { get; set; }

        private ObservableCollection<ProfecionalItem> listProfecionales { get; set; }


        public ConsDisponibilidad()
        {
            IsVisible = false;
            cargarListaEsp();
            ConsultarDisponibilidad = new Command(async () => await consultarDisponibilidad());
            ConsultarProfecionales = new Command(async () => await consultarProfecional());
        }

        private async Task consultarProfecional()
        {
            IsVisibleProfecional = false;
            UserDialogs.Instance.ShowLoading("Consultando...");
            if (FechaInicial == null || Especialidad == 0 )
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Campos no validos por favor diligenciar valores correctos.", "Acceptar");
                IsVisibleProfecional = false;
            }
            else
            {
                try
                {
                    var paramsQuery = $"FechaDesde={FechaInicial.ToString("yyyy-MM-dd")}&FechaHasta={FechaInicial.AddDays(30).ToString("yyyy-MM-dd")}&OIDEspecialidad={Especialidad}";
                    ResponseHomil<Profecional> responseListProfecionales = await homilClient.executeRequestGet<ResponseHomil<Profecional>>("/Citas/EHR/ListarDisponibilidadProfesional", paramsQuery, 1);
                    if (responseListProfecionales.sucess)
                    {
                        ListProfecionales = new ObservableCollection<ProfecionalItem>(responseListProfecionales.result.profesional);
                        IsVisibleProfecional = true;
                        if (ListProfecionales.Count() == 0)
                        {
                            await Application.Current.MainPage.DisplayAlert("Informacion", "No hay disponibilidad para esta especialidad.", "Acceptar");
                            IsVisibleProfecional = false;
                        }
                        
                    }
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Algo ocurrio.", "Acceptar");
                    IsVisibleProfecional = false;
                }
                
            }
            UserDialogs.Instance.HideLoading();
        }
        private async Task consultarDisponibilidad()
        {
            UserDialogs.Instance.ShowLoading("Consultando...");
            if (FechaInicial == null || Especialidad == 0 || Profecional == 0)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Campos no validos por favor diligenciar valores correctos.", "Acceptar");
                IsVisible = false;
            }
            else
            {
                try
                {
                    var paramsQuery = $"FechaDesde={FechaInicial.ToString("yyyy-MM-dd")}&FechaHasta={FechaInicial.AddDays(30).ToString("yyyy-MM-dd")}&OIDProfesional={Profecional}";
                    ResponseHomil<Disponibilidad> responseListTurnosDisponibles = await homilClient.executeRequestGet<ResponseHomil<Disponibilidad>>("/Citas/EHR/ListarDisponibilidadFecha", paramsQuery, 1);
                    if (responseListTurnosDisponibles.sucess)
                    {
                        ListTurnosDisponibles = new ObservableCollection<TurnosDisponibles>(responseListTurnosDisponibles.result.citafecha);
                        ListTurnosDisponibles = new ObservableCollection<TurnosDisponibles>(ListTurnosDisponibles.Select(item =>
                        {
                            item.EspecialidadId = Especialidad;
                            item.MedicoId = Profecional;
                            item.Especialidad = ListEspecialidades.Where(v => v.oid == Especialidad).First().geeDescri;
                            item.Medico = ListProfecionales.Where(v => v.oid == Profecional).First().gmenomcom;
                            return item;
                        }).OrderByDescending(item => item.fechaInicial).ToList());
                        IsVisible = true;
                        if (ListTurnosDisponibles.Count() == 0)
                        {
                            await Application.Current.MainPage.DisplayAlert("Informacion", "No hay disponibilidad para esta especialidad.", "Acceptar");
                            IsVisible = false;
                        }
                    }
                    
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Algo ocurrio.", "Acceptar");
                    IsVisible = false;
                }
                
            }
            UserDialogs.Instance.HideLoading();
        }

        private async Task cargarListaEsp()
        {
            ResponseHomil<Especialidad> responseListESp = await homilClient.executeRequestGet<ResponseHomil<Especialidad>>("/Citas/EHR/ListarEspecialidades", "" , 1);
            if (responseListESp.sucess)
            {
                ListEspecialidades = new ObservableCollection<EspecialidadItem>(responseListESp.result.especialidad);
            }
        }

        public ObservableCollection<ProfecionalItem> ListProfecionales { get => listProfecionales; set { listProfecionales = value; OnPropertyChanged("ListProfecionales"); } }
        public ObservableCollection<EspecialidadItem> ListEspecialidades { get => listEspecialidades; set { listEspecialidades = value; OnPropertyChanged("ListEspecialidades"); } }
        public ObservableCollection<TurnosDisponibles> ListTurnosDisponibles { get => listTurnosDisponibles; set { listTurnosDisponibles = value; OnPropertyChanged("ListTurnosDisponibles"); } }
        public DateTime FechaInicial { get => fechaInicial; set { fechaInicial = value; OnPropertyChanged("FechaInicial"); } } 
        public int Especialidad { get => especialidad; set { especialidad = value; OnPropertyChanged("Especialidad"); } }

        public int Profecional { get => profecional; set { profecional = value; OnPropertyChanged("Profecional"); } }

        public DateTime MinDate { get; set; } = DateTime.Now;

        public bool IsVisible { get => isVisible; set { isVisible = value; OnPropertyChanged("IsVisible"); } }

        public bool IsVisibleProfecional { get => isVisibleProfecional; set { isVisibleProfecional = value; OnPropertyChanged("IsVisibleProfecional"); } }
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
