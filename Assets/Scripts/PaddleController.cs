using UnityEngine;

public class PaddleController : MonoBehaviour
{
    public KeyCode moveLeft; // define as a
    public KeyCode moveRight; // define as d

    private Rigidbody2D paddle;

    public float paddleSpeed = 500f;
    public float paddleForce = 100f;

    private void Start()
    {
        paddle = GetComponent<Rigidbody2D>();
    }

    // FixedUpdate is used with physics
    void FixedUpdate()
    {
        // move paddle Left
        if (Input.GetKey(moveLeft))
        {
            paddle.AddForce(new Vector2(-paddleSpeed * Time.deltaTime, 0));
        }
        // move paddle Right
        else if (Input.GetKey(moveRight))
        {
            paddle.AddForce(new Vector2(paddleSpeed * Time.deltaTime, 0));
        }

        // stop moving
        else
        {
            if (Mathf.Abs(paddle.velocity.x) > 0)
            {
                if (paddle.velocity.x > 0)
                {
                    paddle.AddForce(new Vector2(-paddleSpeed * Time.deltaTime / .5f, 0));
                }
                else if (paddle.velocity.x < 0)
                {
                    paddle.AddForce(new Vector2(paddleSpeed * Time.deltaTime / .5f, 0));
                }
            }
        }
        // need to clamp velocity to 0 to max speed
        paddle.velocity = new Vector2(Mathf.Clamp(paddle.velocity.x, -paddleSpeed, paddleSpeed), 0);

        // remove drifting
        if (Mathf.Abs(paddle.velocity.x) < 0.01)
        {
            paddle.velocity = new Vector2(0, 0);
        }
    }
}
