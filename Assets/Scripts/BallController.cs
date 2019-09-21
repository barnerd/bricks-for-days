using UnityEngine;

public class BallController : MonoBehaviour
{
    public Transform paddle;
    public KeyCode releaseBall;
    public float ballSpeed = 50f;
    public bool ballHeld;

    public GameController gc;
    public BrickManager bm;

    // Start is called before the first frame update
    void Start()
    {
        ResetBall();
    }

    // Update is called once per frame
    void Update()
    {
        if (ballHeld)
        {
            transform.position = new Vector2(paddle.position.x, transform.position.y);
        }
    }

    // FixedUpdate is used with physics
    void FixedUpdate()
    {
        // If the ball needs to be released, release ball on key stroke
        if (ballHeld && Input.GetKey(releaseBall))
        {
            ballHeld = false;
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, ballSpeed));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "paddle")
        {
            float ballCenter = collision.contacts[0].point.x;
            float paddleCenter = collision.collider.transform.position.x;
            float paddleWidth = collision.collider.transform.localScale.x / 2;
            float percentOnPaddle = (ballCenter - paddleCenter) / paddleWidth;

            // add force based on where on the paddle it hits
            // TODO: factor in current velocity?
            // TODO: factor in paddle velocity?
            GetComponent<Rigidbody2D>().AddForce(new Vector2(ballSpeed*Mathf.Sin(percentOnPaddle), 0));
        }
    }

    public void ResetBall()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
        transform.position = new Vector2(transform.position.x, paddle.localScale.y + paddle.position.y - transform.localScale.y);
        ballHeld = true;
    }
}
