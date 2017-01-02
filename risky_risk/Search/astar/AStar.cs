using RiskAi.Game;
using RiskAi.Game.Land;
using RiskAi.Game.Agent;
using System;
using System.Collections.Generic;
using RiskAi.Search.BFS;

namespace RiskAi.Search.AStar
{
    class AStar
    {
        private List<StarTile> OpenSet { get; set; }
        private List<StarTile> CloseSet { get; set; }
        private bool IsGoalFound { get; set; }
        private List<string> Path { get; set; }
        private BreadthFirstSearch bfs { get; set; }
		private Dictionary<string, int> sldMap;

        public AStar(Board board)
        {
            OpenSet = new List<StarTile>();
            CloseSet = new List<StarTile>();
            IsGoalFound = false;
            Path = new List<string>();
            bfs = new BreadthFirstSearch(board);
        }

        public void start(Board board, Player agent, Territory start, Territory finish)
		{
			StarTile current = null;

			// Must create a path from start territory to finish territory
			List<Territory> territories = board.Territories;

			// Calculate the straight line distance from all territories
			// to the end territory
			sldMap = CalculateSLD(finish.Name);

			// Add start territory as part of the open set
			OpenSet.Add(new StarTile(start.Name, start));

			// loop through A* steps until goal is reached
			while (OpenSet.Count > 0 && !IsGoalFound)
			{
				// Grab the tile with the lowest/best value
				current = GetMinFValue();

				// Add the current tile to the close set as part of the path
				CloseSet.Add(current);

				// Finish the algorithm if we have reach the goal territory
				if (current.Name == finish.Name)
				{
					IsGoalFound = true;

					Path = ConstructPath(current);
				}

				// Continue on with the algorithm
				else
				{
					// Grab the neighbours of the current tile
					List<StarTile> frontier = GetFrontier(current, territories);

					foreach (StarTile successor in frontier)
					{
						// associate frontier territory to parent tile
						successor.Parent = current; // is this really necessary since we do it below?

						// enemy territories are considered empty tiles, available to be conquered
						if (successor.Territory.Owner != current.Territory.Owner)
						{
							CalculateHeuristic(successor);

							OpenSet.Add(successor);
						}

						// goal territory is reached
						else if (successor.Territory.Name == finish.Name)
						{
							successor.Parent = current;

							IsGoalFound = true;

							Path = ConstructPath(successor);
						}

						// self owned territory, not seen as obstacle because it is already owned
						else if (successor.Territory.Owner == current.Territory.Owner)
						{
							// do not need to calculate anything, just skip
						}
					} // foreach
					
				} // A* logic
			} // while OpenSet.Count > 0 && !IsGoalFound
		}

        private List<string> ConstructPath(StarTile start)
        {
            StarTile current = start;
            List<string> path = new List<string>();
            
            do
            {
                path.Add(current.Name);
                current = current.Parent;
            } while (current != null);

            return path;
        }

		/// <summary>
		/// Calculates the Straight Line Distance using Breadth First Search.
		/// </summary>
		/// <returns>Dictionary of Straight Line Distance from a given territory.</returns>
		/// <param name="name">Name.</param>
        private Dictionary<string, int> CalculateSLD(string name)
        {
            Dictionary<string, int> sldMap = new Dictionary<string, int>();
            bfs.Start(name);
            foreach (BfsTile bfsTile in bfs.Tiles)
            {
                sldMap.Add(bfsTile.Name, bfsTile.Distance);
            }

            return sldMap;
        }

		/// <summary>
		/// Calculates the heuristic for a given tile.
		/// </summary>
		/// <param name="tile">Tile.</param>
		/// <param name="sldMap">Sld map.</param>
		private void CalculateHeuristic(StarTile tile)
		{
			if (tile.Parent != null)
			{
				tile.GValue = tile.Parent.GValue + 1;
				tile.HValue = sldMap[tile.Name];
				tile.FValue = tile.GValue + tile.HValue;
			}
		}

		/// <summary>
		/// Get the frontier tiles of the current tile; frontier is the neighbours of a a given tile.
		/// </summary>
		/// <returns>The frontier.</returns>
		/// <param name="current">Current.</param>
		private List<StarTile> GetFrontier(StarTile current, List<Territory> territories)
        {
			List<StarTile> frontier = new List<StarTile>();

			// Grab the neighbours of the given tile
			List<string> neighbours = territories.Find(x => x.Name == current.Name).Neighbours;

			// For each neighbour, check to see if it is not a member of the Close Set or Open Set
			// so that we can add it to the frontier
			foreach(string neighbour in neighbours)
			{
				if(!OpenSet.Exists(x => x.Name == neighbour) && !CloseSet.Exists(x => x.Name == neighbour))
				{
					StarTile tile = new StarTile(neighbour, territories.Find(x => x.Name == neighbour));
					tile.Parent = current;
					frontier.Add(tile);
				}
			}

			return frontier;
        }

		/// <summary>
		/// Gets the minimum F Value from the Open Set.
		/// </summary>
		/// <returns>AStar Tile with minimum F Value.</returns>
		private StarTile GetMinFValue()
		{
			// Grab first member of the Open Set
			StarTile minTile = OpenSet[0];

			foreach (StarTile tile in OpenSet)
			{
				if (tile.Name != minTile.Name &&
				    tile.FValue < minTile.FValue)
				{
					minTile = tile;
				}
			}

			return minTile;
        }
    }
}
