using UnityEngine;

public class Ball : MonoBehaviour
{
    [Header("Ball Speed")]
    public float ballSpeed = 50f;
    public float minBallSpeed = .3333f;
    public float maxBallSpeed = 5f;
    public FloatReference ballSpeedMultiplier;

    [Header("Ball Size")]
    public FloatReference ballSizeScaler;

    [Header("Ball hold & power")]
    public bool ballHeld;
    public IntReference ballPower;

    [Header("Sprites")]
    public Sprite ballSprite;
    public Color ballSpriteColor;

    [Space()]
    public Sprite bananaSprite;
    public Color bananaSpriteColor;

    public BoolReference bananaBall;

    [Header("Trail")]
    public Gradient normalTrail;
    public Gradient powerballTrail;

    // cache components
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private AudioSource bounceSound;
    private TrailRenderer trail;

    #region Start and Update
    // Start is called before the first frame update
    void Start()
    {
        // cache Components
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        bounceSound = GetComponent<AudioSource>();
        trail = GetComponent<TrailRenderer>();

        // Reset Power Ups
        SetBallSize();
        SetBallTrail();
        SetSprite();
    }

    // Update is called once per frame
    void Update()
    {
        if (!ballHeld)
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
    #endregion

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!ballHeld && collision.collider.tag != "ball")
        {
            bounceSound.Play();
        }
    }

    public void ResetPowerUps()
    {
        ballSpeedMultiplier.Value = ballSpeedMultiplier.Variable.InitialValue;
        ballPower.Value = ballPower.Variable.InitialValue;
        bananaBall.Value = bananaBall.Variable.InitialValue;
        ballSizeScaler.Value = ballSizeScaler.Variable.InitialValue;
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

    public void SetBallSize()
    {
        transform.localScale = Vector3.one * ballSizeScaler.Value;
    }

    public void ClampBallSpeed()
    {
        // ensure that the ball speed multiplier doesn't get below min, or above max
        ballSpeedMultiplier.Value = Mathf.Clamp(ballSpeedMultiplier.Value, minBallSpeed, maxBallSpeed);
    }

    public void SetBallTrail()
    {
        trail.colorGradient = (ballPower.Value == 1) ? normalTrail : powerballTrail;
    }
}
