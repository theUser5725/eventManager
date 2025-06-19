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
    internal class Participantes
    {
        public int IdParticipante { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Mail { get; set; }
        public int Dni { get; set; }
        public int Contraseña { get; set; }

        public Participantes(int idParticipante, string nombre, string apellido, string mail, int dni, int contraseña)
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
