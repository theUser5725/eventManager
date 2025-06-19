using WinFormsApp1.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.Controladores
{
    internal class pLugares
    {
        private List<Lugar> lugares = new List<Lugar>();

        public Lugar GetById(int id)
        {
            return lugares.FirstOrDefault(l => l.IdLugar == id);
        }
        public List<Lugar> GetAll()
        {
            return lugares;
        }
        public List<Lugar> GetAllByCapacidad(int capacidad)
        {
            return lugares.Where(l => l.Capacidad >= capacidad).ToList();
        }
    }
}

