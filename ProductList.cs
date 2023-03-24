using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using ProductCatalog.Models;

namespace ProductCatalog.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            int startRow = (pageNumber - 1) * pageSize + 1;
            int endRow = pageNumber * pageSize;
            SqlParameter[] parameters =
            {
                new SqlParameter("@StartRow", startRow),
                new SqlParameter("@EndRow", endRow)
            };
            DataTable dataTable = ProductCatalogContext.ExecuteQuery("SELECT * FROM (SELECT ROW_NUMBER() OVER (ORDER BY ProductId) AS RowNumber, ProductId, ProductName, CategoryId FROM Product) AS Temp WHERE Temp.RowNumber BETWEEN @StartRow AND @EndRow", parameters);
            var products = dataTable.AsEnumerable().Select(row => new Product
            {
                ProductId = row.Field<int>("ProductId"),
                ProductName = row.Field<string>("ProductName"),
                CategoryId = row.Field<int>("CategoryId"),
                Category = new Category { CategoryId = row.Field<int>("CategoryId"), CategoryName = row.Field<string>("CategoryName") }
            }).ToList();
            return View(products);
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            var categories = GetCategories();
            ViewBag.Categories = new SelectList(categories, "CategoryId", "CategoryName");
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                SqlParameter[] parameters =
                {
                    new SqlParameter("@ProductName", product.ProductName),
                    new SqlParameter("@CategoryId", product.CategoryId)
                };
                ProductCatalogContext.ExecuteNonQuery("INSERT INTO Product (ProductName, CategoryId) VALUES (@ProductName, @CategoryId)", parameters);
                return RedirectToAction("Index");
            }
            var categories = GetCategories();
            ViewBag.Categories = new SelectList(categories, "CategoryId", "CategoryName");
            return View(product);
        }

        // GET: Product/Edit/5
        public ActionResult Edit(int id)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@ProductId", id)
            };
            DataTable dataTable = ProductCatalogContext.ExecuteQuery("SELECT * FROM Product WHERE ProductId = @ProductId", parameters);
            if (dataTable.Rows.Count == 0)
            {
                return HttpNotFound();
            }
            DataRow row = dataTable.Rows[0];
            Product product = new Product
            {
                ProductId = row.Field<int>("ProductId"),
                ProductName = row.Field<string>("ProductName"),
                CategoryId = row.Field<int>("CategoryId")
            };
            var categories = GetCategories();
            ViewBag.Categories = new SelectList(categories, "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

        // POST: Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                SqlParameter[] parameters =
                {
                    new SqlParameter("@ProductId", product.ProductId),
                    new SqlParameter("@ProductName", product.ProductName),
                    new SqlParameter("@CategoryId", product.CategoryId)
                };
                ProductCatalogContext.ExecuteNonQuery("UPDATE Product SET ProductName = @ProductName, CategoryId = @CategoryId WHERE ProductId = @ProductId", parameters);
                return RedirectToAction("Index");
            }
            var categories = Get
        // GET: Product/Delete/5
        public ActionResult Delete(int id)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@ProductId", id)
            };
            DataTable dataTable = ProductCatalogContext.ExecuteQuery("SELECT * FROM Product WHERE ProductId = @ProductId", parameters);
            if (dataTable.Rows.Count == 0)
            {
                return HttpNotFound();
            }
            DataRow row = dataTable.Rows[0];
            Product product = new Product
            {
                ProductId = row.Field<int>("ProductId"),
                ProductName = row.Field<string>("ProductName"),
                CategoryId = row.Field<int>("CategoryId")
            };
            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@ProductId", id)
            };
            ProductCatalogContext.ExecuteNonQuery("DELETE FROM Product WHERE ProductId = @ProductId", parameters);
            return RedirectToAction("Index");
        }

        private List<Category> GetCategories()
        {
            DataTable dataTable = ProductCatalogContext.ExecuteQuery("SELECT * FROM Category");
            var categories = dataTable.AsEnumerable().Select(row => new Category
            {
                CategoryId = row.Field<int>("CategoryId"),
                CategoryName = row.Field<string>("CategoryName")
            }).ToList();
            return categories;
        }
    }
}
}