using System;
using System.Collections.Generic;
using System.Data.SQLite;
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
            cmd.Connection = Conexion.Connection;

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
            cmd.Connection = Conexion.Connection;

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
    }
}
