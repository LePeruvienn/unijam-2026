using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public int maxHealth = 3;
    public int currentHealth;
    public Text healthText;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
        healthSlider.maxValue = maxHealth;
        currentHealth = maxHealth;
        Sync(currentHealth);
       
    }
        
    

    // Update is called once per frame
    void Update()
    {
        Sync(currentHealth);
    }

    private void Sync(int health)
    {
        
        healthSlider.value = health;
        healthText.text = health.ToString();
        healthSlider.maxValue = maxHealth;
       // if (health > maxHealth)
      //  {
        //    maxHealth = health;
        //}
    }
    
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Sync(currentHealth);
    }

   
    public void IncreaseHealth(int amount)
    {
        currentHealth += amount;
        Sync(currentHealth);
    }
    
}
