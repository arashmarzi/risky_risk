using System.Collections.Generic;
using System;
using RiskAi.Game.Agent;
using RiskAi.Game;
using RiskAi.Search.BFS;
using RiskAi.Search.AStar;
using System.Linq;
using RiskAi.Game.Land;

namespace RiskAi
{
	class Program
	{
		static Board GameBoard;
		const int NUM_AGENTS = 4;
		const int MAX_NUM_TERRITORIES = 42;
		const int MAX_NUM_CONTINENTS = 6;
		const int NUM_START_TROOPS = 30;
		const string TERRITORY_FILE_PATH = @"../../resources/territories.json";
		const string CONTINENT_FILE_PATH = @"../../resources/continents.json";
		static List<Player> Players;
		static bool finished;

		static void Main(string[] args)
		{
			// Assume the game is not finished since we are starting
			finished = false;

			// Initialize Players and the board
			Players = InitializePlayers(NUM_AGENTS, NUM_START_TROOPS);
			GameBoard = InitializeBoard(TERRITORY_FILE_PATH, CONTINENT_FILE_PATH);

			// Assign territories to each player
			DistributeTerritoriesToPlayers(Players, (Board)GameBoard.Clone());

			// Update the board
			UpdateBoard(Players, GameBoard);

			// Place troops on territories for each player
			EquallyDistributeTroops(Players);

			// Update the board
			UpdateBoard(Players, GameBoard);

			//TestAStar(Board);

			do
			{
				foreach (Player a in Players)
				{
					// Calculate and apply continent bonus 
					Mechanics.ApplyContinentBonus(a, (Board)GameBoard.Clone());

					// Cash in cards, if possible
					Mechanics.ApplyCardBonus(a, GameBoard); // not fully tested

					// Collect territory reinforcements
					Mechanics.GetReinforcements(a);

					// place reinforcements (no intelligent mechanism for now)
					Mechanics.GetReinforcements(a);

					// update territories on board
					Mechanics.UpdateBoard(Players, GameBoard);

					// perform attacking

					// tactical move

					// end turn
				}

				// prevent an infinite loop
				finished = true;

			} while (!finished);
		}

		/// <summary>
		/// Gets the reinforcements for a given player.
		/// </summary>
		/// <param name="agent">Agent.</param>
		private static void GetReinforcements(Player agent)
		{
			Mechanics.GetReinforcements(agent);
		}

		/// <summary>
		/// Updates the board.
		/// </summary>
		/// <param name="agents">Agents.</param>
		/// <param name="board">Board.</param>
		private static void UpdateBoard(List<Player> agents, Board board)
		{
			Mechanics.UpdateBoard(agents, board);
		}

		/// <summary>
		/// Equally distributes a player's troops amongst controlled territories.
		/// </summary>
		/// <param name="agents">Agents.</param>
		private static void EquallyDistributeTroops(List<Player> agents)
		{
			Mechanics.EquallyDistributeTroops(agents);
		}

		/// <summary>
		/// Initializes the players.
		/// </summary>
		/// <returns>The players.</returns>
		/// <param name="numPlayers">Number players.</param>
		/// <param name="numStartingTroops">Number starting troops.</param>
		private static List<Player> InitializePlayers(int numPlayers, int numStartingTroops)
		{
			return Mechanics.InitializePlayers(numPlayers, numStartingTroops);
		}

		/// <summary>
		/// Initializes the board.
		/// </summary>
		/// <returns>The board.</returns>
		/// <param name="territoryFilePath">Territory file path.</param>
		/// <param name="continentFilePath">Continent file path.</param>
		private static Board InitializeBoard(string territoryFilePath, string continentFilePath)
		{
			return Mechanics.InitializeBoard(territoryFilePath, continentFilePath);
		}

		/// <summary>
		/// Distributes the territories to players;
		/// this is used at the start of the game.
		/// </summary>
		/// <param name="agents">Agents.</param>
		/// <param name="board">Board.</param>
		private static void DistributeTerritoriesToPlayers(List<Player> agents, Board board)
		{
			Mechanics.DistributeTerritories(agents, board.Territories, MAX_NUM_TERRITORIES);
		}




	}
}
