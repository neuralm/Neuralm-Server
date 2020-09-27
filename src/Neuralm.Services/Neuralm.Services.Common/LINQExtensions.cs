using System;
using System.Collections.Generic;
using System.Linq;

namespace Neuralm.Services.Common
{
    /// <summary>
    /// Represents the <see cref="LINQExtensions"/> class.
    /// Used for extensions on enumerations.
    /// </summary>
    public static class LINQExtensions
    {
        /// <summary>
        /// Gets the item with the maximum value based on the given selector.
        /// </summary>
        /// <param name="items">The items to enumerate.</param>
        /// <param name="selector">The selector.</param>
        /// <typeparam name="T">The item type T.</typeparam>
        /// <typeparam name="U">The item type U.</typeparam>
        /// <returns>Returns the item with the maximum value based on the selector.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static T GetMax<T, U>(this IEnumerable<T> items, Func<T, U> selector)
        {
            if (!items.Any())
                throw new InvalidOperationException("Empty input sequence");

            Comparer<U> comparer = Comparer<U>.Default;
            T   maxItem  = items.First();
            U   maxValue = selector(maxItem);

            foreach (T item in items.Skip(1))
            {
                // Get the value of the item and compare it to the current max.
                U value = selector(item);
                if (comparer.Compare(value, maxValue) <= 0) 
                    continue;
                maxValue = value;
                maxItem  = item;
            }

            return maxItem;
        }
    }
}