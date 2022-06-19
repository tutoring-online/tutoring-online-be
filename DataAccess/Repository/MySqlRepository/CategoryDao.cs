using Anotar.NLog;
using DataAccess.Entities.Category;
using DataAccess.Utils;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.MySqlRepository;

public class CategoryDao:ICategoryDao
{
    private readonly ILogger<CategoryDao> logger;

    public CategoryDao(ILogger<CategoryDao> logger)
    {
        this.logger = logger;
    }

    public IEnumerable<Category?> GetCategories()
    {
        var category = new List<Category?>();

        try
        {
            using var connection = DbUtils.GetMySqlDbConnection();
            connection.Open();

            var selectStatement = "Select Id, Name, Description, CreatedDate, UpdatedDate, Type, Status";
            var fromStatement = "From Category";
            var query = selectStatement + " " + fromStatement;

            using var command = DbUtils.CreateMySqlCommand(query, connection);

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                category.Add(new Category
                {
                    Id = DbUtils.SafeGetString(reader, "Id"),                
                    Name = DbUtils.SafeGetString(reader, "Name"),
                    Description = DbUtils.SafeGetString(reader, "Description"),                  
                    CreatedDate = DbUtils.SafeGetDateTime(reader, "CreatedDate"),
                    UpdatedDate = DbUtils.SafeGetDateTime(reader, "UpdatedDate"),
                    Type = DbUtils.SafeGetInt16(reader, "Type"),
                    Status = DbUtils.SafeGetInt16(reader, "Status")
                });

            }
        }
        catch (MySqlException e)
        {
            LogTo.Info(e.ToString);
        }
        catch (Exception e)
        {
            LogTo.Info(e.ToString);
        }
        finally
        {
            DbUtils.CloseMySqlDbConnection();
        }

        return category;
    }
    public IEnumerable<Category?> GetCategoryById(string id)
    {
        var categories = new List<Category?>();

        try
        {
            using var connection = DbUtils.GetMySqlDbConnection();
            connection.Open();
            var param1 = "@id";

            var selectStatement = "Select Id, Name, Description, CreatedDate, UpdatedDate, Type, Status";
            var fromStatement = "From Category";
            var whereStatement = $"Where Id = {param1}";
            var query = selectStatement + " " + fromStatement + " " + whereStatement;

            using var command = DbUtils.CreateMySqlCommand(query, connection);
            command.CommandText = query;

            command.Parameters.Add(param1, MySqlDbType.VarChar).Value = id;
            command.Prepare();

            foreach (MySqlParameter commandParameter in command.Parameters)
            {
                LogTo.Info($"Param {commandParameter}: {commandParameter.Value}");
            }

            var reader = command.ExecuteReader();

            if (reader.Read())
            {
                categories.Add(new Category
                {
                    Id = DbUtils.SafeGetString(reader, "Id"),
                    Name = DbUtils.SafeGetString(reader, "Name"),
                    Description = DbUtils.SafeGetString(reader, "Description"),
                    CreatedDate = DbUtils.SafeGetDateTime(reader, "CreatedDate"),
                    UpdatedDate = DbUtils.SafeGetDateTime(reader, "UpdatedDate"),
                    Type = DbUtils.SafeGetInt16(reader, "Type"),
                    Status = DbUtils.SafeGetInt16(reader, "Status")
                });

            }
        }
        catch (MySqlException e)
        {
            LogTo.Info(e.ToString);
        }
        catch (Exception e)
        {
            LogTo.Info(e.ToString);
        }
        finally
        {
            DbUtils.CloseMySqlDbConnection();
        }

        return categories;
    }


    public void CreateCategories(IEnumerable<Category> categories)
    {
        try
        {
            using var connection = DbUtils.GetMySqlDbConnection();
            connection.Open();

            using var command = MySqlUtils.CreateInsertStatement(categories, connection);
            command.ExecuteNonQuery();

        }
        catch (MySqlException e)
        {
            LogTo.Info(e.ToString);

        }
        catch (Exception e)
        {
            LogTo.Info(e.ToString);

        }
        finally
        {
            DbUtils.CloseMySqlDbConnection();
        }

    }

    public void UpdateCategories(Category category, string id)
    {
        try
        {
            using var connection = DbUtils.GetMySqlDbConnection();
            connection.Open();

            using var command = MySqlUtils.CreateUpdateStatement(category, connection, $"id = {id}");
            command.ExecuteNonQuery();

        }
        catch (MySqlException e)
        {
            LogTo.Info(e.ToString);

        }
        catch (Exception e)
        {
            LogTo.Info(e.ToString);

        }
        finally
        {
            DbUtils.CloseMySqlDbConnection();
        }
    }
    public int DeleteCategory(string id)
    {
        try
        {
            using var connection = DbUtils.GetMySqlDbConnection();
            connection.Open();

            using var command = MySqlUtils.CreateUpdateStatusForDelete(typeof(Category).Name, connection, id);
            return command.ExecuteNonQuery();
        }
        catch (MySqlException e)
        {
            LogTo.Info(e.ToString);

        }
        catch (Exception e)
        {
            LogTo.Info(e.ToString);

        }
        finally
        {
            DbUtils.CloseMySqlDbConnection();
        }

        return 0;
    }
    }
