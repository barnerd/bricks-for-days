using UnityEngine;

public class BottomWallTrigger : MonoBehaviour
{
    public IntReference playerLives;
    public GameEvent OnBallExit;
    public GameEvent OnGameOver;

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.CompareTag("ball"))
        {
            // check for number of balls remaining
            playerLives.Value -= 1;

            if(playerLives.Value <= 0)
            {
                OnGameOver.Raise();
            }
            else
            {
                OnBallExit.Raise();
            }
        }
        if (hitInfo.CompareTag("powerUp"))
        {
            Destroy(hitInfo.gameObject, 2f);
        }
    }
}
