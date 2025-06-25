using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.Modelos
{
    internal class CategoriaDirectivas
    {
        public int IdCatDirectiva { get; set; }
        public string Nombre { get; set; }
        public CategoriaDirectivas(int idCatDirectiva, string nombre)
        {
            IdCatDirectiva = idCatDirectiva;
            Nombre = nombre;
        }
    }
}
