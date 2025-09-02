using UnityEngine;

public class JumpKingController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float minJumpForce = 5f;
    [SerializeField] private float maxJumpForce = 15f;
    [SerializeField] private float chargeSpeed = 5f;
    [SerializeField] private float jumpHeight = 10f;
    [SerializeField] private float jumpDelay = 0.2f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private float jumpCharge;
    private int lastMoveDirection = 1;
    private bool isGrounded;
    private float lastJumpTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
            lastMoveDirection = -1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
            lastMoveDirection = 1;
        }

        if (isGrounded && Input.GetKey(KeyCode.Space))
        {
            jumpCharge += chargeSpeed * Time.deltaTime;
            jumpCharge = Mathf.Clamp(jumpCharge, minJumpForce, maxJumpForce);

            float t = (jumpCharge - minJumpForce) / (maxJumpForce - minJumpForce);
            sr.color = new Color(1f, 1f - t, 1f - t);
        }

        if (isGrounded && Input.GetKeyUp(KeyCode.Space) && Time.time > lastJumpTime + jumpDelay)
        {
            rb.linearVelocity = Vector2.zero;
            Vector2 jumpVector = new Vector2(lastMoveDirection * jumpCharge, jumpHeight);
            rb.AddForce(jumpVector, ForceMode2D.Impulse);

            jumpCharge = 0;
            sr.color = Color.white;
            isGrounded = false;
            lastJumpTime = Time.time;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        isGrounded = true;
    }
}
