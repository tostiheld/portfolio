using System;
using System.Collections.Generic;
using Roadplus.Server.Map;

namespace Roadplus.Server
{
    public class ZoneCollection : IList<Roadplus.Server.Map.Zone>
    {
        private List<Zone> internalList;

        public ZoneCollection()
        {
            internalList = new List<Zone>();
        }

        #region IList implementation

        public int IndexOf(Roadplus.Server.Map.Zone item)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, Roadplus.Server.Map.Zone item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public Roadplus.Server.Map.Zone this[int index]
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region ICollection implementation

        public void Add(Roadplus.Server.Map.Zone item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(Roadplus.Server.Map.Zone item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(Roadplus.Server.Map.Zone[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(Roadplus.Server.Map.Zone item)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsReadOnly
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region IEnumerable implementation

        public IEnumerator<Roadplus.Server.Map.Zone> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IEnumerable implementation

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

