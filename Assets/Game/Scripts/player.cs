using System.Collections;
using UnityEngine;
//using UnityEngine.InputSystem;

public class player : MonoBehaviour
{
    // Static Variables
    private float speed = 4f;
    private float jumpingPower = 8f;
    private float attackRange = 0.8f;
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

    // Dynamic Variables
    private int currentHealth;
    private float horizontal;
    private float attack;
    private bool knockFromRight;
    private bool gameStarted;
    private bool canDash = true;
    private bool isDashing;
    private Vector2 startPos;
    private float particleRotation;

    // Preference Variables
    [SerializeField] private Vector2 velocity;
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
    [SerializeField] private GameUI gameUI;
    [SerializeField] private GameObject opponent;
    [SerializeField] private GameObject scamera;
    [SerializeField] private AnimationCurve punchCurve;
    [SerializeField] private AnimationCurve fallDieCurve;
    [SerializeField] private TrailRenderer tr;
    [SerializeField] private GameObject hitParticle;

    // Start
    private void Start()
    {
        currentHealth = maxHealth;
        audioSource = this.GetComponent<AudioSource>();
        gameStarted = false;
        startPos = transform.position;
    }


  //  public void OnMove(InputAction.CallbackContext ctx) => movementInput = ctx.ReadValue<Vector2>();
   

    // Update
    void Update()
    {
        // Check if the game has started
        if (gameStarted)
        {
            // Check if the player is not currently dashing
            if (!isDashing)
            {
                // Get horizontal input
                horizontal = Input.GetAxisRaw(inputNameHorizontal);

                // Get attack input
                float attack = Input.GetAxisRaw(inputNameAttack);

                // Jump inputt down
                if (Input.GetButtonDown(inputNameJump) && IsGrounded())
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
                }

                // Jump input up
                if (Input.GetButtonUp(inputNameJump) && rb.velocity.y > 0f)
                {
                    rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
                }

                // Dash input
                if (Input.GetButtonDown(inputNameDash) && canDash)
                {
                    StartCoroutine(Dash());
                }

                // Healing by time
                if (Time.time >= nextHealTime && currentHealth < 100)
                {
                    currentHealth = currentHealth + 2;
                    nextHealTime = Time.time + 1f / healRate;
                }

                // Attack cooldown
                if (Time.time >= nextAttackTime)
                {
                    // Attack input and check if the player is grounded
                    if (Input.GetButtonDown(inputNameAttack) && IsGrounded() || attack > 0.1)
                    {
                        // Trigger attack animation
                        anim.SetTrigger("attack");
                        // Count down the cooldown
                        nextAttackTime = Time.time + 1f / attackRate;
                    }

                }
            }
            // If danshing do not move
            else
            {
                horizontal = 0;
            }
            // Change the PlayerUI health to the player health
            playerUI.health = currentHealth;
            // Check which player this is and change the health variable in gameUI to be the current health
            if (this.gameObject.name == "PlayerOne")
            {
                gameUI.p1health = currentHealth;
            }
            if (this.gameObject.name == "PlayerTwo")
            {
                gameUI.p2health = currentHealth;
            }
            // Call the flip function
            Flip();
            // check if player is not grounded and change the animation bool idle to false
            if (!IsGrounded())
            {
                anim.SetBool("idle", false);
            }
            else
            {
                anim.SetBool("idle", true);
            }
        }
        // Change the gameStarted variable in game manager to be true
        gameStarted = gameManager.gameStarted;
        // Set the velocty variable to be the rigidbody's velocity
        velocity = rb.velocity;
    }

    // Attack function
    public void Attack()
    {
        // Check for players inside an area near the player
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayers);
        // Loop all the players inside the area
        foreach (Collider2D player in hitPlayers)
        {
            // Check that the player is different than the current player
            if (player != this.gameObject.GetComponent<Collider2D>())
            {
                // Trigger the take damage function of the opponent player
                player.GetComponent<player>().TakeDamage(attackDamage, transform.position);
            }
        }
    }

    // Fixed Update
    private void FixedUpdate()
    {
        // Check if player is not dashing
        if (!isDashing)
        {
            // check if not currently taking knockback
            if (knockBackTimer <= 0)
            {
                // Check if game has started
                if (gameStarted)
                {
                    // Move the player with rigidbody
                    rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
                }
                else
                {
                    rb.velocity = new Vector2(0, 0);
                }
            }
            else
            {
                // Check if the knockback is coming from right
                if (knockFromRight == true)
                {
                    // Push the player to opposite direction
                    rb.velocity = new Vector2(-knockBackForce, 1);
                    Flip();
                }
                if (knockFromRight == false)
                {
                    rb.velocity = new Vector2(knockBackForce, 1);
                    Flip();
                }
                // Count down the knockback timer
                knockBackTimer -= Time.deltaTime;
            }
            // Animate running
            if (horizontal != 0f)
            {
                this.anim.SetBool("run", true);
            }
            else
            {
                this.anim.SetBool("run", false);
            }
        }

        // Animate jumping and falling
        if (!IsGrounded())
        {
            if (rb.velocity.y > 1)
            {
                anim.SetTrigger("jump");
            }
            else if (rb.velocity.y < -1)
            {
                anim.SetTrigger("fall");
            }
            else
            {
                anim.SetTrigger("levitate");
            }
        }
    }

    // Player Ground Check Function
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    // Player Flip Function
    private void Flip()
    {
        // Flip the player to be facing same direction its moving towards
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }


    // Player Take Damage Function
    public void TakeDamage(int damage, Vector2 hitPos)
    {
        // Decrease the health
        currentHealth -= damage;

        // play damage taken sound
        int punchSound = Random.Range(0, punchSounds.Length);
        audioSource.clip = punchSounds[punchSound];
        audioSource.volume = 1;
        audioSource.Play();
        scamera.GetComponent<camera>().Shake(1, punchCurve);

        // Check if the player was hit from right
        knockBackTimer = knockBackTotalTime;
        if (hitPos.x > (transform.position.x - 2))
        {
            knockFromRight = true;
        }
        if (hitPos.x <= transform.position.x)
        {
            knockFromRight = false;
        }

        // Play Hit Particle
        ParticleSystem ps = hitParticle.GetComponent<ParticleSystem>();
        if (this.gameObject.name == "PlayerOne")
        {
            ps.startColor = new Color32(91, 103, 160, 255);
        }
        if (this.gameObject.name == "PlayerTwo")
        {
            ps.startColor = new Color32(170, 213, 130, 255);
        }
        if (knockFromRight)
        {
            particleRotation = -90;
        }
        if (!knockFromRight)
        {
            particleRotation = 90;
        }
        Instantiate(hitParticle, this.gameObject.transform.position,  Quaternion.Euler(0, 0, particleRotation));
        ps.Play();

        // Invoke hurt animation
        Invoke("hurtAnimation", 0.2f);
        
        // If health is 0 then die
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Player Hurt Animation
    private void hurtAnimation()
    {
        anim.SetTrigger("hurt");
    }

    // Player Death Function
    public void Die()
    {
        currentHealth -= currentHealth;
        playerUI.health = 0;
        anim.SetBool("isDead", true);
        gameManager.EndRound(opponent);
    }

    // Player Respawn Function
    public void respawn()
    {
        anim.SetBool("isDead", false);
        anim.SetTrigger("respawn");
        currentHealth = maxHealth;
        playerUI.health = 100;
        transform.position = startPos;
    }

    // Camera Shake On Fall Animation
    public void onFallDie()
    {
        scamera.GetComponent<camera>().Shake(1, fallDieCurve);
    }

    // Dash Function
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
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
}