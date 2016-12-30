using System;
using System.Collections.Generic;

namespace RiskAi.Game.Land
{

	[Serializable]
	public class Territory
	{
		private string name;
		private string continent;
		private IList<string> neighbours;
		private string owner;
		private int troops;
		private string card;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:RiskAi.Game.Land.Territory"/> class.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="continent">Continent.</param>
		/// <param name="neighbours">Neighbours.</param>
		/// <param name="card">Card.</param>
		public Territory(string name, string continent, IList<string> neighbours, string card)
		{
			this.name = name;
			this.continent = continent;
			this.neighbours = neighbours;
			this.card = card;
		}

		/// <summary>
		/// Updates the owner.
		/// </summary>
		/// <param name="owner">Owner.</param>
		/// <param name="troops">Troops.</param>
		public void UpdateOwner(string owner, int troops)
		{
			this.owner = owner;
			this.troops = troops;
		}

		public virtual string Name
		{
			get
			{
				return name;
			}
			set
			{
				name = value;
			}
		}

		public virtual string Continent
		{
			get
			{
				return continent;
			}
			set
			{
				continent = value;
			}
		}

		public virtual IList<string> Neighbours
		{
			get
			{
				return neighbours;
			}
			set
			{
				neighbours = value;
			}
		}

		public virtual string Owner
		{
			get
			{
				return owner;
			}
			set
			{
				owner = value;
			}
		}

		public virtual int Troops
		{
			get
			{
				return troops;
			}
			set
			{
				troops = value;
			}
		}

		public virtual string Card
		{
			get
			{
				return card;
			}
			set
			{
				card = value;
			}
		}
	}
}