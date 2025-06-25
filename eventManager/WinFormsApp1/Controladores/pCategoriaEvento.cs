
﻿using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormsApp1.Modelos;


namespace WinFormsApp1.Controladores
{
    internal class pCategoriaEvento
    {
        public static void Save(Modelos.CategoriaEvento categoriaEvento)
        {
            SQLiteCommand command = new SQLiteCommand("INSERT INTO CategoriasEventos (IdCatEvento, Nombre) VALUES (@IdCategoriaEvento, @Nombre)", Conexion.Connection);
            command.Parameters.AddWithValue("@IdCategoriaEvento", categoriaEvento.IdCategoriaEvento);
            command.Parameters.AddWithValue("@Nombre", categoriaEvento.Nombre);
            command.ExecuteNonQuery();
        }

        public static void Update(Modelos.CategoriaEvento categoriaEvento)
        {
            SQLiteCommand command = new SQLiteCommand("UPDATE CategoriasEventos SET Nombre = @Nombre WHERE IdCatEvento = @IdCategoriaEvento", Conexion.Connection);
            command.Parameters.AddWithValue("@IdCategoriaEvento", categoriaEvento.IdCategoriaEvento);
            command.Parameters.AddWithValue("@Nombre", categoriaEvento.Nombre);
            command.ExecuteNonQuery();
        }

        public static void Delete(int idCategoriaEvento)
        {
            SQLiteCommand command = new SQLiteCommand("DELETE FROM CategoriasEventos WHERE IdCatEvento = @IdCategoriaEvento", Conexion.Connection);
            command.Parameters.AddWithValue("@IdCategoriaEvento", idCategoriaEvento);
            command.ExecuteNonQuery();
        }

        public static List<CategoriaEvento> GetAll() 
        {
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM CategoriasEventos", Conexion.Connection);
            SQLiteDataReader reader = command.ExecuteReader();
            List<CategoriaEvento> categorias = new List<CategoriaEvento>();

            while (reader.Read())
            {
                CategoriaEvento categoria = new CategoriaEvento
                {
                    IdCategoriaEvento = reader.GetInt32(0),
                    Nombre = reader.GetString(1)
                };
                categorias.Add(categoria);
            }
            return categorias;
        }

        public static CategoriaEvento GetById(int idCategoriaEvento)
        {
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM CategoriasEventos WHERE IdCatEvento = @IdCategoriaEvento", Conexion.Connection);
            command.Parameters.AddWithValue("@IdCategoriaEvento", idCategoriaEvento);
            SQLiteDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new CategoriaEvento
                {
                    IdCategoriaEvento = reader.GetInt32(0),
                    Nombre = reader.GetString(1)
                };
            }
            return null; // or throw an exception if not found
        }
    }
}

