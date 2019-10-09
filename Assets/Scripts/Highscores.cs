using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class Highscores
{
	public string[] names;
	public DateTime[] dates;
	public int[] scores;

    public Highscores(int length = 10)
	{
		names = new string[length];
		dates = new DateTime[length];
		scores = new int[length];
	}

    public int AddHighscore(int _score)
    {
        for (int i = 0; i < scores.Length; i++)
        {
            if (_score > scores[i])
            {
                // highscore found. make space
                for (int j = scores.Length - 1; j > i; j--)
                {
                    names[j] = names[j - 1];
                    dates[j] = dates[j - 1];
                    scores[j] = scores[j - 1];
                }

                names[i] = "";
                dates[i] = DateTime.Now;
                scores[i] = _score;

                return i;
            }
        }
        return -1;
    }
}
