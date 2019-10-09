using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    public static void SaveHighScores(Highscores hs)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/bricks.highscores";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, hs);
        stream.Close();
    }

    public static Highscores LoadHighScores()
    {
        string path = Application.persistentDataPath + "/bricks.highscores";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            Highscores hs = formatter.Deserialize(stream) as Highscores; 
            stream.Close();

            return hs;
        }
        else
        {
            Debug.LogError("Highscores file doesn't exsist");
            return null;
        }
    }
}
