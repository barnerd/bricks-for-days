using UnityEngine;

public class Paddle : MonoBehaviour
{
    // cache the Rigidbody on this paddle
    private Rigidbody2D paddle;
    public GameObject ball;

    public float paddleSpeed = 500f;
    public float paddleForce = 100f;

    public InputController inputs;

    private void Start()
    {
        paddle = GetComponent<Rigidbody2D>();
        inputs.Initialize(gameObject);
    }

    // FixedUpdate is used with physics
    void FixedUpdate()
    {
        inputs.ProcessInput(gameObject);

        // need to clamp velocity to 0 to max speed
        paddle.velocity = new Vector2(Mathf.Clamp(paddle.velocity.x, -paddleSpeed, paddleSpeed), 0);
    }

    public void MoveLeft()
    {
        paddle.AddForce(new Vector2(-paddleSpeed * Time.deltaTime, 0));
    }

    public void MoveRight()
    {
        paddle.AddForce(new Vector2(paddleSpeed * Time.deltaTime, 0));
    }
}
