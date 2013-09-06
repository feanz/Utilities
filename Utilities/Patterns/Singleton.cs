using System;
using System.Reflection;

namespace Utilities.Patterns
{
    /// <summary>
    ///   Base class used for singletons
    /// </summary>
    /// <typeparam name="T"> The class type </typeparam>
    public class Singleton<T> where T : class
    {
        private static T _Instance;

        /// <summary>
        ///   Constructor
        /// </summary>
        protected Singleton()
        {
        }

        /// <summary>
        ///   Gets the instance of the singleton
        /// </summary>
        public static T Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (typeof (T))
                    {
                        var Constructor = typeof (T).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic,
                                                                    null, new Type[0], null);
                        if (Constructor == null || Constructor.IsAssembly)
                            throw new Exception("Constructor is not private or protected for type " + typeof (T).Name);
                        _Instance = (T) Constructor.Invoke(null);
                    }
                }
                return _Instance;
            }
        }
    }
}