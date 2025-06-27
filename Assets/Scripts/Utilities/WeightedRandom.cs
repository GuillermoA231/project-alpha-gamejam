using UnityEngine;

/// <summary>
/// A utility MonoBehaviour containing a static helper to pick an index from a weighted array.
/// </summary>
public class WeightedRandom : MonoBehaviour
{
    /// <summary>
    /// Selects an index based on weights. Each weight influences the chance of its index being returned.
    /// </summary>
    /// <param name="weights">An array where weights[i] is the relative chance of selecting index i.</param>
    /// <returns>The chosen index, from 0 to weights.Length - 1.</returns>
    public static int GetIndexByWeight(float[] weights)
    {
        // Sum all weights
        float totalWeight = 0f;
        foreach (float w in weights)
        {
            totalWeight += w;
        }

        // Pick a random value in [0, totalWeight)
        float randomValue = Random.value * totalWeight;

        // Walk the cumulative weights
        float cumulative = 0f;
        for (int i = 0; i < weights.Length; i++)
        {
            cumulative += weights[i];
            if (randomValue < cumulative)
            {
                return i;
            }
        }

        // In case of rounding errors, return the last index
        return weights.Length - 1;
    }
}
