using RiskAi.Graph.Land;
using System.Collections.Generic;
using RiskAi.Graph;
using System.Linq;

namespace RiskAi.Player
{
    /// Agent represents a game player
    public class Agent
    {
        public string Name { get; set; }
        public int TotalTroops { get; set; }
        public int AvailableTroops { get; set; }
        public Dictionary<string, int> ControlledTerritories { get; set; } // territory name and number of troops on territory
        public List<string> cards { get; set; }

        /// <summary>Constructor for initializing a player</summary>
        /// <param name="Name">Player name</param>
        /// <param name="TotalTroops">Player's total troop count</param>
        /// <param name="ControlledTerritories">List of player's controlled territories</param>
        public Agent(string Name, int TotalTroops, List<Territory> ControlledTerritories)
        {
            this.Name = Name;
            this.TotalTroops = TotalTroops;
            this.ControlledTerritories = new Dictionary<string, int>();
            this.AvailableTroops = TotalTroops;
            this.cards = new List<string>();

            foreach ( Territory t in ControlledTerritories )
            {
                this.ControlledTerritories.Add(t.Name, 0);
            }
        }

        /// <summary>Apply continent bonus to player's AvailableTroops count</summary>
        /// <param name="Board">Representation of the current game board</param>
        public void ApplyContinentBonus(Board board)
        {
            AvailableTroops += CalculateContinentBonus(board);
        }

        /// <summary>Calculate player's continent bonus based on number of territories controlled</summary>
        /// <param name="Board">Representation of the current game board</param>
        /// <return>int representing player's continent bonus</return>
        private int CalculateContinentBonus(Board board)
        {
            int bonus = 0;
            foreach ( Continent c in board.Continents )
            {
                // get all territories of a given continent
                var result = from Territory t in board.Territories 
                             where t.Continent == c.Name 
                             select board.Territories[t.Name];

                // get all controlled territories of the continent
                var cTerritories = from Territory t in board.Territories 
                                   where (t.Continent == c.Name) && (t.Owner == Name) 
                                   select board.Territories[t.Name];
            }
        }

        /// <summary>Remove player's card bonus set from its hand</summary>
        /// <remarks>Will end up having to do a CanCashIn check before calling CardsToCashIn</remarks>
        /// <param name="Board">Representation of the current game board</param>
        public void ApplyCardBonus(Board board)
        {
            List<string> cards = CardsToCashIn();

            foreach ( string card in cards )
            {
                this.cards.Remove(card);
            }

            board.UpdateCardBonus();
        }

        /// <summary>Determine if player can cash in its cards</summary>
        /// <returns>true if player can cash in, false otherwise</returns>
        public bool CanCashIn()
        {
            if ( cards.Count >= 5 )
            {
                return true;
            }
            else if ( hasCardSet(cards) )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>Determine if player has a valid card combination that it can cash in</summary>
        /// <param name="Cards">List of player's cards</param>
        /// <returns>true if the player has a valid card combination, false otherwise</returns>
        private bool hasCardSet(List<string> Cards)
        {
            int soldiers = (from card in Cards where card == "soldier" select card).Count();
            int cannons = (from card in Cards where card == "cannon" select card).Count();
            int horses = (from card in Cards where card == "horse" select card).Count();

            if ( soldiers > 3 )
            {
                return true;
            }
            else if ( cannons > 3 )
            {
                return true;
            }
            else if ( horses > 3 )
            {
                return true;
            }
            else if ( soldiers >= 1 && 
                      cannons >= 1 && 
                      horses >= 1 )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>Create a valid card combination set, from the player's hand, to cash in</summary>
        /// <remarks>This function may need revision to actually remove the valid card combination from the player's hand</remarks>
        /// <returns>Valid card conmbination based on what is available in player's hand</returns>
        private List<string> CardsToCashIn(List<string> Cards)
        {
            
            int soldiers = (from card in Cards where card == "soldier" select card).Count();
            int cannons = (from card in Cards where card == "cannon" select card).Count();
            int horses = (from card in Cards where card == "horse" select card).Count();

            if (soldiers > 3)
            {
                return new List<string> { "soldier", "soldier", "soldier"};
            }
            else if (cannons > 3)
            {
                return new List<string> { "cannon", "cannon", "cannon" };
            }
            else if (horses > 3)
            {
                return new List<string> { "horse", "horse", "horse" }; ;
            }
            else if ( soldiers >= 1 && 
                      cannons >= 1 &&
                      horses >= 1 )
            {
                return new List<string> { "soldier", "cannon", "horse" };
            }
        }

        /// <summary>Add a given territory to player's territory list</summary>
        /// <param name="Name">Name of territory</param>
        /// <param name="NumTroops">Number of troops to be on the given territory</param>
        public void GainTerritory(string Name, int NumTroops)
        {
            int result;
            if ( !ControlledTerritories.TryGetValue(Name, out result) )
            {
                ControlledTerritories.Add(Name, PlaceTroops(NumTroops));
            }
        }

        /// <summary>Place troops on a territory by updating AvailableTroops count</summary>
        /// <remarks>This function name and behavior might get refactored later</remarks>
        /// <param name="NumTroops">Number of troops to place</param>
        /// <returns>Number of troops placed</returns>
        private int PlaceTroops(int NumTroops)
        {
            if (this.AvailableTroops - numTroops < 0)
            {
                return 0;
            }
            else
            {
                this.AvailableTroops -= numTroops;
                return numTroops;
            }
        }

        /// <summary>Place troops on a given territory by updating troops on the territory</summary>
        /// <param name="Name">Name of territory to place troops</param>
        /// <param name="NumTroops">Number of troops to place</param>
        public void PlaceReinforcements(string Name, int NumTroops)
        {
            ControlledTerritories[Name] += AddTroopsToTerritory(NumTroops);
        }

        /// <summary>Update AvailableTroops and TotalTroops based on reinforcements</summary>
        /// <param name="Reinforcements">Number of reinforcement troops</param>
        public void TroopsReinforcements(int Reinforcements)
        {
            AvailableTroops += reinforcements;
            TotalTroops += reinforcements;
        }
    }
}
