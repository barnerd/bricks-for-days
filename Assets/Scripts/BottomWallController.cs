using UnityEngine;

public class BottomWallController : MonoBehaviour
{
    public GameController gc;

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.tag == "ball")
        {
            gc.LoseLife();
        }
    }
}
