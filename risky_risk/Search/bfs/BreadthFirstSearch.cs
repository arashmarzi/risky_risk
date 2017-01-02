using RiskAi.Game;
using RiskAi.Game.Land;
using RiskAi.Search.BFS;
using System.Collections.Generic;

namespace RiskAi.Search.BFS
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

        private List<BfsTile> InitializeTiles(List<Territory> territories)
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
            return Tiles.Find(x => x.Name == id);
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
