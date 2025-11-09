
using Newtonsoft.Json;

namespace Evaluation.Api.Extensions
{
    public static class AppExtensions
    {
       
        public static TSource StringToObject<TSource>(this string source)
        {
           
            var result = JsonConvert.DeserializeObject<TSource>(source);
            if (result == null)
                throw new InvalidOperationException($"Failed to deserialize JSON into {typeof(TSource).Name}");

            return result;
        }
       

       
    }
}
