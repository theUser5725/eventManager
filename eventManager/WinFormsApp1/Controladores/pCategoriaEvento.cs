using WinFormsApp1.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.Controladores
{
    internal class CategoriaEventoControlador
    {
        private List<CategoriaEvento> categorias = new List<CategoriaEvento>();
        public CategoriaEventoControlador()
        {
            categorias.Add(new CategoriaEvento { IdCatEvento = 1, Nombre = "Conferencia" });
            categorias.Add(new CategoriaEvento { IdCatEvento = 2, Nombre = "Taller" });
        }
        public CategoriaEvento GetById(int id)
        {
            return categorias.FirstOrDefault(c => c.IdCatEvento == id);
        }
        public List<CategoriaEvento> GetAll()
        {
            return categorias;
        }
    }
}

