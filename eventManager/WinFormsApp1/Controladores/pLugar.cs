using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using Org.BouncyCastle.Tls;
using WinFormsApp1.Modelos; // Assuming Lugar is in this namespace
namespace WinFormsApp1.Controladores
{
    internal class pLugar
    {
        public static void Save(Modelos.Lugar lugar)
        {
            SQLiteCommand command = new SQLiteCommand("INSERT INTO Lugares (idLugar, nombre, capacidad) VALUES (@IdLugar, @Nombre, @Capacidad)", Conexion.Connection);
            command.Parameters.AddWithValue("@IdLugar", lugar.idLugar);
            command.Parameters.AddWithValue("@Nombre", lugar.nombre);
            command.Parameters.AddWithValue("@Capacidad", lugar.capacidad);
            command.ExecuteNonQuery();
        }

        public static void Update(Modelos.Lugar lugar)
        {
            SQLiteCommand command = new SQLiteCommand("UPDATE Lugares SET nombre = @Nombre, capacidad = @Capacidad WHERE idLugar = @IdLugar", Conexion.Connection);
            command.Parameters.AddWithValue("@IdLugar", lugar.idLugar);
            command.Parameters.AddWithValue("@Nombre", lugar.nombre);
            command.Parameters.AddWithValue("@Capacidad", lugar.capacidad);
            command.ExecuteNonQuery();
        }

        public static void Delete(int idLugar)
        {
            SQLiteCommand command = new SQLiteCommand("DELETE FROM Lugares WHERE idLugar = @IdLugar", Conexion.Connection);
            command.Parameters.AddWithValue("@IdLugar", idLugar);
            command.ExecuteNonQuery();
        }
        public static Lugar GetById(int idlugar) 
        {
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM Lugares WHERE idLugar = @IdLugar", Conexion.Connection);
            command.Parameters.AddWithValue("@IdLugar", idlugar);

            SQLiteDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                Lugar lugar = new Lugar
                {
                    idLugar = reader.GetInt32(0),
                    nombre = reader.GetString(1),
                    capacidad = reader.GetInt32(2)
                };
                return lugar;
            }
            return null; // or throw an exception if not found
        }

        public static List<Lugar> GetAll()
        {
            List<Lugar> lugares = new List<Lugar>();
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM Lugares", Conexion.Connection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Lugar lugar = new Lugar
                {
                    idLugar = reader.GetInt32(0),
                    nombre = reader.GetString(1),
                    capacidad = reader.GetInt32(2)
                };
                lugares.Add(lugar);
            }
            return lugares;
        }

        public static List<Lugar> GetAllByCapacidad(int capacidad)
        {
            List<Lugar> lugares = new List<Lugar>();
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM Lugares WHERE capacidad >= @Capacidad", Conexion.Connection);
            command.Parameters.AddWithValue("@Capacidad", capacidad);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Lugar lugar = new Lugar
                {
                    idLugar = reader.GetInt32(0),
                    nombre = reader.GetString(1),
                    capacidad = reader.GetInt32(2)
                };
                lugares.Add(lugar);
            }
            return lugares;
        }
        
        public static List<Lugar> GetLugarByEventid(Evento evento)
        {
            if (evento.IdEvento <= 0) 
            {
                return null; // Return an empty list if the event ID is invalid
            }
            else 
            {
                List<Lugar> lugares = pReunion.GetAllByEventoId(evento.IdEvento).Where(r => r.IdLugar > 0).GroupBy(r => r.IdLugar).Select(l => GetById(l.Key)).Distinct().ToList(); // Get all unique places by event ID

                return lugares;
            }
        }
        


    }
}
