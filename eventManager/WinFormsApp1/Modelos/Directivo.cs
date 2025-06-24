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
    public class Directivo
    {
        public int IdReunion { get; set; }
        public int IdParticipante { get; set; }
        public int IdCatDirectiva { get; set; }
        public CategoriaDirectiva Categoria { get; set; }
        public Participante Participante { get; set; }

        public Directivo(int idReunion, int idParticipante, int idCatDirectiva, Participante participante, CategoriaDirectiva categoria) 
        {
            IdReunion = idReunion;
            IdParticipante = idParticipante;
            IdCatDirectiva = idCatDirectiva;
            Participante = participante;
            Categoria = categoria;

        }

    }
}
