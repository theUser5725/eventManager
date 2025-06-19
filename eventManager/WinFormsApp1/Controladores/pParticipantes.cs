using System;
using System.Collections.Generic;
using System.Data.SQLite;
using WinFormsApp1.Modelos;

namespace WinFormsApp1.Controladores
{
    internal class pParticipantes
    {
        public static Participantes getById(int idParticipante)
        {
            Participantes participante = new Participantes();

            SQLiteCommand cmd = new SQLiteCommand("SELECT idParticipante, nombre, apellido, mail, dni, contraseña FROM Participantes WHERE idParticipante = @idParticipante");
            cmd.Parameters.Add(new SQLiteParameter("@idParticipante", idParticipante));
            cmd.Connection = Conexion.Connection;

            SQLiteDataReader objDBReader = cmd.ExecuteReader();

            while (objDBReader.Read())
            {
                participante = new Participantes(
                    objDBReader.GetInt32(0),
                    objDBReader.GetString(1),
                    objDBReader.GetString(2),
                    objDBReader.GetString(3),
                    objDBReader.GetInt32(4),
                    objDBReader.GetInt32(5)
                );
            }

            return participante;
        }

        public static List<Participantes> getAll()
        {
            List<Participantes> participantes = new List<Participantes>();

            SQLiteCommand cmd = new SQLiteCommand("SELECT idParticipante, nombre, apellido, mail, dni, contraseña FROM Participantes");
            cmd.Connection = Conexion.Connection;

            SQLiteDataReader objDBReader = cmd.ExecuteReader();

            while (objDBReader.Read())
            {
                Participantes participante = new Participantes(
                    objDBReader.GetInt32(0),
                    objDBReader.GetString(1),
                    objDBReader.GetString(2),
                    objDBReader.GetString(3),
                    objDBReader.GetInt32(4),
                    objDBReader.GetInt32(5)
                );

                participantes.Add(participante);
            }

            return participantes;
        }

        public static List<Participantes> getAllByEventoId(int idEvento)
        {
            List<Participantes> participantes = new List<Participantes>();

            SQLiteCommand cmd = new SQLiteCommand(@"
        SELECT p.idParticipante, p.nombre, p.apellido, p.mail, p.dni, p.contraseña
        FROM Participantes p
        INNER JOIN Inscripciones i ON p.idParticipante = i.idParticipante
        WHERE i.idEvento = @idEvento");

            cmd.Parameters.Add(new SQLiteParameter("@idEvento", idEvento));
            cmd.Connection = Conexion.Connection;

            SQLiteDataReader objDBReader = cmd.ExecuteReader();

            while (objDBReader.Read())
            {
                Participantes participante = new Participantes(
                    objDBReader.GetInt32(0),
                    objDBReader.GetString(1),
                    objDBReader.GetString(2),
                    objDBReader.GetString(3),
                    objDBReader.GetInt32(4),
                    objDBReader.GetInt32(5)
                );

                participantes.Add(participante);
            }

            return participantes;
        }
    }
}
