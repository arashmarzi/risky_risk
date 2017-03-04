using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.IO;
using RiskAi.Game.Agent;
using RiskAi.Game.Land;
using Newtonsoft.Json;

namespace RiskAi.Game
{
	/// <summary>
	/// Mechanics of the Game.
	/// </summary>
    public static class Mechanics
	{
        private static RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();

		/// <summary>
		/// Distributes the territories equally to each player.
		/// </summary>
		/// <param name="players">Players.</param>
		/// <param name="territories">Territories.</param>
		/// <param name="max_num_territories">Max number territories.</param>
		public static void DistributeTerritories(List<Player> players, List<Territory> territories, int max_num_territories)
        {
            int numPlayers = players.Count;
			int territoryIndex = -1;
			string territoryToRemove = null;
			int initialTroop = 1;

			// Randomly remove a territory from the territory list,
			// assign it to a player
            while (territories.Count > 0)
            {
                foreach (Player player in players)
                {

                    try {
                        territoryIndex = GetRandomValue((byte)territories.Count); // get random territory index

						/* TODO: This might not be the best approach, might want to
						 * change how territories are stored; use of a dictionary 
						 * instead of a list
						 */
						territoryToRemove = IndexToName(territoryIndex, territories);
						player.GainTerritory(territoryToRemove, initialTroop);
						territories.RemoveAll(x => x.Name == territoryToRemove);
                    } catch (ArgumentOutOfRangeException ex)
					{
						Console.WriteLine(ex.Message);

                        break;
                    }
                }
            }
        }

		/// <summary>
		/// Gets the reinforcements for a given player.
		/// </summary>
		/// <param name="player">Player.</param>
        public static void GetReinforcements(Player player)
        {
            if(player.ControlledTerritories.Count <= 9)
            {
                player.GetReinforcements(3);
                
            }
            else
            {
                player.GetReinforcements((player.ControlledTerritories.Count / 3));
            }
        }

		/// <summary>
		/// Applies the card bonus.
		/// </summary>
		/// <param name="player">Player.</param>
		/// <param name="board">Board.</param>
        public static void ApplyCardBonus(Player player, Board board)
        {
            if(player.CanCashIn())
            {
                player.ApplyCardBonus(board);
            }
        }

		/// <summary>
		/// Applies the continent bonus.
		/// </summary>
		/// <param name="player">Player.</param>
		/// <param name="board">Board.</param>
        public static void ApplyContinentBonus(Player player, Board board)
        {
            player.ApplyContinentBonus(board);
        }

		/// <summary>
		/// Uses a territory's index to grab its name.
		/// </summary>
		/// <returns>Name of the territory.</returns>
		/// <param name="index">Index.</param>
		/// <param name="territories">Territories.</param>
		private static string IndexToName(int index, List<Territory> territories)
        {
            string name = null;
            int i = 0;

			// Search through the territory list in order to find 
            foreach(Territory t in territories)
            {
                if (i == index)
                {
                    name = t.Name;
                    break;
                }
                else
                    i++;
            }

            return name;
        }

		/// <summary>
		/// Gets the random value.
		/// </summary>
		/// <returns>The random value.</returns>
		/// <param name="maxValue">Max value.</param>
        private static byte GetRandomValue(byte maxValue)
        {
			byte[] randomNumber = new byte[1];

			// Sanity check parameters
			if (maxValue <= 0)
			{
				throw new ArgumentOutOfRangeException(nameof(maxValue));
			}

			// Get a valid random number
            do
            {
                rngCsp.GetBytes(randomNumber);
            }
            while (!IsValidValue(randomNumber[0], maxValue));

			// clean up
            rngCsp.Dispose();

            return (byte)(randomNumber[0] % maxValue);
        }

		/// <summary>
		/// Checks to see if a number is a valid value.
		/// </summary>
		/// <returns><c>true</c>, if the value was valid, <c>false</c> otherwise.</returns>
		/// <param name="value">Value.</param>
		/// <param name="maxValue">Max value.</param>
        private static bool IsValidValue(byte value, byte maxValue)
        {

            int fullSetsOfValues = Byte.MaxValue / maxValue;

			return (value < maxValue * fullSetsOfValues);
        }

		/// <summary>
		/// Updates info on the board based on the player info.
		/// </summary>
		/// <param name="players">Players.</param>
		/// <param name="board">Board.</param>
        public static void UpdateBoard(List<Player> players, Board board)
        {
			/* TODO: Must determine if the board is passed by reference the entire time,
			 * if so then UpdateBoard function is not necessary
			 */
            foreach (Player a in players)
            {
                foreach (KeyValuePair<string, int> kvp in a.ControlledTerritories)
                {
					board.Territories.Find(x => x.Name == kvp.Key).UpdateOwner(a.Name, kvp.Value);
                }
            }
        }

		/// <summary>
		/// Equally distributes a player's troops amongst controlled territories.
		/// </summary>
		/// <param name="players">Players.</param>
        public static void EquallyDistributeTroops(List<Player> players)
        {
			int singleTroop = 1;

            foreach (Player a in players)
            {
                while (a.AvailableTroops > 0)
                {
                    List<string> names = new List<string>(a.ControlledTerritories.Keys);
                    foreach (string name in names)
                    {
						a.PlaceReinforcements(name, singleTroop);
                    }
                }
            }
        }

		/// <summary>
		/// Initialize list of players with an empty list of territories
		/// </summary>
		/// <returns>The players.</returns>
		/// <param name="numPlayers">Number of players.</param>
		/// <param name="numStartingTroops">Number of starting troops.</param>
        public static List<Player> InitializePlayers(int numPlayers, int numStartingTroops)
        {
            List<Player> players = new List<Player>(numPlayers);

            for (int i = 0; i < numPlayers; i++)
            {
                players.Add(new Player(i.ToString(), numStartingTroops, new List<Territory>()));
            }

            return players;
        }

		/// <summary>
		/// Initializes the board.
		/// </summary>
		/// <returns>The board.</returns>
		/// <param name="territoryFilePath">Territory file path.</param>
		/// <param name="continentFilePath">Continent file path.</param>
        public static Board InitializeBoard(string territoryFilePath, string continentFilePath)
        {
            // instantiate board graph
            Board board = new Board(); 

            // read json data from file, create a territory, continent object for each member of json collection
            List<Territory> territories = JsonConvert.DeserializeObject<List<Territory>>(File.ReadAllText(territoryFilePath));
            List<Continent> continents = JsonConvert.DeserializeObject<List<Continent>>(File.ReadAllText(continentFilePath));

            // add territory object to board graph
            foreach (Territory t in territories)
            {
                board.AddNode(t);
            }

            foreach (Continent c in continents)
            {
                board.AddNode(c);
            }

            return board;
        }
    }
}
