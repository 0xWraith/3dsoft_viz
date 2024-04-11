// using System.Collections.Generic;
// using System.Text.Json.Serialization;


// public class Edge
// {
//     public string Label { get; private set; }
//     [JsonIgnore] //Property list je len na moje interne ucely, mozes zmazat, neserializuje sa
//     public IReadOnlyCollection<(string, string)> PropertyList { get; private set; }
//     public IReadOnlyDictionary<string, string> Properties { get; private set; }
//     public string From { get; private set; }
//     public string To { get; private set; }

//     public Edge(string label, IDictionary<string, string> properties) //tento konstruktor je tiez len pre mna, asi ho mozes tiez zmazat
//     {
//         Label = label;

//         var dict = new Dictionary<string, string>();
//         var propList = new List<(string, string)>();
//         foreach (var prop in properties)
//         {
//             dict[prop.Key] = prop.Value;
//             propList.Add((prop.Key, prop.Value));
//         }

//         Properties = dict;
//         PropertyList = propList;
//         From = Properties["from"];
//         To = Properties["to"];
//     }

// }
