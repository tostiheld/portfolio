using System;
using System.Reflection;
using System.Collections.Generic;

namespace Roadplus.Server
{
    public static class UtilityMethods
    {
        /// <summary>
        /// Tries to find a type from a string in the current Assembly
        /// </summary>
        /// <returns>
        /// <c>true</c>, if a single type in the current 
        /// namespace was found, <c>false</c> otherwise.
        /// </returns>
        /// <param name="value">The string that contains the type name</param>
        /// <param name="result">A type if it was found, otherwise null</param>
        public static bool TryFindType(string value, out Type result)
        {
            List<Type> foundTypes = new List<Type>();
            result = null;
            
            foreach (Type t in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (t.Name == value)
                {
                    foundTypes.Add(t);
                }
            }

            if (foundTypes.Count == 1)
            {
                result = foundTypes[0];
                return true;
            }

            return false;
        }
    }
}

