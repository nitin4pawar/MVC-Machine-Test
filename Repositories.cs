public class CategoryRepository
{
    private readonly string connectionString;

    public CategoryRepository(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public IEnumerable<Category> GetAll()
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string sql = "SELECT CategoryId, CategoryName FROM Category";
            SqlCommand command = new SqlCommand(sql, connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            List<Category> categories = new List<Category>();
            while (reader.Read())
            {
                Category category = new Category
                {
                    CategoryId = reader.GetInt32(0),
                    CategoryName = reader.GetString(1)
                };
                categories.Add(category);
            }
            return categories;
        }
    }

    public Category GetById(int id)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string sql = "SELECT CategoryId, CategoryName FROM Category WHERE CategoryId = @CategoryId";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@CategoryId", id);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                Category category = new Category
                {
                    CategoryId = reader.GetInt32(0),
                    CategoryName = reader.GetString(1)
                };
                return category;
            }
            return null;
        }
    }

    public void Add(Category category)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string sql = "INSERT INTO Category (CategoryName) VALUES (@CategoryName)";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@CategoryName", category.CategoryName);
            connection.Open();
            command.ExecuteNonQuery();
        }
    }

    public void Update(Category category)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string sql = "UPDATE Category SET CategoryName = @CategoryName WHERE CategoryId = @CategoryId";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@CategoryName", category.CategoryName);
            command.Parameters.AddWithValue("@CategoryId", category.CategoryId);
           
            connection.Open();
            command.ExecuteNonQuery();
        }
    }

    public void Delete(int id)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string sql = "DELETE FROM Category WHERE CategoryId = @CategoryId";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@CategoryId", id);
            connection.Open();
            command.ExecuteNonQuery();
        }
    }
}

public class ProductRepository
{
    private readonly string connectionString;

    public ProductRepository(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public IEnumerable<Product> GetAll(int pageSize, int pageNumber)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            int offset = (pageNumber - 1) * pageSize;
            string sql = @"SELECT P.ProductId, P.ProductName, P.CategoryId, C.CategoryName 
                           FROM Product P INNER JOIN Category C ON P.CategoryId = C.CategoryId
                           ORDER BY P.ProductId OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Offset", offset);
            command.Parameters.AddWithValue("@PageSize", pageSize);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            List<Product> products = new List<Product>();
            while (reader.Read())
            {
                Product product = new Product
                {
                    ProductId = reader.GetInt32(0),
                    ProductName = reader.GetString(1),
                    CategoryId = reader.GetInt32(2),
                    CategoryName = reader.GetString(3)
                };
                products.Add(product);
            }
            return products;
        }
    }

    public Product GetById(int id)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string sql = @"SELECT P.ProductId, P.ProductName, P.CategoryId, C.CategoryName 
                           FROM Product P INNER JOIN Category C ON P.CategoryId = C.CategoryId
                           WHERE P.ProductId = @ProductId";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@ProductId", id);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                Product product = new Product
                {
                    ProductId = reader.GetInt32(0),
                    ProductName = reader.GetString(1),
                    CategoryId = reader.GetInt32(2),
                    CategoryName = reader.GetString(3)
                };
                return product;
            }
            return null;
        }
    }

    public void Add(Product product)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string sql = "INSERT INTO Product (ProductName, CategoryId) VALUES (@ProductName, @CategoryId)";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@ProductName", product.ProductName);
            command.Parameters.AddWithValue("@CategoryId", product.CategoryId);
            connection.Open();
            command.ExecuteNonQuery();
        }
    }

    public void Update(Product product)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string sql = "UPDATE Product SET ProductName = @ProductName, CategoryId = @CategoryId WHERE ProductId = @ProductId";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@ProductName", product.ProductName);
            command.Parameters.AddWithValue("@CategoryId", product.CategoryId);
            command.Parameters.AddWithValue("@ProductId", product.ProductId);
            connection.Open();
            command.ExecuteNonQuery();
        }
    }

    public void Delete(int id)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string sql = "DELETE FROM Product WHERE ProductId = @ProductId";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@ProductId", id);
            connection.Open();
            command.ExecuteNonQuery();
        }
    }
}
