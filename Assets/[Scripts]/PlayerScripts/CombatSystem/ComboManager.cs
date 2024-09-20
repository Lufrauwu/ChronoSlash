using System.IO;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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

    void Start()
    {
        string[] combos = File.ReadAllLines("Assets/combos.txt");
        comboList.AddRange(combos);
    }

    void Update()
    {
        Debug.Log(playerInput.Count);
        if (InputManager.GetInstance().LightAttack())
        {
            playerInput.Add(0);
            CheckCombo();
        }

        if (InputManager.GetInstance().HeavyAttack())
        {
            playerInput.Add(1);
            CheckCombo();
            
        }
    }

    void CheckCombo()
    {
        string currentInput = string.Join("", playerInput);
        bool foundMatch = false;
        textInput.text = currentInput;

        foreach (string combo in comboList)
        {
            if (playerInput.Count <= maxComboInputs && combo.StartsWith(currentInput))
            {
                foundMatch = true;
            
                if (currentInput == combo)
                {
                    Debug.Log("Combo ejecutado: " + combo);
                    playerInput.Clear();
                    return;
                }
            }
        }

        if (playerInput.Count >= maxComboInputs && !foundMatch)
        {
            Debug.Log("Combo no válido");
            playerInput.Clear();
        }

        if (playerInput.Count > maxComboInputs)
        {
            playerInput.RemoveAt(0);
        }
    }
}
