using RiskAi.Graph.Land;

namespace RiskAi.search
{
    class Tile
    {
        public Territory Territory { get; set; }
        public string Id { get; set; }
        public Tile Parent { get; set; }
    }
}
