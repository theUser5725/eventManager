using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormsApp1.Modelos;

namespace WinFormsApp1.Controladores
{
    internal class pCategoriaDirectivaControlador
    {
        private readonly string connectionString;

        public pCategoriaDirectivaControlador(string connectionString)
        {
            this.connectionString = connectionString;
        }

        // Devuelve todas las categorías de directiva.

        public List<CategoriaDirectivas> GetAll()
        {
            var lista = new List<CategoriaDirectivas>();

            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                const string sql =
                  "SELECT idCatDirectiva, nombre " +
                  "FROM CategoriasDirectivos;";

                using (var cmd = new SQLiteCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new CategoriaDirectivas(
                            reader.GetInt32(0),   // idCatDirectiva
                            reader.GetString(1)   // Nombre
                        ));
                    }
                }
            }

            return lista;
        }

        // Devuelve una categoría de directiva por su ID.
  
        public CategoriaDirectivas GetById(int idCatDirectiva)
        {
            CategoriaDirectivas categoria = null;

            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                const string sql =
                  "SELECT idCatDirectiva, nombre " +
                  "FROM CategoriasDirectivos " +
                  "WHERE idCatDirectiva = @id;";

                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", idCatDirectiva);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            categoria = new CategoriaDirectivas(
                                reader.GetInt32(0),
                                reader.GetString(1)
                            );
                        }
                    }
                }
            }

            return categoria;
        }
    }
}
