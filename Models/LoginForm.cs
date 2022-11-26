using HomilApp.Service;
using HomilApp.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HomilApp.Models
{
    internal class LoginForm: INotifyPropertyChanged
    {
        private string userName;
        private string password;
        public ICommand AutenticateUser { get; set; }

        HomilClient homilClient = new HomilClient();

        public List<Paciente> listPacientes = new List<Paciente>() {
                new Paciente() { Identificacion = "79575558", TipoIdentificacion = "CC" , PrimerNombre="Paciente" ,PrimerApellido="Prueba" , Password= "79575558"},
        };
        public LoginForm()
        {
            AutenticateUser = new Command(async () => await autenticateUser());
        }
        private async Task autenticateUser()
        {
            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password)) {
                await Application.Current.MainPage.DisplayAlert("Error","Usuario o contraseña no validos.","Acceptar");
            }
            else
            {
                var json = JsonConvert.SerializeObject(new { userName = "apphomil", password = "AppHomil123*" });
                responseAuth response = await homilClient.executeRequestPost<responseAuth>("/Auth/createToken", json);
                if (listPacientes.Where(item => item.Identificacion == UserName && item.Password == Password).Count() > 0)
                {
                    bool valido = await validateUser(listPacientes.First(item => item.Identificacion == UserName && item.Password == Password));
                    if (!valido) {
                        await Application.Current.MainPage.DisplayAlert("Error", "No es un paciente valido.", "Acceptar");
                    }
                    else
                    {
                        await Shell.Current.GoToAsync($"//{nameof(PrincipalPage)}");
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Usuario o contraseña incorrecta.", "Acceptar");
                }
            }
        }
        public string UserName { get => userName; set  { userName = value; OnPropertyChanged("Username"); } }
        public string Password { get => password; set  { password = value; OnPropertyChanged("Password"); } }


        public event PropertyChangedEventHandler PropertyChanged;

        public INavigation Navigation { get; set; }

        private void OnPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async Task<bool> validateUser(Paciente paciente)
        {
            var json = JsonConvert.SerializeObject(new { userName = "apphomil", password = "AppHomil123*" });
            responseAuth response = await homilClient.executeRequestPost<responseAuth>("/Auth/createToken", json);
            if (response.statusCode == 200)
            {
                await SecureStorage.SetAsync("AccessToken", response.value);
                responseValidPaciente responseValPacSaludsis = await homilClient.executeRequestGet<responseValidPaciente>("/ApiSis/Autorizaciones/verificarPaciente", $"documento={paciente.Identificacion}&tipoDocumento={paciente.TipoIdentificacion}");
                if ((responseValPacSaludsis != null && responseValPacSaludsis.PrimerNombre == null) || responseValPacSaludsis == null)
                {
                    return false;
                }
                responseValidHomil responseValPacHomil = await homilClient.executeRequestGet<responseValidHomil>("/HospitalMilitar/CitasMedicas/validarPaciente", $"documento={paciente.Identificacion}&tipoDocumento={paciente.TipoIdentificacion}");
                if ((responseValPacHomil != null && responseValPacHomil.estado == 0) || responseValPacHomil == null)
                {
                    return false;
                }
                paciente.PacienteId = responseValPacHomil.respuesta;
                await SecureStorage.SetAsync("paciente", JsonConvert.SerializeObject(paciente));
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    internal class responseAuth
    {
        public string  value { get; set; }
        public int statusCode { get; set; }
        public string contentType { get; set; }
    }
    internal class responseValidPaciente
    {
        public string PrimerNombre { get; set; }
        public string SegundoNombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public string Estado { get; set; }
    }
    internal class responseValidHomil
    {
        public int estado { get; set; }
        public string respuesta { get; set; }
    }
}
