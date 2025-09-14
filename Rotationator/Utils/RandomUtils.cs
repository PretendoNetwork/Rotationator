namespace Rotationator.Utils;

public static class RandomUtils
{

    /// <summary>
    /// Utility function to pick a random element from a pool.
    /// </summary>
    /// <param name="pool">The pool of items to pick from</param>
    /// <param name="filter">Function to filter which items are allowed to be returned</param>
    /// <param name="random">The seeded random</param>
    /// <returns>A random filtered item from the pool</returns>
    public static T GetRandomElementFromPool<T>(List<T> pool, Func<T, bool> filter, SeadRandom random)
    {
        T element;
        int tries = 0;
    
        do
        {
            element = pool[random.GetInt32(pool.Count)];

            tries++;

            if (tries > 10000)
            {
                throw new Exception("Possible infinite loop detected in GetRandomElementFromPool");
            }
        } while (!filter(element));
    
        pool.Remove(element);

        return element;
    }
}