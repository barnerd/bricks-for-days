using UnityEngine;

public class BottomWallTrigger : MonoBehaviour
{
    public IntVariable playerLives;

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.CompareTag("ball"))
        {
            playerLives.Value -= 1;
            hitInfo.GetComponent<Ball>().ResetBall();
        }
        if (hitInfo.CompareTag("powerUp"))
        {
            Destroy(hitInfo.gameObject, 2f);
        }
    }
}
