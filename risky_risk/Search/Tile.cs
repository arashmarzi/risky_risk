using RiskAi.Game.Land;

namespace RiskAi.Search
{
    class Tile
    {
        public Territory Territory { get; set; }
        public string Name { get; set; }
        public Tile Parent { get; set; }
    }
}
