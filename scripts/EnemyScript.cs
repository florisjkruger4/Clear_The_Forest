using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour
{
    public GameObject player;
    private Transform playerPos;
    private Vector2 currentPos;
    public float distance;
    private float speedEnemy;
    public Animator enemyAnim;
    public Collider2D BAcollider;
    public Collider2D PlayerCol;
    public Collider2D BulletCol;
    public Collider2D GemCol;
    
    public int maxHealth = 100;
    public int currentBAHealth;
    public HealthBar healthBar;

    public bool isFacingRight = true;

    public LayerMask PlayerLayer;
    public LayerMask DeadEnemy;

    public Transform AttackRange;
    public int BADamage = 10;

    public float DazedTime;
    public float StartDazedTime;

    public Material DefultMaterial;
    public Material GlowMaterial;

    public ParticleSystem ArmPS;
    public ParticleSystem ShoulderPS;
    public ParticleSystem HeadPS;

    public GameObject HeartHit;
    public GameObject DmgPoints;
    public GameObject AxeDmgPoints;

    private bool bulletCol;

    public Rigidbody2D enemyRB;

    public Collider2D AxeCol;

    private void Start()
    {
        BAcollider = GetComponent<Collider2D>();
        playerPos = player.GetComponent<Transform>();
        currentPos = GetComponent<Transform>().position;
        enemyAnim = GetComponent<Animator>();

        Physics2D.IgnoreCollision(BAcollider, GemCol);

        currentBAHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

    }

    void TakeDamage(int damage)
    {
        currentBAHealth -= damage;

        healthBar.SetHealth(currentBAHealth);
        Instantiate(DmgPoints, transform.position, Quaternion.identity);

        if (isFacingRight == false)
        {
            enemyRB.AddForce(Vector2.up * 60);
            enemyRB.AddForce(Vector2.left * 60);
        }
        else
        {
            enemyRB.AddForce(Vector2.up * 60);
            enemyRB.AddForce(Vector2.right * 60);
        }
    }
    public void TakeAxeDamage(int axeDamage)
    {
        enemyAnim.SetTrigger("AxeHit");
        currentBAHealth -= axeDamage;
        DazedTime = StartDazedTime;

        if (isFacingRight == false)
        {
            enemyRB.AddForce(Vector2.up * 120);
            enemyRB.AddForce(Vector2.left * 120);
        }
        else
        {
            enemyRB.AddForce(Vector2.up * 120);
            enemyRB.AddForce(Vector2.right * 120);
        }

        healthBar.SetHealth(currentBAHealth);
        Instantiate(AxeDmgPoints, transform.position, Quaternion.identity);
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        isFacingRight = !isFacingRight;
    }


    void Update()
    {
        healthBar.SetHealth(currentBAHealth);

        if (DazedTime <= 0)
        {
            speedEnemy = 3f;

  
            if (currentBAHealth <= 0)
            {
                speedEnemy = 0f;

            }
            

        }
        else
        {
            speedEnemy = 0f;
            DazedTime -= Time.deltaTime;

        }

        if (Vector2.Distance(transform.position, playerPos.position) < distance && currentBAHealth > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, playerPos.position, speedEnemy * Time.deltaTime);
            if (playerPos.position.x > transform.position.x && isFacingRight) 
                Flip();
            if (playerPos.position.x < transform.position.x && !isFacingRight)
                Flip();

            enemyAnim.SetBool("Alert", true);
            enemyAnim.SetBool("Idle", false);

        }

        else
        {

            enemyAnim.SetBool("Alert", true);
            enemyAnim.SetBool("Idle", false);

            if (Vector2.Distance(transform.position,currentPos) <= 0)
            {
                enemyAnim.SetBool("Idle", true);
            }
            else
            {
                enemyAnim.SetBool("Alert", false);
                enemyAnim.SetBool("Idle", true);

            }
           
        }
        if (bulletCol == true)
        {
            enemyAnim.SetBool("Death", true);
            Destroy(gameObject, 3f);
            gameObject.layer = LayerMask.NameToLayer("DeadEnemy");
            speedEnemy = 0f;
        }
        if (currentBAHealth <= 0)
        {
            enemyAnim.SetBool("AxeDeath", true);
            Destroy(gameObject, 3f);
            gameObject.layer = LayerMask.NameToLayer("DeadEnemy");
            speedEnemy = 0f;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Bullet") && currentBAHealth > 0)
        {
            TakeDamage(10);
            DazedTime = StartDazedTime;
            enemyAnim.SetTrigger("Hit");

        }
        if (collision.gameObject.CompareTag("Player"))
        {
            speedEnemy = 0f;

            enemyAnim.SetBool("Attack", true);
            enemyAnim.SetBool("Alert", false);
        }
        if (currentBAHealth <= 0 && collision.gameObject.CompareTag("Bullet"))
        {
            bulletCol = true;
            
        }

    }

    private void HeadParticlePlayTime()
    {
        HeadPS.Play();
    }

    private void ArmParticlePlayTime()
    {
        ArmPS.Play();
    }

    private void ShoulderParticlePlayTime()
    {
        ShoulderPS.Play();
    }

    private void PlayAxeDeathSound()
    {
        FindObjectOfType<AudioManager>().Play("Bralien Axe");
    }

    private void PlayBulletDeathSound()
    {
        FindObjectOfType<AudioManager>().Play("Bralien Bullet");
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            enemyAnim.SetBool("Attack", false);
            enemyAnim.SetBool("Alert", true);


        }

    }

    private void Attack()
    {

        Collider2D[] InRange = Physics2D.OverlapCircleAll(AttackRange.position, 0.15f, PlayerLayer);
        foreach (Collider2D player in InRange)
        {
            Manager.instance.DecreaseHP(BADamage);

            player.GetComponent<Playermovement>().TakeDamage();

            HeartHit.GetComponent<Animator>().SetTrigger("HeartHit");

            speedEnemy = 0f;
            player.GetComponent<Playermovement>().currentHealth -= BADamage;
            player.GetComponent<Playermovement>().anim.SetTrigger("Hit");


            FindObjectOfType<AudioManager>().Play("Bralien Attack");

        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(AttackRange.position, 0.15f);
    }



}
