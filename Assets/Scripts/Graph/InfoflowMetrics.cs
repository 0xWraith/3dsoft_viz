namespace Softviz.Graph
{
    /// <summary>
    /// Trieda, ktorá predstavuje metriky Infoflow. Údaje o metrikách posiela Lua.
    /// </summary>
    [System.Serializable]
    public class InfoflowMetrics
    {
        /// <summary>
        /// Metrika informationFlow.
        /// </summary>
        public int informationFlow;
        /// <summary>
        /// Metrika interfaceComplexity.
        /// </summary>
        public int interfaceComplexity;
        /// <summary>
        /// Metrika argumentsIn.
        /// </summary>
        public int argumentsIn;
        /// <summary>
        /// Metrika argumentsOut.
        /// </summary>
        public int argumentsOut;

        /// <summary>
        /// Konštruktor triedy InfoflowMetrics, ktorý nastaví jednotlivým metrikám zadané hodnoty.
        /// </summary>
        /// <param name="informationFlow">Metrika informationFlow.</param>
        /// <param name="interfaceComplexity">Metrika interfaceComplexity.</param>
        /// <param name="argumentsIn">Metrika argumentsIn.</param>
        /// <param name="argumentsOut">Metrika argumentsOut.</param>
        public InfoflowMetrics(int informationFlow, int interfaceComplexity, int argumentsIn, int argumentsOut)
        {
            this.informationFlow = informationFlow;
            this.interfaceComplexity = interfaceComplexity;
            this.argumentsIn = argumentsIn;
            this.argumentsOut = argumentsOut;
        }
    }
}
