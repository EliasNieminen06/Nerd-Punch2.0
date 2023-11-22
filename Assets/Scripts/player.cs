using System.Collections;
using UnityEngine;

public class player : MonoBehaviour
{
    private int currentHealth;
    private float horizontal;
    private float speed = 4f;
    private float jumpingPower = 8f;
    private bool isFacingRight = true;
    private float attackRange = 0.5f;
    private int attackDamage = 10;
    private int maxHealth = 100;
    private float attackRate = 2f;
    private float nextAttackTime = 0f;
    private float knockBackForce = 10;
    private float knockBackTimer = 0f;
    private float knockBackTotalTime = 0.2f;
    private bool knockFromRight;
    private AudioSource audioSource;

    [SerializeField] private Animator anim;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask playerLayers;

    [SerializeField] private string inputNameHorizontal;
    [SerializeField] private string inputNameJump;
    [SerializeField] private string inputNameAttack;

    [SerializeField] private AudioClip[] punchSounds;

    private void Start()
    {
        currentHealth = maxHealth;
        audioSource = this.GetComponent<AudioSource>();
    }

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

        if (Time.time >= nextAttackTime)
        {
            if (Input.GetButtonDown(inputNameAttack))
            {
                anim.SetTrigger("attack");

                Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayers);
                foreach(Collider2D player in hitPlayers)
                {
                    player.GetComponent<player>().TakeDamage(attackDamage, transform.position);
                }
                nextAttackTime = Time.time + 1f / attackRate;
            }

        }

        Flip();
    }

    private void FixedUpdate()
    {
        if (knockBackTimer <= 0)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }
        else
        {
            if(knockFromRight == true)
            {
                rb.velocity = new Vector2(-knockBackForce, 1);
                Flip();
            }
            if (knockFromRight == false)
            {
                rb.velocity = new Vector2(knockBackForce, 1);
                Flip();
            }
            knockBackTimer -= Time.deltaTime;
        }
        if (horizontal != 0f)
        {
            this.anim.SetBool("run", true);
        }
        else
        {
            this.anim.SetBool("run", false);
        }
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

    public void TakeDamage(int damage, Vector2 hitPos)
    {
        currentHealth -= damage;

        int punchSound = Random.Range(0, punchSounds.Length);
        audioSource.clip = punchSounds[punchSound];
        audioSource.Play();

        knockBackTimer = knockBackTotalTime;
        if(hitPos.x > transform.position.x)
        {
            knockFromRight = true;
        }
        if (hitPos.x <= transform.position.x)
        {
            knockFromRight = false;
        }

        Invoke("hurtAnimation", 0.2f);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void hurtAnimation()
    {
        anim.SetTrigger("hurt");
    }

    private void Die()
    {
        anim.SetBool("isDead", true);

        this.enabled = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}