using Acr.UserDialogs;
using CRUD_MVVM.Models;
using CRUD_MVVM.Services;
using CRUD_MVVM.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace CRUD_MVVM.ViewModels
{
    public class ListarAlumnosViewModel : BaseViewModels
    {

        private List<Alumno> _ListaPersonas;
        PersonaServices personaServices;

        public List<Alumno> ListaPersonas
        {
            get { return _ListaPersonas; }
            set { 
                _ListaPersonas = value; 
                OnPropertyChanged(); 
            }
        }

        public ListarAlumnosViewModel() {
            personaServices = new PersonaServices();

            EditarPersonaCommand = new Command<Alumno>(async (Persona) => await EditarPersona(Persona));
            EliminarPersonaCommand = new Command<Alumno>(async (Persona) => await EliminarPersona(Persona));
        }

        private async Task EliminarPersona(Alumno persona)
        {
            bool confirm = await Application.Current.MainPage.DisplayAlert("Advertencia", "¿Esta seguro de eliminar a " + persona.Nombre + "?", "Si", "No");

            if (confirm)
            {
                bool response = await personaServices.DeletePerson(persona.Key);
                if (response)
                {
                    await Application.Current.MainPage.DisplayAlert("Exito", "Empleado Eliminado Exitosamente", "Ok");
                    CargarDatos();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Se produjo un error al eliminar al empleado", "Ok");
                }
            }
        }

        private async Task EditarPersona(Alumno persona)
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new AgregarAlumnoView("Editar", persona));
        }

        public async void CargarDatos()
        {
            ListaPersonas = await personaServices.ListarPersonas();
            if (ListaPersonas.Count == 0)
            {
              // UserDialogs.Instance.ShowLoading("Cargando Cartelera", MaskType.Clear);
                await Application.Current.MainPage.DisplayAlert("Advertencia", "No hay empleados en la lista", "Ok");
               // UserDialogs.Instance.HideLoading();
               // await Task.Delay(500);
            }
        }

        public ICommand EditarPersonaCommand { get; set; }
        public ICommand EliminarPersonaCommand { get; set; }
    }
}
