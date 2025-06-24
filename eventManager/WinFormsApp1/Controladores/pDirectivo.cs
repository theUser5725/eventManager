using System;
using System.Collections.Generic;
using System.Data.SQLite;
using WinFormsApp1.Modelos;

namespace WinFormsApp1.Controladores
{
    internal class nDirectivo
    {
        public static Directivo getByPk(int idReunion, int idParticipante)
        {
            Directivo directivo = null;

            SQLiteCommand cmd = new SQLiteCommand("SELECT idReunion, idParticipante, idCatDirectiva FROM Directivos WHERE idReunion = @idReunion AND idParticipante = @idParticipante");
            cmd.Parameters.Add(new SQLiteParameter("@idReunion", idReunion));
            cmd.Parameters.Add(new SQLiteParameter("@idParticipante", idParticipante));
            cmd.Connection = Conexion.Connection;

            SQLiteDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                directivo = new Directivo(
                    reader.GetInt32(0),
                    reader.GetInt32(1),
                    reader.GetInt32(2),
                    pParticipante.getById(reader.GetInt32(1))
                );
            }

            return directivo;
        }

        public static List<Directivo> getAllByReunionId(int idReunion)
        {
            List<Directivo> directivos = new List<Directivo>();

            SQLiteCommand cmd = new SQLiteCommand("SELECT idReunion, idParticipante, idCatDirectiva FROM Directivos WHERE idReunion = @idReunion");
            cmd.Parameters.Add(new SQLiteParameter("@idReunion", idReunion));
            cmd.Connection = Conexion.Connection;

            SQLiteDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Directivo directivo = new Directivo(
                    reader.GetInt32(0),
                    reader.GetInt32(1),
                    reader.GetInt32(2),
                    pParticipante.getById(reader.GetInt32(1))
                );

                directivos.Add(directivo);
            }

            return directivos;
        }

        public static int Save(Directivo directivo)
        {
            SQLiteCommand cmd = new SQLiteCommand("INSERT INTO Directivos (idReunion, idParticipante, idCatDirectiva) VALUES (@idReunion, @idParticipante, @idCatDirectiva)");
            cmd.Parameters.Add(new SQLiteParameter("@idReunion", directivo.IdReunion));
            cmd.Parameters.Add(new SQLiteParameter("@idParticipante", directivo.IdParticipante));
            cmd.Parameters.Add(new SQLiteParameter("@idCatDirectiva", directivo.IdCatDirectiva));
            cmd.Connection = Conexion.Connection;
            cmd.ExecuteNonQuery();

            cmd = new SQLiteCommand("SELECT last_insert_rowid()", Conexion.Connection);
            long lastId = (long)cmd.ExecuteScalar();
            return (int)lastId;
        }
        public static void Delete(Directivo directivo)
        {
            SQLiteCommand cmd = new SQLiteCommand("DELETE FROM Directivos WHERE idReunion = @idReunion AND idParticipante = @idParticipante");
            cmd.Parameters.Add(new SQLiteParameter("@idReunion", directivo.IdReunion));
            cmd.Parameters.Add(new SQLiteParameter("@idParticipante", directivo.IdParticipante));
            cmd.Connection = Conexion.Connection;
            cmd.ExecuteNonQuery();
        }
        public static void Update(Directivo directivo)
        {
            SQLiteCommand cmd = new SQLiteCommand("UPDATE Directivos SET idCatDirectiva = @idCatDirectiva WHERE idReunion = @idReunion AND idParticipante = @idParticipante");
            cmd.Parameters.Add(new SQLiteParameter("@idCatDirectiva", directivo.IdCatDirectiva));
            cmd.Parameters.Add(new SQLiteParameter("@idReunion", directivo.IdReunion));
            cmd.Parameters.Add(new SQLiteParameter("@idParticipante", directivo.IdParticipante));
            cmd.Connection = Conexion.Connection;
            cmd.ExecuteNonQuery();
        }
    }
}
