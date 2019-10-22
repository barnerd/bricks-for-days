using UnityEngine;

public class BottomWallTrigger : MonoBehaviour
{
    public Paddle paddle;
    public IntReference playerLives;
    public GameEvent OnBallExit;
    public GameEvent OnGameOver;

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.CompareTag("ball"))
        {
            paddle.balls.Remove(hitInfo.gameObject);
            Destroy(hitInfo.gameObject);

            if (paddle.balls.Count <= 1) // <= 1 because the Destroy hasn't happened yet
            {
                playerLives.Value -= 1;

                if (playerLives.Value <= 0)
                {
                    OnGameOver.Raise();
                }
                else
                {
                    OnBallExit.Raise();
                }
            }
        }
        if (hitInfo.CompareTag("powerUp"))
        {
            Destroy(hitInfo.gameObject, 2f);
        }
    }
}
