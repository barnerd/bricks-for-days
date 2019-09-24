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
            transform.position = new Vector2(paddle.transform.position.x, transform.position.y);
        }
        //Debug.Log(GetComponent<Rigidbody2D>().velocity);
        //Debug.Log(GetComponent<Rigidbody2D>().velocity.magnitude);

    }

    // FixedUpdate is used with physics
    void FixedUpdate()
    {
        // If the ball needs to be released, release ball on key stroke
        if (ballHeld && Input.GetKey(releaseBall))
        {
            ballHeld = false;
            // add random angle from 80 to 100 degrees, or something like that.
            float angle = Mathf.Deg2Rad * Random.Range(80, 100);
            //Debug.Log("Angle: " + angle + " cos: " + ballSpeed * Mathf.Cos(angle) + " sin: " + ballSpeed * Mathf.Sin(angle));
            GetComponent<Rigidbody2D>().velocity = new Vector2(ballSpeed * Mathf.Cos(angle), ballSpeed * Mathf.Sin(angle));
        }

        GetComponent<Rigidbody2D>().velocity = ballSpeed * (GetComponent<Rigidbody2D>().velocity.normalized);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "paddle")
        {
            float ballCenter = collision.contacts[0].point.x;
            float paddleCenter = collision.collider.bounds.center.x;
            float paddleWidth = collision.collider.bounds.extents.x;
            float percentOnPaddle = (ballCenter - paddleCenter) / paddleWidth;

            GetComponent<Rigidbody2D>().AddForce(new Vector2(paddle.GetComponent<PaddleController>().paddleForce * Mathf.Sin(percentOnPaddle) * Mathf.PI / 2, 0));
        }
    }

    public void ResetBall()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
        transform.position = new Vector2(paddle.transform.position.x, paddle.GetComponent<CapsuleCollider2D>().bounds.center.y + paddle.GetComponent<CapsuleCollider2D>().bounds.extents.y + GetComponent<CircleCollider2D>().bounds.extents.y);
        ballHeld = true;
    }
}
