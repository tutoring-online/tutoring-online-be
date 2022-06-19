using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using tutoring_online_be.Security;

namespace tutoring_online_be.Utils;

public class AppUtils
{
    public static Boolean HaveQueryString(object o)
    {
        var properties = o.GetType().GetProperties();

        return properties.Any(propertyInfo => propertyInfo.GetValue(o, null) is not null);
    }

     public static Dictionary<string, string> SortFieldParsing(string sortQuery, Type type)
     {
         sortQuery = sortQuery.ToLower();
         int i = 1;
      
         var exception = new AppException.ValidationFailException();
         var regex = new Regex(",");
         var tmp = regex.Split(sortQuery);
         var propertyMap = new Dictionary<string, string>();

         if (tmp.Length != tmp.Distinct().Count())
         {
             exception.Data.Add(i++, "Sort field are duplicate");
         }
         
         foreach (var s in tmp)
         {
             if(s.StartsWith("-")) 
                 propertyMap.Add(s.Replace("-", ""), "-");
             else if(s.StartsWith("+"))
                 propertyMap.Add(s.Replace("+", ""), "+");
             else
                 exception.Data.Add(i++ ,$"Invalid order by expression for {s}");
         }
         
         var properties = type
             .GetProperties()
             .Select(t => t.Name.ToLower())
             .ToList();
         
         foreach (var keyValuePair in propertyMap)
         {
             if(!properties.Contains(keyValuePair.Key))
                 exception.Data.Add(i++, $"Field {keyValuePair.Key} not found");
         }

         if (exception.Data.Count > 0)
             throw exception;

         return propertyMap;

     }
}