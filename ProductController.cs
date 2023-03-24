public class ProductController : Controller
{
    private readonly string connectionString = "your connection string here";

    public IActionResult Index(int pageNumber = 1, int pageSize = 10)
    {
        List<Product> products = GetAllProducts(pageNumber, pageSize);

        return View(products);
    }

    private List<Product> GetAllProducts(int pageNumber, int pageSize)
    {
        List<Product> products = new List<Product>();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = @"SELECT p.ProductId, p.ProductName, c.CategoryId, c.CategoryName
                             FROM Products p
                             INNER JOIN Categories c ON p.CategoryId = c.CategoryId
                             ORDER BY p.ProductId
                             LIMIT @startIndex, @pageSize";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                int startIndex = (pageNumber - 1) * pageSize;
                int endIndex = pageNumber * pageSize;

                command.Parameters.AddWithValue("@startIndex", startIndex);
                command.Parameters.AddWithValue("@pageSize", pageSize);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Product product = new Product
                    {
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        ProductName = reader["ProductName"].ToString(),
                        CategoryId = Convert.ToInt32(reader["CategoryId"]),
                        CategoryName = reader["CategoryName"].ToString()
                    };

                    products.Add(product);
                }
            }
        }

        return products;
    }
}
