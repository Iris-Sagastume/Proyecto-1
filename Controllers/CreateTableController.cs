using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System;

namespace Proyecto.Controllers
{
  public class CreateTableController : Controller
    {
        // Base de datos
        private readonly string _connectionString = "Server=localhost;Database=Proyecto;Trusted_Connection=True;TrustServerCertificate=True";

        // Acción para mostrar el formulario (GET)
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // Acción para procesar el formulario (POST)
        [HttpPost]
        public IActionResult Index(string tableName, List<string> columnNames, List<string> columnTypes)
        {
            // Verificación de los valores recibidos
            if (string.IsNullOrEmpty(tableName) || columnNames == null || columnNames.Count == 0 || columnTypes == null || columnTypes.Count == 0)
            {
                ViewData["ErrorMessage"] = "Debe proporcionar un nombre de tabla y al menos una columna.";
                return View();
            }

            try
            {
                // Generar la consulta SQL para crear la tabla
                string query = $"CREATE TABLE {tableName} (";
                for (int i = 0; i < columnNames.Count; i++)
                {
                    query += $"{columnNames[i]} {columnTypes[i]}";
                    if (i < columnNames.Count - 1)
                    {
                        query += ", ";
                    }
                }
                query += ");";

                // Ejecutar la consulta SQL
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.ExecuteNonQuery();
                }

                ViewData["SuccessMessage"] = "Tabla creada exitosamente.";
                return View(); // Notificar
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = $"Error al crear la tabla: {ex.Message}";
                return View();
            }
        }
    }
}