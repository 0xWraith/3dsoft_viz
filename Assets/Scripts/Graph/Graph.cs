using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;
using System.Collections;
using Softviz.Controllers;


namespace Softviz.Graph
{
    /// <summary>
    /// Trieda, ktorá predstavuje graf. Spravuje sa tu deserializovanie údajov a tvorba a updatovanie uzlov a hrán.
    /// </summary>
    public class Graph : MonoBehaviour
    {
        /// <summary>
        /// Zoznam všetkých označených objektov.
        /// </summary>
        /// List<ISelectableObject> selectedObjects;
        /// <summary>
        /// Zoznam všetkých uzlov. Jeden záznam = Id uzla + Komponent (skript) Node pre daný uzol
        /// </summary>
        public Dictionary<int, Node> Nodes;
        /// <summary>
        /// Zoznam všetkých hrán. Jeden záznam = Id hrany + Komponent (skript) Edge pre danú hranu
        /// </summary>
        public Dictionary<int, Edge> Edges;
        /// <summary>
        /// Predpripravený prefab pre uzol.
        /// </summary>
        [SerializeField]
        private GameObject nodePrefab;
        /// <summary>
        /// Predpripravený prefab pre hranu.
        /// </summary>
        [SerializeField]
        private GameObject edgePrefab;
        /// <summary>
        /// Objekt, do ktorého sa budú ukladať všetky vytvorené uzly.
        /// </summary>
        [SerializeField]
        private Transform nodesHolder;
        /// <summary>
        /// Objekt, do ktorého sa budú ukladať všetky vytvorené hrany.
        /// </summary>
        [SerializeField]
        private Transform edgesHolder;
        /// <summary>
        /// Pomocná bool hodnota, ktorou sa momentálne ovláda zapnutie recreatovania hierarchie objektov.
        /// V zapnutej Unity aplikácii je na to potrebné kliknúť po zvolení metafory mesta, 
        /// aby sa usporiadali objekty a správne sa zobrazila metafora mesta.
        /// </summary>

        #region Create

        /// <summary>
        /// Metóda, ktorá vytvára inštancie uzlov grafu (viditeľné objekty v spustenej aplikácii), zaradí ich do zoznamu uzlov a updatne komponenty uzlov. 
        /// </summary>
        public void CreateNodes(ColumnNodeId nodesId)
        {
            Nodes = new Dictionary<int, Node>();

            // Pre každý uzol zo zoznamu, ktorý sme získali deserializáciou jsonu, chceme vytvoriť jeho inštanciu v Unity, 
            // inicializovať jeho komponent (skript) Node a zaradiť uzol do zoznamu uzlov (Dictionary).
            foreach (var item in nodesId.list)
            {
                var v = Instantiate(nodePrefab, nodesHolder);
                v.GetComponent<Node>().Initialize(item.id, new Vector3(UnityEngine.Random.Range(-5f, 5f), UnityEngine.Random.Range(-5f, 5f), UnityEngine.Random.Range(-5f, 5f)));
                Nodes.Add(item.id, v.GetComponent<Node>());
            }
        }

        /// <summary>
        /// Metóda, ktorá vytvára inštancie hrán grafu (viditeľné objekty v spustenej aplikácii), zaradí ich do zoznamu hrán a updatne komponenty hrán. 
        /// </summary>
        public void CreateEdges(ColumnEdgeDestinationId destination, ColumnEdgeSourceId source)
        {
            Edges = new Dictionary<int, Edge>();

            // Zoznam, v ktorom si chvíľu budeme uchovávať id hrany a inštanciu triedy NodesEdgePair, čo je vlastne id zdrojového a id cieľového uzla.
            Dictionary<int, NodesEdgePair> dictionary = new Dictionary<int, NodesEdgePair>();

            // Každú hranu a zdrojový uzol uložíme do zoznamu.
            foreach (var item in source.list)
            {
                var tmp = new NodesEdgePair();
                tmp.source = item.sourceId;
                dictionary.Add(item.id, tmp);
            }

            // Každý cieľový uzol pridáme do zoznamu.
            foreach (var item in destination.list)
            {
                dictionary[item.id].destination = item.destinationId;
            }

            // Pre každú hranu vytvoríme jej inštanciu v Unity, inicializujeme jej komponent (skript) Edge a zaradíme hranu do hlavného zoznamu hrán (Dictionary). 
            foreach (var item in dictionary)
            {
                if (Edges.ContainsKey(item.Key))
                {
                    // Pre istotu - ak by sa tam už hrana nachádzala, iba ju inicializujeme.
                    Edges[item.Key].Initialize(item.Key, Nodes[item.Value.source], Nodes[item.Value.destination], this);
                }
                else
                {
                    var v = Instantiate(edgePrefab, edgesHolder);
                    v.GetComponent<Edge>().Initialize(item.Key, Nodes[item.Value.source], Nodes[item.Value.destination], this);
                    Edges.Add(item.Key, v.GetComponent<Edge>());
                }
            }
        }

        #endregion

        #region Update

        /// <summary>
        /// Unity metóda. Vykonáva sa neustále počas behu programu. Po splnení podmienok updatne uzly, hrany a recreatne hierarchiu objektov.
        /// Podmienkou updatovania grafu je zapnuté v hlavnom menu Update periodically a splnený určený časový limit.
        /// (Dočasnou) podmienkou spustenia recreatovania hierarchie je Check nastavené na true.
        /// </summary>
        private void Update()
        {
            if (GraphController.Instance.UpdateHierarchy)
            {
                GraphController.Instance.UpdateNodes();
                GraphController.Instance.UpdateEdges();
                GraphController.Instance.UpdateHierarchy = false;
                StartCoroutine(HandleIt());
            }
        }

        private IEnumerator HandleIt()
        {
            Debug.Log("Waiting to recreate hierarchy");
            yield return new WaitForSeconds(3);
            Debug.Log("Starting recreating");
            RecreateHierarchy();
        }

        /// <summary>
        /// Metóda na recreatovanie hierarchie objektov, potrebná pri metafore mesta.
        /// (Prevzatá metóda zo starého projektu.)
        /// </summary>
        public void RecreateHierarchy()
        {
            var Hierarchy = new List<Tuple<Node, Node>>();
            foreach (var edge in Edges.Values)
            {
                if (edge.IsParent && !edge.IsFiltred)
                {
                    var sourceNode = Nodes.Values.FirstOrDefault(x => x.id == edge.Source.id);
                    var destinatioNode = Nodes.Values.FirstOrDefault(x => x.id == edge.Destination.id);
                    Hierarchy.Add(new Tuple<Node, Node>(sourceNode, destinatioNode));
                }
            }
            ReloadHierarchy(true, Hierarchy);
        }

        /// <summary>
        /// Metóda, ktorá znovu načíta hierarchiu objektov. (Prevzatá metóda zo starého projektu.)
        /// </summary>
        /// <param name="resetPositions"></param>
        /// <param name="Hierarchy"></param>
        private void ReloadHierarchy(bool resetPositions, List<Tuple<Node, Node>> Hierarchy)
        {
            var nodesWithParents = new List<Node>();
            if (Hierarchy != null)
            {
                foreach (var link in Hierarchy)
                {
                    var source = Nodes.Values.FirstOrDefault(a => a.id == link.Item1.id);
                    var destination = Nodes.Values.FirstOrDefault(a => a.id == link.Item2.id);

                    nodesWithParents.Add(destination);

                    if (destination.gameObject.transform.parent == source.gameObject.transform)
                    {
                        continue;
                    }

                    destination.gameObject.transform.SetParent(source.gameObject.transform, true);

                    if (resetPositions)
                    {
                        GameObject graph = GameObject.FindWithTag("Graph");

                        destination.gameObject.transform.localPosition = new Vector3(destination.transform.position.x, destination.transform.position.y, destination.transform.position.z);
                    }
                }
            }

            foreach (var node in Nodes.Values)
            {
                if (!nodesWithParents.Contains(node) && node.gameObject.transform.parent != gameObject.transform)
                {
                    node.gameObject.transform.SetParent(gameObject.transform, true);
                }
            }
        }
        #endregion

        #region Update Nodes

        /// <summary>
        /// Metóda, ktorá podľa Lui updatne farbu všetkým uzlom. 
        /// Funguje tak, že Lua pošle údaje vo formáte json, ktoré sa následne deserializujú, uložia sa do zoznamu (dictionary) ako id uzla + Color 
        /// a pre každý aktívny uzol v tomto zozname sa zavolá príslušný update v Node, ktorý už vykoná viditeľné zmeny v Unity.
        /// </summary>
        public void UpdateNodesColor(Dictionary<int, Color> dictionary)
        {
            foreach (var item in dictionary)
            {
                if (Nodes[item.Key].gameObject.activeSelf)
                    Nodes[item.Key].UpdateColor(item.Value);
            }
        }

        /// <summary>
        /// Metóda, ktorá podľa Lui updatne pozíciu všetkým uzlom. 
        /// Funguje tak, že Lua pošle údaje vo formáte json, ktoré sa následne deserializujú, uložia sa do zoznamu (dictionary) ako id uzla + pozícia ako Vector3 
        /// a pre každý aktívny uzol v tomto zozname sa zavolá príslušný update v Node, ktorý už vykoná viditeľné zmeny v Unity.
        /// </summary>
        public void UpdateNodesPosition(Dictionary<int, Vector3> dictionary)
        {
            foreach (var item in dictionary)
            {
                if (Nodes[item.Key].gameObject.activeSelf)
                    Nodes[item.Key].UpdatePosition(item.Value);
            }
        }

        /// <summary>
        /// Metóda, ktorá podľa Lui updatne veľkosť všetkým uzlom. 
        /// Funguje tak, že Lua pošle údaje vo formáte json, ktoré sa následne deserializujú, uložia sa do zoznamu (dictionary) ako id uzla + veľkosť ako Vector3 
        /// a pre každý aktívny uzol v tomto zozname sa zavolá príslušný update v Node, ktorý už vykoná viditeľné zmeny v Unity.
        /// </summary>
        public void UpdateNodesSize(Dictionary<int, Vector3> dictionary)
        {
            foreach (var item in dictionary)
            {
                if (Nodes[item.Key].gameObject.activeSelf)
                    Nodes[item.Key].UpdateSize(item.Value);
            }
        }

        /// <summary>
        /// Metóda, ktorá podľa Lui updatne tvar všetkým uzlom. 
        /// Funguje tak, že Lua pošle údaje vo formáte json, ktoré sa následne deserializujú, uložia sa do zoznamu (dictionary) ako id uzla + jeden z dostupných tvarov (AvailableShapes)
        /// a pre každý aktívny uzol v tomto zozname sa zavolá príslušný update v Node, ktorý už vykoná viditeľné zmeny v Unity.
        /// </summary>
        public void UpdateNodesShapes(Dictionary<int, Node.AvailableShapes> dictionary)
        {
            foreach (var item in dictionary)
            {
                if (Nodes[item.Key].gameObject.activeSelf)
                    Nodes[item.Key].UpdateShape(item.Value);
            }
        }

        /// <summary>
        /// Metóda, ktorá podľa Lui updatne popisok všetkým uzlom. 
        /// Funguje tak, že Lua pošle údaje vo formáte json, ktoré sa následne deserializujú, uložia sa do zoznamu (dictionary) ako id uzla + popisok ako string 
        /// a pre každý aktívny uzol v tomto zozname sa zavolá príslušný update v Node, ktorý už vykoná viditeľné zmeny v Unity.
        /// </summary>
        public void UpdateNodesLabel(Dictionary<int, string> dictionary)
        {
            foreach (var item in dictionary)
            {
                if (Nodes[item.Key].gameObject.activeSelf)
                    Nodes[item.Key].UpdateLabel(item.Value);
            }
        }

        /// <summary>
        /// Metóda, ktorá podľa Lui updatne všetkým uzlom, či sú viditeľné alebo nie. 
        /// Funguje tak, že Lua pošle údaje vo formáte json, ktoré sa následne deserializujú, uložia sa do zoznamu (dictionary) ako id uzla + viditeľnosť ako bool 
        /// a pre každý aktívny uzol v tomto zozname sa zavolá príslušný update v Node, ktorý už vykoná viditeľné zmeny v Unity.
        /// </summary>
        public void UpdateNodesVisibility(Dictionary<int, bool> dictionary)
        {
            foreach (var item in dictionary)
            {
                if (Nodes[item.Key].gameObject.activeSelf)
                    Nodes[item.Key].UpdateVisibility(item.Value);
            }
        }

        /// <summary>
        /// Metóda, ktorá podľa Lui updatne všetkým uzlom, či majú byť odfiltrované alebo nie. 
        /// Funguje tak, že Lua pošle údaje vo formáte json, ktoré sa následne deserializujú, uložia sa do zoznamu (dictionary) ako id uzla + odfiltrovanie ako bool 
        /// a pre každý uzol (aby sa dokázali aktivovať aj neaktívne uzly) v tomto zozname sa zavolá príslušný update v ClientNode, ktorý už vykoná viditeľné zmeny v Unity.
        /// </summary>
        public void UpdateNodesFiltered(Dictionary<int, bool> dictionary)
        {
            foreach (var item in dictionary)
            {
                Nodes[item.Key].UpdateIsFiltered(item.Value);
            }
        }

        /// <summary>
        /// Metóda, ktorá podľa Lui updatne názov všetkým uzlom. 
        /// Funguje tak, že Lua pošle údaje vo formáte json, ktoré sa následne deserializujú, uložia sa do zoznamu (dictionary) ako id uzla + názov ako string 
        /// a pre každý aktívny uzol v tomto zozname sa zavolá príslušný update v Node, ktorý už vykoná viditeľné zmeny v Unity.
        /// </summary>
        public void UpdateNodesName(Dictionary<int, string> dictionary)
        {
            // TODO Dictionary<int, string> dictionary = JsonToColumn<NodeColumnName>(ApiController.Instance.GetNodeNameColumn()).ToDictionary() as Dictionary<int, string>;
            //foreach (var item in dictionary)
            //{
            //    if (Nodes[item.Key].gameObject.activeSelf)
            //        Nodes[item.Key].UpdateName(item.Value);
            //}
        }

        /// <summary>
        /// Metóda, ktorá podľa Lui updatne metriky LOC, Cyclomatic a Halstead všetkým uzlom. 
        /// Funguje tak, že Lua pošle údaje vo formáte json, ktoré sa následne deserializujú, uložia sa do zoznamu (dictionary) ako id uzla + metriky 
        /// a pre každý aktívny uzol v tomto zozname sa zavolá príslušný update v Node, ktorý už vykoná viditeľné zmeny v Unity.
        /// </summary>
        public void UpdateNodesMetrics(Dictionary<int, Metrics> dictionary)
        {
            // TODO Dictionary<int, Metrics> dictionary = JsonToColumn<ColumnNodeMetrics>(ApiController.Instance.GetNodeMetricsColumn()).ToDictionary() as Dictionary<int, Metrics>;
            foreach (var item in dictionary)
            {
                if (Nodes[item.Key].gameObject.activeSelf)
                    Nodes[item.Key].UpdateMetrics(item.Value);
            }
        }

        /// <summary>
        /// Metóda, ktorá podľa Lui updatne metriky Infoflow všetkým uzlom. 
        /// Funguje tak, že Lua pošle údaje vo formáte json, ktoré sa následne deserializujú, uložia sa do zoznamu (dictionary) ako id uzla + infoflow metriky 
        /// a pre každý aktívny uzol v tomto zozname sa zavolá príslušný update v Node, ktorý už vykoná viditeľné zmeny v Unity.
        /// </summary>
        public void UpdateNodesInfoflowMetrics(Dictionary<int, InfoflowMetrics> dictionary)
        {
            // TODO Dictionary<int, InfoflowMetrics> dictionary = JsonToColumn<ColumnNodeInfoflowMetrics>(ApiController.Instance.GetNodeInfoflowMetricsColumn()).ToDictionary() as Dictionary<int, InfoflowMetrics>;
            foreach (var item in dictionary)
            {
                if (Nodes[item.Key].gameObject.activeSelf)
                    Nodes[item.Key].UpdateInfoflowMetrics(item.Value);
            }
        }

        /// <summary>
        /// Metóda, ktorá podľa Lui updatne typ všetkým uzlom. 
        /// Funguje tak, že Lua pošle údaje vo formáte json, ktoré sa následne deserializujú, uložia sa do zoznamu (dictionary) ako id uzla + typ ako string 
        /// a pre každý aktívny uzol v tomto zozname sa zavolá príslušný update v Node, ktorý už vykoná viditeľné zmeny v Unity.
        /// </summary>
        public void UpdateNodesType(Dictionary<int, string> dictionary)
        {
            // TODO Dictionary<int, string> dictionary = JsonToColumn<ColumnNodeType>(ApiController.Instance.GetNodeTypeColumn()).ToDictionary() as Dictionary<int, string>;
            foreach (var item in dictionary)
            {
                if (Nodes[item.Key].gameObject.activeSelf)
                    Nodes[item.Key].UpdateType(item.Value);
            }
        }

        /// <summary>
        /// Metóda, ktorá podľa Lui updatne telo všetkým uzlom. 
        /// Funguje tak, že Lua pošle údaje vo formáte json, ktoré sa následne deserializujú, uložia sa do zoznamu (dictionary) ako id uzla + telo ako string 
        /// a pre každý aktívny uzol v tomto zozname sa zavolá príslušný update v Node, ktorý už vykoná viditeľné zmeny v Unity.
        /// </summary>
        public void UpdateNodesBody(Dictionary<int, string> dictionary)
        {
            // TODO Dictionary<int, string> dictionary = JsonToColumn<ColumnNodeBody>(ApiController.Instance.GetNodeBodyColumn()).ToDictionary() as Dictionary<int, string>;
            foreach (var item in dictionary)
            {
                if (Nodes[item.Key].gameObject.activeSelf)
                    Nodes[item.Key].UpdateBody(item.Value);
            }
        }
        #endregion

        #region Update Edges

        /// <summary>
        /// Metóda, ktorá podľa Lui updatne farbu všetkým hranám. 
        /// Funguje tak, že Lua pošle údaje vo formáte json, ktoré sa následne deserializujú, uložia sa do zoznamu (dictionary) ako id hrany + Color 
        /// a pre každú aktívnu hranu v tomto zozname sa zavolá príslušný update v Edge, ktorý už vykoná viditeľné zmeny v Unity.
        /// </summary>
        public void UpdateEdgesColor(Dictionary<int, Color> dictionary)
        {
            // TODO Dictionary<int, Color> dictionary = JsonToColumn<ColumnNodeEdgeColor>(ApiController.Instance.GetEdgeColorColumn()).ToDictionary() as Dictionary<int, Color>;
            foreach (var item in dictionary)
            {
                Edges[item.Key].UpdateColor(item.Value);
            }
        }

        /// <summary>
        /// Metóda, ktorá podľa Lui updatne popisok všetkým hranám. 
        /// Funguje tak, že Lua pošle údaje vo formáte json, ktoré sa následne deserializujú, uložia sa do zoznamu (dictionary) ako id hrany + popisok ako string 
        /// a pre každú aktívnu hranu v tomto zozname sa zavolá príslušný update v Edge, ktorý už vykoná viditeľné zmeny v Unity.
        /// </summary>
        public void UpdateEdgesLabel(Dictionary<int, string> dictionary)
        {
            // TOOD Dictionary<int, string> dictionary = JsonToColumn<ColumnNodeEdgeLabel>(ApiController.Instance.GetEdgeLabelColumn()).ToDictionary() as Dictionary<int, string>;
            foreach (var item in dictionary)
            {
                if (Edges[item.Key].gameObject.activeSelf)
                    Edges[item.Key].UpdateLabel(item.Value);
            }
        }

        /// <summary>
        /// Metóda, ktorá podľa Lui updatne všetkým hranám, či je parent alebo nie. 
        /// Funguje tak, že Lua pošle údaje vo formáte json, ktoré sa následne deserializujú, uložia sa do zoznamu (dictionary) ako id hrany + bool, či je parent 
        /// a pre každú aktívnu hranu v tomto zozname sa zavolá príslušný update v Edge, ktorý už vykoná viditeľné zmeny v Unity.
        /// </summary>
        public void UpdateEdgesParent(Dictionary<int, bool> dictionary)
        {
            // TODO Dictionary<int, bool> dictionary = JsonToColumn<ColumnEdgeIsParent>(ApiController.Instance.GetEdgeIsParentColumn()).ToDictionary() as Dictionary<int, bool>;
            foreach (var item in dictionary)
            {
                Edges[item.Key].UpdateIsParent(item.Value);
            }
        }

        /// <summary>
        /// Metóda, ktorá podľa Lui updatne všetkým hranám, či je odfiltrovaná alebo nie. 
        /// Funguje tak, že Lua pošle údaje vo formáte json, ktoré sa následne deserializujú, uložia sa do zoznamu (dictionary) ako id hrany + odfiltrovanie ako bool 
        /// a pre každú aktívnu hranu v tomto zozname sa zavolá príslušný update v Edge, ktorý už vykoná viditeľné zmeny v Unity.
        /// </summary>
        public void UpdateEdgesFiltered(Dictionary<int, bool> dictionary)
        {
            // TODO Dictionary<int, bool> dictionary = JsonToColumn<ColumnNodeEdgeIsFiltered>(ApiController.Instance.GetEdgeFilteredColumn()).ToDictionary() as Dictionary<int, bool>;
            foreach (var item in dictionary)
            {
                Edges[item.Key].UpdateIsFiltred(item.Value);
            }
        }

        #endregion  

        /// <summary>
        /// Metóda, ktorá vráti hranice grafu. (Prevzatá metóda zo starého projektu.)
        /// </summary>
        /// <returns>Hranice grafu.</returns>
        public Bounds GetGraphBounds()
        {
            Bounds bounds = GetComponent<Renderer>().bounds;
            foreach (Transform item in transform)
            {
                var renderer = item.GetChild(0).GetComponent<Renderer>();
                if (renderer != null)
                {
                    bounds.Encapsulate(renderer.bounds);
                }
            }
            return bounds;
        }

        public Transform GetNodesHolder() {
            return nodesHolder;
        }

        // public void UpdateSizeUsingSlider(SliderEventData eventData)
        // {
        //     GameObject nodesHolder = GameObject.FindGameObjectWithTag("nodesHolder");
        //     nodesHolder.transform.localScale = new Vector3(eventData.NewValue, eventData.NewValue, eventData.NewValue);
        // }
    }
}
