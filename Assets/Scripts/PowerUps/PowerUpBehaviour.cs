using UnityEngine;

public class PowerUpBehaviour : MonoBehaviour
{
    public PowerUp powerUp;
    public FloatVariable powerUpDropSpeed;

    private void Start()
    {
        GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, powerUpDropSpeed.Value));
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("paddle"))
        {
            powerUp.UsePowerUpPayload();
            Destroy(gameObject);
        }
    }

    public void DestroyPowerUp()
    {
        Destroy(gameObject);
    }
}