using System;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace WinFormsApp1.Modelos
{
    public class Reunion
    {
        public int IdReunion { get; set; }
        public int IdEvento { get; set; }
        public int IdLugar { get; set; }
        
        public string Nombre { get; set; }
        public DateTime HorarioInicio { get; set; }
        public DateTime HorarioFinalizacion { get; set; }
        public Evento Evento { get; set; }
        public Lugar Lugar { get; set; }
        public List<Directivo> Directivos { get; set; } = new();

        public Reunion() { }

        public Reunion(int idReunion, Evento evento, int idLugar, string nombre, DateTime horarioInicio, DateTime horarioFinalizacion, Lugar lugar)
        {
            IdReunion = idReunion;
            Evento = evento;
            IdLugar = idLugar;
            Nombre = nombre;
            HorarioInicio = horarioInicio;
            HorarioFinalizacion = horarioFinalizacion;
            Lugar = lugar;
        }
    }
}
