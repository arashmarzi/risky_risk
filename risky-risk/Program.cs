using System.Collections.Generic;
using RiskAi.Graph;
using RiskAi.Player;
using RiskAi.utils;
using System;
using RiskAi.bfs;
using RiskAi.search.bfs;

namespace RiskAi
{
    /// Main game logic
    class Program
    {
        static Board Board;
        const int NUM_AGENTS = 4;
        const int MAX_NUM_TERRITORIES = 42;
        const int MAX_NUM_CONTINENTS = 6;
        const int NUM_START_TROOPS = 30;
        const string TERRITORY_FILE_PATH = @"..\..\resources\territories.json";
        const string CONTINENT_FILE_PATH = @"..\..\resources\continents.json";
        static List<Agent> Agents;
        static bool finished;

        /// <summary>Main game loop</summary>
        static void Main(string[] args)
        {
            finished = false;
            Agents = InitializeAgents(NUM_AGENTS, NUM_START_TROOPS);
            Board = InitializeBoard(TERRITORY_FILE_PATH, CONTINENT_FILE_PATH);

            DistributeTerritoriesToAgents(Agents, (Board)Board.Clone());
            UpdateBoard(Agents, Board);
            EquallyDistributeTroops(Agents);
            UpdateBoard(Agents, Board);

            BreadthFirstSearch bfs = new BreadthFirstSearch((Board)Board.Clone());

            bfs.Start("alaska");
            List<BfsTile> tiles = bfs.Tiles;
            foreach (BfsTile t in tiles)
            {
                Console.WriteLine(t.Id + " : " + t.Distance);
            }

            do
            {
                foreach (Agent a in Agents)
                {
                    // continent bonus 
                    Mechanics.ApplyContinentBonus(a, (Board)Board.Clone());

                    // cash in cards, if possible
                    Mechanics.ApplyCardBonus(a, Board); // not fully tested

                    // collect territory reinforcements
                    Mechanics.GetReinforcements(a);

                    // place reinforcements (no intelligent mechanism for now)
                    Mechanics.PlaceReinforcements(a);

                    // update territories on board
                    Mechanics.UpdateBoard(Agents, Board);
                    

                    // perform attacking

                    // tactical move

                    // end turn
                }
            } while (!finished);
        }


        private static void GetReinforcements(Agent agent)
        {
            Mechanics.GetReinforcements(agent);
        }
        private static void UpdateBoard(List<Agent> agents, Board board)
        {
            Mechanics.UpdateBoard(agents, board);
        }

        private static void EquallyDistributeTroops(List<Agent> agents)
        {
            Mechanics.EquallyDistributeTroops(agents);
        }

        private static List<Agent> InitializeAgents(int numAgents, int numStartingTroops)
        {
            return Mechanics.InitializeAgents(numAgents, numStartingTroops);
        }

        private static Board InitializeBoard(string territoryFilePath, string continentFilePath)
        {
            return Mechanics.InitializeBoard(territoryFilePath, continentFilePath);
        }

        private static void DistributeTerritoriesToAgents(List<Agent> agents, Board board)
        {
            Mechanics.DistributeTerritories(agents, board.Territories, MAX_NUM_TERRITORIES);
        }
    }
}
