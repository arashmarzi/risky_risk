using RiskAi.Game.Land;
using RiskAi.Search;

namespace RiskAi.Search.AStar
{
    class StarTile : Tile
    {
        public int HValue { get; set; }
        public int GValue { get; set; }
        public int FValue { get; set; }
        public StarTile Parent { get; set; }
        public StarTile(string id, Territory territory)
        {
            Id = id;
            Territory = territory;
            Parent = null;
            HValue = 0;
            GValue = 0;
            FValue = 0;
        }
    }
}
