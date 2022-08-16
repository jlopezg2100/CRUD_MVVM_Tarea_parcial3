using Acr.UserDialogs;
using CRUD_MVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CRUD_MVVM.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListarAlumnosView : ContentPage
    {
        ListarAlumnosViewModel listarAlumnosViewModel;
        public ListarAlumnosView()
        {
            InitializeComponent();
            listarAlumnosViewModel = new ListarAlumnosViewModel();
            BindingContext = listarAlumnosViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            listarAlumnosViewModel.CargarDatos();
           // UserDialogs.Instance.HideLoading();
            //await Task.Delay(500);
        }
    }
}