using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace CRUD_MVVM.Firebase
{
    public class Conexion
    {
        public static FirebaseClient firebase = new FirebaseClient("https://prueba-8fe11-default-rtdb.firebaseio.com/Alumno");
    }
}
