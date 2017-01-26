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

		/// <summary>
		/// Initializes a new instance of the <see cref="T:RiskAi.Search.BFS.BreadthFirstSearch"/> class.
		/// </summary>
		/// <param name="board">Board.</param>
        public BreadthFirstSearch(Board board)
        {
            Tiles = InitializeTiles(board.Territories);
            Queue = new Queue<BfsTile>();
        }

		/// <summary>
		/// Initializes the tiles.
		/// </summary>
		/// <returns>The tiles.</returns>
		/// <param name="territories">Territories.</param>
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

		/// <summary>
		/// Finds the bfs tile given tile id.
		/// </summary>
		/// <returns>The bfs tile.</returns>
		/// <param name="id">Identifier.</param>
        private BfsTile FindBfsTile(string id)
        {
            return Tiles.Find(x => x.Name == id);
        }

		/// <summary>
		/// Start bfs from specified root.
		/// </summary>
		/// <param name="root">Root.</param>
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
