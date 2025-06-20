using System;
using System.Collections.Generic;
using System.Data.SQLite;
using tp4_Prueba.Modelos;
using WinFormsApp1.Modelos;

namespace WinFormsApp1.Controladores
{
    internal class nDirectivos
    {
        public static Directivos getByPk(int idReunion, int idParticipante)
        {
            Directivos directivo = null;

            SQLiteCommand cmd = new SQLiteCommand("SELECT idReunion, idParticipante, idCatDirectiva FROM Directivos WHERE idReunion = @idReunion AND idParticipante = @idParticipante");
            cmd.Parameters.Add(new SQLiteParameter("@idReunion", idReunion));
            cmd.Parameters.Add(new SQLiteParameter("@idParticipante", idParticipante));
            cmd.Connection = connection.Connection;

            SQLiteDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                directivo = new Directivos(
                    reader.GetInt32(0),
                    reader.GetInt32(1),
                    reader.GetInt32(2)
                );
            }

            return directivo;
        }

        public static List<Directivos> getAllByReunionId(int idReunion)
        {
            List<Directivos> directivos = new List<Directivos>();

            SQLiteCommand cmd = new SQLiteCommand("SELECT idReunion, idParticipante, idCatDirectiva FROM Directivos WHERE idReunion = @idReunion");
            cmd.Parameters.Add(new SQLiteParameter("@idReunion", idReunion));
            cmd.Connection = connection.Connection;

            SQLiteDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Directivos directivo = new Directivos(
                    reader.GetInt32(0),
                    reader.GetInt32(1),
                    reader.GetInt32(2)
                );

                directivos.Add(directivo);
            }

            return directivos;
        }

        public static int Save(Directivos directivo)
        {
            SQLiteCommand cmd = new SQLiteCommand("INSERT INTO Directivos (idReunion, idParticipante, idCatDirectiva) VALUES (@idReunion, @idParticipante, @idCatDirectiva)");
            cmd.Parameters.Add(new SQLiteParameter("@idReunion", directivo.IdReunion));
            cmd.Parameters.Add(new SQLiteParameter("@idParticipante", directivo.IdParticipante));
            cmd.Parameters.Add(new SQLiteParameter("@idCatDirectiva", directivo.IdCatDirectiva));
            cmd.Connection = connection.Connection;
            cmd.ExecuteNonQuery();

            cmd = new SQLiteCommand("SELECT last_insert_rowid()", connection.Connection);
            long lastId = (long)cmd.ExecuteScalar();
            return (int)lastId;
        }

        public static void Delete(Directivos directivo)
        {
            SQLiteCommand cmd = new SQLiteCommand("DELETE FROM Directivos WHERE idReunion = @idReunion AND idParticipante = @idParticipante");
            cmd.Parameters.Add(new SQLiteParameter("@idReunion", directivo.IdReunion));
            cmd.Parameters.Add(new SQLiteParameter("@idParticipante", directivo.IdParticipante));
            cmd.Connection = connection.Connection;
            cmd.ExecuteNonQuery();
        }

        public static void Update(Directivos directivo)
        {
            SQLiteCommand cmd = new SQLiteCommand("UPDATE Directivos SET idCatDirectiva = @idCatDirectiva WHERE idReunion = @idReunion AND idParticipante = @idParticipante");
            cmd.Parameters.Add(new SQLiteParameter("@idCatDirectiva", directivo.IdCatDirectiva));
            cmd.Parameters.Add(new SQLiteParameter("@idReunion", directivo.IdReunion));
            cmd.Parameters.Add(new SQLiteParameter("@idParticipante", directivo.IdParticipante));
            cmd.Connection = connection.Connection;
            cmd.ExecuteNonQuery();
        }
    }
}
