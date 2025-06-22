using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
idParticipante
nombre
apellido
mail
dni
contraseña
*/
namespace tp4_Prueba.Modelos
{
    public class Participante
    {
        public int IdParticipante { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Mail { get; set; }
        public string Dni { get; set; }
        public string Contraseña { get; set; }
        
        public Participante() 
        { 
        }


        public Participante(int idParticipante, string nombre, string apellido, string mail, string dni, string contraseña)
        {
            IdParticipante = idParticipante;
            Nombre = nombre;
            Apellido = apellido;
            Mail = mail;
            Dni = dni;
            Contraseña = contraseña;
        }
    }
}
