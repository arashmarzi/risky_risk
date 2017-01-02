using RiskAi.Game.Land;
using RiskAi.Search;

namespace RiskAi.Search.AStar
{
    class StarTile : Tile
    {
		public string Name { get; set; }
        public int HValue { get; set; }
        public int GValue { get; set; }
        public int FValue { get; set; }
        public StarTile Parent { get; set; }
		public StarTile(string name, Territory territory)
        {
            Name = name;
            Territory = territory;
            Parent = null;
            HValue = 0;
            GValue = 0;
			FValue = GValue + HValue;
        }
    }
}
