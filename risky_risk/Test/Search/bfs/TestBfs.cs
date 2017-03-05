using System;
using System.Collections.Generic;
using Moq;
using RiskAi.Game;
using RiskAi.Search.BFS;

namespace risky_risk
{
	/// <summary>
	/// Test bfs.
	/// </summary>
	public class TestBfs
	{

		//public static void TestBfsFromAlaska_DisplaySearchResults()
		//{
		//	const string TERRITORY_FILE_PATH = @"..\..\resources\territories.json";
		//	const string CONTINENT_FILE_PATH = @"..\..\resources\continents.json";

		//	Board GameBoard = null; //InitializeBoard(TERRITORY_FILE_PATH, CONTINENT_FILE_PATH);;

		//	// Initialize bfs object
		//	BreadthFirstSearch bfs = new BreadthFirstSearch((Board)GameBoard.Clone());

		//	// Perform a bfs starting from alaska and print out 
		//	// the territories in visited order and distance
		//	bfs.Start("alaska");
		//	List<BfsTile> tiles = bfs.Tiles;

		//	foreach (BfsTile t in tiles)
		//	{
		//		Console.WriteLine(t.Name + " : " + t.Distance);
		//	}
		//}

	}
}
