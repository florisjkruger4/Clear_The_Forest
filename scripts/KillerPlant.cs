using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillerPlant : MonoBehaviour
{
    public GameObject player;
    private Transform playerPos;
    private Vector2 currentPos;
    public float distance;

    public Animator enemyAnim;
    public Collider2D KPcollider;
    public Collider2D PlayerCol;
    public Collider2D BulletCol;

    public int maxHealth = 100;
    public int currentKPHealth;
    public HealthBar healthBar;

    public bool isFacingRight = true;

    public LayerMask DeadEnemy;

    public float fireRate = 0.6f;
    public Transform firingPoint;
    public GameObject bulletPrefab;

    public float DazedTime;
    public float StartDazedTime;

    public GameObject DmgPoints;
    public GameObject AxeDmgPoints;

    public Collider2D AxeCol;

    void Start()
    {
        KPcollider = GetComponent<Collider2D>();
        enemyAnim = GetComponent<Animator>();

        playerPos = player.GetComponent<Transform>();
        currentPos = GetComponent<Transform>().position;

        currentKPHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        isFacingRight = !isFacingRight;
    }

    void Shoot()
    {
        float angle = !isFacingRight ? 0f : 180f;
        Instantiate(bulletPrefab, firingPoint.position, Quaternion.Euler(new Vector3(0f, 0f, angle)));
        FindObjectOfType<AudioManager>().Play("KP Shoot");
    }

    void TakeDamage(int damage)
    {
        currentKPHealth -= damage;
        DazedTime = StartDazedTime;

        healthBar.SetHealth(currentKPHealth);
        Instantiate(DmgPoints, transform.position, Quaternion.identity);
        enemyAnim.SetTrigger("Hit");
    }

    public void TakeAxeDamage(int axeDamage)
    {
        currentKPHealth -= axeDamage;
        DazedTime = StartDazedTime;

        healthBar.SetHealth(currentKPHealth);
        Instantiate(AxeDmgPoints, transform.position, Quaternion.identity);
        enemyAnim.SetTrigger("Hit");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Bullet"))
        {
            TakeDamage(20);
        }
    }

    void Update()
    {

        healthBar.SetHealth(currentKPHealth);

        if (DazedTime > 0)
        {
            DazedTime -= Time.deltaTime;
        }
        if (currentKPHealth <= 0)
        {
            Destroy(gameObject); //?f)
        }

        if (Vector2.Distance(transform.position, playerPos.position) < distance && currentKPHealth > 0)
        {
            if (playerPos.position.x > transform.position.x && isFacingRight)
                Flip();
            if (playerPos.position.x < transform.position.x && !isFacingRight)
                Flip();
           
        }

        if (CanSeePlayer(distance))
        {
            enemyAnim.SetBool("Attack", true);

        }
        else
        {
            enemyAnim.SetBool("Attack", false);

        }
        
    }
    
    bool CanSeePlayer(float distance)
    {
        bool val = false;
        float CastDist = distance;

        if (isFacingRight == true)
        {
            CastDist = -distance;
        }

        Vector2 endPos = firingPoint.position + Vector3.right * CastDist;

        RaycastHit2D hit = Physics2D.Linecast(firingPoint.position, endPos, 1 << LayerMask.NameToLayer("PlayerLayer"));

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                val = true;
            }
            else
            {
                val = false;
            }

        }
        return val;
    }
    
}
