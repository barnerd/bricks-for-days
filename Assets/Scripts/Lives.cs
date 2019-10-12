using UnityEngine;
using UnityEngine.UI;

public class Lives : MonoBehaviour
{
    public IntReference playerLives;
    public Text livesText;

    // Update is called once per frame
    public void UpdateText()
    {
        livesText.text = playerLives.Value.ToString();
    }
}
