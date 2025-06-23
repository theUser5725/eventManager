using System;
using System.Collections.Generic;
using System.Data.SQLite;
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

        public static int Save(Participante participante)
        {
            SQLiteCommand cmd = new SQLiteCommand("INSERT INTO Participantes (nombre, apellido, mail, dni, contraseña) VALUES (@nombre, @apellido, @mail, @dni, @contraseña)");
            cmd.Parameters.Add(new SQLiteParameter("@nombre", participante.Nombre));
            cmd.Parameters.Add(new SQLiteParameter("@apellido", participante.Apellido));
            cmd.Parameters.Add(new SQLiteParameter("@mail", participante.Mail));
            cmd.Parameters.Add(new SQLiteParameter("@dni", participante.Dni));
            cmd.Parameters.Add(new SQLiteParameter("@contraseña", participante.Contraseña));
            cmd.Connection = Conexion.Connection;
            cmd.ExecuteNonQuery();

            cmd = new SQLiteCommand("SELECT last_insert_rowid()", Conexion.Connection);
            long lastId = (long)cmd.ExecuteScalar();
            return (int)lastId;
        }

        public static void Update(Participante participante)
        {
            SQLiteCommand cmd = new SQLiteCommand("UPDATE Participantes SET nombre = @nombre, apellido = @apellido, mail = @mail, dni = @dni, contraseña = @contraseña WHERE idParticipante = @idParticipante");
            cmd.Parameters.Add(new SQLiteParameter("@nombre", participante.Nombre));
            cmd.Parameters.Add(new SQLiteParameter("@apellido", participante.Apellido));
            cmd.Parameters.Add(new SQLiteParameter("@mail", participante.Mail));
            cmd.Parameters.Add(new SQLiteParameter("@dni", participante.Dni));
            cmd.Parameters.Add(new SQLiteParameter("@contraseña", participante.Contraseña));
            cmd.Parameters.Add(new SQLiteParameter("@idParticipante", participante.IdParticipante));
            cmd.Connection = Conexion.Connection;
            cmd.ExecuteNonQuery();
        }

        public static void Delete(Participante participante)
        {
            SQLiteCommand cmd = new SQLiteCommand("DELETE FROM Participantes WHERE idParticipante = @idParticipante");
            cmd.Parameters.Add(new SQLiteParameter("@idParticipante", participante.IdParticipante));
            cmd.Connection = Conexion.Connection;
            cmd.ExecuteNonQuery();
        }

    }
}
