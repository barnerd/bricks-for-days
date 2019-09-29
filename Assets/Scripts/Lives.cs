using UnityEngine;
using UnityEngine.UI;

public class Lives : MonoBehaviour
{
    public IntVariable playerLives;
    public Text livesText;

    // Update is called once per frame
    void Update()
    {
        livesText.text = "Lives: " + playerLives.Value;
    }
}
