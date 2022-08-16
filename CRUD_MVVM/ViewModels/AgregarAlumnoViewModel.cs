using CRUD_MVVM.Firebase;
using CRUD_MVVM.Models;
using CRUD_MVVM.Services;
using CRUD_MVVM.Views;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CRUD_MVVM.ViewModels
{
    public class AgregarAlumnoViewModel : BaseViewModels
    {

        #region VARIABLES

        private string _Nombre;
        private string _Apellidos;
        private string _Edad;
        private string _Direccion;
        private string _Puesto;
        private string _Foto;
        Image imagenPersona;
        PersonaServices services;
        private string opcion;
        private string key;
        private bool _IsImageDefault;
        private bool _IsImageEdit;
        #endregion

        #region OBJETOS
        public string Nombre
        {
            get { return _Nombre; }
            set
            {
                _Nombre = value;
                OnPropertyChanged();
            }
        }

        public string Apellidos
        {
            get { return _Apellidos; }
            set
            {
                _Apellidos = value;
                OnPropertyChanged();
            }
        }

        public string Edad
        {
            get { return _Edad; }
            set
            {
                _Edad = value;
                OnPropertyChanged();
            }
        }

        public string Direccion
        {
            get { return _Direccion; }
            set
            {
                _Direccion = value;
                OnPropertyChanged();
            }
        }

        public string Puesto
        {
            get { return _Puesto; }
            set
            {
                _Puesto = value;
                OnPropertyChanged();
            }
        }

        public string Foto
        {
            get { return _Foto; }
            set
            {
                _Foto = value;
                OnPropertyChanged();
            }
        }

        public bool IsImageDefault
        {
            get { return _IsImageDefault; }
            set
            {
                _IsImageDefault = value;
                OnPropertyChanged();
            }
        }

        public bool IsImageEdit
        {
            get { return _IsImageEdit; }
            set
            {
                _IsImageEdit = value;
                OnPropertyChanged();
            }
        }


        #endregion


        public AgregarAlumnoViewModel(Image imageParam, string opcionReceived, Alumno personaReceived)
        {
            imagenPersona = imageParam;
            services = new PersonaServices();
            opcion = opcionReceived;

            if (opcion.Equals("Editar"))
            {
                CargarParaEditar(personaReceived);
                IsImageDefault = false;
                IsImageEdit = true;
            }
            else
            {
                IsImageDefault = true;
                IsImageEdit = false;
            }

            TomarFotoCommand = new Command(async () => await TomarFoto());
            GuardarCommand = new Command(() => GuardarPersona());
            ListarCommand = new Command(() => ListarPersonas());
        }

        private void CargarParaEditar(Alumno personaReceived)
        {
            Nombre = personaReceived.Nombre;
            Apellidos = personaReceived.Apellidos;
            Edad = personaReceived.Edad;
            Direccion = personaReceived.Direccion;
            Puesto = personaReceived.Puesto;
            Foto = personaReceived.Foto;
            key = personaReceived.Key;
        }

        private async void GuardarPersona()
        {
            string response = ValidarCampos();
            if (!response.Equals("OK"))
            {
                await Application.Current.MainPage.DisplayAlert("Advertencia", response, "Ok");
                return;
            }

            Alumno persona = new Alumno()
            {
                Nombre = Nombre,
                Apellidos = Apellidos,
                Edad = Edad,
                Direccion = Direccion,
                Puesto = Puesto,
                Foto = Foto
            };

            if (opcion.Equals("Editar"))
            {
                // EDITAR
                bool confirm = await services.UpdatePerson(persona, key);
                if (confirm)
                {
                    await Application.Current.MainPage.DisplayAlert("Exito", "Datos actualizados Exitosamente.", "Ok");
                    await Application.Current.MainPage.Navigation.PopModalAsync();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Se produjo un error al actualizar los Datos.", "Ok");
                }
            }
            else
            {
                // GUARDAR
                bool confirm = await services.InsertarPersona(persona);
                if (confirm)
                {
                    await Application.Current.MainPage.DisplayAlert("Exito", "Empleado registrado Exitosamente.", "Ok");
                    Limpiar();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Se produjo un error al registrar los datos.", "Ok");
                }
            }
            
        }

        private void Limpiar()
        {
            Nombre = "";
            Apellidos = "";
            Edad = "";
            Direccion = "";
            Puesto = "";
            Foto = "";
            imagenPersona.Source = "avatares";
        }

        private string ValidarCampos()
        {
            if (string.IsNullOrEmpty(Nombre))
            {
                return "Debes ingresar el nombre";
            }
            else if (!ValidateOnlyString(Nombre))
            {
                return "Favor solo ingresar letras en su nombre.";
            }
            else if (string.IsNullOrEmpty(Apellidos))
            {
                return "Debes ingresar tus apellidos";
            }
            else if (!ValidateOnlyString(Apellidos))
            {
                return "Favor solo ingresar letras en tu apellido.";
            }
            else if (string.IsNullOrEmpty(Edad))
            {
                return "Debes ingresar la edad";
            }
            else if (!ValidateOnlyNumber(Edad))
            {
                return "Favor ingresar solo numeros en tu edad";
            }
            else if (string.IsNullOrEmpty(Direccion))
            {
                return "Debes ingresar la direccion";
            }
            else if (string.IsNullOrEmpty(Puesto))
            {
                return "Debes ingresar el puesto";
            }
            else if (string.IsNullOrEmpty(Foto))
            {
                return "Debes tomar o agregar la fotografia";
            }

            return "OK";
        }

        public static bool ValidateOnlyString(string text)
        {
            return Regex.IsMatch(text, @"^[a-zA-ZáéíóúÁÉÍÓÚ ]+$");
        }

        public static bool ValidateOnlyNumber(string text)
        {
            return Regex.IsMatch(text, @"^[0-9]*$");
        }

        private async void ListarPersonas()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new ListarAlumnosView());
        }

        private async Task TomarFoto()
        {
            bool opcion = await Application.Current.MainPage.DisplayAlert("Aviso", "Seleccione la opcion de su preferencia", "Galeria", "Camara");

            if (opcion)
                GetImageFromGallery();
            else
                GetImageFromCamera();
        }

        private async void GetImageFromGallery()
        {
            try
            {
                if (CrossMedia.Current.IsPickPhotoSupported)
                {
                    var file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                    {
                        PhotoSize = PhotoSize.Medium,
                    });
                    
                    if (file == null)
                        return;

                    imagenPersona.Source = ImageSource.FromStream(() => { return file.GetStream(); });
                    byte[] byteArray = File.ReadAllBytes(file.Path);
                    Foto = System.Convert.ToBase64String(byteArray);
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Advertencia", "Se produjo un error al seleccionar la imagen.", "Ok");
                }
            }
            catch (Exception)
            {
                await Application.Current.MainPage.DisplayAlert("Advertencia", "Se produjo un error al seleccionar la imagen.", "Ok");
            }

        }

        private async void GetImageFromCamera()
        {
            try
            {
                var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    PhotoSize = PhotoSize.Medium,
                });

                if (file == null)
                    return;

                imagenPersona.Source = ImageSource.FromStream(() => { return file.GetStream(); });
                byte[] byteArray = File.ReadAllBytes(file.Path);
                Foto = System.Convert.ToBase64String(byteArray);
            }
            catch (Exception)
            {
                await Application.Current.MainPage.DisplayAlert("Advertencia", "Se produjo un error al tomar la fotografia.", "Ok");
            }
        }

        public ICommand TomarFotoCommand { get; set; }
        public ICommand GuardarCommand { get; set; }
        public ICommand ListarCommand { get; set; }
    }
}
