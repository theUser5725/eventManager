using proyectoFinal.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proyectoFinal.Controladores
{
    internal class LugarControlador
    {
        private List<Lugar> lugares = new List<Lugar>();
        public LugarControlador()
        {
            // Podés agregar datos de prueba si querés
            lugares.Add(new Lugar { IdLugar = 1, Nombre = "Salón A", Capacidad = 100 });
            lugares.Add(new Lugar { IdLugar = 2, Nombre = "Salón B", Capacidad = 60 });
            lugares.Add(new Lugar { IdLugar = 3, Nombre = "Salón C", Capacidad = 80 });
        }
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

