using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.Modelos
{
    public class CategoriaDirectiva
    {
        public int IdCategoriaDirectivo { get; set; }
        public string Nombre { get; set; }
        public CategoriaDirectiva(int idCategoriaDirectivo, string nombre)
        {
            IdCategoriaDirectivo = idCategoriaDirectivo;
            Nombre = nombre;
        }
        public CategoriaDirectiva() { }

    }
}
