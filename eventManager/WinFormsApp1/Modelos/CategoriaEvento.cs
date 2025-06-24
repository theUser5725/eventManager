using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.Modelos
{
    internal class CategoriaEvento
    {
        public int IdCatEvento { get; set; }
        public string Nombre { get; set; }

        public CategoriaEvento(int idCatEvento, string nombre)
        {
            IdCatEvento = idCatEvento;
            Nombre = nombre;
        }
    }
}
