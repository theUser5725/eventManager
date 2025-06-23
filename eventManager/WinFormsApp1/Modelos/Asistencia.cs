using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.Modelos
{
    internal class Asistencia
    {
        public int idParticipante { get; set; }   
        public int idReunion { get; set; }

        public int horasAsistido { get; set; }

        public Asistencia(int idAsistencia, int idReunion, int horasAsistido)
        {
            this.idParticipante = idAsistencia;
            this.idReunion = idReunion;
            this.horasAsistido = horasAsistido;
        }

        public Asistencia()
        {
            // Constructor por defecto
        }


    }
}
