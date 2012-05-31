using System;
using System.Collections.Generic;

namespace Utilities.Patterns
{
    /// <summary>
    ///   This interface can be used by classes that implement a factory pattern.
    /// </summary>
    /// <typeparam name="TKey"> Type of key. </typeparam>
    /// <typeparam name="T"> Type of generated classes. </typeparam>
    public class Factory<TKey, T>
    {
        /// <summary>
        ///   Dictionary of creators for a specific key.
        /// </summary>
        public static IDictionary<TKey, Func<T>> _creators = new Dictionary<TKey, Func<T>>();

        /// <summary>
        ///   Creation function.
        /// </summary>
        public static Func<T> _defaultCreator;

        /// <summary>
        ///   Default node.
        /// </summary>
        public static T _default;

        /// <summary>
        ///   Create an instance of type T using the key.
        /// </summary>
        /// <param name="key"> Key. </param>
        /// <returns> Created type. </returns>
        public static T Create(TKey key)
        {
            if (!_creators.ContainsKey(key))
                return default(T);

            return _creators[key]();
        }

        /// <summary>
        ///   Create default instance.
        /// </summary>
        /// <returns> Created type. </returns>
        public static T Create()
        {
            return _defaultCreator();
        }

        /// <summary>
        ///   Register a key to implementation.
        /// </summary>
        /// <param name="key"> Key. </param>
        /// <param name="result"> Result of key. </param>
        public static void Register(TKey key, T result)
        {
            _creators[key] = () => result;
        }

        /// <summary>
        ///   Registers the default implementation.
        /// </summary>
        /// <param name="key"> Key. </param>
        /// <param name="creator"> Corresponding creation function. </param>
        public static void Register(TKey key, Func<T> creator)
        {
            _creators[key] = creator;
        }

        /// <summary>
        ///   Registers the default implementation.
        /// </summary>
        /// <param name="result"> Default result. </param>
        public static void RegisterDefault(T result)
        {
            _default = result;
            _defaultCreator = () => _default;
        }

        /// <summary>
        ///   Register default implementation using creator func provided.
        /// </summary>
        /// <param name="creator"> Default creation function. </param>
        public static void RegisterDefault(Func<T> creator)
        {
            _defaultCreator = creator;
        }
    }
}