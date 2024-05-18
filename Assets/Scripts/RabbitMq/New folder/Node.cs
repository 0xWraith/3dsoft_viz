using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace GraphHandlerServer.RabbitMq
{
    public class Node
{
    [JsonIgnore] //Label je len na moje interne ucely, mozes zmazat, neserializuje sa
    public string Label { get; private set; }
    public IReadOnlyCollection<string> Labels { get; private set; } 
    [JsonIgnore]
    public IReadOnlyCollection<(string, string)> PropertyList { get; private set; } //PropertyList je len na moje interne ucely, mozes zmazat, neserializuje sa
    public IReadOnlyDictionary<string, string> Properties { get; private set; }

    public Node(IEnumerable<string> labels, IDictionary<string, string> properties) //tento konstruktor je tiez len pre mna, asi ho mozes tiez zmazat
    {
        Labels = labels.ToList();
        Label = Labels.First();

        var dict = new Dictionary<string, string>();  
        var propList = new List<(string, string)>();
        foreach (var prop in properties)
        {
            dict[prop.Key] = prop.Value;
            propList.Add((prop.Key, prop.Value));   
        }

        Properties = dict;
        PropertyList = propList;
    }

}
}
