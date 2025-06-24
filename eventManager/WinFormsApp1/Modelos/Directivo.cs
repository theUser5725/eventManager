using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormsApp1.Controladores;
using WinFormsApp1.Modelos;

/*
idReunion
idParticipante
idCatDirectiva
 */
namespace WinFormsApp1.Modelos
{
    internal class Directivo
    {
        public int IdReunion { get; set; }
        public int IdParticipante { get; set; }
        public int IdCatDirectiva { get; set; }
        public Participante Participante { get; set; }
        public Directivo() { }
        public Directivo(int idReunion, int idParticipante, int idCatDirectiva, Participante participante) 
        {
            IdReunion = idReunion;
            IdParticipante = idParticipante;
            IdCatDirectiva = idCatDirectiva;
            Participante = participante;

        }

    }
}
