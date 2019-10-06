using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "InputController", menuName = "Input Controller/AI Controller")]
public class AIController : InputController
{
    public float thresholdToBall;
    public float waitTimeToReleaseBall;

    private Paddle paddle;
    private Collider2D paddleCollider;
    private Vector3 paddleCenter;

    private Ball ball;
    private Collider2D ballCollider;
    private Vector3 ballCenter;


    public override void Initialize(GameObject obj)
    {
        paddle = obj.GetComponent<Paddle>();
        paddleCollider = obj.GetComponent<Collider2D>();

        ball = paddle.ball.GetComponent<Ball>();
        ballCollider = paddle.ball.GetComponent<Collider2D>();
    }

    public override void ProcessInput(GameObject obj)
    {
        paddleCenter = paddleCollider.bounds.center;
        ballCenter = ballCollider.bounds.center;

        // TODO: Track PowerUps and determine to go get them
        Vector3 closestPowerUp = GetClosestPowerUp();
        if (closestPowerUp != new Vector3(0, 0, 0))
        {
            Debug.Log("There's a power up. go after it.");
        }

        // check ball to move left or right
        if (Mathf.Abs(ballCenter.x - paddleCenter.x) > thresholdToBall)
        {
            // move paddle Left
            if (paddleCenter.x > ballCenter.x)
            {
                paddle.MoveLeft();
            }
            // move paddle Right
            else if (paddleCenter.x < ballCenter.x)
            {
                paddle.MoveRight();
            }
        }

        // TODO: move to the a good position under bricks and then release ball
        // If the ball needs to be released, release ball
        if (ball.ballHeld)
        {
            ball.StartCoroutine(ReleaseBallCoroutine(ball));
        }
    }

    IEnumerator ReleaseBallCoroutine(Ball b)
    {
        yield return new WaitForSeconds(waitTimeToReleaseBall);
        b.ReleaseBall();
    }

    private Vector3 GetClosestPowerUp()
    {
        Vector3 closestPowerUp = new Vector3();
        GameObject[] powerUps;
        float minDistance = 99999f;
        float newDistance;

        powerUps = GameObject.FindGameObjectsWithTag("powerUp");
        foreach (GameObject powerUp in powerUps)
        {
            newDistance = Vector3.Distance(paddleCenter, powerUp.GetComponent<Collider2D>().bounds.center);

            if (newDistance < minDistance)
            {
                closestPowerUp = powerUp.GetComponent<Collider2D>().bounds.center;
                minDistance = newDistance;
            }
        }

        return closestPowerUp;
    }
}
