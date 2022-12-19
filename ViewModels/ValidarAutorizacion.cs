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
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HomilApp.ViewModels
{
    internal class ValidarAutorizacion : INotifyPropertyChanged
    {
        private string codAutorizacion;

        private string msjError;
        private AutorizacionValida autorizacion { get; set; }

        private bool isVisible;
        public ICommand ConsultarAutorizacion { get; set; }

        HomilClient homilClient = new HomilClient();


        public ValidarAutorizacion()
        {
            IsVisible = false;
            ConsultarAutorizacion = new Command(async () => await consultarAutorizacion());
        }

        private async Task consultarAutorizacion()
        {
            UserDialogs.Instance.ShowLoading("Consultando...");
            if (CodAutorizacion == null || CodAutorizacion == "")
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Campos no validos por favor diligenciar valores correctos.", "Acceptar");
                IsVisible = false;
            }
            else
            {
                try
                {
                    var pacienteJson = await SecureStorage.GetAsync("paciente");
                    var paciente = JsonConvert.DeserializeObject<Paciente>(pacienteJson);
                    var paramsQuery = $"autorizacion={CodAutorizacion}&documento={paciente.Identificacion}&tipoDocumento={paciente.TipoIdentificacion}";
                    AutorizacionValida responseVerificarAutorizacion = await homilClient.executeRequestGet<AutorizacionValida>("/ApiSis/Autorizaciones/verificarAutorizacion", paramsQuery);
                    Autorizacion = responseVerificarAutorizacion;
                    IsVisible = true;
                    if (Autorizacion == null)
                    {
                        await Application.Current.MainPage.DisplayAlert("Informacion", "Autorizacion no valida.", "Acceptar");
                        IsVisible = false;
                    }
                   
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Autorizacion no valida.", "Acceptar");
                    IsVisible = false;
                }
            }
            UserDialogs.Instance.HideLoading();
        }
        public string CodAutorizacion { get => codAutorizacion; set { codAutorizacion = value; OnPropertyChanged("CodAutorizacion"); } }

        public AutorizacionValida Autorizacion { get => autorizacion; set { autorizacion = value; OnPropertyChanged("Autorizacion"); } }
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
