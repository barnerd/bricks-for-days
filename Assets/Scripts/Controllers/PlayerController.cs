using UnityEngine;

[CreateAssetMenu(fileName = "InputController", menuName = "Input Controller/Player Controller")]
public class PlayerController : InputController
{
    public KeyCode moveLeft; // define as a
    public KeyCode moveRight; // define as d
    public KeyCode releaseBall; // define as s

    private Paddle paddle;
    private Collider2D paddleCollider;
    private Vector3 paddleCenter;

    private Camera mainCamera;

    public override void Initialize(GameObject obj)
    {
        paddle = obj.GetComponent<Paddle>();
        paddleCollider = obj.GetComponent<Collider2D>();

        mainCamera = Camera.main;
    }

    public override void ProcessInput(GameObject obj)
    {
        for (int i = 0; i < paddle.balls.Count; i++)
        {
            // holding ball
            if (paddle.balls[i].GetComponent<Ball>().ballHeld)
            {
                paddle.heldBall = paddle.balls[i].GetComponent<Ball>();
                break;
            }
        }

        // Controls on a keyboard
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
        else if (paddle.heldBall != null && paddle.ballHeld && Input.GetKeyUp(releaseBall))
        {
            paddle.heldBall.ReleaseBall();
            paddle.ballHeld = false;
            paddle.heldBall = null;
        }

        // Controls on a touchscreen
        if (Input.touchCount > 0)
        {
            paddleCenter = paddleCollider.bounds.center;
            Touch touch = Input.GetTouch(0);
            Vector3 touchPosition = mainCamera.ScreenToWorldPoint(touch.position);

            if(touchPosition.x < paddleCenter.x)
            {
                paddle.MoveLeft();
            }
            else if(touchPosition.x > paddleCenter.x)
            {
                paddle.MoveRight();
            }
            if (paddle.heldBall != null && paddle.ballHeld && touch.phase == TouchPhase.Began)
            {
                paddle.heldBall.ReleaseBall();
                paddle.ballHeld = false;
                paddle.heldBall = null;
            }
        }
    }
}
