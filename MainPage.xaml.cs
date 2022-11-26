using HomilApp.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HomilApp
{
    public partial class MainPage : Shell
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void Logout(object sender, EventArgs e)
        {
            SecureStorage.RemoveAll();
            Application.Current.MainPage = new MainPage();
            //await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
    }
}
