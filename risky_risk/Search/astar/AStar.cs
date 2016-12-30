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
            // must create a path from start territory to finish territory
            TerritoryList territories = board.Territories;

            OpenSet.Add(new StarTile(start.Name, start));

            Dictionary<string, int> sldMap = CalculateSLD(finish.Name);

            // loop through A* steps until goal is reached
            while (!IsGoalFound)
            {
                StarTile current = GetMinFValue();

                // there are no more members of the OpenSet to examine
                if(current == null)
                {
                    break;
                }

                // current territory is finish territory
                if(current.Id == finish.Name)
                {
                    // do something?
                }

                List<StarTile> frontier = GetFrontier(current);

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

                    // self owned territory, seen as obstacle because it is already
                    else if(t.Territory.Owner == current.Territory.Owner)
                    {
                        // do not need to calculate anything, just skip
                    }
                }
            }
        }

        private void CalculateHeuristic(StarTile tile, Territory start, Territory finish, Dictionary<string,int> sldMap)
        {
            if (tile.Parent != null)
            {
                tile.GValue = tile.Parent.GValue + 1;
                tile.HValue = sldMap[tile.Id];
                tile.FValue = tile.GValue + tile.HValue;
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
                
        private List<StarTile> GetFrontier(StarTile current)
        {
            throw new NotImplementedException();
        }

        private StarTile GetMinFValue()
        {
            throw new NotImplementedException();
        }
    }
}
