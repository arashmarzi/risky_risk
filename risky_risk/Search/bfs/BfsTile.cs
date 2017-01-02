using RiskAi.Game.Land;

namespace RiskAi.Search.BFS
{
    class BfsTile : Tile
    {
        public int Distance { get; set; }

        public BfsTile(string id, Territory territory)
        {
            Name = id;
            Territory = territory;
            Parent = null;
            Distance = 0;
        }
    }
}
