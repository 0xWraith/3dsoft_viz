using System.Text.Json.Serialization; //systemova kniznica, nie je nutne instalovat 

namespace GraphHandlerServer.RabbitMq
{
    public class Message //Vzdy ma obsahovat len jednu z kombinacii {(Method, Request), (Method, Response)}
    {
        [JsonRequired]
        public string Method { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] //Nebude obsiahnuty v JSON ak jeho hodnota je null
        public string? Request { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] //Nebude obsiahnuty v JSON ak jeho hodnota je null
        public string? Response { get; set; }
    }
}
