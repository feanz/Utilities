using System;
using System.Collections.Generic;

namespace Utilities
{
	/// <summary>
	/// A wrapper that holds dictionary multiple dictionaries with TFirst as the key and TSecond as the value and vice versa.
	/// This allows fast lookup by either value
	/// </summary>
	/// <typeparam name="TFirst"></typeparam>
	/// <typeparam name="TSecond"></typeparam>
	public class BiDictionary<TFirst, TSecond>
	{
		readonly IDictionary<TFirst, TSecond> firstToSecond = new Dictionary<TFirst, TSecond>();
		readonly IDictionary<TSecond, TFirst> secondToFirst = new Dictionary<TSecond, TFirst>();

		/// <summary>
		/// Add key,value and value,key pair to bi dictionary
		/// </summary>
		/// <param name="first"></param>
		/// <param name="second"></param>
		public void Add(TFirst first, TSecond second)
		{
			if (firstToSecond.ContainsKey(first) ||
				secondToFirst.ContainsKey(second))
			{
				throw new ArgumentException("Duplicate first or second");
			}
			firstToSecond.Add(first, second);
			secondToFirst.Add(second, first);
		}

		public TSecond this[TFirst first]
		{
			get { return GetByFirst(first); }
		}

		public TFirst this[TSecond second]
		{
			get { return GetBySecond(second); }
		}

		private TSecond GetByFirst(TFirst first)
		{
			TSecond item;
			if (!firstToSecond.TryGetValue(first, out item))
			{
				throw new KeyNotFoundException();
			}
			return item;
		}

		private TFirst GetBySecond(TSecond second)
		{
			TFirst item;
			if (!secondToFirst.TryGetValue(second, out item))
			{
				throw new KeyNotFoundException();
			}
			return item;
		}
		
		/// <summary>
		/// Access TSecond using TFirst as key
		/// </summary>
		/// <param name="first"></param>
		/// <param name="second"></param>
		/// <returns></returns>
		public bool TryGetByFirst(TFirst first, out TSecond second)
		{
			return firstToSecond.TryGetValue(first, out second);
		}

		/// <summary>
		/// Access TFirest using TSecond as key
		/// </summary>
		/// <param name="second"></param>
		/// <param name="first"></param>
		/// <returns></returns>
		public bool TryGetBySecond(TSecond second, out TFirst first)
		{
			return secondToFirst.TryGetValue(second, out first);
		}
	}
}