using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KPbullet : MonoBehaviour
{
    public float bulletSpeed;
    public float bulletDamage;
    public Rigidbody2D rb;
    private Animator bulletAnim;
    public Collider2D col;

    public int BullletDmg = 10;

    public Transform bulletTransform;

    public ParticleSystem explosion;
    public ParticleSystem trail;

    void Start()
    {
        trail.Play();
    }

    private void FixedUpdate()
    {
        rb.velocity = transform.right * bulletSpeed;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        FindObjectOfType<AudioManager>().Play("KP Bullet Explosion");
        explosion.transform.SetParent(null);
        explosion.Play();
        Destroy(explosion.gameObject, 1f);

        trail.transform.SetParent(null);
        Destroy(trail.gameObject, 0.8f);
        Destroy(gameObject);

    }
}
