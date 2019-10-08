using UnityEngine;

public class Paddle : MonoBehaviour
{
    // cache the Rigidbody on this paddle
    private Rigidbody2D paddle;
    public GameObject ball;

    public float paddleSpeed = 500f;
    public float paddleForce = 100f;

    public PlayerController playerInputs;
    public AIController aiInputs;
    private InputController currentController;

    public KeyCode setAI; // define as i

    private void Start()
    {
        paddle = GetComponent<Rigidbody2D>();
        playerInputs.Initialize(gameObject);
        aiInputs.Initialize(gameObject);

        currentController = aiInputs;
    }

    // Called once a frame.
    void Update()
    {
        if (Input.GetKeyUp(setAI))
        {
            SetInputController(aiInputs);
        }
    }

    // FixedUpdate is used with physics
    void FixedUpdate()
    {
        currentController.ProcessInput(gameObject);

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

    public void SetInputController(InputController _inputController)
    {
        currentController = _inputController;
    }
}
