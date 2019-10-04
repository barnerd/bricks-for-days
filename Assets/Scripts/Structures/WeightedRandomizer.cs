using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Static class to improve readability
/// Grabbed from https://wiki.unity3d.com/index.php/WeightedRandomizer
/// Example:
/// <code>
/// var selected = WeightedRandomizer.From(weights).TakeOne();
/// </code>
/// 
/// </summary>
/* public static class WeightedRandomizer
{
    public static WeightedRandomizer<R> From<R>(Dictionary<R, int> spawnRate)
    {
        return new WeightedRandomizer<R>(spawnRate);
    }
} */

public class WeightedRandomizer<T> : ScriptableObject
{
    //private static Random _random = new Random();
    protected List<WeightedObject<T>> _weights;

    /// <summary>
    /// Instead of calling this constructor directly,
    /// consider calling a static method on the WeightedRandomizer (non-generic) class
    /// for a more readable method call, i.e.:
    /// 
    /// <code>
    /// var selected = WeightedRandomizer.From(weights).TakeOne();
    /// </code>
    /// 
    /// </summary>
    /// <param name="weights"></param>
    /*public WeightedRandomizer(Dictionary<T, int> weights)
    {
        _weights = weights;
    }*/

    /// <summary>
    /// Randomizes one item
    /// </summary>
    /// <param name="spawnRate">An ordered list withe the current spawn rates. The list will be updated so that selected items will have a smaller chance of being repeated.</param>
    /// <returns>The randomized item.</returns>
    public T TakeOne()
    {
        // Sorts the spawn rate list
        //var sortedSpawnRate = Sort(weights);
        var sortedSpawnRate = Sort();

        // Sums all spawn rates
        int sum = 0;
        foreach (var spawn in _weights)
        {
            sum += spawn.weight;
        }

        // Randomizes a number from Zero to Sum
        //int roll = _random.Next(0, sum);
        int roll = Random.Range(0, sum);

        // Finds chosen item based on spawn rate
        T selected = sortedSpawnRate[sortedSpawnRate.Count - 1].obj;
        foreach (var spawn in sortedSpawnRate)
        {
            if (roll < spawn.weight)
            {
                selected = spawn.obj;
                break;
            }
            roll -= spawn.weight;
        }

        // Returns the selected item
        return selected;
    }

    //private List<KeyValuePair<T, int>> Sort(Dictionary<T, int> weights)
    private List<WeightedObject<T>> Sort()
    {
        var list = new List<WeightedObject<T>>(_weights);

        // Sorts the Spawn Rate List for randomization later
        list.Sort(
            delegate (WeightedObject<T> firstPair,
                     WeightedObject<T> nextPair)
            {
                return firstPair.weight.CompareTo(nextPair.weight);
            }
         );

        return list;
    }
}
