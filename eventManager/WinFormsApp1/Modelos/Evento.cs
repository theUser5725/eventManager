using System.Collections.Generic;
using WinFormsApp1.Modelos;

namespace WinFormsApp1.Models
{
    public class Evento
    {
        public int IdEvento { get; set; }
        public int IdCatEvento { get; set; }
        public string Nombre { get; set; }
        public int? TotalHoras { get; set; }

        public CategoriaEvento Categoria { get; set; }

        public List<Participante> Participantes { get; set; } = new();

        public List<Reunion> Reuniones { get; set; } = new();
    }

}

