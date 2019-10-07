using UnityEngine;

public class Ball : MonoBehaviour
{
    public Transform paddle;
    public Transform ballPositionOnPaddle;
    public float ballSpeed = 50f;
    public FloatVariable ballSpeedMultiplier;
    public bool ballHeld;
    public BoolVariable ballAlwaysHeld;
    public IntVariable ballPower;

    [Header("Sprites")]
    public Sprite ballSprite;
    public Color ballSpriteColor;
    [Space()]
    public Sprite bananaSprite;
    public Color bananaSpriteColor;

    public BoolVariable bananaBall;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        ResetBall();
    }

    // Update is called once per frame
    void Update()
    {
        if (ballHeld)
        {
            transform.position = new Vector2(paddle.transform.position.x, transform.position.y);
        }
        else
        {
            transform.Rotate(Vector3.forward * 10f);
        }
    }

    // FixedUpdate is used with physics
    void FixedUpdate()
    {
        if (ballHeld)
        {
            rb.velocity = new Vector2(0f, 0f);
        }
        else
        {
            // maintain a constant speed
            rb.velocity = ballSpeed * ballSpeedMultiplier.Value * rb.velocity.normalized;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "paddle")
        {
            if (ballAlwaysHeld.Value)
            {
                ballHeld = true;
                ResetBallPosition();
            }
            else
            {
                float ballCenter = collision.contacts[0].point.x;
                float paddleCenter = collision.collider.bounds.center.x;
                float paddleWidth = collision.collider.bounds.extents.x;
                float percentOnPaddle = (ballCenter - paddleCenter) / paddleWidth;

                // TODO: turn paddleForce into a FloatVariable and reference it here, instead of having GetComponent<Paddle>
                rb.AddForce(new Vector2(paddle.GetComponent<Paddle>().paddleForce * Mathf.Sin(percentOnPaddle) * Mathf.PI / 2, 0));
            }
        }
    }

    public void ResetBall()
    {
        // Reset Power Ups
        ballSpeedMultiplier.Value = 1f;
        ballPower.Value = 1;
        ballAlwaysHeld.Value = false;

        bananaBall.Value = false;
        SetSprite();

        ResetBallPosition();
    }

    public void ResetBallPosition()
    {
        ballHeld = true;
        rb.velocity = Vector2.zero;

        // use an x value of ballAlwaysHeld.Value ? transform.position.x : paddle.transform.position.x
        // but this conflicts with Update();
        transform.position = ballPositionOnPaddle.position;
    }

    public void ReleaseBall()
    {
        ballHeld = false;
        // add random angle from 80 to 100 degrees, or something like that.
        float angle = Mathf.Deg2Rad * Random.Range(80, 100);
        rb.velocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
    }

    public void SetSprite()
    {
        if (bananaBall.Value)
        {
            spriteRenderer.sprite = bananaSprite;
            spriteRenderer.color = bananaSpriteColor;
        }
        else
        {
            spriteRenderer.sprite = ballSprite;
            spriteRenderer.color = ballSpriteColor;
        }
    }
}
