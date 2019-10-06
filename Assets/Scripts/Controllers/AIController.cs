﻿using System.Collections;
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

    private Ball ball;
    private Collider2D ballCollider;
    private Rigidbody2D ballRigidBody;
    private Vector3 ballCenter;

    public override void Initialize(GameObject obj)
    {
        paddle = obj.GetComponent<Paddle>();
        paddleCollider = obj.GetComponent<Collider2D>();

        ball = paddle.ball.GetComponent<Ball>();
        ballCollider = paddle.ball.GetComponent<Collider2D>();
        ballRigidBody = paddle.ball.GetComponent<Rigidbody2D>();
    }

    public override void ProcessInput(GameObject obj)
    {
        paddleCenter = paddleCollider.bounds.center;
        paddleWidth = paddleCollider.bounds.extents.x;
        ballCenter = ballCollider.bounds.center;

        Vector3 closestPowerUp = GetClosestPowerUp();
        Vector3 bestBrick = GetBestBrick();

        /******* Logic *******
         *
         * If the ball is held, collect power ups
         * position under the highest level brick
         * release ball
         *
         * else
         * chase after power ups until the ball is closer
         * then go after the ball
         *
         */


        if (ball.ballHeld)
        {
            if (closestPowerUp != new Vector3(0, 0, 0))
            {
                Debug.Log("Chasing Power Up because I'm holding the ball");
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
            else if (Mathf.Abs(bestBrick.x - paddleCenter.x) > thresholdToPaddle + paddleWidth / 2)
            {
                Debug.Log("Positioning under a good brick because I have the ball");
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
            else
            {
                Debug.Log("I can't hold onto the ball forever");
                ball.StartCoroutine(ReleaseBallCoroutine(ball));
            }
        }
        else
        {
            /*
             * if there's no powerup, go after the ball
             * if the ball is closer go after the ball
             * else go after the powerup, if there's one
             * 
             * */
            if (closestPowerUp == new Vector3(0, 0, 0))
            {
                Debug.Log("Let's get that ball");
                // check ball to move left or right
                if (Mathf.Abs(ballCenter.x - paddleCenter.x) > thresholdToPaddle)
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
            }
            else
            {
                if (Vector3.Distance(paddleCenter, ballCenter) < Vector3.Distance(paddleCenter, closestPowerUp) || (ballRigidBody.velocity.y < 0 && ballCenter.y < heightToFocusOnBall))
                {
                    Debug.Log("Let's get that ball");
                    // check ball to move left or right
                    if (Mathf.Abs(ballCenter.x - paddleCenter.x) > thresholdToPaddle)
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
                }
                else
                {
                    Debug.Log("Let's get that powerup");
                    // check ball to move left or right
                    if (Mathf.Abs(closestPowerUp.x - paddleCenter.x) > thresholdToPaddle)
                    {
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
                }
            }
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

            if(newLevel > maxLevel || (newDistance < minDistance && newLevel == maxLevel))
            {
                bestBrick = brick.GetComponent<Collider2D>().bounds.center;
                minDistance = newDistance;
                maxLevel = newLevel;
            }
        }

        return bestBrick;
    }
}