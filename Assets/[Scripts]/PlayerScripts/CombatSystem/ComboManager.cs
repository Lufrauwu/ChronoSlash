using System;
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
    [SerializeField] private TextMeshProUGUI secondsText;
    private List<string> comboList = new List<string>();
    private List<int> playerInput = new List<int>();
    private bool isComboComplete = false;
    [Header ("Attack Variables")]
    [SerializeField] private int maxEnergy = 100;
    [SerializeField] private int currentEnergy;
    [SerializeField] private int lightAttackCost = 10;
    [SerializeField] private int heavyAttackCost = 30;
    private int lightAttackDamage = 10;
    private int heavyAttackDamage = 30;
    [SerializeField] private Enemy currentEnemy;
    
    [Header("UI Elements")]
    [SerializeField] private GameObject combatUI;
    [SerializeField] private Image progressBar;

    [Header("TimeManagement")] [SerializeField]
    private int maxTimeInSeconds = 5;
    private int currentFrame;
    private bool playerTurn;
    
    private GAME_STATE currentGameState;
    
    private void SubscribeToGameState()
    {
        GameManager.GetInstance().OnGameStateChanged += GameStateChange;
        GameStateChange(GameManager.GetInstance().GetGameState());
    }

    private void GameStateChange(GAME_STATE _newGameState)
    {
        
        currentGameState = _newGameState;
        
        switch (_newGameState)
        {
            case GAME_STATE.EXPLORATION:
                playerTurn = false;
                combatUI.SetActive(false);
                break;
            case GAME_STATE.PLAYERTURN:
                playerTurn = true;
                combatUI.SetActive(true);
                maxTimeInSeconds = maxTimeInSeconds * 60;
                //El codigo para cambiar el estado a el turno del jugador esta en Enemy.cs
                break;
            case GAME_STATE.PLAYERATTACK:
                playerTurn = false;
                break;
            case GAME_STATE.ENEMYTURN:
                playerTurn = false;
                break;
            case GAME_STATE.ENEMATACK:
                playerTurn = false;
                break;
        }
    }
    

    void Start()
    {
        SubscribeToGameState();
        string[] combos = File.ReadAllLines("Assets/combos.txt");
        comboList.AddRange(combos);
        currentEnergy = maxEnergy;
        //maxTimeInSeconds = maxTimeInSeconds * 60;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            currentEnemy = other.GetComponent<Enemy>();
            
        }
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

        if (currentEnergy <= lightAttackCost || currentEnergy <= 0 || currentGameState == GAME_STATE.PLAYERATTACK && playerInput.Count < maxComboInputs)
        {
            CheckIncompleteCombo();
            
        }

        if (currentGameState == GAME_STATE.PLAYERATTACK)
        {
            ExecuteAttack();
        }
    }

    private void FixedUpdate()
    {
        if (playerTurn)
        {
            TimerManager();
        }
        int currentSecond = maxTimeInSeconds / 60;
        secondsText.text = currentSecond.ToString();
        Debug.Log(currentGameState);
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

    private void ExecuteAttack()
    {
        
        currentEnemy.TakeDamage(heavyAttackCost);
        Debug.Log("XD");
        
    }

    private void TimerManager()
    {
        maxTimeInSeconds--;
        if (maxTimeInSeconds <= 0 && currentGameState == GAME_STATE.PLAYERTURN)
        {
            GameManager.GetInstance().ChangeGameState(GAME_STATE.PLAYERATTACK);
        }
            
        /*if (maxTimeInSeconds<= 0 && currentGameState == GAME_STATE.PLAYERATTACK) 
        {
            GameManager.GetInstance().ChangeGameState(GAME_STATE.ENEMYTURN);
        }*/
    }

    private void ActivateCombatUI()
    {
        
    }
}