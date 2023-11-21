using System.Collections;
using UnityEngine;

public class player : MonoBehaviour
{
    private float horizontal;
    private float speed = 4f;
    private float jumpingPower = 8f;
    private bool isFacingRight = true;

    private bool isPunching = false;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private string inputNameHorizontal;
    [SerializeField] private string inputNameJump;

    [SerializeField] private Transform punchPoint;
    [SerializeField] private float punchRadius = 0.5f;
    [SerializeField] private string inputNamePunch;

    void Update()
    {
        horizontal = Input.GetAxisRaw(inputNameHorizontal);

        if (Input.GetButtonDown(inputNameJump) && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }

        if (Input.GetButtonUp(inputNameJump) && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        if (Input.GetButtonDown(inputNamePunch) && !isPunching)
        {
            StartCoroutine(Punch());
        }

        Flip();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
    private IEnumerator Punch()
    {
        isPunching = true;
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(punchPoint.position, punchRadius);
        foreach (Collider2D playerCollider in hitPlayers)
        {
            if (playerCollider.CompareTag("Player") && playerCollider.gameObject != gameObject)
            {
                Rigidbody2D otherPlayerRb = playerCollider.GetComponent<Rigidbody2D>();
                if (otherPlayerRb != null)
                {
                    Vector2 knockbackDirection = (otherPlayerRb.transform.position - transform.position).normalized;
                    otherPlayerRb.AddForce(knockbackDirection * 5f, ForceMode2D.Impulse);
                }
            }
        }
        yield return new WaitForSeconds(0.5f);
        isPunching = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(punchPoint.position, punchRadius);
    }
}