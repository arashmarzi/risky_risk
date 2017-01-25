using RiskAi.Graph.Land;

namespace RiskAi.search.bfs
{
    class BfsTile : Tile
    {
        public int Distance { get; set; }

        public BfsTile(string id, Territory territory)
        {
            Id = id;
            Territory = territory;
            Parent = null;
            Distance = 0;
        }
    }
}
