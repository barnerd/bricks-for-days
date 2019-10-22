using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InputController", menuName = "Input Controller/AI Controller")]
public class AIController : InputController
{
    public float thresholdToPaddle;
    public float heightToFocusOnBall;
    public float waitTimeToReleaseBall;

    private Paddle paddle;
    private Collider2D paddleCollider;
    private Vector3 paddleCenter;
    private float paddleWidth;

    public override void Initialize(GameObject obj)
    {
        paddle = obj.GetComponent<Paddle>();
        paddleCollider = obj.GetComponent<Collider2D>();
    }

    public override void ProcessInput(GameObject obj)
    {
        paddleCenter = paddleCollider.bounds.center;
        paddleWidth = paddleCollider.bounds.extents.x;
        float paddleHeight = paddleCenter.y;

        Vector3 closestBall = new Vector3();
        float minDistance = 99999f;

        for (int i = 0; i < paddle.balls.Count; i++)
        {
            // cache components
            Ball b = paddle.balls[i].GetComponent<Ball>();
            float ballYVelocity = paddle.balls[i].GetComponent<Rigidbody2D>().velocity.y;

            // find closest ball
            float newDistance = Vector3.Distance(paddleCenter, paddle.balls[i].GetComponent<Collider2D>().bounds.center);
            float ballHeight = paddle.balls[i].GetComponent<Collider2D>().bounds.center.y;

            if (newDistance < minDistance && ballHeight > paddleHeight && !b.ballHeld && ballYVelocity < 0)
            {
                closestBall = paddle.balls[i].GetComponent<Collider2D>().bounds.center;
                minDistance = newDistance;
            }
        }

        Vector3 closestPowerUp = GetClosestPowerUp();
        Vector3 bestBrick = GetBestBrick();

        /******* Logic *******
         *
         * If all the balls are held, collect power ups
         *
         * if a ball or power up is under the threshold
         * chase after power ups until the ball is closer
         * then go after the ball
         *
         * else
         * position under the highest level brick
         * release a ball
         * 
         */

        if (paddle.ballHeld && paddle.balls.Count == 1 && closestPowerUp != new Vector3(0, 0, 0))
        {
            //Debug.Log("Chasing Power Up because I'm holding all the balls");
            // move paddle Left
            if (paddleCenter.x > closestPowerUp.x)
            {
                paddle.MoveLeft();
            }
            // move paddle Right
            else if (paddleCenter.x < closestPowerUp.x)
            {
                paddle.MoveRight();
            }
        }
        else if (closestBall != new Vector3(0, 0, 0) && closestBall.y < heightToFocusOnBall)
        {
            //Debug.Log("Let's get that ball");
            // check ball to move left or right
            if (Mathf.Abs(closestBall.x - paddleCenter.x) > thresholdToPaddle)
            {
                // move paddle Left
                if (paddleCenter.x > closestBall.x)
                {
                    paddle.MoveLeft();
                }
                // move paddle Right
                else if (paddleCenter.x < closestBall.x)
                {
                    paddle.MoveRight();
                }
            }
        }
        else if (closestPowerUp != new Vector3(0, 0, 0) && Mathf.Abs(closestPowerUp.x - paddleCenter.x) > thresholdToPaddle)
        {
            //Debug.Log("Let's get that powerup");

            // move paddle Left
            if (paddleCenter.x > closestPowerUp.x)
            {
                paddle.MoveLeft();
            }
            // move paddle Right
            else if (paddleCenter.x < closestPowerUp.x)
            {
                paddle.MoveRight();
            }
        }
        else if (paddle.ballHeld && Mathf.Abs(bestBrick.x - paddleCenter.x) > thresholdToPaddle + paddleWidth / 2)
        {
            //Debug.Log("Positioning under a good brick because I have the ball");
            // move paddle Left
            if (paddleCenter.x > bestBrick.x)
            {
                paddle.MoveLeft();
            }
            // move paddle Right
            else if (paddleCenter.x < bestBrick.x)
            {
                paddle.MoveRight();
            }
        }
        else if (paddle.heldBall != null && paddle.ballHeld)
        {
            //Debug.Log("I can't hold onto the ball forever");
            paddle.StartCoroutine(ReleaseBallCoroutine(paddle.heldBall));
        }
    }

    private IEnumerator ReleaseBallCoroutine(Ball b)
    {
        yield return new WaitForSeconds(waitTimeToReleaseBall);
        b.ReleaseBall();
        paddle.ballHeld = false;
        paddle.heldBall = null;
    }

    private Vector3 GetClosestPowerUp()
    {
        Vector3 closestPowerUp = new Vector3();
        GameObject[] powerUps;
        float minDistance = 99999f;
        float newDistance;
        float paddleHeight = paddleCenter.y;
        float powerUpHeight;

        powerUps = GameObject.FindGameObjectsWithTag("powerUp");
        foreach (GameObject powerUp in powerUps)
        {
            newDistance = Vector3.Distance(paddleCenter, powerUp.GetComponent<Collider2D>().bounds.center);
            powerUpHeight = powerUp.GetComponent<Collider2D>().bounds.center.y;

            if (newDistance < minDistance && powerUpHeight > paddleHeight)
            {
                closestPowerUp = powerUp.GetComponent<Collider2D>().bounds.center;
                minDistance = newDistance;
            }
        }

        return closestPowerUp;
    }

    private Vector3 GetBestBrick()
    {
        Vector3 bestBrick = new Vector3();
        GameObject[] bricks;
        float minDistance = 99999f;
        float newDistance;
        int maxLevel = 0;
        int newLevel;

        bricks = GameObject.FindGameObjectsWithTag("brick");
        foreach (GameObject brick in bricks)
        {
            newDistance = Vector3.Distance(paddleCenter, brick.GetComponent<Collider2D>().bounds.center);
            newLevel = brick.GetComponent<Brick>().level;

            if (newLevel > maxLevel || (newDistance < minDistance && newLevel == maxLevel))
            {
                bestBrick = brick.GetComponent<Collider2D>().bounds.center;
                minDistance = newDistance;
                maxLevel = newLevel;
            }
        }

        return bestBrick;
    }
}
