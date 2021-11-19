using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DOTNET_REST_WEB_API.Helper
{
    public class PropertyType
    {
        public static DbType GetType(object model)
        {
            /* 
          * used .PropertyType 
         string - string - DbType.String 
         int32 - int  - DbType.Int32 
         int64 - Long - DbType.Int64 
         double - double - DbType.Double 
         single - float - DbType.Single 
         decimal - decimal - DbType.Decimal 
         char - char - DbType.String 
         boolean - bool - DbType.Boolean 
         */
            var properties = model.ToString().Split('.')[1].ToLower();

            if (properties == "string") { return DbType.String; }
            else if (properties == "int32") { return DbType.String; }
            else if (properties == "int64") { return DbType.Int64; }
            else if (properties == "double") { return DbType.Decimal; }
            else if (properties == "single") { return DbType.Single; }
            else if (properties == "decimal") { return DbType.Decimal; }
            else if (properties == "char") { return DbType.String; }
            else if (properties == "boolean") { return DbType.Boolean; }
            else if (properties == "datetime") { return DbType.DateTime; }
            else { return DbType.Object; }
        }

        public static DynamicParameters parameters(object model)
        {
            var param = new DynamicParameters();
            foreach (var item in model.GetType().GetProperties())
            {
                param.Add(item.Name, item.GetValue(model), GetType(item.PropertyType));
            }
            return param;
        }
    }
}
