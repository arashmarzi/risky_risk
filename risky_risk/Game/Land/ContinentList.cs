using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace RiskAi.Game.Land
{
    /// <summary>
    /// The NodeList class represents a collection of nodes.  Internally, it uses a Hashtable instance to provide
    /// fast lookup via a <see cref="Node"/> class's <b>Key</b> value.  The <see cref="Graph"/> class maintains its
    /// list of nodes via this class.
    /// </summary>
    [Serializable]
    public class ContinentList : IEnumerable, ICloneable
    {
        // private member variables
        private Hashtable data = new Hashtable();

        public ContinentList() { }

        public ContinentList (Hashtable data)
        {
            this.data = data;
        }

        #region Public Methods
        /// <summary>
        /// Adds a new Continent to the NodeList.
        /// </summary>
        public virtual void Add(Continent c)
        {
            data.Add(c.Name, c);
        }

        /// <summary>
        /// Removes a Continent from the NodeList.
        /// </summary>
        public virtual void Remove(Continent c)
        {
            data.Remove(c.Name);
        }

        /// <summary>
        /// Determines if a node with a specified <b>Key</b> value exists in the NodeList.
        /// </summary>
        /// <param name="key">The <b>Key</b> value to search for.</param>
        /// <returns><b>True</b> if a Continent with the specified <b>Key</b> exists in the NodeList; <b>False</b> otherwise.</returns>
        public virtual bool ContainsKey(string name)
        {
            return data.ContainsKey(name);
        }

        /// <summary>
        /// Clears out all of the nodes from the NodeList.
        /// </summary>
        public virtual void Clear()
        {
            data.Clear();
        }

        /// <summary>
        /// Returns an enumerator that can be used to iterate through the Nodes.
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            return new NodeListEnumerator(data.GetEnumerator());
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
        /// Returns a particular <see cref="Node"/> instance by index.
        /// </summary>
        public virtual Continent this[string name]
        {
            get
            {
                return (Continent)data[name];
            }
        }

        /// <summary>
        /// Returns the number of nodes in the NodeList.
        /// </summary>
        public virtual int Count
        {
            get
            {
                return data.Count;
            }
        }
        #endregion



        #region NodeList Enumerator
        /// <summary>
        /// The NodeListEnumerator method is a custom enumerator for the NodeList object.  It essentially serves
        /// as an enumerator over the NodeList's Hashtable class, but rather than returning DictionaryEntry values,
        /// it returns just the Continent object.
        /// <p />
        /// This allows for a developer using the Board class to use a foreach to enumerate the Nodes in the graph.
        /// </summary>
        public class NodeListEnumerator : IEnumerator, IDisposable
        {
            IDictionaryEnumerator list;
            public NodeListEnumerator(IDictionaryEnumerator coll)
            {
                list = coll;
            }

            public void Reset()
            {
                list.Reset();
            }

            public bool MoveNext()
            {
                return list.MoveNext();
            }

            public Continent Current
            {
                get
                {
                    return (Continent)((DictionaryEntry)list.Current).Value;
                }
            }

            // The current property on the IEnumerator interface:
            object IEnumerator.Current
            {
                get
                {
                    return (Current);
                }
            }

            public void Dispose()
            {
                list = null;
            }
        }
        #endregion
    }
}