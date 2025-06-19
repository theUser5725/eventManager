using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
idReunion
idParticipante
idCatDirectiva
 */
namespace WinFormsApp1.Modelos
{
    internal class Directivos
    {
        public int IdReunion { get; set; }
        public int IdParticipante { get; set; }
        public int IdCatDirectiva { get; set; }

        public Directivos(int idReunion, int idParticipante, int idCatDirectiva) 
        {
            IdReunion = idReunion;
            IdParticipante = idParticipante;
            IdCatDirectiva = idCatDirectiva;
        }

    }
}
