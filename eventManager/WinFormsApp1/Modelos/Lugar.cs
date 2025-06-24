using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.Modelos
{
    public class Lugar
    {
        public int idLugar { get; set; }    
        public string nombre {get; set; }
        public int capacidad { get; set; }
        
        public Lugar(int idlugar, string nombreLugar, int capacidadLugar)
        {
            idLugar = idlugar;
            nombre = nombreLugar;
            capacidad = capacidadLugar;
        }

        public Lugar() { }
    }
}
