using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    [SerializeField] private HealthController _healthController; 
    private List<Image> healthImages = new List<Image>();
    private float healthPercentage;

    private void GetHealthImage(float percentage, bool isReceivingDamage)
    {
        float index = healthImages.Count * percentage / 100;
        int roundedIndex = (int)index;
        float residue = index - roundedIndex;
        if (isReceivingDamage)
        {
            for (int i = healthImages.Count-1; i > roundedIndex; i--)
            {
                healthImages[i].fillAmount = 0;
            } 
        }
        else
        {
            for (int i = 0; i < roundedIndex; i++)
            {
                healthImages[i].fillAmount = 1;
            }
        }
        if (percentage < 100)
        {
            healthImages[roundedIndex].fillAmount = Mathf.Clamp(residue, 0,1);;
        }
        
    }
    
    private void OnEnable()
    {
        _healthController.OnHealthChanged += OnHealthUpdate;
    }

    private void Start()
    {
        
        healthImages.AddRange(GetComponentsInChildren<Image>());
        healthImages.Remove(GetComponent<Image>());
        
    }
    

    private void OnHealthUpdate(float health, bool isDamage)
    {
        healthPercentage = HealthPercent(health);
        GetHealthImage(healthPercentage, isDamage);
    }

    private float HealthPercent(float health)
    {
        return (health / _healthController.GetMaxHealth()) * 100;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            OnHealthUpdate(_healthController.GetCurrentHealth(), true);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            OnHealthUpdate(_healthController.GetCurrentHealth(), false);
        }
    }

    private void OnDisable()
    {
        _healthController.OnHealthChanged -= OnHealthUpdate;
    }
}
