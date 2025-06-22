using System;
using System.Collections.Generic;
using System.Data.SQLite;
using tp4_Prueba.Modelos;
using WinFormsApp1.Modelos;

namespace WinFormsApp1.Controladores
{
    internal class pParticipante
    {
        public static Participante getById(int idParticipante)
        {
            Participante participante = new Participante();

            SQLiteCommand cmd = new SQLiteCommand("SELECT idParticipante, nombre, apellido, mail, dni, contraseña FROM Participantes WHERE idParticipante = @idParticipante");
            cmd.Parameters.Add(new SQLiteParameter("@idParticipante", idParticipante));
            cmd.Connection = Conexion.Connection;

            SQLiteDataReader objDBReader = cmd.ExecuteReader();

            while (objDBReader.Read())
            {
                participante = new Participante(
                    objDBReader.GetInt32(0),
                    objDBReader.GetString(1),
                    objDBReader.GetString(2),
                    objDBReader.GetString(3),
                    objDBReader.GetString(4),
                    objDBReader.GetString(5)
                );
            }

            return participante;
        }

        public static List<Participante> getAll()
        {
            List<Participante> participantes = new List<Participante>();

            SQLiteCommand cmd = new SQLiteCommand("SELECT idParticipante, nombre, apellido, mail, dni, contraseña FROM Participantes");
            cmd.Connection = Conexion.Connection;

            SQLiteDataReader objDBReader = cmd.ExecuteReader();

            while (objDBReader.Read())
            {
                Participante participante = new Participante(
                    objDBReader.GetInt32(0),
                    objDBReader.GetString(1),
                    objDBReader.GetString(2),
                    objDBReader.GetString(3),
                    objDBReader.GetString(4),
                    objDBReader.GetString(5)
                );

                participantes.Add(participante);
            }

            return participantes;
        }

        public static List<Participante> getAllByEventoId(int idEvento)
        {
            List<Participante> participantes = new List<Participante>();

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
                Participante participante = new Participante(
                    objDBReader.GetInt32(0),
                    objDBReader.GetString(1),
                    objDBReader.GetString(2),
                    objDBReader.GetString(3),
                    objDBReader.GetString(4),
                    objDBReader.GetString(5)
                );

                participantes.Add(participante);
            }

            return participantes;
        }
    }
}
