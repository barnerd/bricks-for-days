using UnityEngine;

public class BottomWallController : MonoBehaviour
{
    public IntVariable playerLives;

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.tag == "ball")
        {
            playerLives.Value -= 1;
            hitInfo.GetComponent<BallController>().ResetBall();
        }
    }
}
