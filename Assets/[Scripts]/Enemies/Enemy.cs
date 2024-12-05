using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Input = UnityEngine.Windows.Input;

public class Enemy : MonoBehaviour
{
    private ThirdPersonController thirdPersonController;
    [SerializeField] private int currentHealth = 100;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int damage;
    private HealthController healthController;
    [SerializeField] private ENEMY_TYPE enemyType;
    [SerializeField] private Animator animator;
    [SerializeField] private ComboManager2 comboManager2;
    [SerializeField] private GameObject Gate;
    [SerializeField] private GameObject canvas;

    private void Awake()
    {
        thirdPersonController = FindAnyObjectByType<ThirdPersonController>();
        switch (enemyType)
        {
            case ENEMY_TYPE.LIGHT:
                maxHealth = 50; 
                damage = 30;
                break;
            case ENEMY_TYPE.MEDIUM:
                maxHealth = 100;
                damage = 20;
                break;
            case ENEMY_TYPE.HEAVY:
                maxHealth = 200;
                damage = 10;
                break;
        }
        
        currentHealth = maxHealth;

    }


    private void OnTriggerEnter(Collider other)
    {
        if (thirdPersonController.isInCombat)
        {
            Debug.Log(gameObject.name + " is in combat");
            return;
        }
        if (other.tag == "Player")
        {
            SoundManager.GetInstance().PlaySFX(SoundManager.GetInstance().CombatMusic);
            InputManager.GetInstance().ActivateCombat();
            CameraController.GetInstance().SetLockOn();
            healthController = other.GetComponent<HealthController>();
            GameManager.GetInstance().ChangeGameState(GAME_STATE.START_TURN);
            comboManager2.playerTurn = true;
        }
    }

    private void Update()
    {
        if (currentHealth<= 0)
        {
            Death();
        }

        
        
    }
    private void Death()
    {
        this.gameObject.SetActive(false);
        if(Gate.gameObject != null)
        {
            Gate.SetActive(false);
        }
    }

    private void OnDisable()
    {
        if (canvas == null)
        {
            return;
        }
        thirdPersonController.isInCombat = false;
        GameManager.GetInstance().ChangeGameState(GAME_STATE.END_TURN);
        InputManager.GetInstance().DeactivateCombat();
        CameraController.GetInstance().SetLockOff();
        canvas.SetActive(false);
        thirdPersonController.ResetEnemy();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
    }

    public void Attack()
    {
        if (thirdPersonController.isInCombat == false)
        {
            return;
        }
        if (thirdPersonController == null)
        {
            return;
        }

        
        GameManager.GetInstance().ChangeGameState(GAME_STATE.ATTACK_STATE);
        Vector3 direction = thirdPersonController.transform.position - transform.position;
        direction.y = 0;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation,  10f);
        //Debug.Log("EnemyAttacked");
        animator.SetBool("Attack", true);
        healthController.TakeDamage(damage);
//        Debug.Log("YACAMBIO");
        //animator.SetBool("Attack", false);

    }
}

public enum ENEMY_TYPE
{
    LIGHT,
    MEDIUM,
    HEAVY
}
