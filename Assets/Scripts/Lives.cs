using UnityEngine;
using UnityEngine.UI;

public class Lives : MonoBehaviour
{
    public GameController gc;
    public Text livesText;

    // Update is called once per frame
    void Update()
    {
        livesText.text = "Lives: " + gc.lives;
    }
}
