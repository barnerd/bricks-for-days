using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// pulled from https://wiki.unity3d.com/index.php/SerializableDictionary

[CreateAssetMenu(fileName = "WeightedPowerUps", menuName = "Structures/Weighted Power Ups")]
public class WeightedPowerUps : ScriptableObject
{
    [SerializeField]
    private PowerUpIntDictionary powerUpIntLookup = PowerUpIntDictionary.New<PowerUpIntDictionary>();
    private Dictionary<PowerUp, int> powerUpInts
    {
        get { return powerUpIntLookup.dictionary; }
    }

    /// <summary>
    /// Randomizes one item
    /// </summary>
    /// <param name="spawnRate">An ordered list withe the current spawn rates. The list will be updated so that selected items will have a smaller chance of being repeated.</param>
    /// <returns>The randomized item.</returns>
    public PowerUp TakeOne()
    {
        // Sorts the spawn rate list
        var sortedSpawnRate = Sort(powerUpInts);

        // Sums all spawn rates
        int sum = 0;
        foreach (var spawn in powerUpInts.Values)
        {
            sum += spawn;
        }

        // Randomizes a number from Zero to Sum
        //int roll = _random.Next(0, sum);
        int roll = UnityEngine.Random.Range(0, sum);

        // Finds chosen item based on spawn rate
        PowerUp selected = sortedSpawnRate[sortedSpawnRate.Count - 1].Key;
        foreach (var spawn in sortedSpawnRate)
        {
            if (roll < spawn.Value)
            {
                selected = spawn.Key;
                break;
            }
            roll -= spawn.Value;
        }

        // Returns the selected item
        return selected;
    }

    private List<KeyValuePair<PowerUp, int>> Sort(Dictionary<PowerUp, int> weights)
    {
        var list = new List<KeyValuePair<PowerUp, int>>(weights);

        // Sorts the Spawn Rate List for randomization later
        list.Sort(
            delegate (KeyValuePair<PowerUp, int> firstPair,
                     KeyValuePair<PowerUp, int> nextPair)
            {
                return firstPair.Value.CompareTo(nextPair.Value);
            }
         );

        return list;
    }
}
