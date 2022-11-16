using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DgBooks.Models
{
    public class newUser
    {
        public int Id { get; set; }

        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Ingresa el formato solicitado")]
        public string strNombrePersona { get; set; }

        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Ingresa el formato solicitado")]
        public string strApellidoP { get; set; }

        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Ingresa el formato solicitado")]
        public string strApellidoM { get; set; }
        public System.DateTime dtFechaNacimiento { get; set; }
        public string strNombreUsuario { get; set; }
        public string strCorreo { get; set; }
        public string strContraseña { get; set; }
        public string strConfirmContraseña { get; set; }
    }
}