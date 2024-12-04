using System;
using UnityEngine;
using TMPro;

public class HealthController : MonoBehaviour
{
    [SerializeField] private int currentHealth = 100;
    [SerializeField] private int maxHealth = 100;
    public Action<float, bool> OnHealthChanged;
    public float MedKits = 0;

    public int MaxHeal;



    void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        MaxHeal = (maxHealth / 2);
        if(InputManager.GetInstance().HealInput() && MedKits >= 1)
        {
            Heal(MaxHeal);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        OnHealthChanged.Invoke(currentHealth, true);
    }

    public void Heal(int healthToHeal)
    {
        currentHealth += healthToHeal;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        OnHealthChanged.Invoke(currentHealth, false);
        MedKits -= 1;
    }
    
    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Life_Upgrade")
        {
            maxHealth += 10;
            other.gameObject.SetActive(false);
        }
        if (other.gameObject.tag == "MedKit")
        {
            MedKits += 1;
            other.gameObject.SetActive(false);
        }
    }

}
