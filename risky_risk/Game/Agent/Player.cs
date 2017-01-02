using System.Collections.Generic;
using System.Linq;
using RiskAi.Game.Land;

namespace RiskAi.Game.Agent
{
	public class Player
	{
		public string Name { get; set; }
		public int TotalTroops { get; set; }
		public int AvailableTroops { get; set; }
		public Dictionary<string, int> ControlledTerritories { get; set; } // territory name and number of troops on territory
		public List<string> cards { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="T:RiskAi.Game.Agent.Player"/> class.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="totalTroops">Total troops.</param>
		/// <param name="controlledTerritories">Controlled territories.</param>
		public Player(string name, int totalTroops, List<Territory> controlledTerritories)
		{
			this.Name = name;
			this.TotalTroops = totalTroops;
			this.ControlledTerritories = new Dictionary<string, int>();
			this.AvailableTroops = totalTroops;
			this.cards = new List<string>();

			foreach (Territory t in controlledTerritories)
			{
				this.ControlledTerritories.Add(t.Name, 0);
			}
		}

		/// <summary>
		/// Applies the continent bonus.
		/// </summary>
		/// <param name="board">Board.</param>
		public void ApplyContinentBonus(Board board)
		{
			AvailableTroops += CalculateContinentBonus(board);
		}

		/// <summary>
		/// Calculates the continent bonus.
		/// </summary>
		/// <returns>The continent bonus.</returns>
		/// <param name="board">Board.</param>
		private int CalculateContinentBonus(Board board)
		{
			int bonus = 0;
			foreach (Continent c in board.Continents)
			{
				// Get all territories of a given continent
				var result = from Territory t in board.Territories where t.Continent == c.Name select board.Territories.Find(x => x.Name == t.Name);

				// Get all controlled territories of the continent
				var controlledTerritories = from Territory t in board.Territories where (t.Continent == c.Name) && (t.Owner == Name) select board.Territories.Find(x => x.Name == t.Name);

				if (result.Count<Territory>() > 0 && 
				    controlledTerritories.Count<Territory>() > 0 && 
				    result.Count<Territory>() == controlledTerritories.Count<Territory>())
				{
					bonus += c.Bonus;
				}
			}
			return bonus;
		}

		public void ApplyCardBonus(Board board)
		{
			List<string> cards = CardsToCashIn();

			foreach (string card in cards)
			{
				this.cards.Remove(card);
			}

			board.UpdateCardBonus();
		}

		public bool CanCashIn()
		{
			if (cards.Count >= 5)
			{
				return true;
			}
			else if (hasCardSet(cards))
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		private bool hasCardSet(List<string> cards)
		{
			int soldiers = (from card in cards where card == "soldier" select card).Count();
			int cannons = (from card in cards where card == "cannon" select card).Count();
			int horses = (from card in cards where card == "horse" select card).Count();

			if (soldiers > 3)
			{
				return true;
			}
			else if (cannons > 3)
			{
				return true;
			}
			else if (horses > 3)
			{
				return true;
			}
			else if (soldiers >= 1 && cannons >= 1 && horses >= 1)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		private List<string> CardsToCashIn()
		{

			int soldiers = (from card in cards where card == "soldier" select card).Count();
			int cannons = (from card in cards where card == "cannon" select card).Count();
			int horses = (from card in cards where card == "horse" select card).Count();

			if (soldiers > 3)
			{
				return new List<string> { "soldier", "soldier", "soldier" };
			}
			else if (cannons > 3)
			{
				return new List<string> { "cannon", "cannon", "cannon" };
			}
			else if (horses > 3)
			{
				return new List<string> { "horse", "horse", "horse" }; ;
			}
			else
			{
				return new List<string> { "soldier", "cannon", "horse" };
			}
		}

		/// <summary>
		/// Add a territory to player's controlled territory list .
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="numTroops">Number troops.</param>
		public void GainTerritory(string name, int numTroops)
		{
			int result;
			if (!ControlledTerritories.TryGetValue(name, out result))
			{
				ControlledTerritories.Add(name, numTroops);
			}
		}

		/// <summary>
		/// Places reinforcements to a given territory.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="numTroops">Number troops.</param>
		public int PlaceReinforcements(string name, int numTroops)
		{
			if (this.AvailableTroops - numTroops < 0)
			{
				// Do not have enough troop to place on territory
				return 0;
			}
			else
			{
				// update territory troop count and adjust available troops
				ControlledTerritories[name] += numTroops;
				this.AvailableTroops -= numTroops;

				return numTroops;
			}
		}

		/// <summary>
		/// Apply reinforcements to players troop counts.
		/// </summary>
		/// <param name="reinforcements">Reinforcements.</param>
		public void GetReinforcements(int reinforcements)
		{
			AvailableTroops += reinforcements;
			TotalTroops += reinforcements;
		}
	}
}
