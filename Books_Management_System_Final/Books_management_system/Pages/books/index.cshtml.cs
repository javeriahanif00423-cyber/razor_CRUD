
using books_management_system.models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace books_management_system.Pages.books
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public IndexModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<Books> Books { get; set; } = new();

        public void OnGet()
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
                ORDER BY Id DESC";

            using SqlCommand cmd = new SqlCommand(query, con);
            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Books.Add(new Books
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    title = reader["title"] as string,
                    author = reader["author"] as string,
                    description = reader["description"] as string,
                    publisher = reader["publisher"] as string,
                    status = reader["status"] as string,
                    category = reader["category"] as string
                });
            }
        }
    }
}
