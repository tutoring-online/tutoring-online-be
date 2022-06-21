using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using DataAccess.Utils;
using tutoring_online_be.Security;

namespace tutoring_online_be.Utils;

public class AppUtils
{
    public static Boolean HaveQueryString(object o)
    {
        var properties = o.GetType().GetProperties();

        return properties.Any(propertyInfo => propertyInfo.GetValue(o, null) is not null);
    }

     public static List<Tuple<string, string>> SortFieldParsing(string sortQuery, Type type)
     {
         sortQuery = sortQuery.ToLower();
         int i = 1;
      
         var exception = new AppException.ValidationFailException();
         var regex = new Regex(",");
         var tmp = regex.Split(sortQuery);
         var propertyMap = new List<Tuple<string, string>>();

         if (tmp.Length != tmp.Distinct().Count())
         {
             exception.Data.Add(i++, "Sort field are duplicate");
         }

         int z = 0;
         foreach (var s in tmp)
         {
             if(s.StartsWith("-")) 
                 propertyMap.Add(Tuple.Create(s.Replace("-",""), "-"));
             else if(s.StartsWith("+"))
                 propertyMap.Add(Tuple.Create(s.Replace("+", ""), "+"));
             else
                 exception.Data.Add(i++ ,$"Invalid order by expression for {s}");
         }
         
         var properties = type
             .GetProperties()
             .Select(t => t.Name.ToLower())
             .ToList();
         
         foreach (var keyValuePair in propertyMap)
         {
             if(!properties.Contains(keyValuePair.Item1))
                 exception.Data.Add(i++, $"Field {keyValuePair.Item1} not found");
         }

         if (exception.Data.Count > 0)
             throw exception;

         return propertyMap;

     }
}