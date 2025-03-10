namespace ImaginaryShop.Model.Repos
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Configuration;
    using Microsoft.Data.SqlClient;
    public class CategoryRepository
    {
        private  string _connectionString;

        public CategoryRepository()
        {
            // Hent forbindelsesstrengen fra app.config eller web.config
            _connectionString = "Data Source=mssql16.unoeuro.com,1433;Initial Catalog=mathiasabel_dk_db_abel;User ID=mathiasabel_dk;Password=Hnmxry4ftGBFzeadDwgp;Connect Timeout=30;Encrypt=True;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
        }

        public List<Category> GetAllCategories()
        {
            var categories = new List<Category>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("select distinct(category) from products", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var category = new Category
                            {
                                
                                Name = reader.GetString(0)
                            };
                            categories.Add(category);
                        }
                    }
                }
            }

            return categories;
        }

        public Category GetCategoryById(int id)
        {
            Category category = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT Id, Name FROM Categories WHERE Id = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            category = new Category
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1)
                            };
                        }
                    }
                }
            }

            return category;
        }
    }

   
}
