using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ComboManager2 : MonoBehaviour
{
    [Header ("Combo Inputs")]
    [SerializeField] private int maxComboInputs;
    [SerializeField] private TextMeshProUGUI textInput;
    [SerializeField] private TextMeshProUGUI secondsText;
    [SerializeField] private string currentInput;
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
    [SerializeField] private AttackTriggerer attackTriggerer;
    
    [Header("UI Elements")]
    [SerializeField] private GameObject combatUI;
    [SerializeField] private Image progressBar;

    [Header("TimeManagement")] [SerializeField]
    private float maxTimeInSeconds = 5f; // Cambiado a float
    [SerializeField] private float currentTimer;
    private int currentFrame;
    public bool playerTurn = true;
    
    private GAME_STATE currentGameState;

    [SerializeField] private GameObject postProcessingVolume;
    [SerializeField] private GameObject explorePostProcessingVolume;
    
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
                combatUI.SetActive(false);
                break;
            case GAME_STATE.START_TURN:
                if (playerTurn)
                {
                    InputManager.GetInstance().ActivateCombat();
                    ResetVariables();
                }
                else
                {
                    EnemyAttack();
                }
                //El codigo para cambiar el estado a el turno del jugador esta en Enemy.cs
                break;
            case GAME_STATE.ATTACK_STATE:
                InputManager.GetInstance().DeactivateInputs();
                if (playerTurn)
                {
                    FindAttack();
                }
                break;
            case GAME_STATE.END_TURN:
                playerTurn = !playerTurn;
                ResetVariables();
                GameManager.GetInstance().ChangeGameState(GAME_STATE.START_TURN);
                //playerTurn = !playerTurn;
                break;
            
        }
    }
    

    void Start()
    {
        playerTurn = true;
        SubscribeToGameState();
        string[] combos = File.ReadAllLines("Assets/combos.txt");
        comboList.AddRange(combos);
        currentEnergy = maxEnergy;
        currentTimer = maxTimeInSeconds; 
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            playerTurn = true;
            currentEnemy = other.GetComponent<Enemy>();
            
        }
    }

    void Update()
    {
        Debug.Log("El turno es: " + playerTurn);
        progressBar.fillAmount = (float)currentEnergy / maxEnergy;
        if (InputManager.GetInstance().LightAttack() && playerInput.Count < maxComboInputs)
        {
            if (currentEnergy >= lightAttackCost)
            {
                playerInput.Add(0);
                currentEnergy -= lightAttackCost;
                CheckCombo();
                //ExecuteAttack(currentInput);
            }
        }

        if (InputManager.GetInstance().HeavyAttack() && playerInput.Count < maxComboInputs)
        {
            if (currentEnergy >= heavyAttackCost)
            { 
                playerInput.Add(1);
                currentEnergy -= heavyAttackCost;
                CheckCombo();
            }
            
        }

        if (currentEnergy <= lightAttackCost || currentEnergy <= 0 || currentGameState == GAME_STATE.ATTACK_STATE && playerInput.Count < maxComboInputs)
        {
            CheckIncompleteCombo();
            
        }
        Debug.Log(GameManager.GetInstance().GetGameState());
       
    }

    private void FixedUpdate()
    {
        if (playerTurn && GameManager.GetInstance().GetGameState() == GAME_STATE.START_TURN)
        {
            TimerManager();
        }
    }

    void CheckCombo()
    {
         currentInput = string.Join("", playerInput);
       // bool foundMatch = false; 
       string promptInput = currentInput.Replace("0","a").Replace("1", "b");
       textInput.text = promptInput;

        if (playerInput.Count == maxComboInputs)
        {
            currentTimer = 0;
//            Debug.Log("TIEMPO: " + currentTimer);
            //GameManager.GetInstance().ChangeGameState(GAME_STATE.PLAYERATTACK);
        }
        
       
    }

    void CheckIncompleteCombo()
    { 
        currentInput = string.Join("", playerInput);
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
                    attackTriggerer.ChooseAnimation(combo);
                    playerInput.Clear();
                    currentEnergy = maxEnergy;
                    //GameManager.GetInstance().ChangeGameState(GAME_STATE.ENEMYTURN);

                    return;
                }
            }
        }

        if (!foundMatch)
        {
            Debug.Log("Combo incompleto no válido");
            playerInput.Clear();
            currentEnergy = maxEnergy;
           // GameManager.GetInstance().ChangeGameState(GAME_STATE.ENEMYTURN);
        }
    }

    private void ExecuteAttack(string attackToExecute)
    {
        bool foundMatch = false; 
        foreach (string combo in comboList)
        {
            if (combo == attackToExecute && playerInput.Count == maxComboInputs)
            {
                foundMatch = true;
            
                if (attackToExecute == combo)
                {
                    Debug.Log("PIO PIO");

                    Debug.Log("Combo ejecutado: " + combo);
                    MoveTowardsTarget(currentEnemy.transform);
                    attackTriggerer.ChooseAnimation(combo);
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

    private void TimerManager()
    {
        if (currentGameState == GAME_STATE.START_TURN)
        {
            currentTimer -= Time.fixedUnscaledDeltaTime; 
                    if (currentTimer <= 0)
                    {
                        GameManager.GetInstance().ChangeGameState(GAME_STATE.ATTACK_STATE);
                    }
                    
                    int currentSecond = Mathf.CeilToInt(currentTimer);
                    secondsText.text = currentSecond.ToString();
        }
        
    }
    private Vector3 TargetOffset()
    {
        Vector3 position = currentEnemy.transform.position;
        // Calcula una posición hacia la cual moverse a una distancia de 1.2f desde la posición actual
        return Vector3.MoveTowards(position, transform.position, 1.2f);
    }

    public void MoveTowardsTarget(Transform target)
    {
        // Verifica la distancia antes de iniciar el movimiento
        if (Vector3.Distance(transform.position, target.position) > 1 && Vector3.Distance(transform.position, target.position) < 10)
        {
            // Inicia la corrutina para mover y rotar el objeto
            StartCoroutine(MoveAndLookAtTarget(TargetOffset(), currentEnemy.transform.position, 0.5f, 0.2f));
        }
    }

    private IEnumerator MoveAndLookAtTarget(Vector3 targetPosition, Vector3 lookAtPosition, float moveDuration, float lookDuration)
    {
        // Movimiento
        Vector3 startPosition = transform.position;
        float moveElapsedTime = 0f;

        while (moveElapsedTime < moveDuration)
        {
            // Interpola la posición usando Lerp
            transform.position = Vector3.Lerp(startPosition, targetPosition, moveElapsedTime / moveDuration);
            moveElapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition; // Asegura la posición final exacta
        
    }

    public void DeactivatePostProcessVolume()
    {
        postProcessingVolume.SetActive(false);
    }

    public void ResetVariables()
    {
        if (playerTurn && GameManager.GetInstance().GetGameState() != GAME_STATE.EXPLORATION)
        {
            //Debug.Log("HOLAAAAA");
            postProcessingVolume.SetActive(true);
            Time.timeScale = 0.5f;
            combatUI.SetActive(true);
            textInput.text = "";
            playerInput.Clear();
            currentInput = "";
            currentEnergy = maxEnergy;
            currentTimer = 5;
        }
        textInput.text = "";
        playerInput.Clear();
        currentInput = "";
        currentEnergy = maxEnergy;
        currentTimer = 5;
//        Debug.Log("CurrentlyReseting");
    }

    public void SetAllToDefault()
    {
        InputManager.GetInstance().DeactivateCombat();
        playerTurn = true;
        textInput.text = "";
        playerInput.Clear();
        currentInput = "";
        currentEnergy = maxEnergy;
        currentTimer = 5;

    }

    private void FindAttack()
    {
        Time.timeScale = 1f;
        postProcessingVolume.SetActive(false);
        ExecuteAttack(currentInput);

    }

    private void EnemyAttack()
    {
//        Debug.Log("ATAQUEEEEE");
        currentEnemy.Attack();
    }

    private void ActivateCombatUI()
    {
        
    }
}