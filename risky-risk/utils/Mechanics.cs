using RiskAi.Graph.Land;
using RiskAi.Graph;
using RiskAi.Player;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Newtonsoft.Json;
using System.IO;

namespace RiskAi.utils
{
    /// Mechanics represents aspects of the game's mechanics
    public static class Mechanics
    {
        private static RNGCryptoServiceProvider rngCsp = new RNGCryptoServicePApplyCardBonusrovider();

        /// <summary>Randomly distribute territories amongst players</summary>
        /// <param name="Agents">List of players</param>
        /// <param name="Territories">List of territories</param>
        public static void DistributeTerritories(List<Agent> Agents, TerritoryList Territories)
        {
            
            int numAgents = Agents.Count;

            // can this loop be optimized?
            while ( Territories.Count > 0 )
            {
                foreach ( Agent a in Agents )
                {

                    try {
                        int index = (int)GetRandomValue((byte)Territories.Count); // get random territory index
                        string toRemove = IndexToName(index, Territories);
                        a.GainTerritory(toRemove);
                        territories.Remove(toRemove);
                    }
                    catch (ArgumentOutOfRangeException ex)
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>Determine number of reinforcements player receives in this turn</summary>
        /// <param name="Agent">Player to receive reinforcements</param>
        public static void GetReinforcements(Agent Agent)
        {
            if( Agent.ControlledTerritories.Count <= 9 )
            {
                Agent.GetReinforcements(3);
                
            }
            else
            {
                Agent.GetReinforcements((Agent.ControlledTerritories.Count / 3));
            }
        }

        /// <summary>Player will check to see if it can cash in its cards</summary>
        /// <remarks>Too many apply/check cards functions in Agent.cs, try to clean this up</remarks>
        /// <param name="Agent">Player to cash in cards</param>
        /// <param name="Board">Game board</param>
        public static void ApplyCardBonus(Agent Agent, Board Board)
        {
            if( Agent.CanCashIn() )
            {
                Agent.ApplyCardBonus(Board);
            }
        }

        /// <summary></summary>
        /// <param name=""></param>
        /// <param name=""></param>
        public static void ApplyContinentBonus(Agent Agent, Board Board)
        {
            agent.ApplyContinentBonus(board);
        }

        /// <summary></summary>
        /// <param name=""></param>
        public static void PlaceReinforcements(Agent agent)
        {
            // add troops to territories with lowest population until more intelligent mechanism is implemented

            // for each available troop, find a territory with the lowest number of troops, place the reinforcement until finished
            
            //foreach (Territory t in territoriesToReinforce)
            //{
            //    agent.PlaceReinforcements(t.Name);
            //}
        }

        /// <summary></summary>
        /// <param name=""></param>
        private static string IndexToName(int index, TerritoryList territories)
        {
            string name = "";
            int i = 0;
            foreach( Territory t in territories )
            {
                if ( i == index )
                {
                    name = t.Name;
                    break;
                }
                else
                {
                    i++;
                }
            }

            return name;
        }

        /// <summary></summary>
        /// <param name=""></param>
        /// <returns></returns>
        private static byte GetRandomValue(byte maxValue)
        {
            if ( maxValue <= 0 )
                throw new ArgumentOutOfRangeException("maxValue");

            byte[] randomNumber = new byte[1];

            do
            {
                rngCsp.GetBytes(randomNumber);
            }
            while ( !IsValidValue(randomNumber[0], maxValue) );

            rngCsp.Dispose();

            return (byte)((randomNumber[0] % maxValue));
        }

        /// <summary></summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns></returns>
        private static bool IsValidValue(byte value, byte maxValue)
        {

            int fullSetsOfValues = Byte.MaxValue / maxValue;

            return value < maxValue * fullSetsOfValues;
        }

        /// <summary></summary>
        /// <param name=""></param>
        /// <param name=""></param>
        public static void UpdateBoard(List<Agent> agents, Board board)
        {
            foreach ( Agent a in agents )
            {
                foreach ( KeyValuePair<string, int> kvp in a.ControlledTerritories )
                {
                    board.Territories[kvp.Key].UpdateOwner(a.Name, kvp.Value);
                }
            }
        }

        /// <summary></summary>
        /// <param name=""></param>
        public static void EquallyDistributeTroops(List<Agent> agents)
        {
            foreach ( Agent a in agents )
            {
                while ( a.AvailableTroops > 0 )
                {
                    List<string> names = new List<string>(a.ControlledTerritories.Keys);
                    foreach ( string name in names )
                    {
                        a.PlaceReinforcements(name);
                    }
                }
            }
        }

        /// <summary></summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns></returns>
        public static List<Agent> InitializeAgents(int numAgents, int numStartingTroops)
        {
            List<Agent> agents = new List<Agent>(numAgents);

            for ( int i = 0; i < numAgents; i++ )
            {
                agents.Add(new Agent(i.ToString(), numStartingTroops, new List<Territory>()));
            }

            return agents;
        }

        /// <summary></summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns</returns>
        public static Board InitializeBoard(string territoryFilePath, string continentFilePath)
        {
            // instantiate board graph
            Board board = new Board(); 

            // read json data from file, create a territory, continent object for each member of json collection
            List<Territory> territories = JsonConvert.DeserializeObject<List<Territory>>(File.ReadAllText(territoryFilePath));
            List<Continent> continents = JsonConvert.DeserializeObject<List<Continent>>(File.ReadAllText(continentFilePath));

            // add territory object to board graph
            foreach ( Territory t in territories )
            {
                board.AddNode(t); 
            }

            foreach ( Continent c in continents )
            {
                board.AddNode(c);
            }

            return board;
        }
    }
}
