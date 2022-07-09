using Anotar.NLog;
using DataAccess.Entities.Category;
using DataAccess.Models;
using DataAccess.Models.Category;
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

    public Dictionary<string, Category> GetCategories(HashSet<string> ids)
    {
        var categories = new Dictionary<string, Category?>();

        try
        {
            using var connection = DbUtils.GetMySqlDbConnection();
            connection.Open();

            var selectStatement = "Select Id, Name, Description, CreatedDate, UpdatedDate, Type, Status";
            var fromStatement = "From Category";
            var whereSatement = $"Where id In ( {MySqlUtils.CreateInStatementValues(ids)})";
            var query = MySqlUtils.ConstructQueryByStatements(new List<string>
            {
                selectStatement,
                fromStatement,
                whereSatement
            });

            using var command = DbUtils.CreateMySqlCommand(query, connection);

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                categories.Add(
                    DbUtils.SafeGetString(reader, "Id"),
                    new Category { 
                        Id = DbUtils.SafeGetString(reader, "Id"),
                        Name = DbUtils.SafeGetString(reader, "Name"),
                        Description = DbUtils.SafeGetString(reader, "Description"),
                        CreatedDate = DbUtils.SafeGetDateTime(reader, "CreatedDate"),
                        UpdatedDate = DbUtils.SafeGetDateTime(reader, "UpdatedDate"),
                        Type = DbUtils.SafeGetInt16(reader, "Type"),
                        Status = DbUtils.SafeGetInt16(reader, "Status")
                        }
                    );

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
    public Page<Category?> GetCategories(int? limit, int? offSet, List<Tuple<string, string>> orderByParams,
        SearchCategoryDto searchCategoryDto, bool isNotPaging)
    {
        var page = new Page<Category>();
        page.Pagination = new PageDetail();
        var categories = new List<Category?>();

        try
        {
            using var connection = DbUtils.GetMySqlDbConnection();
            connection.Open();
            var param1 = "@id";

            var selectStatement = "Select Id, Name, Description, CreatedDate, UpdatedDate, Type, Status";
            var selectCountStatement = "Select count(id) as TotalElement";
            var fromStatement = "From Category";
            var whereStatement = $"Where (@Name is null or Name = @Name)" +
                                 $"and (@Description is null or Description = @Description)" +
                                 $"and (@FromCreatedDate is null or CreatedDate >= @FromCreatedDate)" +
                                 $"and (@ToCreatedDate is null or CreatedDate <= @ToCreatedDate)" +
                                 $"and (@FromUpdatedDate is null or UpdatedDate >= @FromUpdatedDate)" +
                                 $"and (@ToUpdatedDate is null or UpdatedDate <= @ToUpdatedDate)" +
                                 $"and (@Type is null or Type = @Type) " +
                                 $"and (@Status is null or Status = @Status) ";
            var orderByStatement = MySqlUtils.CreateOrderByStatement(orderByParams);
            var limitStatement = $"Limit {limit} offSet {offSet}";

            if (!orderByParams.Any())
                orderByStatement = "";

            if (isNotPaging)
                limitStatement = "";

            var listStatement1 = new List<string>();
            listStatement1.Add(selectStatement);
            listStatement1.Add(fromStatement);
            listStatement1.Add(whereStatement);
            listStatement1.Add(orderByStatement);
            listStatement1.Add(limitStatement);

            var query = string.Join(" ", listStatement1);

            using var command = DbUtils.CreateMySqlCommand(query, connection);

            command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = searchCategoryDto.Name;
            command.Parameters.Add("@Description", MySqlDbType.VarChar).Value = searchCategoryDto.Description;
            command.Parameters.Add("@FromCreatedDate", MySqlDbType.DateTime).Value = searchCategoryDto.FromCreatedDate;
            command.Parameters.Add("@ToCreatedDate", MySqlDbType.DateTime).Value = searchCategoryDto.ToCreatedDate;
            command.Parameters.Add("@FromUpdatedDate", MySqlDbType.DateTime).Value = searchCategoryDto.FromUpdatedDate;
            command.Parameters.Add("@ToUpdatedDate", MySqlDbType.DateTime).Value = searchCategoryDto.ToUpdatedDate;
            command.Parameters.Add("@Type", MySqlDbType.VarChar).Value = searchCategoryDto.Type;
            command.Parameters.Add("@Status", MySqlDbType.VarChar).Value = searchCategoryDto.Status;

            command.Prepare();

            foreach (MySqlParameter commandParameter in command.Parameters)
            {
                LogTo.Info($"Param {commandParameter}: {commandParameter.Value}");
            }

            var reader = command.ExecuteReader();

            while (reader.Read())
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
            reader.Close();
            page.Data = categories;

            var listStatement2 = new List<string>();
            listStatement2.Add(selectCountStatement);
            listStatement2.Add(fromStatement);
            listStatement2.Add(whereStatement);

            query = string.Join(" ", listStatement2);
            command.CommandText = query;
            reader = command.ExecuteReader();
            page.Pagination.TotalItems = reader.Read() ? DbUtils.SafeGetInt16(reader, "TotalElement") : 0;

        }
        catch (MySqlException e)
        {
            LogTo.Info(e.ToString());
        }
        catch (Exception e)
        {
            LogTo.Info(e.ToString());
        }
        finally
        {
            DbUtils.CloseMySqlDbConnection();
        }

        return page;
    }
}
