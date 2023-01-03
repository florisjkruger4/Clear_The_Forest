using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public static Manager instance;

    public int currency = 0;
    public Text currencyUI;

    public int maxHPvalue = 100;
    public Text HPdisplay;

    public GameObject Player;

    public int heal = 50;

    public Transform respawnPoint;

    public CinemachineVirtualCameraBase cam;

    public HealthBar healthBar;


    public void Respawn()
    {
        HPdisplay.text = "100/100";
        Player.transform.position = respawnPoint.position;
        cam.Follow = Player.transform;
        FindObjectOfType<AudioManager>().Play("Player Death");
        
    }


    public void IncreaseCurrency(int amount)
    {
        currency += amount;
        currencyUI.text = "$" + currency;
        FindObjectOfType<AudioManager>().Play("Gem Pickup");
    }

    public void IncreaseHealth(int amount)
    {
        healthBar.GetComponent<HealthBar>().slider.value += amount;
        HPdisplay.text = healthBar.GetComponent<HealthBar>().slider.value + "/100";
        Player.GetComponent<Playermovement>().currentHealth += heal;

        FindObjectOfType<AudioManager>().Play("Player Heal");

        if (Player.GetComponent<Playermovement>().currentHealth > 100)
        {
            Player.GetComponent<Playermovement>().currentHealth = maxHPvalue;
        }

        if (healthBar.GetComponent<HealthBar>().slider.value > 100)
        {
            healthBar.GetComponent<HealthBar>().slider.value = maxHPvalue;
            HPdisplay.text = healthBar.GetComponent<HealthBar>().slider.value + "/100";
        }
    }

    public void DecreaseHP(int amount)
    {
        healthBar.GetComponent<HealthBar>().slider.value -= amount;
        HPdisplay.text = healthBar.GetComponent<HealthBar>().slider.value + "/100";
        FindObjectOfType<AudioManager>().Play("Player Hit");
    }

    private void Awake()
    {
        instance = this;
        FindObjectOfType<AudioManager>().Play("Theme");
    }

}
