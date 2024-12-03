using System;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField] private int currentHealth = 100;
    [SerializeField] private int maxHealth = 100;
    public Action<float, bool> OnHealthChanged;
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        OnHealthChanged.Invoke(currentHealth, true);
    }

    public void Heal(int healthToHeal)
    {
        currentHealth += healthToHeal;
        OnHealthChanged.Invoke(currentHealth, false);
    }
    
    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }
}
