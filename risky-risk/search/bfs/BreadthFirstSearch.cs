using RiskAi.Graph;
using RiskAi.Graph.Land;
using RiskAi.search.bfs;
using System.Collections.Generic;

namespace RiskAi.bfs
{
    class BreadthFirstSearch
    {
        public List<BfsTile> Tiles { get; set; }
        public Queue<BfsTile> Queue { get; set; }

        public BreadthFirstSearch(Board board)
        {
            Tiles = InitializeTiles(board.Territories);
            Queue = new Queue<BfsTile>();
        }

        private List<BfsTile> InitializeTiles(TerritoryList territories)
        {
            List<BfsTile> tiles = new List<BfsTile>();
            foreach (Territory t in territories)
            {
                BfsTile bfsTile = new BfsTile(t.Name, t);
                bfsTile.Distance = -1;
                bfsTile.Parent = null;

                tiles.Add(bfsTile);    
            }

            return tiles;
        }

        private BfsTile FindBfsTile(string id)
        {
            return Tiles.Find(x => x.Id == id);
        }

        public void Start(string root)
        {
            BfsTile rootTile = FindBfsTile(root);
            rootTile.Distance = 0;

            Queue.Enqueue(rootTile);

            do
            {
                BfsTile currentTile = Queue.Dequeue();

                foreach(string neighbour in currentTile.Territory.Neighbours)
                {
                    BfsTile tile = FindBfsTile(neighbour);
                    if(tile.Distance == -1)
                    {
                        tile.Distance = currentTile.Distance + 1;
                        tile.Parent = currentTile;
                        Queue.Enqueue(tile);
                    }
                }
            } while (Queue.Count > 0);
        }
    }
}
