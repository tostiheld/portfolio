using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;

using ProtoBuf;

namespace Roadplus.Server.Traffic
{
    public class ZoneManager : IDictionary<int, Zone>
    {
        public int Count
        {
            get
            {
                return zones.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        
        public Zone this[int index]
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
        public ICollection<int> Keys
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        public ICollection<Zone> Values
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        private Dictionary<int, Zone> zones;

        public ZoneManager()
        {
            zones = new Dictionary<int, Zone>();
        }

        #region IDictionary implementation
        public void Add(int key, Zone value)
        {
            throw new NotImplementedException();
        }
        public bool ContainsKey(int key)
        {
            throw new NotImplementedException();
        }
        public bool Remove(int key)
        {
            throw new NotImplementedException();
        }
        public bool TryGetValue(int key, out Zone value)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region ICollection implementation
        public void Add(KeyValuePair<int, Zone> item)
        {
            throw new NotImplementedException();
        }
        public void Clear()
        {
            throw new NotImplementedException();
        }
        public bool Contains(KeyValuePair<int, Zone> item)
        {
            throw new NotImplementedException();
        }
        public void CopyTo(KeyValuePair<int, Zone>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }
        public bool Remove(KeyValuePair<int, Zone> item)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region IEnumerable implementation
        public IEnumerator<KeyValuePair<int, Zone>> GetEnumerator()
        {
            return zones.GetEnumerator();
        }
        #endregion


        public void Load()
        {
            try
            {
                throw new FileNotFoundException();
                using (FileStream fs = new FileStream(Settings.ZoneFilePath,
                                                      FileMode.Open))
                {
                    zones = Serializer.Deserialize<List<Zone>>(fs);
                }

                Trace.WriteLine("zones loaded.");
            }
            catch (FileNotFoundException)
            {
                zones = new Dictionary<int, Zone>();
                Trace.WriteLine("No zones file found, so new a collection was created.");
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        public void Save()
        {
            
            try
            {
                using (FileStream fs = new FileStream(Settings.ZoneFilePath,
                                                      FileMode.Create))
                {
                    Serializer.Serialize<List<Zone>>(fs, zones);
                }

                Trace.WriteLine("zones saved to file.");
            }
            catch(Exception ex)
            {
                throw new NotImplementedException();
            }
        }
    }
}

