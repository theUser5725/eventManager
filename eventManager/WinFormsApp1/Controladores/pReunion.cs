using System;
using System.Collections.Generic;
using System.Data.SQLite;
using WinFormsApp1.Modelos;

namespace WinFormsApp1.Controladores
{
    public class pReunion
    {

        // Devuelve todas las reuniones de un evento SIN cargar directivos ni lugar
        public static List<Reunion> GetAllByEventoId(int idEvento)

        {
            var reuniones = new List<Reunion>();
            string query = @"
                SELECT 
                    r.idReunion, 
                    r.idEvento, 
                    r.idLugar, 
                    r.nombre, 
                    r.horarioInicio, 
                    r.horarioFinalizacion
                FROM Reuniones r
                WHERE r.idEvento = @idEvento";


            using (var cmd = new SQLiteCommand(query, Conexion.Connection))
            {
                cmd.Parameters.AddWithValue("@idEvento", idEvento);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        reuniones.Add(new Reunion
                        {
                            IdReunion = reader.GetInt32(0),
                            IdEvento = reader.GetInt32(1),
                            IdLugar = reader.GetInt32(2),
                            Nombre = reader.GetString(3),
                            HorarioInicio = reader.GetDateTime(4),
                            HorarioFinalizacion = reader.GetDateTime(5)
                        });
                    }
                }
            }
            return reuniones;
        }

        // Devuelve una reunión por ID, cargando directivos
        public static Reunion? GetById(int idReunion)
        {
            Reunion reunion = null;
            string query = @"
                SELECT 
                    r.idReunion, 
                    r.idEvento, 
                    r.idLugar, 
                    r.nombre, 
                    r.horarioInicio, 
                    r.horarioFinalizacion
                FROM Reuniones r
                WHERE r.idReunion = @idReunion";


            using (var cmd = new SQLiteCommand(query, Conexion.Connection))
            {
                cmd.Parameters.AddWithValue("@idReunion", idReunion);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        reunion = new Reunion
                        {
                            IdReunion = reader.GetInt32(0),
                            IdEvento = reader.GetInt32(1),
                            IdLugar = reader.GetInt32(2),
                            Nombre = reader.GetString(3),
                            HorarioInicio = reader.GetDateTime(4),
                            HorarioFinalizacion = reader.GetDateTime(5)
                        };
                    }
                }
            }

            if (reunion != null)
            {
                // Cargar directivos
                reunion.Directivos = pDirectivo.getAllByReunionId(reunion.IdReunion);
            }

            return reunion;
        }

        // Guarda una nueva reunión y devuelve el Id insertado
        public static int Save(Reunion reunion)
        {
            var cmd = new SQLiteCommand(
                @"INSERT INTO Reuniones (idEvento, idLugar, nombre, horarioInicio, horarioFinalizacion) 
                  VALUES (@idEvento, @idLugar, @nombre, @horarioInicio, @horarioFinalizacion)",
                Conexion.Connection);
            cmd.Parameters.Add(new SQLiteParameter("@idEvento", reunion.IdEvento));
            cmd.Parameters.Add(new SQLiteParameter("@idLugar", reunion.IdLugar));
            cmd.Parameters.Add(new SQLiteParameter("@nombre", reunion.Nombre));
            cmd.Parameters.Add(new SQLiteParameter("@horarioInicio", reunion.HorarioInicio));
            cmd.Parameters.Add(new SQLiteParameter("@horarioFinalizacion", reunion.HorarioFinalizacion));
            cmd.ExecuteNonQuery();

            cmd = new SQLiteCommand("SELECT last_insert_rowid()", Conexion.Connection);
            long lastId = (long)cmd.ExecuteScalar();
            return (int)lastId;
        }

        // Actualiza una reunión existente
        public static void Update(Reunion reunion)
        {
            var cmd = new SQLiteCommand(
                @"UPDATE Reuniones 
                  SET idEvento = @idEvento, 
                      idLugar = @idLugar, 
                      nombre = @nombre, 
                      horarioInicio = @horarioInicio, 
                      horarioFinalizacion = @horarioFinalizacion
                  WHERE idReunion = @idReunion",
                Conexion.Connection);
            cmd.Parameters.Add(new SQLiteParameter("@idReunion", reunion.IdReunion));
            cmd.Parameters.Add(new SQLiteParameter("@idEvento", reunion.IdEvento));
            cmd.Parameters.Add(new SQLiteParameter("@idLugar", reunion.IdLugar));
            cmd.Parameters.Add(new SQLiteParameter("@nombre", reunion.Nombre));
            cmd.Parameters.Add(new SQLiteParameter("@horarioInicio", reunion.HorarioInicio));
            cmd.Parameters.Add(new SQLiteParameter("@horarioFinalizacion", reunion.HorarioFinalizacion));
            cmd.ExecuteNonQuery();
        }

        // Borra una reunión por su instancia
        public static void Delete(Reunion reunion)
        {
            var cmd = new SQLiteCommand(
                "DELETE FROM Reuniones WHERE idReunion = @idReunion",
                Conexion.Connection);
            cmd.Parameters.Add(new SQLiteParameter("@idReunion", reunion.IdReunion));
            cmd.ExecuteNonQuery();
        }
    }
}
