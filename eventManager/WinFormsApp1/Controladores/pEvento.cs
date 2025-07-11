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
        public static List<Evento> GetAll()
        {
            var eventos = new List<Evento>();
            string query = @"
                SELECT 
                    e.idEvento, 
                    e.idCatEvento, 
                    e.nombre, 
                    e.totalHoras,
                    e.fechaInicio,
                    e.fechaFinalizacion,
                    e.estado,
                    (SELECT COUNT(*) FROM Inscripciones i WHERE i.idEvento = e.idEvento) as cantidadParticipantes
                FROM Eventos e";

            using (var cmd = new SQLiteCommand(query, Conexion.Connection))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    eventos.Add(new Evento
                    {
                        IdEvento = reader.GetInt32(0),
                        IdCatEvento = reader.GetInt32(1),
                        Nombre = reader.GetString(2),
                        TotalHoras = reader.IsDBNull(3) ? null : reader.GetInt32(3),
                        FechaInicio = reader.IsDBNull(4) ? null : reader.GetDateTime(4).Date,
                        FechaFinalizacion = reader.IsDBNull(5) ? null : reader.GetDateTime(5).Date,
                        Estado = reader.GetInt32(6),
                        CantidadParticipantes = reader.IsDBNull(7) ? 0 : reader.GetInt32(7),
                        Categoria = pCategoriaEvento.GetById(reader.GetInt32(1))
                    });
                }
            }

            return eventos;
        }

        // Devuelve un evento por ID, cargando participantes y reuniones
        public static Evento? GetById(int idEvento)
        {
            Evento evento = new Evento();

            using (var cmd = new SQLiteCommand(
                @"SELECT 
                    e.idEvento, 
                    e.idCatEvento, 
                    e.nombre, 
                    e.totalHoras,
                    e.fechaInicio,
                    e.fechaFinalizacion,
                    e.estado,
                    (SELECT COUNT(*) FROM Inscripciones i WHERE i.idEvento = e.idEvento) as cantidadParticipantes
                  FROM Eventos e
                  WHERE e.idEvento = @id", Conexion.Connection))
            {
                cmd.Parameters.AddWithValue("@id", idEvento);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        {
                            evento.IdEvento = reader.GetInt32(0);
                            evento.IdCatEvento = reader.GetInt32(1);
                            evento.Nombre = reader.GetString(2);
                            evento.TotalHoras = reader.IsDBNull(3) ? null : reader.GetInt32(3);
                            evento.FechaInicio = reader.IsDBNull(4) ? null : reader.GetDateTime(4);
                            evento.FechaFinalizacion = reader.IsDBNull(5) ? null : reader.GetDateTime(5);
                            evento.Estado = reader.GetInt32(6);
                            evento.CantidadParticipantes = reader.IsDBNull(7) ? 0 : reader.GetInt32(7);
                            evento.Categoria = pCategoriaEvento.GetById(evento.IdCatEvento);
                        }
                        ;
                    }
                }
            }

            if (evento == null)
            {
                return null;

            }
            else
            {

                // Cargar participantes
                evento.Participantes = pParticipante.getAllByEventoId(idEvento);

                evento.Reuniones = pReunion.GetAllByEventoId(idEvento);

                return evento;
            }
        }

            
        
        // Guarda un nuevo evento y devuelve el Id insertado
        public static int Save(Evento evento)
        {
            var cmd = new SQLiteCommand(
                @"INSERT INTO Eventos (idCatEvento, nombre, totalHoras, fechaInicio, fechaFinalizacion, estado) 
                  VALUES (@idCatEvento, @nombre, @totalHoras, @fechaInicio, @fechaFinalizacion, @estado)",
                Conexion.Connection);
            cmd.Parameters.Add(new SQLiteParameter("@idCatEvento", evento.Categoria.IdCategoriaEvento));
            cmd.Parameters.Add(new SQLiteParameter("@nombre", evento.Nombre));
            cmd.Parameters.Add(new SQLiteParameter("@totalHoras", (object?)evento.TotalHoras ?? DBNull.Value));
            cmd.Parameters.Add(new SQLiteParameter("@fechaInicio", (object?)evento.FechaInicio ?? DBNull.Value));
            cmd.Parameters.Add(new SQLiteParameter("@fechaFinalizacion", (object?)evento.FechaFinalizacion ?? DBNull.Value));
            cmd.Parameters.Add(new SQLiteParameter("@estado", evento.Estado));
            cmd.ExecuteNonQuery();

            cmd = new SQLiteCommand("SELECT last_insert_rowid()", Conexion.Connection);
            long lastId = (long)cmd.ExecuteScalar();
            return (int)lastId;
        }

        // Actualiza un evento existente
        public static void Update(Evento evento)
        {

            var cmd = new SQLiteCommand(
                @"UPDATE Eventos 
                  SET idCatEvento = @idCatEvento, 
                      nombre = @nombre, 
                      totalHoras = @totalHoras,
                      fechaInicio = @fechaInicio,
                      fechaFinalizacion = @fechaFinalizacion,
                      estado = @estado
                  WHERE idEvento = @idEvento",
                Conexion.Connection);
            cmd.Parameters.Add(new SQLiteParameter("@idEvento", evento.Categoria.IdCategoriaEvento));
            cmd.Parameters.Add(new SQLiteParameter("@idCatEvento", evento.IdCatEvento));
            cmd.Parameters.Add(new SQLiteParameter("@nombre", evento.Nombre));
            cmd.Parameters.Add(new SQLiteParameter("@totalHoras", (object?)evento.TotalHoras ?? DBNull.Value));
            cmd.Parameters.Add(new SQLiteParameter("@fechaInicio", (object?)evento.FechaInicio ?? DBNull.Value));
            cmd.Parameters.Add(new SQLiteParameter("@fechaFinalizacion", (object?)evento.FechaFinalizacion ?? DBNull.Value));
            cmd.Parameters.Add(new SQLiteParameter("@estado", evento.Estado));
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
        public static string EstadoToString(int estado)
        {
            return estado switch
            {
                0 => "No iniciado",
                1 => "En curso",
                2 => "Finalizado",
                3 => "Cancelado",
                _ => "Desconocido"
            };
        }

        
            public static List<Evento> BuscarEnBD(int tipoBusqueda, string termino)
            {
                var eventos = new List<Evento>();
                termino = termino?.Trim() ?? "";

                string query = @"
            SELECT 
                e.idEvento, 
                e.idCatEvento, 
                e.nombre, 
                e.totalHoras,
                e.fechaInicio,
                e.fechaFinalizacion,
                e.estado,
                (SELECT COUNT(*) FROM Inscripciones i WHERE i.idEvento = e.idEvento) as cantidadParticipantes,
                c.nombre as nombreCategoria
            FROM Eventos e
            LEFT JOIN CategoriasEventos c ON e.idCatEvento = c.idCatEvento
            WHERE " + GetWhereClause(tipoBusqueda);

                try
                {
                    Conexion.OpenConnection();

                    using (var cmd = new SQLiteCommand(query, Conexion.Connection))
                    {
                        cmd.Parameters.AddWithValue("@termino", $"%{termino}%");

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                eventos.Add(new Evento
                                {
                                    IdEvento = reader.GetInt32(0),
                                    IdCatEvento = reader.GetInt32(1),
                                    Nombre = reader.GetString(2),
                                    TotalHoras = reader.IsDBNull(3) ? null : reader.GetInt32(3),
                                    FechaInicio = reader.IsDBNull(4) ? null : reader.GetDateTime(4),
                                    FechaFinalizacion = reader.IsDBNull(5) ? null : reader.GetDateTime(5),
                                    Estado = reader.GetInt32(6),
                                    CantidadParticipantes = reader.IsDBNull(7) ? 0 : reader.GetInt32(7),
                                    Categoria = pCategoriaEvento.GetById(reader.GetInt32(1))
                                });
                            }
                        }
                    }

                    return eventos;
                }
                finally
                {
                    Conexion.CloseConnection();
                }
            }

            private static string GetWhereClause(int tipoBusqueda)
            {
                return tipoBusqueda switch
                {
                    0 => "e.nombre LIKE @termino", // B�squeda por nombre

                    1 => "c.nombre LIKE @termino", // B�squeda por categor�a
                    
                    2 => "strftime('%d/%m/%Y', e.fechaInicio) LIKE @termino OR " +
                         "strftime('%d/%m/%Y', e.fechaFinalizacion) LIKE @termino", // B�squeda por fecha
                    
                    _ => "e.nombre LIKE @termino OR c.nombre LIKE @termino OR " +
                         "strftime('%d/%m/%Y', e.fechaInicio) LIKE @termino OR " +
                         "strftime('%d/%m/%Y', e.fechaFinalizacion) LIKE @termino" // B�squeda general
                };
            }
        
    }

}
