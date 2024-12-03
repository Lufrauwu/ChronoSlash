using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTriggerer : MonoBehaviour
{
    [SerializeField] private int[] savedCombo;
    [SerializeField] private Animator animator;
    [SerializeField] private Enemy _enemy;
    [SerializeField] private AnimationController _animController;
    void Start()
    {
        _animController = GetComponentInChildren<AnimationController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        _enemy = other.GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChooseAnimation(string combo)
    {
        switch (combo)
        {
            case "000000":
              // Debug.Log("hOLA");
              _animController.ChangeAnimation("A_000000");
              _enemy.TakeDamage(20);
               combo = "";
                break;
            case "001100":
                _animController.ChangeAnimation("A_001100");
                _enemy.TakeDamage(20);
                combo = "";
                break;
            case "000111":
                _animController.ChangeAnimation("A_000111");
                _enemy.TakeDamage(20);
                combo = "";                break;
            case "110011":
                _animController.ChangeAnimation("A_110011");
                _enemy.TakeDamage(20);
                combo = "";
                break;
            case "111111":
                _animController.ChangeAnimation("A_111111");
                _enemy.TakeDamage(20);
                combo = "";
                break;
            case "00000":
                _animController.ChangeAnimation("A_00000");
                _enemy.TakeDamage(20);
                combo = "";
                break;
            case "00011":
                _animController.ChangeAnimation("A_00011");
                _enemy.TakeDamage(20);
                combo = "";
                break;
            case "11000":
                _animController.ChangeAnimation("A_11000");
                _enemy.TakeDamage(20);
                combo = "";
                break;
            case "0000":
                _animController.ChangeAnimation("A_0000");
                _enemy.TakeDamage(20);
                combo = "";
                break;
            case "0101":
                _animController.ChangeAnimation("A_0101");
                _enemy.TakeDamage(20);
                combo = "";
                break; 
            case "1000":
                _animController.ChangeAnimation("A_1000");
                _enemy.TakeDamage(20);
                combo = "";
                break;  
            case "1100":
                _animController.ChangeAnimation("A_1100");
                _enemy.TakeDamage(20);
                combo = "";                
                break;
            
            

            
        }
    }
}
