using UnityEngine;

public class BottomWallTrigger : MonoBehaviour
{
    public IntVariable playerLives;
    public GameEvent OnBallExit;

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.CompareTag("ball"))
        {
            // check for number of balls remaining
            playerLives.Value -= 1;
            OnBallExit.Raise();
        }
        if (hitInfo.CompareTag("powerUp"))
        {
            Destroy(hitInfo.gameObject, 2f);
        }
    }
}
