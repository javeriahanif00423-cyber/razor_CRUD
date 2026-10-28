
using books_management_system.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace books_management_system.Pages.books
{
    public class CreateModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public CreateModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [BindProperty]
        public Books Books { get; set; } = new Books();

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            string? cs = _configuration
                .GetConnectionString("DefaultConnection");

            if (string.IsNullOrWhiteSpace(cs))
            {
                throw new InvalidOperationException(
                    "DefaultConnection connection string is missing.");
            }

            using SqlConnection con = new SqlConnection(cs);
            con.Open();

            string query = @"
                INSERT INTO dbo.books
                (title, author, description, publisher, status, category)
                VALUES
                (@title, @author, @description, @publisher, @status, @category)";

            using SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue(
                "@title", (object?)Books.title ?? DBNull.Value);

            cmd.Parameters.AddWithValue(
                "@author", (object?)Books.author ?? DBNull.Value);

            cmd.Parameters.AddWithValue(
                "@description", (object?)Books.description ?? DBNull.Value);

            cmd.Parameters.AddWithValue(
                "@publisher", (object?)Books.publisher ?? DBNull.Value);

            cmd.Parameters.AddWithValue(
                "@status", (object?)Books.status ?? DBNull.Value);

            cmd.Parameters.AddWithValue(
                "@category", (object?)Books.category ?? DBNull.Value);

            cmd.ExecuteNonQuery();

            return RedirectToPage("Index");
        }
    }
}
