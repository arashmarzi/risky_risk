using System;
using RiskAi.Graph.Land;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace RiskAi.Graph
{
    /// <summary>
    /// The Board class represents a graph, which is composed of a collection of nodes and edges.  This Board class
    /// maintains its collection of nodes using the NodeList class, which is a collection of Territory objects.
    /// It delegates the edge maintenance to the Territory class.  The Territory class maintains the edge information using
    /// the adjacency list technique.
    /// </summary>
    [Serializable]
    public class Board : ICloneable
    {
        #region Private Member Variables
        // private member variables
        private TerritoryList territories;
        private ContinentList continents;
        private int cardBonus;
        #endregion

        #region Constructor
        /// <summary>
        /// Default constructor.  Creates a new Board class instance.
        /// </summary>
        public Board()
        {
            territories = new TerritoryList();
            continents = new ContinentList();
            cardBonus = 4;
        }

        /// <summary>
        /// Creates a new graph class instance based on a list of nodes.
        /// </summary>
        /// <param name="territories">The list of nodes to populate the newly created Board class with.</param>
        public Board(TerritoryList territories, ContinentList continents)
        {
            this.territories = territories;
            this.continents = continents;
            cardBonus = 4;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Clears out all of the nodes in the graph.
        /// </summary>
        public virtual void Clear()
        {
            territories.Clear();
            continents.Clear();
        }

        #region Adding Node Methods
        /// <summary>
        /// Adds a new node to the graph.
        /// </summary>
        /// <param name="key">The key value of the node to add.</param>
        /// <param name="data">The data of the node to add.</param>
        /// <returns>A reference to the Territory that was created and added to the graph.</returns>
        /// <remarks>If there already exists a node in the graph with the same <b>key</b> value then an
        /// <b>ArgumentException</b> exception will be thrown.</remarks>
        public virtual Territory AddNode(Territory t)
        {
            // Make sure the key is unique
            if (!territories.ContainsKey(t.Name))
            {
                territories.Add(t);
                return t;
            }
            else
                throw new ArgumentException("There already exists a node in the graph with name " + t.Name);
        }

        public virtual Continent AddNode(Continent c)
        {
            if (!continents.ContainsKey(c.Name))
            {
                continents.Add(c);
                return c;
            }
            else
                throw new ArgumentException("There already exists a node in the graph with name " + c.Name);
        }
        #endregion

        public void UpdateCardBonus()
        {
            if(cardBonus < 60)
            {
                cardBonus += 2;
            }
            else
            {
                cardBonus = 4;
            }
        }

        #region Contains Methods
        /// <summary>
        /// Determines if a node exists within the graph.
        /// </summary>
        /// <param name="t">The node to check for in the graph.</param>
        /// <returns><b>True</b> if the node <b>n</b> exists in the graph, <b>False</b> otherwise.</returns>
        public virtual bool Contains(Territory t)
        {
            return ContainsTerritory(t.Name);
        }

        public virtual bool Contains(Continent c)
        {
            return ContainsContinent(c.Name);
        }
        #endregion

        /// <summary>
        /// Determines if a node exists within the graph.
        /// </summary>
        /// <param name="key">The key of the node to check for in the graph.</param>
        /// <returns><b>True</b> if a node with key <b>key</b> exists in the graph, <b>False</b> otherwise.</returns>
        public virtual bool ContainsTerritory(string name)
        {
            return territories.ContainsKey(name);
        }

        public virtual bool ContainsContinent(string name)
        {
            return continents.ContainsKey(name);
        }

        public object Clone()
        {
            BinaryFormatter BF = new BinaryFormatter();
            MemoryStream memStream = new MemoryStream();

            BF.Serialize(memStream, this);
            memStream.Flush();
            memStream.Position = 0;

            return (BF.Deserialize(memStream));
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Returns the number of nodes in the graph.
        /// </summary>
        public virtual int Count
        {
            get
            {
                return territories.Count;
            }
        }

        /// <summary>
        /// Returns a reference to the set of nodes in the graph.
        /// </summary>
        public virtual TerritoryList Territories
        {
            get
            {
                return this.territories;
            }
        }

        public virtual ContinentList Continents
        {
            get
            {
                return this.continents;
            }
        }

        public virtual int CardBonus
        {
            get
            {
                return cardBonus;
            }
            set
            {
                cardBonus = value;
            }
        }
        #endregion

    }
}