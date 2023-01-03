using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playermovement : MonoBehaviour
{
    public float movementSpeed;
    public Rigidbody2D rb;
    public Animator anim;

    public float doubleJumpForce;
    public float jumpForce;
    public Transform feet;
    public LayerMask groundLayers;

    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;

    public GameObject Bralien;
    public GameObject KillerPlant;

    public ParticleSystem footsteps;
    private ParticleSystem.EmissionModule footemission;
    public Transform LandPartStartPos;
    public ParticleSystem landingEffect;
    public ParticleSystem JetpackPaticles;
    public ParticleSystem AxeSparkParticles;
    private bool wasOnGround;

    public float hangtime;
    public float hangcounter;

    public bool Falling = false;
    public float FallingThreshold;

    public int jumpcount = 2;
    public int currentJump = 0;

    public Transform AxeAttackRange;
    public LayerMask EnemyLayer;

    public GameObject RespawnPoint;

    public Collider2D AxeCol;

    public int KPDamage = 10;
    public GameObject KPbullet;
    public GameObject HeartHit;

    private void AxeAttack()
    {
        Collider2D[] InRange = Physics2D.OverlapCircleAll(AxeAttackRange.position, 0.15f, EnemyLayer);
        foreach (Collider2D Bralien in InRange)
        {
            Bralien.GetComponent<EnemyScript>().TakeAxeDamage(20);
        }

    }

    private void KPaxeAttack()
    {
        Collider2D[] InRange = Physics2D.OverlapCircleAll(AxeAttackRange.position, 0.15f, EnemyLayer);
        foreach (Collider2D KillerPlant in InRange)
        {
            KillerPlant.GetComponent<KillerPlant>().TakeAxeDamage(10);
        }

    }
    


    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        footemission = footsteps.emission;

        anim.SetBool("Falling", false);
    }

    public void TakeDamage()
    {
        healthBar.SetHealth(currentHealth);
        
        rb.AddForce(Vector2.up * 150);
    }

    public bool isFacingRight = true;

    float mx;



    private void Update()
    {

        healthBar.GetComponent<HealthBar>().slider.value = currentHealth;
        
        if (currentHealth <= 0 && RespawnPoint.activeSelf)
        {
            Manager.instance.Respawn();
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);
        }

        mx = Input.GetAxisRaw("Horizontal");

        if (rb.velocity.y < FallingThreshold && !IsGrounded())
        {
            Falling = true;
            anim.SetBool("Falling", true);

        }
        else
        {
            Falling = false;
        }

        if (IsGrounded())
        {
            anim.SetBool("Falling", false);
            hangcounter = hangtime;
        }
        else
        {
            hangcounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && hangcounter > 0f && jumpcount > currentJump)
        {
            anim.SetBool("Falling", false);
            anim.SetBool("isRunning", false);
            anim.SetBool("isGrounded", false);
            Jump();
            
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        if (Mathf.Abs(mx) > 0.05 && currentJump == 0)
        {
            anim.SetBool("isRunning", true);
            JetpackPaticles.Stop();
            FindObjectOfType<AudioManager>().Stop("Jetpack");
        }
        else
        {
            IsGrounded();
            anim.SetBool("isRunning", false);
            FindObjectOfType<AudioManager>().Stop("Footsteps");
        }


        if (mx > 0f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
            isFacingRight = true;
        }

        else if (mx < 0f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
            isFacingRight = false;
        }

        anim.SetBool("isGrounded", IsGrounded());

        if (Input.GetAxisRaw("Horizontal") != 0 && IsGrounded())
        {
            footemission.rateOverTime = 35f;
        }
        else
        {
            footemission.rateOverTime = 0f;
        }

        if (!wasOnGround && IsGrounded())
        {
            landingEffect.gameObject.SetActive(true);
            landingEffect.Stop();
            landingEffect.transform.position = LandPartStartPos.transform.position;
            landingEffect.Play();
            FindObjectOfType<AudioManager>().Play("Landing");
        }

        if (Input.GetButtonDown("Jump") && jumpcount > currentJump && hangcounter <= 0f)
        {
            anim.SetBool("Falling", false);
            anim.SetBool("isRunning", false);
            anim.SetBool("isGrounded", false);
            DoubleJump();
            
        }

        if (Input.GetButtonDown("Jump"))
        {
            currentJump++;
        }

        wasOnGround = IsGrounded();

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsGrounded() == true)
        {
            currentJump = 0;
        }

        if (collision.collider.CompareTag("KPbullet"))
        {
            TakeDamage();
            FindObjectOfType<AudioManager>().Play("Player Hit");
            Manager.instance.DecreaseHP(KPDamage);

            HeartHit.GetComponent<Animator>().SetTrigger("HeartHit");


            currentHealth -= KPDamage;
            anim.SetTrigger("Hit");


            Debug.Log("_____Damage_____");
        }
    }

    private void StartJetpackParticles()
    {
        JetpackPaticles.Play();
        FindObjectOfType<AudioManager>().Play("Jetpack");
    }

    private void StopJetpackPlayTime()
    {
        JetpackPaticles.Stop();
        FindObjectOfType<AudioManager>().Stop("Jetpack");
    }

    private void PlayAxeparks()
    {
        if (IsGrounded() == true)
        {
            AxeSparkParticles.Play();
            FindObjectOfType<AudioManager>().Play("Axe Ding");
        }
    }

    private void PlayFootsteps()
    {
        if (Mathf.Abs(mx) > 0.05 && IsGrounded())
        {
            FindObjectOfType<AudioManager>().Play("Footsteps");
        }
    }

    private void FixedUpdate()
    {
        Vector2 movement = new Vector2(mx * movementSpeed, rb.velocity.y);

        rb.velocity = movement;

        if (IsGrounded() == false)
        {
            FindObjectOfType<AudioManager>().Stop("Footsteps");
        }
    }

    void DoubleJump()
    {
        Vector2 movement = new Vector2(rb.velocity.x, doubleJumpForce);
        rb.velocity = movement;
    }

    void Jump()
    {
        Vector2 movement = new Vector2(rb.velocity.x, jumpForce);
        rb.velocity = movement;
    }

    public bool IsGrounded()
    {
        Collider2D groundCheck = Physics2D.OverlapCircle(feet.position, 0.13f, groundLayers);


        if (groundCheck != null)
        {
            return true;
        }

        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(feet.position, 0.13f);

        Gizmos.DrawWireSphere(AxeAttackRange.position, 0.15f);
    }
}
