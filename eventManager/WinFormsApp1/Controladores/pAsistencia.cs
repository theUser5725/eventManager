using Org.BouncyCastle.Asn1.Mozilla;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormsApp1.Modelos;

namespace WinFormsApp1.Controladores
{
    internal class pAsistencia
    {

        public static void Save(Asistencia asistencia)
        {
            SQLiteCommand command = new SQLiteCommand("INSERT INTO Asistencias (idParticipante, idReunion, horasAsistido) VALUES (@idParticipante, @idReunion, @horasAsistido)", Conexion.Connection);
            command.Parameters.AddWithValue("@idParticipante", asistencia.idParticipante);
            command.Parameters.AddWithValue("@idReunion", asistencia.idReunion);
            command.Parameters.AddWithValue("@horasAsistido", asistencia.horasAsistido);
            command.ExecuteNonQuery();

        }

        public static void Update(Asistencia asistencia)
        {
            SQLiteCommand command = new SQLiteCommand("UPDATE Asistencias SET horasAsistido = @horasAsistido WHERE idParticipante = @idParticipante AND idReunion = @idReunion", Conexion.Connection);
            command.Parameters.AddWithValue("@idParticipante", asistencia.idParticipante);
            command.Parameters.AddWithValue("@idReunion", asistencia.idReunion);
            command.Parameters.AddWithValue("@horasAsistido", asistencia.horasAsistido);
            command.ExecuteNonQuery();
        }

        public static void Delete(int idParticipante, int idReunion)
        {
            SQLiteCommand command = new SQLiteCommand("DELETE FROM Asistencias WHERE idParticipante = @idParticipante AND idReunion = @idReunion", Conexion.Connection);
            command.Parameters.AddWithValue("@idParticipante", idParticipante);
            command.Parameters.AddWithValue("@idReunion", idReunion);
            command.ExecuteNonQuery();
        }

        public static List<Asistencia> GetAllByReunionId(int idReunion)
        {
            List<Asistencia> asistencias = new List<Asistencia>();
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM Asistencias WHERE idReunion = @idReunion", Conexion.Connection);
            command.Parameters.AddWithValue("@idReunion", idReunion);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Asistencia asistencia = new Asistencia
                {
                    idParticipante = reader.GetInt32(0),
                    idReunion = reader.GetInt32(1),
                    horasAsistido = reader.GetInt32(2)
                };
                asistencias.Add(asistencia);
            }
            return asistencias;


        }

        public static List<Asistencia> GetAllByParticipanteId(int idParticipante)
        {
            List<Asistencia> asistencias = new List<Asistencia>();
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM Asistencias WHERE idParticipante = @idParticipante", Conexion.Connection);
            command.Parameters.AddWithValue("@idParticipante", idParticipante);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Asistencia asistencia = new Asistencia
                {
                    idParticipante = reader.GetInt32(0),
                    idReunion = reader.GetInt32(1),
                    horasAsistido = reader.GetInt32(2)
                };
                asistencias.Add(asistencia);
            }
            return asistencias;

        }
    }
}
