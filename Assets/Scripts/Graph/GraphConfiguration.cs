using System;
using Utils;

namespace Softviz.Graph
{
    [Serializable]
    public class GraphConfiguration
    {
        public Enums.LayoutAlgorithm actualLayoutAlgorithm;
        public Enums.FunctionType actualFunctionType;
        public Enums.VariableType actualVariableType;
        public Enums.BuildingLayoutAlgorithm actualBuildingLayoutAlgorithm;
        public Enums.DirectoryMode actualDirectoryMode;
        public Enums.EvolutionMetricsMode actualEvolutionMetricsMode;
        public float actualAttractiveForceValue;
        public float actualRepulsiveForceValue;
        public float actualMinNodeDistanceValue;

        public GraphConfiguration()
        {
            this.actualLayoutAlgorithm = Enums.LayoutAlgorithm.FruchtermanReingold;
            this.actualFunctionType = Enums.FunctionType.Cyclomatic;
            this.actualVariableType = Enums.VariableType.None;
            this.actualBuildingLayoutAlgorithm = Enums.BuildingLayoutAlgorithm.RowAlgorithm;
            this.actualDirectoryMode = Enums.DirectoryMode.Normal;
            this.actualEvolutionMetricsMode = Enums.EvolutionMetricsMode.ComparisonMode;
            this.actualAttractiveForceValue = 1f;
            this.actualRepulsiveForceValue = 1f;
            this.actualMinNodeDistanceValue = 1f;
        }
    }
}
