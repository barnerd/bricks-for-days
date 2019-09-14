using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public GameController gc;
    public Text scoreText;

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + gc.score;
    }
}
