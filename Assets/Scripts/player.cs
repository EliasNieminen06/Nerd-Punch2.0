using System.Collections;
using UnityEngine;

public class player : MonoBehaviour
{
    private float speed = 4f;
    private float jumpingPower = 8f;
    private float attackRange = 0.5f;
    private int attackDamage = 10;
    private int maxHealth = 100;
    private float attackRate = 2f;
    private float nextAttackTime = 0f;
    private float healRate = 0.2f;
    private float nextHealTime = 0f;
    private float knockBackForce = 10;
    private float knockBackTimer = 0f;
    private float knockBackTotalTime = 0.2f;
    private bool isFacingRight = true;
    private AudioSource audioSource;
    private float dashingPower = 24f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;

    private int currentHealth;
    private float horizontal;
    private bool knockFromRight;
    private bool gameStarted;
    private bool canDash = true;
    private bool isDashing;
    private Vector2 startPos;

    [SerializeField] private Animator anim;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask playerLayers;

    [SerializeField] private string inputNameHorizontal;
    [SerializeField] private string inputNameJump;
    [SerializeField] private string inputNameAttack;
    [SerializeField] private string inputNameDash;

    [SerializeField] private AudioClip[] punchSounds;

    [SerializeField] private AudioClip dashSound;

    [SerializeField] private GameManager gameManager;

    [SerializeField] private PlayerUI playerUI;

    [SerializeField] private GameObject opponent;
    [SerializeField] private GameObject scamera;

    [SerializeField] private AnimationCurve punchCurve;
    [SerializeField] private AnimationCurve fallDieCurve;

    private void Start()
    {
        currentHealth = maxHealth;
        audioSource = this.GetComponent<AudioSource>();
        gameStarted = false;
        startPos = transform.position;
    }

    void Update()
    {
        if (gameStarted)
        {
            if (!isDashing)
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

                if (Input.GetButtonDown(inputNameDash) && canDash)
                {
                    StartCoroutine(Dash());
                }

                if (Time.time >= nextHealTime && currentHealth < 100)
                {
                    currentHealth = currentHealth + 2;
                    nextHealTime = Time.time + 1f / healRate;
                }

                if (Time.time >= nextAttackTime)
                {
                    if (Input.GetButtonDown(inputNameAttack))
                    {
                        anim.SetTrigger("attack");

                        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayers);
                        foreach (Collider2D player in hitPlayers)
                        {
                            player.GetComponent<player>().TakeDamage(attackDamage, transform.position);
                        }
                        nextAttackTime = Time.time + 1f / attackRate;
                    }

                }
            }
            else
            {
                horizontal = 0;
            }
            playerUI.health = currentHealth;
            Flip();
        }
        gameStarted = gameManager.gameStarted;
    }

    private void FixedUpdate()
    {
        if (!isDashing)
        {
            if (knockBackTimer <= 0)
            {
                if (gameStarted)
                {
                    rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
                }
                else
                {
                    rb.velocity = new Vector2(0, 0);
                }
            }
            else
            {
                if (knockFromRight == true)
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
        audioSource.volume = 1;
        audioSource.Play();
        scamera.GetComponent<camera>().Shake(1, punchCurve);

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

    public void Die()
    {
        currentHealth -= currentHealth;
        playerUI.health = 0;
        anim.SetBool("isDead", true);
        gameManager.EndRound(opponent);
    }

    public void respawn()
    {
        anim.SetBool("isDead", false);
        anim.SetTrigger("respawn");
        currentHealth = maxHealth;
        playerUI.health = 100;
        transform.position = startPos;
    }

    private void onFallDie()
    {
        scamera.GetComponent<camera>().Shake(1, fallDieCurve);
    }

    IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        audioSource.clip = dashSound;
        audioSource.volume = 0.5f;
        audioSource.Play();
        yield return new WaitForSeconds(dashingTime);
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
}