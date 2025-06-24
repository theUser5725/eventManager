using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormsApp1.Modelos;

namespace WinFormsApp1.Controladores
{
    internal class pCategoriaEvento
    {
        internal class pCategoriaEventoControlador
        {
            private readonly string connectionString;

            public pCategoriaEventoControlador(string connectionString)
            {
                this.connectionString = connectionString;
            }

          
            //Devuelve todas las categorías de evento.
            public List<CategoriaEvento> GetAll()
            {
                var lista = new List<CategoriaEvento>();

                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    const string sql =
                      "SELECT idCatEvento, nombre " +
                      "FROM CategoriasEventos;";

                    using (var cmd = new SQLiteCommand(sql, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new CategoriaEvento(
                                reader.GetInt32(0),   // idCatEvento
                                reader.GetString(1)   // Nombre
                            ));
                        }
                    }
                }

                return lista;
            }

         
            // Devuelve una categoría de evento por su ID.

            public CategoriaEvento GetById(int idCatEvento)
            {
                CategoriaEvento categoria = null;

                using (var conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    const string sql =
                      "SELECT idCatEvento, nombre " +
                      "FROM CategoriasEventos " +
                      "WHERE idCatEvento = @id;";

                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", idCatEvento);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                categoria = new CategoriaEvento(
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


}



//Probando Modificaciones
//internal class CategoriaEventoControlador
//{
//    private List<CategoriaEvento> categorias = new List<CategoriaEvento>();
//    public CategoriaEventoControlador()
//    {
//        categorias.Add(new CategoriaEvento { IdCatEvento = 1, Nombre = "Conferencia" });
//        categorias.Add(new CategoriaEvento { IdCatEvento = 2, Nombre = "Taller" });
//    }
//    public CategoriaEvento GetById(int id)
//    {
//        return categorias.FirstOrDefault(c => c.IdCatEvento == id);
//    }
//    public List<CategoriaEvento> GetAll()
//    {
//        return categorias;
//    }
//}

