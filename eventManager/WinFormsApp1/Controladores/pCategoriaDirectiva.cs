using WinFormsApp1.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.Controladores
{
    internal class CategoriaDirectivaControlador
    {
        private List<CategoriaDirectivas> categorias = new List<CategoriaDirectivas>();
        public CategoriaDirectivaControlador()
        {
            categorias.Add(new CategoriaDirectivas { IdCatDirectiva = 1, Nombre = "Presidencia" });
            categorias.Add(new CategoriaDirectivas { IdCatDirectiva = 2, Nombre = "Secretaría" });
        }
        public CategoriaDirectivas GetById(int id)
        {
            return categorias.FirstOrDefault(c => c.IdCatDirectiva == id);
        }
        public List<CategoriaDirectivas> GetAll()
        {
            return categorias;
        }
    }
}

