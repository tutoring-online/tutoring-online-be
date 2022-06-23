using System.Reflection;
using System.Runtime.CompilerServices;
using Anotar.NLog;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

namespace DataAccess.Utils;

public class MySqlUtils
{
    //TODO Refactor for ignore install CreatedDate
    public static MySqlCommand CreateInsertStatement(IEnumerable<object> entities, MySqlConnection connection)
    {
        
        var insertStatement = "";
        var insertParameter = "";
        var tableName = "";
        var valueParameter = "";
        
        //Construct insert parameters
        var entity = entities.ElementAt(0);
        var parameters = entity.GetType().GetProperties();

        for (int i = 0; i < parameters.Length; i++)
        {
            if (i == parameters.Length - 1)
            {
                insertParameter += parameters[i].Name;
                continue;
            }

            insertParameter = insertParameter + parameters[i].Name + ",";
        }
        
        //tableName = entityName
        tableName = entity.GetType().Name;
        
        //Construct value parameters
        for (int i = 0; i < entities.Count(); i++)
        {
            if (i == entities.Count() - 1)
            {
                var tmp = "";
                for (int z = 0; z < parameters.Length; z++)
                {
                    if (z == parameters.Length - 1)
                    {
                        tmp = tmp + "@" + parameters[z].Name + i;
                        continue;
                    }

                    tmp = tmp + "@" + parameters[z].Name + i + ",";
                }

                valueParameter += $"({tmp})";
            }
            else
            {
                var tmp = "";
                for (int z = 0; z < parameters.Length; z++)
                {
                    if (z == parameters.Length - 1)
                    {
                        tmp = tmp + "@" + parameters[z].Name + i;
                        continue;
                    }

                    tmp = tmp + "@" + parameters[z].Name + i + ",";
                }

                valueParameter = valueParameter + $"({tmp})" + ",";
            }
        }
        
        //construct final insertStatement
        insertStatement = $"Insert into {tableName} ({insertParameter}) values {valueParameter}";

        //construct command
        var command = DbUtils.CreateMySqlCommand(insertStatement, connection);
        command.CommandText = insertStatement;
        
        for (int i = 0; i < entities.Count(); i++)
        {
            for (int z = 0; z < parameters.Length; z++)
            {
                PropertyInfo propertyInfo = entities.ElementAt(i).GetType().GetProperty(parameters[z].Name);
                command.Parameters.AddWithValue("@" + parameters[z].Name + i, propertyInfo.GetValue(entities.ElementAt(i), null));
            }
        }
        LogTo.Info($"Sql string: {command.CommandText}");
        command.Prepare();

        return command;
    }

    public static MySqlCommand CreateUpdateStatement(object entity, MySqlConnection connection, string condition)
    {
        var updateStatement = "";
        var updateParameter = "";
        var tableName = "";
        var valueParameter = "";
        
        var properties = entity.GetType().GetProperties();
        
        //Construct update Parameter
        var updateParameters = new List<string>();
        var updateDic = new Dictionary<string, PropertyInfo>();
 
        for (int z = 0; z < properties.Length; z++)
        {
            PropertyInfo propertyInfo = entity.GetType().GetProperty(properties[z].Name);
            if (propertyInfo.GetValue(entity, null) is not null)
            {
                updateParameters.Add(properties[z].Name + "=" + "@" + properties[z].Name);
                updateDic.Add("@" + properties[z].Name, propertyInfo);
            }

        }

        updateParameter = string.Join(",", updateParameters);

        updateStatement = $"Update {entity.GetType().Name} " +
                          $"Set {updateParameter} " +
                          $"Where {condition} ";

        //construct command
        var command = DbUtils.CreateMySqlCommand(updateStatement, connection);
        command.CommandText = updateStatement;

        foreach (var x in updateDic)
        {
            command.Parameters.AddWithValue(x.Key, x.Value.GetValue(entity, null));
        }
        
        LogTo.Info($"Sql string: {command.CommandText}");
        command.Prepare();
        
        return command;
    }

    public static MySqlCommand CreateUpdateStatusForDelete(string tableName, MySqlConnection connection, string id)
    {
        var updateStatement = $"Update {tableName} Set Status = 0 where Id = {id}";
        
        var command = DbUtils.CreateMySqlCommand(updateStatement, connection);
        command.CommandText = updateStatement;
        
        LogTo.Info($"Sql string: {command.CommandText}");
        command.Prepare();
        
        return command;
    }

    public static string CreateOrderByStatement(List<Tuple<string, string>> orderByParams)
    {
        var orderByList = new List<string>();
        foreach (var orderByParam in orderByParams)
        {
            switch (orderByParam.Item2)
            {
                case "+":
                    orderByList.Add($"{orderByParam.Item1} ASC ");
                    break;
                case "-":
                    orderByList.Add($"{orderByParam.Item1} DESC ");
                    break;
            }
        }

        return "Order by " + string.Join(",", orderByList);
    }

    public static string CreateInStatementValues(HashSet<string?> values)
    {
        return $"({string.Join(",", values)})";
    }

    public static string ConstructQueryByStatements(List<string> queries)
    {
        return string.Join(" ", queries);
    }
    
}