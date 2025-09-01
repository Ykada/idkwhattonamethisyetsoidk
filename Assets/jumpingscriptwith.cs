using UnityEngine;
using static Unity.VisualScripting.Member;

public class jumpingscriptwith : MonoBehaviour
{

    public float maxChargeTime = 2f;
    public float maxJumpForce = 15f;
    public float jumpCooldown = 0.1f;
    public float horizontalMoveSpeed = 5f;
    private Rigidbody2D rb;
    private float chargeTimer = 0f;
    private bool isCharging = false;
    private bool isGrounded = false;
    private bool canCharge = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        HandleHorizontalInput();

        if (isGrounded && canCharge)
        {
            HandleJumpCharging();
        }
    }

    void HandleHorizontalInput()
    {
        float move = 0f;

        if (Input.GetKey(KeyCode.A))
            move = -1f;
        else if (Input.GetKey(KeyCode.D))
            move = 1f;

        Vector2 velocity = rb.linearVelocity;
        velocity.x = move * horizontalMoveSpeed;
        rb.linearVelocity = velocity;
    }

    void HandleJumpCharging()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isCharging = true;
            chargeTimer = 0f;
        }

        if (Input.GetKey(KeyCode.Space) && isCharging)
        {
            chargeTimer += Time.deltaTime;
            chargeTimer = Mathf.Clamp(chargeTimer, 0, maxChargeTime);
        }

        if (Input.GetKeyUp(KeyCode.Space) && isCharging)
        {
            float chargePercent = chargeTimer / maxChargeTime;
            float jumpForce = chargePercent * maxJumpForce;

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

            isCharging = false;
            canCharge = false;
            Invoke(nameof(ResetCharge), jumpCooldown);
        }
    }

    void ResetCharge()
    {
        canCharge = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts.Length > 0)
        {
            if (collision.contacts[0].normal.y > 0.5f)
            {
                isGrounded = true;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }
}
