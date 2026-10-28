
using books_management_system.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Data;

namespace books_management_system.Pages.books
{
    public class EditModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public EditModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [BindProperty]
        public Books Books { get; set; } = new Books();

        public IActionResult OnGet(int id)
        {
            string? cs = _configuration
                .GetConnectionString("DefaultConnection");

            if (string.IsNullOrWhiteSpace(cs))
            {
                throw new InvalidOperationException(
                    "DefaultConnection is missing.");
            }

            using SqlConnection con = new SqlConnection(cs);
            con.Open();

            string query = @"
                SELECT Id, title, author, description,
                       publisher, status, category
                FROM dbo.books
                WHERE Id = @Id";

            using SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            using SqlDataReader reader = cmd.ExecuteReader();

            if (!reader.Read())
                return NotFound();

            Books = new Books
            {
                Id = (int)reader["Id"],
                title = reader["title"] as string,
                author = reader["author"] as string,
                description = reader["description"] as string,
                publisher = reader["publisher"] as string,
                status = reader["status"] as string,
                category = reader["category"] as string
            };

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            string? cs = _configuration
                .GetConnectionString("DefaultConnection");

            if (string.IsNullOrWhiteSpace(cs))
            {
                throw new InvalidOperationException(
                    "DefaultConnection is missing.");
            }

            using SqlConnection con = new SqlConnection(cs);
            con.Open();

            string query = @"
                UPDATE dbo.books SET
                    title = @title,
                    author = @author,
                    description = @description,
                    publisher = @publisher,
                    status = @status,
                    category = @category
                WHERE Id = @Id";

            using SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = Books.Id;

            cmd.Parameters.Add("@title", SqlDbType.NVarChar, 50)
                .Value = (object?)Books.title ?? DBNull.Value;

            cmd.Parameters.Add("@author", SqlDbType.NVarChar, 50)
                .Value = (object?)Books.author ?? DBNull.Value;

            cmd.Parameters.Add("@description", SqlDbType.NVarChar, 500)
                .Value = (object?)Books.description ?? DBNull.Value;

            cmd.Parameters.Add("@publisher", SqlDbType.NVarChar, 50)
                .Value = (object?)Books.publisher ?? DBNull.Value;

            cmd.Parameters.Add("@status", SqlDbType.NVarChar, 50)
                .Value = (object?)Books.status ?? DBNull.Value;

            cmd.Parameters.Add("@category", SqlDbType.NVarChar, 50)
                .Value = (object?)Books.category ?? DBNull.Value;

            int rows = cmd.ExecuteNonQuery();

            if (rows == 0)
                return NotFound();

            return RedirectToPage("/books/Index");
        }
    }
}
