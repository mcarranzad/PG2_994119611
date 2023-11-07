using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Primitives;

namespace sgc.ml.Util
{
    public class SgcFunctions
    {
        public static string JsonDebug(object model)
        {
            try
            {
                return JsonSerializer.Serialize(model);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        
        public static string QueryTransform(string str, string column)
        {
            return new StringBuilder()
                .Append(string.Format(" select ROW_NUMBER() over (ORDER BY {0} ) id, T1.* from ( \n ", column))
                .Append(str)
                .Append("  ) As T1 \n ").ToString();
        }
       

        public static bool Isnull(object getData) => getData == null;

   

        public static DateTime getNow()
        {
            return DateTime.Now;
        }
    }
}