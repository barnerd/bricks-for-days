using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    // cache the Rigidbody on this paddle
    private Rigidbody2D paddle;
    public GameObject ballPrefab;
    public List<GameObject> balls;
    public Transform ballPositionOnPaddle;
    public BoolReference ballAlwaysHeld;
    public bool ballHeld;
    public Ball heldBall;

    public float paddleSpeed = 500f;
    public float paddleForce = 100f;

    public InputController playerInputs;
    public InputController aiInputs;
    private InputController currentController;

    public KeyCode setAI; // define as i

    public BoolReference gameOver;

    #region Start and Update
    private void Start()
    {
        // cache components
        paddle = GetComponent<Rigidbody2D>();

        // initialize input controllers
        playerInputs.Initialize(gameObject);
        aiInputs.Initialize(gameObject);

        currentController = aiInputs;

        InitializeBall();
    }

    // Called once a frame.
    void Update()
    {
        if (Input.GetKeyUp(setAI))
        {
            SetInputController(aiInputs);
        }

        if (ballHeld && heldBall != null)
        {
            heldBall.transform.position = ballPositionOnPaddle.position;
        }
    }

    // FixedUpdate is used with physics
    void FixedUpdate()
    {
        if (!gameOver.Value)
        {
            currentController.ProcessInput(gameObject);
        }

        // need to clamp velocity to 0 to max speed
        paddle.velocity = new Vector2(Mathf.Clamp(paddle.velocity.x, -paddleSpeed, paddleSpeed), 0);
    }
    #endregion

    #region Controls
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
    #endregion

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "ball")
        {
            Ball b = collision.gameObject.GetComponent<Ball>();

            if (ballAlwaysHeld.Value && !ballHeld)
            {
                ballHeld = true;
                heldBall = b;
                b.ballHeld = true;
                b.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                b.transform.position = ballPositionOnPaddle.position;
            }
            else
            {
                float ballCenter = collision.contacts[0].point.x;
                float paddleCenter = collision.collider.bounds.center.x;
                float paddleWidth = collision.collider.bounds.extents.x;
                float percentOnPaddle = (ballCenter - paddleCenter) / paddleWidth;

                b.GetComponent<Rigidbody2D>().AddForce(new Vector2(paddleForce * Mathf.Sin(percentOnPaddle), 0));
            }
        }
    }

    public void ResetPowerUps()
    {
        ballAlwaysHeld.Value = ballAlwaysHeld.Variable.InitialValue;
    }

    public void InitializeBall()
    {
        ClearBalls();

        Ball b = Instantiate(ballPrefab, ballPositionOnPaddle.position, Quaternion.identity).GetComponent<Ball>();
        balls.Add(b.gameObject);
        ballHeld = true;
        heldBall = b;
    }

    public void ClearBalls()
    {
        foreach (var b in balls)
        {
            Destroy(b);
        }
        balls.Clear();
    }

    public void splitBalls(ScriptableObject powerUp)
    {
        PowerUpSetInt splitBallsPowerUp = (PowerUpSetInt)powerUp;
        int numNewBalls = splitBallsPowerUp._value;

        List<GameObject> ballsToAdd = new List<GameObject>();
        Ball[] newBalls = new Ball[numNewBalls];

        if (balls.Count < 9)
        {
            for (int i = balls.Count - 1; i >= 0; i--)
            {
                Ball thisBall = balls[i].GetComponent<Ball>();

                for (int j = 0; j < numNewBalls - 1; j++)
                {
                    newBalls[j] = Instantiate(ballPrefab, new Vector3(0, 0, 99f), Quaternion.identity).GetComponent<Ball>();
                    newBalls[j].transform.position = thisBall.transform.position;

                    // split velocities by 33 degrees
                    newBalls[j].GetComponent<Rigidbody2D>().velocity = ((j % 2 == 0) ? 1 : -1) * Mathf.Cos(45f) * thisBall.GetComponent<Rigidbody2D>().velocity;
                    //balls[i].GetComponent<Rigidbody2D>().velocity = Quaternion.AngleAxis(-33f, Vector3.up) * thisBall.GetComponent<Rigidbody2D>().velocity;

                    // set ball speed?
                    // set ball size?
                    // set ball power?
                    // set ball trail?
                    newBalls[j].ballHeld = false;

                    ballsToAdd.Add(newBalls[j].gameObject);
                }
            }

            foreach (var b in ballsToAdd)
            {
                balls.Add(b);
            }
        }
    }
}
