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

			// Add start territory as part of the open set
            OpenSet.Add(new StarTile(start.Name, start));

			// Calculate the straight line distance from all territories
			// to the end territory
            Dictionary<string, int> sldMap = CalculateSLD(finish.Name);

            // loop through A* steps until goal is reached
			while(OpenSet.Count > 0)
			{
				GetMinFValue();
			}
			while (!IsGoalFound)
            {
                current = GetMinFValue();

                // there are no more members of the OpenSet to examine
                if(current == null)
                {
                    break;
                }

                // current territory is finish territory
                if(current.Id == finish.Name)
                {
					// Reach the goal Territory
					IsGoalFound = true;

					// Create Path
                }

                List<StarTile> frontier = GetFrontier(current, board);

                // loop through frontier and calculate heuristic for current territory
                foreach(StarTile t in frontier)
                {
                    // associate frontier territory to parent tile
                    t.Parent = current; // is this really necessary since we do it below?

                    // enemy territories are considered empty tiles, available to be conquered
                    if(t.Territory.Owner != current.Territory.Owner)
                    {
						// TODO: This is where ty
                        //CalculateHeuristic(t, start, finish);

                        t.Parent = current;
                    }
                    
                    // goal territory is reached
                    else if(t.Territory.Name == finish.Name)
                    {
                        t.Parent = current;

                        IsGoalFound = true;

                        Path = ConstructPath(t);
                    }

                    // self owned territory, not seen as obstacle because it is already owned
                    else if(t.Territory.Owner == current.Territory.Owner)
                    {
                        // do not need to calculate anything, just skip
                    }
                }
            }
        }

        private List<string> ConstructPath(StarTile start)
        {
            StarTile current = start;
            List<string> path = new List<string>();
            
            do
            {
                path.Add(current.Id);
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
                sldMap.Add(bfsTile.Id, bfsTile.Distance);
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
				tile.HValue = sldMap[tile.Id];
				tile.FValue = tile.GValue + tile.HValue;
			}
		}

		/// <summary>
		/// Get the frontier tiles of the current tile.
		/// </summary>
		/// <returns>The frontier.</returns>
		/// <param name="current">Current.</param>
        private List<StarTile> GetFrontier(StarTile current, Board board)
        {
			List<StarTile> frontier = new List<StarTile>();

			List<string> neighbours = board.Territories.Find(x => x.Name == current.Id).Neighbours;

			foreach(string neighbour in neighbours)
			{
				StarTile tile = new StarTile(neighbour, board.Territories.Find(x => x.Name == neighbour));
				tile.Parent = current;
				frontier.Add(tile);
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
				if (tile.Id != minTile.Id &&
				    tile.FValue < minTile.FValue)
				{
					minTile = tile;
				}
			}

			return minTile;
        }

		private void Start2(Board board, Player agent, Territory start, Territory finish)
		{
			StarTile current = null;

			// Must create a path from start territory to finish territory
			List<Territory> territories = board.Territories;

			// Add start territory as part of the open set
			OpenSet.Add(new StarTile(start.Name, start));

			// Calculate the straight line distance from all territories
			// to the end territory
			sldMap = CalculateSLD(finish.Name);

			// A*
		//			initialize the open list
		//initialize the closed list
		//put the starting node on the open list (you can leave its f at zero)

		//while the open list is not empty
		//	find the node with the least f on the open list, call it "q"

		//	pop q off the open list

		//	generate q's 8 successors and set their parents to q

		//	for each successor

		//		if successor is the goal, stop the search

		//		successor.g = q.g + distance between successor and q

		//		successor.h = distance from goal to successor

		//		successor.f = successor.g + successor.h


		//		if a node with the same position as successor is in the OPEN list \
		//            which has a lower f than successor, skip this successor

		//		if a node with the same position as successor is in the CLOSED list \ 
		//            which has a lower f than successor, skip this successor
		//		otherwise, add the node to the open list

		//	end
		//	push q on the closed list
		//end

			while(OpenSet.Count > 0)
			{
				foreach(StarTile tile in OpenSet)
				{
					CalculateHeuristic(tile);
				}
				
				current = GetMinFValue();

				List<StarTile> successors = GetFrontier(current, board);
				
			}
			
		}
    }
}
