using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public IntReference gameScore;
    public Text scoreText;

    // Update is called once per frame
    void Update()
    {
        scoreText.text = gameScore.Value.ToString();
    }
}
