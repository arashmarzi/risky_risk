using System;

namespace RiskAi.Game.Land
{
	[Serializable]
	public class Continent
	{
		public string Name { get; set; }
		public int Bonus { get; set; }
		public int NumTerritories { get; set; }

		public readonly string[] continents = { "africa", "asia", "australia", "europe", "northamerica", "southamerica" };

		/// <summary>
		/// Initializes a new instance of the <see cref="T:RiskAi.Game.Land.Continent"/> class.
		/// </summary>
		/// <param name="name">Name.</param>
		public Continent(string name)
		{
			this.Name = name;
			this.Bonus = getBonus(Name);
			this.NumTerritories = getNumTerritories(Name);
		}

		// must remove unnecessary methods later, all this done through json

		/// <summary>
		/// Gets the number of territories.
		/// </summary>
		/// <returns>The number territories.</returns>
		/// <param name="name">Name.</param>
		private int getNumTerritories(string name)
		{
			int numTerritories = 0;
			switch (Array.IndexOf(continents, name))
			{
				case 0: // africa
					{
						numTerritories = 6;
						break;
					}
				case 1: // asia
					{
						numTerritories = 12;
						break;
					}
				case 2: // australia
					{
						numTerritories = 4;
						break;
					}
				case 3: // europe
					{
						numTerritories = 7;
						break;
					}
				case 4: // north america
					{
						numTerritories = 9;
						break;
					}
				case 5: // south america
					{
						numTerritories = 4;
						break;
					}
			}
			return numTerritories;
		}

		/// <summary>
		/// Gets the bonus.
		/// </summary>
		/// <returns>The bonus.</returns>
		/// <param name="name">Name.</param>
		private int getBonus(string name)
		{
			int bonus = 0;
			switch (Array.IndexOf(continents, name))
			{
				case 0: // africa
					{
						bonus = 3;
						break;
					}
				case 1: // asia
					{
						bonus = 7;
						break;
					}
				case 2: // australia
					{
						bonus = 2;
						break;
					}
				case 3: // europe
					{
						bonus = 5;
						break;
					}
				case 4: // north america:
					{
						bonus = 5;
						break;
					}
				case 5: // south america
					{
						bonus = 2;
						break;
					}
			}

			return bonus;
		}
	}
}
