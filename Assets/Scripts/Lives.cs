using UnityEngine;
using UnityEngine.UI;

public class Lives : MonoBehaviour
{
    public IntVariable playerLives;
    public Text livesText;

    // Update is called once per frame
    public void UpdateText()
    {
        livesText.text = "Lives: " + playerLives.Value;
    }
}
