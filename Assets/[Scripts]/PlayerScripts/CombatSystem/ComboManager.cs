using System.IO;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ComboManager : MonoBehaviour
{
    [Header ("Combo Inputs")]
    [SerializeField] private int maxComboInputs;
    [SerializeField] private TextMeshProUGUI textInput;
    private List<string> comboList = new List<string>();
    private List<int> playerInput = new List<int>();
    [Header ("Attack Variables")]
    [SerializeField] private int maxEnergy = 100;
    [SerializeField] private int currentEnergy;
    [SerializeField] private int lightAttackCost = 10;
    [SerializeField] private int heavyAttackCost = 30;
    private int lightAttackDamage = 10;
    private int heavyAttackDamage = 30;
    
    [Header("UI Elements")]
    [SerializeField] private Image progressBar;
    

    void Start()
    {
        string[] combos = File.ReadAllLines("Assets/combos.txt");
        comboList.AddRange(combos);
        currentEnergy = maxEnergy;
    }

    void Update()
    {
        progressBar.fillAmount = (float)currentEnergy / maxEnergy;
        if (InputManager.GetInstance().LightAttack())
        {
            if (currentEnergy >= lightAttackCost)
            {
                playerInput.Add(0);
                currentEnergy -= lightAttackCost;
                CheckCombo();
            }
        }

        if (InputManager.GetInstance().HeavyAttack())
        {
            if (currentEnergy >= heavyAttackCost)
            { 
                playerInput.Add(1);
                currentEnergy -= heavyAttackCost;
                CheckCombo();
            }
            
        }

        if (currentEnergy <= lightAttackCost || currentEnergy <= 0)
        {
            CheckIncompleteCombo();
        }
    }

    void CheckCombo()
    {
        string currentInput = string.Join("", playerInput);
        bool foundMatch = false; 
        textInput.text = currentInput;

        foreach (string combo in comboList)
        {
            if (combo == currentInput && playerInput.Count == maxComboInputs)
            {
                foundMatch = true;
            
                if (currentInput == combo)
                {
                    Debug.Log("Combo ejecutado: " + combo);
                    playerInput.Clear();
                    currentEnergy = maxEnergy;
                    return;
                }
            }
        }

        if (playerInput.Count >= maxComboInputs && !foundMatch)
        {
            Debug.Log("Combo no válido");
            playerInput.Clear();
            currentEnergy = maxEnergy;
        }
    }

    void CheckIncompleteCombo()
    {
        string currentInput = string.Join("", playerInput);
        bool foundMatch = false; 
        textInput.text = currentInput;
        
        foreach (string combo in comboList)
        {
            if (combo == currentInput)
            {
                foundMatch = true;
            
                if (currentInput == combo)
                {
                    Debug.Log("Combo incompleto ejecutado: " + combo);
                    playerInput.Clear();
                    currentEnergy = maxEnergy;
                    return;
                }
            }
        }

        if (!foundMatch)
        {
            Debug.Log("Combo incompleto no válido");
            playerInput.Clear();
            currentEnergy = maxEnergy;
        }
    }
}