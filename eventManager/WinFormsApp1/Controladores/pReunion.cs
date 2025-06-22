
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormsApp1.Modelos; // Assuming Reunion is in this namespace



namespace WinFormsApp1.Controladores
{
    internal class pReunion
    {
        public static void Save(Reunion reunion)
        {
            SQLiteCommand command = new SQLiteCommand("INSERT INTO Reuniones (IdEvento, IdLugar, Horario) VALUES (@IdEvento, @IdLugar, @Horario)", Conexion.Connection);
            command.Parameters.AddWithValue("@IdEvento", reunion.IdEvento);
            command.Parameters.AddWithValue("@IdLugar", reunion.IdLugar);
            command.Parameters.AddWithValue("@Horario", reunion.Horario);
            //command.Parameters.AddWithValue("@Fecha", reunion.Fecha);
            //command.Parameters.AddWithValue("@Nombre", reunion.Nombre);
            //command.Parameters.AddWithValue("@Descripcion", reunion.Descripcion);
            command.ExecuteNonQuery();
        }

        public static void Update(Reunion reunion)
        {
            SQLiteCommand command = new SQLiteCommand("UPDATE Reuniones SET IdEvento = @IdEvento, IdLugar = @IdLugar, Horario = @Horario WHERE IdReunion = @IdReunion", Conexion.Connection);
            command.Parameters.AddWithValue("@IdReunion", reunion.IdReunion);
            command.Parameters.AddWithValue("@IdEvento", reunion.IdEvento);
            command.Parameters.AddWithValue("@IdLugar", reunion.IdLugar);
            command.Parameters.AddWithValue("@Horario", reunion.Horario);
            //command.Parameters.AddWithValue("@Fecha", reunion.Fecha);
            //command.Parameters.AddWithValue("@Nombre", reunion.Nombre);
            //command.Parameters.AddWithValue("@Descripcion", reunion.Descripcion);
            command.ExecuteNonQuery();
        }

        public static void Delete(int idReunion)
        {
            SQLiteCommand command = new SQLiteCommand("DELETE FROM Reuniones WHERE IdReunion = @IdReunion", Conexion.Connection);
            command.Parameters.AddWithValue("@IdReunion", idReunion);
            command.ExecuteNonQuery();
        }
        public static Reunion GetById(int idReunion)
        {
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM Reuniones WHERE IdReunion = @IdReunion");
            command.Parameters.AddWithValue("@IdReunion", idReunion);
            command.Connection = Conexion.Connection;

            SQLiteDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                Reunion returnReunion = new Reunion
                {
                    IdReunion = reader.GetInt32(0),
                    IdEvento = reader.GetInt32(1),
                    IdLugar = reader.GetInt32(2),
                    Horario = reader.GetDateTime(3),

                    //Fecha = reader.GetDateTime(4),
                    //Nombre = reader.GetString(5),
                    //Descripcion = reader.GetString(6)
                };
                return returnReunion;

            }
            else
            {
                return null;
            }
            Reunion reunion = null;
        }
        public static List<Reunion> GetAll()
        {
            List<Reunion> reuniones = new List<Reunion>();
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM Reuniones", Conexion.Connection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Reunion reunion = new Reunion
                {
                    IdReunion = reader.GetInt32(0),
                    IdEvento = reader.GetInt32(1),
                    IdLugar = reader.GetInt32(2),
                    Horario = reader.GetDateTime(3),
                    //Fecha = reader.GetDateTime(4),
                    //Nombre = reader.GetString(5),
                    //Descripcion = reader.GetString(6)
                };
                reuniones.Add(reunion);
            }
            return reuniones;
        }

        public static List<Reunion> GetAllByEventoId(int idEvento)
        {
            List<Reunion> reuniones = new List<Reunion>();
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM Reuniones WHERE IdEvento = @IdEvento", Conexion.Connection);
            command.Parameters.AddWithValue("@IdEvento", idEvento);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Reunion reunion = new Reunion
                {
                    IdReunion = reader.GetInt32(0),
                    IdEvento = reader.GetInt32(1),
                    IdLugar = reader.GetInt32(2),
                    Horario = reader.GetDateTime(3),
                    //Fecha = reader.GetDateTime(4),
                    //Nombre = reader.GetString(5),
                    //Descripcion = reader.GetString(6)
                };
                reuniones.Add(reunion);
            }
            return reuniones;
        }

        public static List<Reunion> GetAllByLugarId(int idLugar) 
        {
            List<Reunion> reuniones = new List<Reunion>();
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM Reuniones WHERE IdLugar = @IdLugar", Conexion.Connection);
            command.Parameters.AddWithValue("@IdLugar", idLugar);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Reunion reunion = new Reunion
                {
                    IdReunion = reader.GetInt32(0),
                    IdEvento = reader.GetInt32(1),
                    IdLugar = reader.GetInt32(2),
                    Horario = reader.GetDateTime(3),
                    //Fecha = reader.GetDateTime(4),
                    //Nombre = reader.GetString(5),
                    //Descripcion = reader.GetString(6)
                };
                reuniones.Add(reunion);
            }
            return reuniones;

        }

    }
}
