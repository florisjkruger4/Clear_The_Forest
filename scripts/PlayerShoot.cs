using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public float fireRate = 0.2f;
    public Transform firingPoint;
    public GameObject bulletPrefab;

    public Animator anim;
    

    float timeUntilFire;
    Playermovement pm;

    public int CurrentWeaponNumber;

    public bool CanFire = true;

    void changeWeapon()
    {
        if (CurrentWeaponNumber == 0)
        {
            CurrentWeaponNumber += 1;
            anim.SetLayerWeight(CurrentWeaponNumber - 1, 0);
            anim.SetLayerWeight(CurrentWeaponNumber, 1);
        }
        else
        {
            CurrentWeaponNumber -= 1;
            anim.SetLayerWeight(CurrentWeaponNumber + 1, 0);
            anim.SetLayerWeight(CurrentWeaponNumber, 1);
        }


    }

    private void Start()
    {
        pm = gameObject.GetComponent<Playermovement>();
    }


    private void Update()
    {
        if (PauseMenu.GameIsPaused == true)
        {
            CanFire = false;
        }
        else
        {
            CanFire = true;
        }

        if (Input.GetKeyDown(KeyCode.C) && CanFire == true)
        {
            
            anim.SetTrigger("SwitchToAxe");
            changeWeapon();
        }


        if (Input.GetMouseButtonDown(0) && timeUntilFire < Time.time && CurrentWeaponNumber == 0 && CanFire == true)
        {
            FindObjectOfType<AudioManager>().Play("Player Shoot");
            Shoot();
            timeUntilFire = Time.time + fireRate;
        }

        if (CurrentWeaponNumber == 0 && Input.GetKeyDown(KeyCode.C))
        {
            FindObjectOfType<AudioManager>().Play("Reload");
        }
    }

    void Shoot()
    {
        float angle = pm.isFacingRight ? 0f : 180f;
        Instantiate(bulletPrefab, firingPoint.position, Quaternion.Euler(new Vector3(0f, 0f, angle)));
    }
    
}
