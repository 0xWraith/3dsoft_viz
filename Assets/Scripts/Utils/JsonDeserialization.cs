using System;
using System.Collections.Generic;
using UnityEngine;
using Softviz.Graph;

namespace Utils
{
    /* 
     * Tento súbor obsahuje jednotlivé pomocné triedy, ktoré sú potrebné pre deserializáciu jsonov, ktoré posiela Lua.
     * Triedy, ktoré so sebou súvisia, sú oddelené regiónmi.
     * Pre deserializáciu konkrétneho typu jsonu sa triedy musia vytvoriť tak, aby sedeli názvy atribútov a aj ich štruktúra.
     * Viac k tvorbe takýchto tried na https://docs.unity3d.com/ScriptReference/JsonUtility.FromJson.html
     */

    /// <summary>
    /// Abstraktná trieda, z ktorej by mali dediť všetky triedy, ktoré predstavujú Column. 
    /// </summary>
    public abstract class AttributeColumn
    {
        /// <summary>
        /// Metóda, ktorá údaje z Column, ktoré poslala Lua, zaobalí do Dictionary.
        /// </summary>
        /// <returns>Dictionary, kde kľúč bude id (uzla, hrany a pod.) a value budú údaje z Lui.</returns>
        public abstract object ToDictionary();
    }

    #region Column Node Id
    /// <summary>
    /// Trieda, ktorá predstavuje atribút id uzla. Pomocná trieda pre deserializáciu jsonu.
    /// </summary>
    [System.Serializable]
    public class ColumnNodeId : AttributeColumn
    {
        /// <summary>
        /// Údaje z Lui pre atribút id uzla zabalené do listu.
        /// </summary>
        public List<NodeId> list;

        public override object ToDictionary()
        {
            throw new NotImplementedException();
        }

    }

    [System.Serializable]
    public class NodeId
    {
        public int id;
    }
    #endregion

    #region Column Node Position
    /// <summary>
    /// Trieda, ktorá predstavuje atribút pozícia uzla. Pomocná trieda pre deserializáciu jsonu.
    /// </summary>
    [System.Serializable]
    public class ColumnNodePosition : AttributeColumn
    {
        /// <summary>
        /// Údaje z Lui pre atribút pozícia uzla zabalené do listu.
        /// </summary>
        public List<NodePosition> list;
        public override object ToDictionary()
        {
            Dictionary<int, UnityEngine.Vector3> dictionary = new Dictionary<int, Vector3>();

            foreach (var item in list)
            {
                dictionary.Add(item.id, new Vector3(item.position.x, item.position.y, item.position.z));
            }

            return dictionary;
        }

    }

    [System.Serializable]
    public class NodePosition
    {
        public Position position;
        public int id;
    }

    [System.Serializable]
    public class Position
    {
        public float x, y, z;
    }
    #endregion

    #region Column Node & Edge Color
    /// <summary>
    /// Trieda, ktorá predstavuje atribút farba uzla/hrany. Pomocná trieda pre deserializáciu jsonu.
    /// </summary>
    [System.Serializable]
    public class ColumnNodeEdgeColor : AttributeColumn
    {
        /// <summary>
        /// Údaje z Lui pre atribút farba uzla/hrany zabalené do listu.
        /// </summary>
        public List<NodeEdgeColor> list;

        public override object ToDictionary()
        {
            Dictionary<int, Color> dictionary = new Dictionary<int, Color>();

            foreach (var item in list)
            {
                dictionary.Add(item.id, new Color(item.color.R, item.color.G, item.color.B, item.color.A));
            }

            return dictionary;
        }
    }

    [System.Serializable]
    public class NodeEdgeColor
    {
        public MyColor color;
        public int id;
    }

    [System.Serializable]
    public class MyColor
    {
        public float R, G, B, A;
    }
    #endregion

    #region Column Node Size
    /// <summary>
    /// Trieda, ktorá predstavuje atribút veľkosť uzla. Pomocná trieda pre deserializáciu jsonu.
    /// </summary>
    [System.Serializable]
    public class ColumnNodeSize : AttributeColumn
    {
        /// <summary>
        /// Údaje z Lui pre atribút veľkosť uzla zabalené do listu.
        /// </summary>
        public List<NodeSize> list;
        public override object ToDictionary()
        {
            Dictionary<int, Vector3> dictionary = new Dictionary<int, Vector3>();

            foreach (var item in list)
            {
                dictionary.Add(item.id, new Vector3(item.size.x, item.size.y, item.size.z));
            }

            return dictionary;
        }
    }

    [System.Serializable]
    public class NodeSize
    {
        public Size size;
        public int id;

    }

    [System.Serializable]
    public class Size
    {
        public float x, y, z;
    }
    #endregion

    #region Column Node Shape
    /// <summary>
    /// Trieda, ktorá predstavuje atribút tvar uzla. Pomocná trieda pre deserializáciu jsonu.
    /// </summary>
    [System.Serializable]
    public class ColumnNodeShape : AttributeColumn
    {
        /// <summary>
        /// Údaje z Lui pre atribút tvar uzla zabalené do listu.
        /// </summary>
        public List<NodeShape2> list;
        public override object ToDictionary()
        {
            Dictionary<int, Node.AvailableShapes> dictionary = new Dictionary<int, Node.AvailableShapes>();
            Node.AvailableShapes shape = new Node.AvailableShapes();

            foreach (var item in list)
            {
                switch (item.shape.type)
                {
                    case "sphere":
                        shape = Node.AvailableShapes.sphere;
                        break;
                    case "box":
                        shape = Node.AvailableShapes.box;
                        break;
                    default:
                        break;
                }
                dictionary.Add(item.id, shape);
            }

            return dictionary;
        }
    }

    [System.Serializable]
    public class NodeShape2
    {
        public Shape shape;
        public int id;
    }

    [System.Serializable]
    public class Shape
    {
        public string type;
    }
    #endregion

    #region Column Node & Edge Label
    /// <summary>
    /// Trieda, ktorá predstavuje atribút popisok uzla/hrany. Pomocná trieda pre deserializáciu jsonu.
    /// </summary>
    [System.Serializable]
    public class ColumnNodeEdgeLabel : AttributeColumn
    {
        /// <summary>
        /// Údaje z Lui pre atribút popisok uzla/hrany zabalené do listu.
        /// </summary>
        public List<MyLabel> list;

        public override object ToDictionary()
        {
            Dictionary<int, string> dictionary = new Dictionary<int, string>();

            foreach (var item in list)
            {
                dictionary.Add(item.id, item.label);
            }

            return dictionary;
        }
    }

    [System.Serializable]
    public class MyLabel
    {
        public string label;
        public int id;
    }
    #endregion

    #region Column Node Visibility
    /// <summary>
    /// Trieda, ktorá predstavuje atribút viditeľnosť uzla. Pomocná trieda pre deserializáciu jsonu.
    /// </summary>
    [System.Serializable]
    public class ColumnNodeVisibility : AttributeColumn
    {
        /// <summary>
        /// Údaje z Lui pre atribút viditeľnosť uzla zabalené do listu.
        /// </summary>
        public List<NodeIsVisible> list;

        public override object ToDictionary()
        {
            Dictionary<int, bool> dictionary = new Dictionary<int, bool>();

            foreach (var item in list)
            {
                dictionary.Add(item.id, (item.visible.value == 0) ? false : true);
            }

            return dictionary;
        }

    }

    [System.Serializable]
    public class NodeIsVisible
    {
        public IsVisible visible;
        public int id;
    }

    [System.Serializable]
    public class IsVisible
    {
        public int value;
    }
    #endregion

    #region Column Node Metrics
    /// <summary>
    /// Trieda, ktorá predstavuje atribút metriky uzla. Pomocná trieda pre deserializáciu jsonu.
    /// </summary>
    [Serializable]
    public class ColumnNodeMetrics : AttributeColumn
    {
        /// <summary>
        /// Údaje z Lui pre atribút metriky uzla zabalené do listu.
        /// </summary>
        public List<NodeMetrics> list;

        public override object ToDictionary()
        {
            Dictionary<int, Softviz.Graph.Metrics> dictionary = new Dictionary<int, Softviz.Graph.Metrics>();

            foreach (var item in list)
            {
                dictionary.Add(item.id, new Softviz.Graph.Metrics(item.LOC, item.Cyclomatic, item.Halstead));
            }

            return dictionary;
        }
    }

    [Serializable]
    public class NodeMetrics
    {
        public int id;
        public LOC LOC;
        public Cyclomatic Cyclomatic;
        public Halstead Halstead;
    }

    [Serializable]
    public class LOC
    {
        public int Total;
        public int Blank;
        public int Comment;
        public int Code;
        public int NonEmpty;
    }

    [Serializable]
    public class Cyclomatic
    {
        public int UpperBound;
        public int UpperBoundAll;
        public int LowerBound;
        public int LowerBoundAll;
        public int Decisions;
        public int DecisionsAll;
        public int Conditions;
        public int ConditionsAll;
    }

    [Serializable]
    public class Halstead
    {
        public float Vocabulary;
        public float Length;
        public float Volume;
        public float Difficulty;
        public float Effort;
    }
    #endregion

    #region Column Node InfoflowMetrics
    /// <summary>
    /// Trieda, ktorá predstavuje atribút infoflow metriky uzla. Pomocná trieda pre deserializáciu jsonu.
    /// </summary>
    [System.Serializable]
    public class ColumnNodeInfoflowMetrics : AttributeColumn
    {
        /// <summary>
        /// Údaje z Lui pre atribút infoflow metriky uzla zabalené do listu.
        /// </summary>
        public List<InfoflowMetric> list;

        public override object ToDictionary()
        {
            Dictionary<int, Softviz.Graph.InfoflowMetrics> dictionary = new Dictionary<int, Softviz.Graph.InfoflowMetrics>();

            foreach (var item in list)
            {
                dictionary.Add(item.id, new Softviz.Graph.InfoflowMetrics(item.information_flow, item.interface_complexity, item.arguments_in, item.arguments_out));
            }

            return dictionary;
        }
    }

    [System.Serializable]
    public class InfoflowMetric
    {
        public int id;
        public int information_flow;
        public int interface_complexity;
        public int arguments_in;
        public int arguments_out;
    }
    #endregion

    #region Column Node Type
    /// <summary>
    /// Trieda, ktorá predstavuje atribút typ uzla. Pomocná trieda pre deserializáciu jsonu.
    /// </summary>
    [System.Serializable]
    public class ColumnNodeType : AttributeColumn
    {
        /// <summary>
        /// Údaje z Lui pre atribút typ uzla zabalené do listu.
        /// </summary>
        public List<NodeType> list;

        public override object ToDictionary()
        {
            Dictionary<int, string> dictionary = new Dictionary<int, string>();

            foreach (var item in list)
            {
                if (item.type != null)
                {
                    dictionary.Add(item.id, item.type);
                }
            }

            return dictionary;
        }
    }

    [System.Serializable]
    public class NodeType
    {
        public string type;
        public int id;
    }
    #endregion

    #region Column Node Body
    /// <summary>
    /// Trieda, ktorá predstavuje atribút telo uzla. Pomocná trieda pre deserializáciu jsonu.
    /// </summary>
    [System.Serializable]
    public class ColumnNodeBody : AttributeColumn
    {
        /// <summary>
        /// Údaje z Lui pre atribút telo uzla zabalené do listu.
        /// </summary>
        public List<NodeBody> list;

        public override object ToDictionary()
        {
            Dictionary<int, string> dictionary = new Dictionary<int, string>();

            foreach (var item in list)
            {
                if (item.body != null)
                {
                    dictionary.Add(item.id, item.body);
                }
            }

            return dictionary;
        }
    }

    [System.Serializable]
    public class NodeBody
    {
        public string body;
        public int id;
    }
    #endregion

    #region Column Edge DestinationId
    /// <summary>
    /// Trieda, ktorá predstavuje atribút id cieľového uzla pre hranu. Pomocná trieda pre deserializáciu jsonu.
    /// </summary>
    [System.Serializable]
    public class ColumnEdgeDestinationId : AttributeColumn
    {
        /// <summary>
        /// Údaje z Lui pre atribút id cieľového uzla pre hranu zabalené do listu.
        /// </summary>
        public List<EdgeDestinationId> list;

        public override object ToDictionary()
        {
            throw new NotImplementedException();
        }
    }

    [System.Serializable]
    public class EdgeDestinationId
    {
        public int destinationId;
        public int id;
    }
    #endregion

    #region Column Edge SourceId
    /// <summary>
    /// Trieda, ktorá predstavuje atribút id zdrojového uzla pre hranu. Pomocná trieda pre deserializáciu jsonu.
    /// </summary>
    [System.Serializable]
    public class ColumnEdgeSourceId : AttributeColumn
    {
        /// <summary>
        /// Údaje z Lui pre atribút id zdrojového uzla pre hranu zabalené do listu.
        /// </summary>
        public List<EdgeSourceId> list;

        public override object ToDictionary()
        {
            throw new NotImplementedException();
        }
    }

    [System.Serializable]
    public class EdgeSourceId
    {
        public int sourceId;
        public int id;
    }
    #endregion

    #region Column Edge IsParent
    /// <summary>
    /// Trieda, ktorá predstavuje atribút isParent pre hranu. Pomocná trieda pre deserializáciu jsonu.
    /// </summary>
    [System.Serializable]
    public class ColumnEdgeIsParent : AttributeColumn
    {
        /// <summary>
        /// Údaje z Lui pre atribút isParent pre hranu zabalené do listu.
        /// </summary>
        public List<EdgeIsParent> list;

        public override object ToDictionary()
        {
            Dictionary<int, bool> dictionary = new Dictionary<int, bool>();

            foreach (var item in list)
            {
                dictionary.Add(item.id, (item.isParent.value == 0) ? false : true);
            }

            return dictionary;
        }
    }

    [System.Serializable]
    public class EdgeIsParent
    {
        public IsParent isParent;
        public int id;
    }

    [System.Serializable]
    public class IsParent
    {
        public int value;
    }
    #endregion

    #region Column Node & Edge IsFiltered
    /// <summary>
    /// Trieda, ktorá predstavuje atribút odfiltrovanie uzla/hrany. Pomocná trieda pre deserializáciu jsonu.
    /// </summary>
    [System.Serializable]
    public class ColumnNodeEdgeIsFiltered : AttributeColumn
    {
        /// <summary>
        /// Údaje z Lui pre atribút odfiltrovanie uzla/hrany zabalené do listu.
        /// </summary>
        public List<NodeEdgeIsFiltered> list;

        public override object ToDictionary()
        {
            Dictionary<int, bool> dictionary = new Dictionary<int, bool>();

            foreach (var item in list)
            {
                dictionary.Add(item.id, (item.filtered.value == 0) ? false : true);
            }

            return dictionary;
        }
    }

    [System.Serializable]
    public class NodeEdgeIsFiltered
    {
        public IsFiltered filtered;
        public int id;
    }

    [System.Serializable]
    public class IsFiltered
    {
        public int value;
    }
    #endregion
}
