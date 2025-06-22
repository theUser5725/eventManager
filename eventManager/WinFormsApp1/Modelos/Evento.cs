using System.Collections.Generic;
using tp4_Prueba.Modelos;
using WinFormsApp1.Modelos;

namespace WinFormsApp1.Modelos
{
    public class Evento
    {
        public int IdEvento { get; set; }
        public int IdCatEvento { get; set; }
        public string Nombre { get; set; }
        public int? TotalHoras { get; set; }
        public int? CantidadParticipantes { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFinalizacion { get; set; }
        public int Estado { get; set; } // 0 = no iniciado, 1 = en curso, 2 = finalizado, 3 = cancelado

        //public CategoriaEvento Categoria { get; set; }
        public List<Participante> Participantes { get; set; } = new();
        public List<Reunion> Reuniones { get; set; } = new();
    }

}

