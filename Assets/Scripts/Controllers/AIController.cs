using UnityEngine;

[CreateAssetMenu(fileName = "InputController", menuName = "Input Controller/AI Controller")]
public class AIController : InputController
{
    public float thresholdToBall;

    public override void ProcessInput(GameObject obj)
    {
        Paddle paddle = obj.GetComponent<Paddle>();
        float paddleCenter = obj.GetComponent<Collider2D>().bounds.center.x;
        float ballCenter = paddle.ball.GetComponent<Collider2D>().bounds.center.x;

        // TODO: Track PowerUps and determine to go get them

        if (Mathf.Abs(ballCenter - paddleCenter) > thresholdToBall)
        {
            // move paddle Left
            if (paddleCenter > ballCenter)
            {
                paddle.MoveLeft();
            }
            // move paddle Right
            else if (paddleCenter < ballCenter)
            {
                paddle.MoveRight();
            }
        }
    }
}
