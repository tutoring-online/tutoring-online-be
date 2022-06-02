using System.Reflection;
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
}