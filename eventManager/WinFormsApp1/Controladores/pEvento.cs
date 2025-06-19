using System;
using System.Collections.Generic;
using System.Data.SQLite;
using WinFormsApp1.Modelos;
using WinFormsApp1.Controladores;

namespace WinFormsApp1.Controladores
{
    public class pEvento
    {
        // Devuelve todos los eventos SIN cargar participantes ni reuniones
        public List<Evento> GetAll()
        {
            var eventos = new List<Evento>();
            
            using (var cmd = new SQLiteCommand("SELECT idEvento, idCatEvento, nombre, totalHoras FROM Eventos", Conexion.Connection))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    eventos.Add(new Evento
                    {
                        IdEvento = reader.GetInt32(0),
                        IdCatEvento = reader.GetInt32(1),
                        Nombre = reader.GetString(2),
                        TotalHoras = reader.IsDBNull(3) ? null : reader.GetInt32(3)
                    });
                }
            }

            return eventos;
        }

        // Devuelve un evento por ID, cargando participantes y reuniones
        public Evento GetById(int idEvento)
        {
            Evento evento = null;


            // Obtener datos del evento
            using (var cmd = new SQLiteCommand("SELECT idEvento, idCatEvento, nombre, totalHoras FROM Eventos WHERE idEvento = @id", Conexion.Connection))
            {
                cmd.Parameters.AddWithValue("@id", idEvento);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        evento = new Evento
                        {
                            IdEvento = reader.GetInt32(0),
                            IdCatEvento = reader.GetInt32(1),
                            Nombre = reader.GetString(2),
                            TotalHoras = reader.IsDBNull(3) ? null : reader.GetInt32(3)
                        };
                    }
                }
            }

            if (evento == null)
            {

                return null;
            }

            // Cargar participantes
            evento.Participantes = pParticipante.GetAllByEventoId(idEvento);

            // Cargar reuniones
            evento.Reuniones = pReunion.GetAllByEventoId(idEvento);
        

            return evento;
        }


        // Guarda un nuevo evento y devuelve el Id insertado
        public static int Save(Evento evento)
        {
            var cmd = new SQLiteCommand(
                "INSERT INTO Eventos (idCatEvento, nombre, totalHoras) VALUES (@idCatEvento, @nombre, @totalHoras)",
                Conexion.Connection);
            cmd.Parameters.Add(new SQLiteParameter("@idCatEvento", evento.IdCatEvento));
            cmd.Parameters.Add(new SQLiteParameter("@nombre", evento.Nombre));
            cmd.Parameters.Add(new SQLiteParameter("@totalHoras", (object?)evento.TotalHoras ?? DBNull.Value));
            cmd.ExecuteNonQuery();

            // Obtener el ID insertado
            cmd = new SQLiteCommand("SELECT last_insert_rowid()", Conexion.Connection);
            long lastId = (long)cmd.ExecuteScalar();
            return (int)lastId;
        }

        // Actualiza un evento existente
        public static void Update(Evento evento)
        {
            var cmd = new SQLiteCommand(
                "UPDATE Eventos SET idCatEvento = @idCatEvento, nombre = @nombre, totalHoras = @totalHoras WHERE idEvento = @idEvento",
                Conexion.Connection);
            cmd.Parameters.Add(new SQLiteParameter("@idEvento", evento.IdEvento));
            cmd.Parameters.Add(new SQLiteParameter("@idCatEvento", evento.IdCatEvento));
            cmd.Parameters.Add(new SQLiteParameter("@nombre", evento.Nombre));
            cmd.Parameters.Add(new SQLiteParameter("@totalHoras", (object?)evento.TotalHoras ?? DBNull.Value));
            cmd.ExecuteNonQuery();
        }

        // Borra un evento por su instancia
        public static void Delete(Evento evento)
        {
            var cmd = new SQLiteCommand(
                "DELETE FROM Eventos WHERE idEvento = @idEvento",
                Conexion.Connection);
            cmd.Parameters.Add(new SQLiteParameter("@idEvento", evento.IdEvento));
            cmd.ExecuteNonQuery();
        }
    }

}
