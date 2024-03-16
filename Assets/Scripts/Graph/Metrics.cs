using Utils;

namespace Softviz.Graph
{
    /// <summary>
    /// Trieda, ktorá predstavuje metriky LOC, Cyclomatic a Halstead.
    /// </summary>
    [System.Serializable]
    public class Metrics
    {
        /// <summary>
        /// Metrika LOC. Typ atribútu LOC je použitý z triedy ClassesForDeserializeJson.
        /// </summary>
        public LOC Loc;
        /// <summary>
        /// Metrika Halstead. Typ atribútu Halstead je použitý z triedy ClassesForDeserializeJson.
        /// </summary>
        public Halstead Halstead;
        /// <summary>
        /// Metrika Cyclomatic. Typ atribútu Cyclomatic je použitý z triedy ClassesForDeserializeJson.
        /// </summary>
        public Cyclomatic Cyclomatic;

        /// <summary>
        /// Konštruktor triedy Metrics, ktorý nastaví jednotlivé metriky podľa zadaných hodnôt.
        /// </summary>
        /// <param name="loc">Metrika LOC.</param>
        /// <param name="cyclomatic">Metrika Cyclomatic.</param>
        /// <param name="halstead">Metrika Halstead.</param>
        public Metrics(LOC loc, Cyclomatic cyclomatic, Halstead halstead)
        {
            Loc = loc;
            Halstead = halstead;
            Cyclomatic = cyclomatic;
        }
    }
}
