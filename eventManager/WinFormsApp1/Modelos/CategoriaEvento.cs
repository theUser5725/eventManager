using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.Modelos
{


    public class CategoriaEvento
    {
        public int IdCategoriaEvento { get; set; }
        public string Nombre { get; set; }

        public CategoriaEvento(int idCategoriaEvento, string nombre)
        {
            IdCategoriaEvento = idCategoriaEvento;
            Nombre = nombre;
        }
        public CategoriaEvento() { }


    }
}
