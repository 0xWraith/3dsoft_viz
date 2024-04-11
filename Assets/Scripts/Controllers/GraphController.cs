using UnityEngine;
using Utils;
using Softviz.Graph;
using System.Linq;
using System.Collections.Generic;
using Communication;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace Softviz.Controllers
{
    public class GraphController : SingletonBase<GraphController>
    {
        public List<int> selectedNodes = new List<int>();

        [SerializeField]
        public Graph.Graph graph;
        private bool scalingEnabled = true;

        public Node[] GetNodes()
        {
            return graph.Nodes.Values.ToArray<Node>();
        }
        //public void Start()
        //{
        //    LoadGraph();
        //}
        //public void Update()
        //{
        //    if (Input.GetKeyDown("space"))
        //    {
        //        RunLayouting();
        //        updatePeriodicaly = true;
        //        //cityFlag = true;

        //        ///// Nadstavenie layoutovacieho algoritmu na City
        //        //config.actualLayoutAlgorithm = Enums.LayoutAlgorithm.City;
        //        ///// Updatnutie configurácie (zavolanie funkcie ktorá pošle informáciu o zmene konfigurácií do luaserveru)
        //        //UpdateGraphConfiguration();
        //    }
        //}

        [SerializeField]
        private GraphConfiguration config = new GraphConfiguration();

        private bool layoutingRunning = false;

        // TODO docasne riesenie, po implementacii legion clustera
        //  sa data budu pushovat zo servera
        private float lastUpdateSeconds;

        public bool LayoutingRunning { get => layoutingRunning; }
        private bool updatePeriodicaly = false;
        private bool hierarchyFlag = false;

        //public bool LayoutingRunning { get => layoutingRunning; }
        public bool UpdatePeriodicaly { get => updatePeriodicaly; set => updatePeriodicaly = value; }
        public bool UpdateHierarchy{ get => hierarchyFlag; set => hierarchyFlag = value; }
        public GraphConfiguration GraphConfiguration { get => config; set => config = value; }

        public void RunLayouting()
        {
            API_out.RunLayouting();
            API_out.UpdateNodes();
            layoutingRunning = true;
        }

        public void PauseAlgorithm()
        {
            API_out.PauseAlgorithm();
            layoutingRunning = false;
        }

        public void UpdateGraphConfiguration()
        {
            API_out.ChangeLayoutAlgorithm(config);
            API_out.Initialize();
            API_out.RunLayouting();
        }

        public void ResumeAlgorithm()
        {
            API_out.ResumeAlgorithm();
            layoutingRunning = true;
        }

        public void SetNodePosition(int nodeId, Vector3 position)
        {
            API_out.SetNodePosition(nodeId, position);
        }

        public void SetScalingEnabled(bool enabled)
        {
            scalingEnabled = enabled;
        }

        public void CreateNodes(string idsRaw)
        {
            ColumnNodeId nodesId = JsonToColumn<ColumnNodeId>(idsRaw);
            graph.CreateNodes(nodesId);
        }

        // TODO 
        private string tmpEdgesSrc = "";
        private string tmpEdgesDest = "";
        public void CreateEdgesSrc(string sourcesRaw)
        {
            tmpEdgesSrc = sourcesRaw;
            if (tmpEdgesSrc.Length > 0)
            {
                CreateEdges();
            }
        }
        public void CreateEdgesDst(string destinationsRaw)
        {
            tmpEdgesDest = destinationsRaw;
            if (tmpEdgesSrc.Length > 0)
            {
                CreateEdges();
            }
        }
        private void CreateEdges()
        {
            // TODO Deserializácia jsonov s id-čkami zdrojových a cieľových uzlov, medzi ktorými neskôr vzniknú hrany.
            var destination = JsonToColumn<ColumnEdgeDestinationId>(tmpEdgesDest);
            var source = JsonToColumn<ColumnEdgeSourceId>(tmpEdgesSrc);
            tmpEdgesDest = "";
            tmpEdgesSrc = "";

            graph.CreateEdges(destination, source);
        }

        /// <summary>
        /// Metóda, ktorá podľa Lui updatne farbu všetkým uzlom. 
        /// Funguje tak, že Lua pošle údaje vo formáte json, ktoré sa následne deserializujú, uložia sa do zoznamu (dictionary) ako id uzla + Color 
        /// a pre každý aktívny uzol v tomto zozname sa zavolá príslušný update v Node, ktorý už vykoná viditeľné zmeny v Unity.
        /// </summary>
        public void UpdateNodesColor(string colorsRaw)
        {
            Dictionary<int, Color> dictionary = JsonToColumn<ColumnNodeEdgeColor>(colorsRaw).ToDictionary() as Dictionary<int, Color>;
            graph.UpdateNodesColor(dictionary);
        }

        /// <summary>
        /// Metóda, ktorá podľa Lui updatne pozíciu všetkým uzlom. 
        /// Funguje tak, že Lua pošle údaje vo formáte json, ktoré sa následne deserializujú, uložia sa do zoznamu (dictionary) ako id uzla + pozícia ako Vector3 
        /// a pre každý aktívny uzol v tomto zozname sa zavolá príslušný update v Node, ktorý už vykoná viditeľné zmeny v Unity.
        /// </summary>
        public void UpdateNodesPosition(string positionsRaw)
        {
            Dictionary<int, Vector3> dictionary = JsonToColumn<ColumnNodePosition>(positionsRaw).ToDictionary() as Dictionary<int, Vector3>;
            graph.UpdateNodesPosition(dictionary);
        }

        /// <summary>
        /// Metóda, ktorá podľa Lui updatne veľkosť všetkým uzlom. 
        /// Funguje tak, že Lua pošle údaje vo formáte json, ktoré sa následne deserializujú, uložia sa do zoznamu (dictionary) ako id uzla + veľkosť ako Vector3 
        /// a pre každý aktívny uzol v tomto zozname sa zavolá príslušný update v Node, ktorý už vykoná viditeľné zmeny v Unity.
        /// </summary>
        public void UpdateNodesSize(string sizesRaw)
        {
            Dictionary<int, Vector3> dictionary = JsonToColumn<ColumnNodeSize>(sizesRaw).ToDictionary() as Dictionary<int, Vector3>;
            graph.UpdateNodesSize(dictionary);
        }

        /// <summary>
        /// Metóda, ktorá podľa Lui updatne tvar všetkým uzlom. 
        /// Funguje tak, že Lua pošle údaje vo formáte json, ktoré sa následne deserializujú, uložia sa do zoznamu (dictionary) ako id uzla + jeden z dostupných tvarov (AvailableShapes)
        /// a pre každý aktívny uzol v tomto zozname sa zavolá príslušný update v Node, ktorý už vykoná viditeľné zmeny v Unity.
        /// </summary>
        public void UpdateNodesShapes(string shapesRaw)
        {
            Dictionary<int, Node.AvailableShapes> dictionary = JsonToColumn<ColumnNodeShape>(shapesRaw).ToDictionary() as Dictionary<int, Node.AvailableShapes>;
            graph.UpdateNodesShapes(dictionary);
        }

        /// <summary>
        /// Metóda, ktorá podľa Lui updatne popisok všetkým uzlom. 
        /// Funguje tak, že Lua pošle údaje vo formáte json, ktoré sa následne deserializujú, uložia sa do zoznamu (dictionary) ako id uzla + popisok ako string 
        /// a pre každý aktívny uzol v tomto zozname sa zavolá príslušný update v Node, ktorý už vykoná viditeľné zmeny v Unity.
        /// </summary>
        public void UpdateNodesLabel(string labelsRaw)
        {
            Dictionary<int, string> dictionary = JsonToColumn<ColumnNodeEdgeLabel>(labelsRaw).ToDictionary() as Dictionary<int, string>;
            graph.UpdateNodesLabel(dictionary);
        }

        /// <summary>
        /// Metóda, ktorá podľa Lui updatne všetkým uzlom, či sú viditeľné alebo nie. 
        /// Funguje tak, že Lua pošle údaje vo formáte json, ktoré sa následne deserializujú, uložia sa do zoznamu (dictionary) ako id uzla + viditeľnosť ako bool 
        /// a pre každý aktívny uzol v tomto zozname sa zavolá príslušný update v Node, ktorý už vykoná viditeľné zmeny v Unity.
        /// </summary>
        public void UpdateNodesVisibility(string visibilitiesRaw)
        {
            Dictionary<int, bool> dictionary = JsonToColumn<ColumnNodeVisibility>(visibilitiesRaw).ToDictionary() as Dictionary<int, bool>;
            graph.UpdateNodesVisibility(dictionary);
        }

        /// <summary>
        /// Metóda, ktorá podľa Lui updatne všetkým uzlom, či majú byť odfiltrované alebo nie. 
        /// Funguje tak, že Lua pošle údaje vo formáte json, ktoré sa následne deserializujú, uložia sa do zoznamu (dictionary) ako id uzla + odfiltrovanie ako bool 
        /// a pre každý aktívny uzol v tomto zozname sa zavolá príslušný update v Node, ktorý už vykoná viditeľné zmeny v Unity.
        /// </summary>
        public void UpdateNodesFiltered(string filteredRaw)
        {
            Dictionary<int, bool> dictionary = JsonToColumn<ColumnNodeEdgeIsFiltered>(filteredRaw).ToDictionary() as Dictionary<int, bool>;
            graph.UpdateNodesFiltered(dictionary);
        }

        public void UpdateNodesIsFixed(string isFixedRaw)
        {
            Dictionary<int, bool> dictionary = JsonToColumn<ColumnNodeEdgeIsFiltered>(isFixedRaw).ToDictionary() as Dictionary<int, bool>;
            Debug.Log("Frozen nodes: ");
            foreach (var id in dictionary.Keys)
            {
                if (dictionary[id])
                {
                    Debug.Log(id);
                }
            }           
        }

        /// <summary>
        /// Metóda, ktorá podľa Lui updatne farbu všetkým hranám. 
        /// Funguje tak, že Lua pošle údaje vo formáte json, ktoré sa následne deserializujú, uložia sa do zoznamu (dictionary) ako id hrany + Color 
        /// a pre každú aktívnu hranu v tomto zozname sa zavolá príslušný update v Edge, ktorý už vykoná viditeľné zmeny v Unity.
        /// </summary>
        public void UpdateEdgesColor(string rawColors)
        {
            Dictionary<int, Color> dictionary = JsonToColumn<ColumnNodeEdgeColor>(rawColors).ToDictionary() as Dictionary<int, Color>;
            graph.UpdateEdgesColor(dictionary);
        }

        /// <summary>
        /// Metóda, ktorá podľa Lui updatne popisok všetkým hranám. 
        /// Funguje tak, že Lua pošle údaje vo formáte json, ktoré sa následne deserializujú, uložia sa do zoznamu (dictionary) ako id hrany + popisok ako string 
        /// a pre každú aktívnu hranu v tomto zozname sa zavolá príslušný update v Edge, ktorý už vykoná viditeľné zmeny v Unity.
        /// </summary>
        public void UpdateEdgesLabel(string labelsRaw)
        {
            Dictionary<int, string> dictionary = JsonToColumn<ColumnNodeEdgeLabel>(labelsRaw).ToDictionary() as Dictionary<int, string>;
            graph.UpdateEdgesLabel(dictionary);
        }

        /// <summary>
        /// Metóda, ktorá podľa Lui updatne všetkým hranám, či je parent alebo nie. 
        /// Funguje tak, že Lua pošle údaje vo formáte json, ktoré sa následne deserializujú, uložia sa do zoznamu (dictionary) ako id hrany + bool, či je parent 
        /// a pre každú aktívnu hranu v tomto zozname sa zavolá príslušný update v Edge, ktorý už vykoná viditeľné zmeny v Unity.
        /// </summary>
        public void UpdateEdgesParent(string parentsRaw)
        {
            Dictionary<int, bool> dictionary = JsonToColumn<ColumnEdgeIsParent>(parentsRaw).ToDictionary() as Dictionary<int, bool>;
            graph.UpdateEdgesParent(dictionary);
        }

        /// <summary>
        /// Metóda, ktorá podľa Lui updatne všetkým hranám, či je odfiltrovaná alebo nie. 
        /// Funguje tak, že Lua pošle údaje vo formáte json, ktoré sa následne deserializujú, uložia sa do zoznamu (dictionary) ako id hrany + odfiltrovanie ako bool 
        /// a pre každú aktívnu hranu v tomto zozname sa zavolá príslušný update v Edge, ktorý už vykoná viditeľné zmeny v Unity.
        /// </summary>
        public void UpdateEdgesFiltered(string filteredRaw)
        {
            Dictionary<int, bool> dictionary = JsonToColumn<ColumnNodeEdgeIsFiltered>(filteredRaw).ToDictionary() as Dictionary<int, bool>;
            graph.UpdateEdgesFiltered(dictionary);
        }

        public void UpdateNodesInfoflowMetrics(string result)
        {
            Dictionary<int, InfoflowMetrics> dictionary = JsonToColumn<ColumnNodeInfoflowMetrics>(result).ToDictionary() as Dictionary<int, InfoflowMetrics>;
            graph.UpdateNodesInfoflowMetrics(dictionary);
        }

        public void UpdateNodesType(string result)
        {
            Dictionary<int, string> dictionary = JsonToColumn<ColumnNodeType>(result).ToDictionary() as Dictionary<int, string>;
            graph.UpdateNodesType(dictionary);

        }

        public void UpdateNodesBody(string result)
        {
            Dictionary<int, string> dictionary = JsonToColumn<ColumnNodeBody>(result).ToDictionary() as Dictionary<int, string>;
            graph.UpdateNodesBody(dictionary);
        }

        public void UpdateNodesMetrics(string result)
        {
            Dictionary<int, Metrics> dictionary = JsonToColumn<ColumnNodeMetrics>(result).ToDictionary() as Dictionary<int, Metrics>;
            graph.UpdateNodesMetrics(dictionary);
        }

        /// <summary>
        /// Metóda, ktorá deserializuje vstupný json na objekty, z ktorých sa stanú komponenty a pod.
        /// </summary>
        /// <typeparam name="T">Typ objektu, na ktorý sa má json deserializovať. Možné typy objektov sa nachádzajú v súbore ClassesForDeserializeJson.</typeparam>
        /// <param name="json">String vo formáte json, ktorý nám vrátila Lua.</param>
        /// <returns>Objekt daného typu vytvorený z jsonu.</returns>
        private T JsonToColumn<T>(string json)
        {
            // Kvôli tomu, ako pracuje JsonUtility, potrebujeme zaobaliť celý json do zátvoriek a nejak ho nazvať (napr. list).
            json = "{ \"list\" :" + json + "}";

            // Použijeme JsonUtility, ktorý namapuje string na objekt.
            T list = JsonUtility.FromJson<T>(json);

            return list;
        }

        
        /// <summary>
        /// A method that calls the update of all node components.
        /// </summary>
        public void UpdateNodes()
        {
            API_out.GetNodePositionColumn();
            API_out.GetNodeColorColumn();
            API_out.GetNodeSizeColumn();
            API_out.GetNodeShapeColumn();
            API_out.GetNodeFilteredColumn();

            // API_out.GetNodeMetricsColumn();
            // API_out.GetNodeInfoflowMetricsColumn();
            // API_out.GetNodeLabelColumn();
        }

        /// <summary>
        /// A method that calls the update of all edge components.
        /// </summary>
        public void UpdateEdges()
        {
            API_out.GetEdgeColorColumn();
            API_out.GetEdgeIsParentColumn();
            API_out.GetEdgeFilteredColumn();
        }

        protected override void Start()
        {
            API_out.LoadGraph(Enums.LayoutAlgorithm.FruchtermanReingold.ToString());
            // API_out.LoadGraph("FruchtermanReingoldNew");
            API_out.Initialize();
            API_out.UpdateNodes();

            // todo(hrumy): WIP, bugs out
            // API_out.GetNodeMetricsColumn();
            // API_out.GetNodeLabelColumn();
            // API_out.GetNodeInfoflowMetricsColumn();

            API_out.GetNodeIdColumn();
            API_out.GetNodeShapeColumn();
            API_out.GetNodeColorColumn();
            API_out.GetNodeSizeColumn();
            API_out.GetNodeLabelColumn();
            API_out.GetNodeTypeColumn();

            API_out.GetEdgeDestinationIdColumn();
            API_out.GetEdgeSourceIdColumn();
            API_out.GetEdgeColorColumn();
            API_out.GetEdgeLabelColumn();
            

            // RabbitMQ test 
            {
                // var producer = Producer.StartInstance("asdasda", "wwwww");
                // var consumer = Consumer.StartInstance("asdasdads", "asdasd");

                
                // Thread.Sleep(5000);
                // var file = File.ReadAllText(@"E:\Hromada\samples\response.json");
                // producer.AddToQueue(file);
            }
        }

        protected override void Update()
        {
            // TODO docasne riesenie
            if (Time.time >= lastUpdateSeconds + 0.25f)
            {
                lastUpdateSeconds = Time.time;
                API_out.UpdateNodes();

                UpdateNodes();
                UpdateEdges();
            }

            if (Input.GetKeyDown("backspace"))
            {
                RunLayouting();
            }
        }

        /// <summary>
        /// Unity metóda. Keď sa vypne aplikácia v Unity, prerušíme spojenie s Luou, aby si Lua neuchovávala staré nastavenia.
        /// </summary>
        private void OnApplicationQuit()
        {
            API_out.TerminateConnection();
        }
    }
}
