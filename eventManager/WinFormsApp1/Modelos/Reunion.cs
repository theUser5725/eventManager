using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1.Modelos;

public class Reunion
{
    public int IdReunion { get; set; } // Identificador único de la reunión
    public int IdEvento { get; set; }
    public int IdLugar { get; set; }
    public DateTime Horario { get; set; }
    // public DateTime Fecha { get; set; } 
    //public string Nombre { get; set; }
    // public string Descripcion { get; set; }
    // public List<Directiva> Directiva { get; set; }

    // Constructor con directivos opcionales
    public Reunion( int idReunion, int idEvento, int idLugar, DateTime horario)// , string nombre, DateTime fecha, string descripcion )// -> eliminar este mensaje una vez se tenga la entidad directiva, List<Directiva> Directiva = null)
    {
        
        IdReunion = idReunion;
        IdEvento = idEvento;
        IdLugar = idLugar;
        Horario = horario;

        //Fecha = fecha;

        //Nombre = nombre;
        //Descripcion = descripcion;
        // Directiva = Directiva ?? new List<Directiva>();
    }

    // Constructor que solo inicia la clase
    public Reunion()
    {
       // Directivos = new List<string>();
    }
}

/*
 * hay que controlar como se va a atrabajar con el horario de las reuniones, ya que en la base de datos se guarda como un string, y no como un DateTime, por lo que hay que parsear el string a DateTime, y luego poder trabajar con el horario de la reunión.
 * este funcion fue creada por una ia, se debe cambiar pero sirve como ejemplo de como se puede parsear el horario de una reunión desde un string a un DateTime, y luego poder trabajar con el horario de la reunión.
 * 
 * en especifico se deben crear tres funciones, 
 * 1- una para parsear el horario de la reunión desde un string a un DateTime, 
 * 2- otra para parsear el horario de la reunión desde un string a un TimeSpan
 * 3- otra para validar que el horario de la reunión sea valido.
 * 
 * 
    public static (DateTime inicio, DateTime fin) ParsearHorarioReunion(string horarioString)
    {
        // Dividir la cadena en fecha y rangos de hora
        var partes = horarioString.Split(new[] { ' ', '-' }, StringSplitOptions.RemoveEmptyEntries);
        
        if (partes.Length != 3)
            throw new FormatException("Formato de horario incorrecto. Se esperaba: 'yyyy-MM-dd HH:mm-HH:mm'");

        // Parsear la fecha base
        DateTime fechaBase;
        if (!DateTime.TryParseExact(partes[0], "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out fechaBase))
            throw new FormatException("Formato de fecha incorrecto. Se esperaba: 'yyyy-MM-dd'");

        // Parsear horas
        TimeSpan horaInicio = ParseHoraMinuto(partes[1]);
        TimeSpan horaFin = ParseHoraMinuto(partes[2]);

        // Combinar fecha con horas
        DateTime inicio = fechaBase.Add(horaInicio);
        DateTime fin = fechaBase.Add(horaFin);

        // Validar que la hora de fin sea después del inicio
        if (fin <= inicio)
            throw new ArgumentException("La hora de finalización debe ser posterior a la hora de inicio");

        return (inicio, fin);
    }

    private static TimeSpan ParseHoraMinuto(string tiempo)
    {
        var partesHora = tiempo.Split(':');
        if (partesHora.Length != 2 || !int.TryParse(partesHora[0], out int horas) || !int.TryParse(partesHora[1], out int minutos))
            throw new FormatException($"Formato de hora incorrecto en: '{tiempo}'. Se esperaba HH:mm");

        if (horas < 0 || horas > 23 || minutos < 0 || minutos > 59)
            throw new ArgumentOutOfRangeException($"Hora no válida: {tiempo}");

        return new TimeSpan(horas, minutos, 0);
    }

_______________________________________ejemplo de uso______________________________________________
string horarioDb = "2025-06-20 14:15-18:00";
        
        try
        {
            var (inicio, fin) = ParsearHorarioReunion(horarioDb);
            
            Console.WriteLine($"Reunión programada para:");
            Console.WriteLine($"Inicio: {inicio:yyyy-MM-dd HH:mm}");
            Console.WriteLine($"Fin:    {fin:yyyy-MM-dd HH:mm}");
            Console.WriteLine($"Duración: {(fin - inicio).TotalHours} horas");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al procesar horario: {ex.Message}");
        }
*/
