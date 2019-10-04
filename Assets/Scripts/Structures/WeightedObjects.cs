using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// pulled from https://wiki.unity3d.com/index.php/SerializableDictionary

[CreateAssetMenu(fileName = "WeightedObjects", menuName = "Structures/Weighted Objects")]
public class WeightedObjects : ScriptableObject
{
    [SerializeField]
    private GameObjectIntDictionary gameObjectIntLookup = GameObjectIntDictionary.New<GameObjectIntDictionary>();
    private Dictionary<GameObject, int> gameObjectWeights
    {
        get { return gameObjectIntLookup.dictionary; }
    }

    /// <summary>
    /// Randomizes one item
    /// </summary>
    /// <param name="spawnRate">An ordered list withe the current spawn rates. The list will be updated so that selected items will have a smaller chance of being repeated.</param>
    /// <returns>The randomized item.</returns>
    public GameObject TakeOne()
    {
        // Sorts the spawn rate list
        var sortedSpawnRate = Sort(gameObjectWeights);

        // Sums all spawn rates
        int sum = 0;
        foreach (var spawn in gameObjectWeights.Values)
        {
            sum += spawn;
        }

        // Randomizes a number from Zero to Sum
        //int roll = _random.Next(0, sum);
        int roll = UnityEngine.Random.Range(0, sum);

        // Finds chosen item based on spawn rate
        GameObject selected = sortedSpawnRate[sortedSpawnRate.Count - 1].Key;
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

    private List<KeyValuePair<GameObject, int>> Sort(Dictionary<GameObject, int> weights)
    {
        var list = new List<KeyValuePair<GameObject, int>>(weights);

        // Sorts the Spawn Rate List for randomization later
        list.Sort(
            delegate (KeyValuePair<GameObject, int> firstPair,
                     KeyValuePair<GameObject, int> nextPair)
            {
                return firstPair.Value.CompareTo(nextPair.Value);
            }
         );

        return list;
    }
}
