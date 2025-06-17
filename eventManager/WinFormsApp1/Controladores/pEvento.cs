using System;
using System.Collections.Generic;
using System.Data.SQLite;
using WinFormsApp1.Models;
using WinFormsApp1.Modelos;

namespace WinFormsApp1.Controladores
{
    public class pEvento
    {
        // Devuelve todos los eventos SIN cargar participantes ni reuniones
        public List<Evento> GetAll()
        {
            var eventos = new List<Evento>();
            Conexion.OpenConnection();
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
            Conexion.CloseConnection();
            return eventos;
        }

        // Devuelve un evento por ID, cargando participantes y reuniones
        public Evento GetById(int idEvento)
        {
            Evento evento = null;
            Conexion.OpenConnection();

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
                Conexion.CloseConnection();
                return null;
            }

            // Cargar participantes
            evento.Participantes = pParticipante.GetAllByEventoId(idEvento)

            // Cargar reuniones
            evento.Reuniones = pReunion.GetAllByEventoId(idEvento);
        }

            Conexion.CloseConnection();
            return evento;
        }
    }
}
