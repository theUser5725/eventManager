using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.Entidades
{
    internal class Reunion
    {
        public int IdReunion { get; set; } // Identificador único de la reunión
        public int IdEvento { get; set; }
        public int IdLugar { get; set; }
        public DateTime Horario { get; set; }
        // public DateTime Fecha { get; set; } 
        //public string Nombre { get; set; }
        // public string Descripcion { get; set; }
        // public List<Directiva> Directiva { get; set; }

        // Constructor con directivos opcionales
        public Reunion( int idReunion, int idEvento, int idLugar, DateTime horario)// , string nombre, DateTime fecha, string descripcion )// -> eliminar este mensaje una vez se tenga la entidad directiva, List<Directiva> Directiva = null)
        {
            
            IdReunion = idReunion;
            IdEvento = idEvento;
            IdLugar = idLugar;
            Horario = horario;
            //Fecha = fecha;

            //Nombre = nombre;
            //Descripcion = descripcion;
            // Directiva = Directiva ?? new List<Directiva>();
        }

        // Constructor que solo inicia la clase
        public Reunion()
        {
           // Directivos = new List<string>();
        }
    }
}
