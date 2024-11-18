using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

public class CreateTableModel : PageModel
{
    private readonly string _connectionString = "Server=localhost;Database=MiBaseDeDatos;Integrated Security=True;"; 

    [BindProperty]
    public string TableName { get; set; }
    [BindProperty]
    public List<string> ColumnNames { get; set; }
    [BindProperty]
    public List<string> ColumnTypes { get; set; }

    public void OnGet()
    {
    }

    public IActionResult OnPost()
    {
        if (string.IsNullOrEmpty(TableName) || ColumnNames == null || ColumnNames.Count == 0)
        {
            ViewData["ErrorMessage"] = "Debe proporcionar un nombre de tabla y al menos una columna.";
            return Page();
        }

        try
        {
            string query = $"CREATE TABLE {TableName} (";
            for (int i = 0; i < ColumnNames.Count; i++)
            {
                query += $"{ColumnNames[i]} {ColumnTypes[i]}";
                if (i < ColumnNames.Count - 1)
                {
                    query += ", ";
                }
            }
            query += ");";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                command.ExecuteNonQuery();
            }

            ViewData["SuccessMessage"] = "Tabla creada exitosamente.";
            return Page();
        }
        catch (Exception ex)
        {
            ViewData["ErrorMessage"] = $"Error al crear la tabla: {ex.Message}";
            return Page();
        }
    }
}