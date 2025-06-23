using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using WinFormsApp1.Controladores;
using WinFormsApp1.Modelos; // Assuming categoriaDirectivo is in this namespace
namespace WinFormsApp1.Modelos
{
    internal class pCategoriaDirectivo
    {
        public static void Save(CategoriaDirectiva categoriaDirectivo)
        {
            SQLiteCommand command = new SQLiteCommand("INSERT INTO CategoriaDirectivo (IdCategoriaDirectivo, nombre) VALUES (@IdCategoriaDirectivo, @Nombre)", Conexion.Connection);
            command.Parameters.AddWithValue("@IdCategoriaDirectivo", categoriaDirectivo.IdCategoriaDirectivo);
            command.Parameters.AddWithValue("@Nombre", categoriaDirectivo.Nombre);
            command.ExecuteNonQuery();
        }

        public static void Update(CategoriaDirectiva categoriaDirectivo)
        {
            SQLiteCommand command = new SQLiteCommand("UPDATE CategoriaDirectivo SET nombre = @Nombre WHERE IdCategoriaDirectivo = @IdCategoriaDirectivo", Conexion.Connection);
            command.Parameters.AddWithValue("@IdCategoriaDirectivo", categoriaDirectivo.IdCategoriaDirectivo);
            command.Parameters.AddWithValue("@Nombre", categoriaDirectivo.Nombre);
            command.ExecuteNonQuery();
        }

        public static void Delete(int idCategoriaDirectivo)
        {
            SQLiteCommand command = new SQLiteCommand("DELETE FROM CategoriaDirectivo WHERE IdCategoriaDirectivo = @IdCategoriaDirectivo", Conexion.Connection);
            command.Parameters.AddWithValue("@IdCategoriaDirectivo", idCategoriaDirectivo);
            command.ExecuteNonQuery();
        }

        public static CategoriaDirectiva GetById(int idCategoriaDirectivo)
        {
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM CategoriaDirectivo WHERE IdCategoriaDirectivo = @IdCategoriaDirectivo", Conexion.Connection);
            command.Parameters.AddWithValue("@IdCategoriaDirectivo", idCategoriaDirectivo);
            SQLiteDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new CategoriaDirectiva
                {
                    IdCategoriaDirectivo = reader.GetInt32(0),
                    Nombre = reader.GetString(1)
                };
            }
            return null; // or throw an exception if not found
        }

        public static List<CategoriaDirectiva> GetAll()
        {
            List<CategoriaDirectiva> categorias = new List<CategoriaDirectiva>();
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM CategoriaDirectivo", Conexion.Connection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                categorias.Add(new CategoriaDirectiva
                {
                    IdCategoriaDirectivo = reader.GetInt32(0),
                    Nombre = reader.GetString(1)
                });
            }
            return categorias;
        }
    }
}
