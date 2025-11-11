
using Newtonsoft.Json;

namespace Evaluation.SharedHelper.Models.Admin
{
    public class OrderingDTO
    {

        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "OrderNo")]
        public int OrderNo { get; set; }

    }

   

   


}