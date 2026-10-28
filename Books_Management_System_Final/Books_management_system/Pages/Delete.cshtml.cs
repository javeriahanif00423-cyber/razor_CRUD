
using books_management_system.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace books_management_system.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public DeleteModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

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
            cmd.Parameters.AddWithValue("@Id", id);

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

        public IActionResult OnPost(int id)
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

            string query = "DELETE FROM dbo.books WHERE Id = @Id";

            using SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@Id", id);

            int rows = cmd.ExecuteNonQuery();

            if (rows == 0)
                return NotFound();

            return RedirectToPage("/books/Index");
        }
    }
}
