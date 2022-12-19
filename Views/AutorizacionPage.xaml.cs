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
	public partial class AutorizacionPage : ContentPage
	{
        private ValidarAutorizacion validarAutorizacion;
        public AutorizacionPage ()
		{
			InitializeComponent ();
            validarAutorizacion = new ValidarAutorizacion();
            this.BindingContext = validarAutorizacion;
        }
	}
}