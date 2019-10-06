using UnityEngine;

[CreateAssetMenu(fileName = "InputController", menuName = "Input Controller/Player Controller")]
public class PlayerController : InputController
{
    public KeyCode moveLeft; // define as a
    public KeyCode moveRight; // define as d
    public KeyCode releaseBall; // define as s

    public override void ProcessInput(GameObject obj)
    {
        Paddle paddle = obj.GetComponent<Paddle>();
        Ball ball = paddle.ball.GetComponent<Ball>();

        // move paddle Left
        if (Input.GetKey(moveLeft))
        {
            paddle.MoveLeft();
        }
        // move paddle Right
        else if (Input.GetKey(moveRight))
        {
            paddle.MoveRight();
        }

        // If the ball needs to be released, release ball on key stroke
        if (ball.ballHeld && Input.GetKeyUp(releaseBall))
        {
            ball.ReleaseBall();
        }
    }
}
