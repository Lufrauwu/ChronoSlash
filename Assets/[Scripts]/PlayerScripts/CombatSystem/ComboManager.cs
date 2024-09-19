using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class ComboManager : MonoBehaviour
{
    private List<string> comboList = new List<string>();
    private List<int> playerInput = new List<int>();
    void Start()
    {
        string[] combos = File.ReadAllLines("Assets/combos.txt");
        comboList.AddRange(combos);
    }

    void Update()
    {
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

        foreach (string combo in comboList)
        {
            if (combo.StartsWith(currentInput))
            {
                foundMatch = true;
            
                // Verificamos si el input coincide completamente con algún combo
                if (currentInput == combo)
                {
                    Debug.Log("Combo ejecutado: " + combo);
                    playerInput.Clear();
                    return;
                }
            }
        }

        // Si no se encontró ninguna coincidencia parcial, limpiamos el input
        if (!foundMatch)
        {
            Debug.Log("Combo no válido");
            playerInput.Clear();
        }

        // Limitar la cantidad de inputs a 4
        if (playerInput.Count > 4)
        {
            playerInput.RemoveAt(0);
        }
    }
}
