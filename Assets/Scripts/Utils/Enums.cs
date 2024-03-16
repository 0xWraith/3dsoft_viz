namespace Utils
{
    public class Enums
    {
        public enum LayoutAlgorithm
        {
            FruchtermanReingold,
            City,
            CityFruchtermanReingold,
            Street
        }

        public enum FunctionType
        {
            LinesOfCode,
            Statements,
            StatementsCube,
            Cyclomatic
        }

        public enum VariableType
        {
            None,
            Typed
        }

        public enum BuildingLayoutAlgorithm
        {
            RowAlgorithm,
            SpiralAlgorithm
        }

        public enum DirectoryMode
        {
            Normal,
            Compact
        }

        public enum EvolutionMetricsMode
        {
            ComparisonMode,
            DifferentialMode
        }

        public enum Shaders
        {
            Standard,
            HighLight
        }
    }
}
