using CRUD_MVVM.Models;
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
    public partial class AgregarAlumnoView : ContentPage
    {
        public AgregarAlumnoView(string opcion = "Guardar", Alumno persona = null)
        {
            InitializeComponent();
            if (opcion.Equals("Guardar"))
            {
                BindingContext = new AgregarAlumnoViewModel(imagePersona, opcion, persona);
            }
            else
            {
                BindingContext = new AgregarAlumnoViewModel(imagePersona2, opcion, persona);
            }
        }
    }
}