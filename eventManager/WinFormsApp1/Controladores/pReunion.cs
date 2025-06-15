using FormularioDinamico.Controladores;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormsApp1.Entidades;


namespace WinFormsApp1.Controladores
{
    internal class pReunion
    {
        public static Reunion g( int idReunion) 
        {
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM Reunion WHERE IdReunion = @IdReunion");
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
    }

    
}
