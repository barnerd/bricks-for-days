using UnityEngine;

public class BottomWallTrigger : MonoBehaviour
{
    public IntReference numBalls;
    public IntReference playerLives;
    public GameEvent OnBallExit;
    public GameEvent OnGameOver;

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.CompareTag("ball"))
        {
            if (numBalls.Value > 1)
            {
                numBalls.Value--;
                Destroy(hitInfo.gameObject);
            }
            else
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
