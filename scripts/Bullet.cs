using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed;
    public float bulletDamage;
    public Rigidbody2D rb;
    private Animator bulletAnim;
    public Collider2D col;

    public ParticleSystem ExplosionParticles;

    public ParticleSystem TrailParticles;

    public Light2D pointlight;

    private void Start()
    {
        bulletAnim = GetComponent<Animator>();
        TrailParticles.Play();

    }

    private void FixedUpdate()
    {
        rb.velocity = transform.right * bulletSpeed;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        FindObjectOfType<AudioManager>().Play("Bullet Explosion");

        ExplosionParticles.transform.SetParent(null);
        ExplosionParticles.Play();
        Destroy(ExplosionParticles.gameObject, 1f);

        TrailParticles.transform.SetParent(null);
        Destroy(TrailParticles.gameObject, 0.8f);
        
        bulletSpeed = 0f;
        Destroy(gameObject);
    }

}